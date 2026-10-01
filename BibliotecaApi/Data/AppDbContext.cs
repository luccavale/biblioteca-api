using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Livro> Livros => Set<Livro>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.Property(a => a.Nome).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Nacionalidade).HasMaxLength(60);
        });

        modelBuilder.Entity<Livro>(entity =>
        {
            entity.Property(l => l.Titulo).IsRequired().HasMaxLength(150);
            entity.Property(l => l.Isbn).IsRequired().HasMaxLength(20);

            entity.HasOne(l => l.Autor)
                  .WithMany(a => a.Livros)
                  .HasForeignKey(l => l.AutorId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}