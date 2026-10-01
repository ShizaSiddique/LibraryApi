 using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(b => b.Isbn)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(b => b.Isbn)
            .IsUnique();

        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Books_Copies",
            "[AvailableCopies] >= 0 AND [AvailableCopies] <= [TotalCopies]"));

        builder.HasData(
            new Book { Id = 1, Title = "Clean Code", Isbn = "978-0132350884", AuthorId = 1, TotalCopies = 5, AvailableCopies = 5 },
            new Book { Id = 2, Title = "The Pragmatic Programmer", Isbn = "978-0201616224", AuthorId = 2, TotalCopies = 3, AvailableCopies = 3 });
    }
}