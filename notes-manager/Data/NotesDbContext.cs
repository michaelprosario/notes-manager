using Microsoft.EntityFrameworkCore;
using NotesManager.Models;

namespace NotesManager.Data
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraint on NoteName
            modelBuilder.Entity<Note>()
                .HasIndex(n => n.NoteName)
                .IsUnique();

            // Configure index on LastModified for sorting
            modelBuilder.Entity<Note>()
                .HasIndex(n => n.LastModified);
        }
    }
}
