using Microsoft.EntityFrameworkCore;

namespace OnlineCourseD2N.Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Trainer)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TrainerId);

            modelBuilder.Entity<Users>()
            .HasOne(u => u.TrainerProfile)
            .WithOne(t => t.User)
            .HasForeignKey<Trainer>(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
