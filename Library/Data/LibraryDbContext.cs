using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options): DbContext(options)
{
    public DbSet<BookClass> Books => Set<BookClass>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookClass>(entity =>
        {
        entity.HasKey(book => book.ISBN);
        entity.Property(book => book.ISBN).ValueGeneratedNever();
        });
    }
}
