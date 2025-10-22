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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Trainer)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TrainerId);
        }
    }
}
