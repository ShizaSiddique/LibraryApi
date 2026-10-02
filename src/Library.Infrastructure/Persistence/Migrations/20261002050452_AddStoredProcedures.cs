using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
               protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE dbo.sp_BorrowBook
    @BookId INT,
    @UserId NVARCHAR(450),
    @LoanDays INT = 14
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    UPDATE dbo.Books
    SET AvailableCopies = AvailableCopies - 1
    WHERE Id = @BookId AND AvailableCopies > 0;

    IF @@ROWCOUNT = 0
    BEGIN
        ROLLBACK TRANSACTION;
        SELECT CAST(0 AS INT) AS Value;
        RETURN;
    END

    INSERT INTO dbo.Loans (BookId, UserId, BorrowedAt, DueDate)
    VALUES (@BookId, @UserId, SYSUTCDATETIME(), DATEADD(DAY, @LoanDays, SYSUTCDATETIME()));

    DECLARE @LoanId INT = CAST(SCOPE_IDENTITY() AS INT);

    COMMIT TRANSACTION;

    SELECT @LoanId AS Value;
END");

            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE dbo.sp_GetOverdueLoans
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        l.Id AS LoanId,
        b.Title AS BookTitle,
        l.UserId,
        u.Email AS UserEmail,
        l.BorrowedAt,
        l.DueDate,
        DATEDIFF(DAY, l.DueDate, SYSUTCDATETIME()) AS DaysOverdue
    FROM dbo.Loans l
    INNER JOIN dbo.Books b ON b.Id = l.BookId
    LEFT JOIN dbo.AspNetUsers u ON u.Id = l.UserId
    WHERE l.ReturnedAt IS NULL AND l.DueDate < SYSUTCDATETIME()
    ORDER BY l.DueDate;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.sp_GetOverdueLoans;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.sp_BorrowBook;");
        }
    }
}
