using Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Data
{
    public class FarmKeeperDbContext: DbContext
    {
        public FarmKeeperDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Stable> Stables { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                 .HasOne(u => u.Farm) 
                 .WithMany(f => f.Workers) 
                 .HasForeignKey(u => u.FarmId) 
                 .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Farm>()
                .HasOne(f => f.Owner) 
                .WithMany(u => u.Farms)
                .HasForeignKey(f => f.OwnerId)  
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Farm>()
                .HasMany(f => f.Animals) 
                .WithOne(a => a.Farm) 
                .HasForeignKey(a => a.FarmId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Animal>()
                 .HasOne(a => a.Stable)
                 .WithMany()
                 .HasForeignKey(a => a.StableId)
                 .OnDelete(DeleteBehavior.Restrict); 

            var userRoles = new List<UserRole>()
            {
                new UserRole()
                {
                    Id = Guid.Parse("75569a09-16cb-48a6-b563-378a86718344"),
                    Name = "Administrator"
                }, 
                new UserRole()
                {
                    Id = Guid.Parse("87382e3b-8c39-4709-92d1-8e00f123d816"),
                    Name = "Worker"
                }
            };

            modelBuilder.Entity<UserRole>().HasData(userRoles);

            var priorities = new List<Priority>()
            {
                new Priority()
                {
                    Id = Guid.Parse("ef966466-7bad-4efc-aee9-1911ffbfdf90"),
                    Name = "Low"
                },
                new Priority()
                {
                    Id = Guid.Parse("27ac5dfb-58ab-4b65-a27e-68c38dea27b1"),
                    Name = "Medium"
                },
                new Priority()
                {
                    Id = Guid.Parse("e0d4a299-4634-4aed-82e1-d57786541446"),
                    Name = "High"
                },
            };

            modelBuilder.Entity<Priority>().HasData(priorities);

            var statuses = new List<Status>()
            {
                new Status()
                {
                    Id = Guid.Parse("ed807b86-8f12-4b61-916a-3f681ff14bf2"),
                    Name = "Not started"
                },
                new Status()
                {
                    Id = Guid.Parse("503c946a-60c3-4241-be97-2efa2e2a6de0"),
                    Name = "In progress"
                },
                new Status()
                {
                    Id = Guid.Parse("f8edc5db-da63-47cd-beea-f3b35e9198bd"),
                    Name = "Done"
                },
            };

            modelBuilder.Entity<Status>().HasData(statuses);
            base.OnModelCreating(modelBuilder);
        }

    }
}
