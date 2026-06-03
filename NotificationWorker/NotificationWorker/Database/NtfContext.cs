using Microsoft.EntityFrameworkCore;
using NotificationWorker.Models;

namespace NotificationWorker.Database
{
    public class NtfContext : DbContext
    {
        public DbSet<NotificationInfo> Notifications { get; set; } = null!;
        public NtfContext(DbContextOptions<NtfContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NotificationInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("UUID");
                entity.Property(e => e.Message).IsRequired(); 
                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20)
                      .IsRequired();
                entity.Property(e => e.SentAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
