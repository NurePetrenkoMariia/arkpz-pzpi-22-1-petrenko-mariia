using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Data
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
            modelBuilder.Entity<Farm>()
                 .HasMany(f => f.Workers)  
                 .WithOne(u => u.Farm)  
                 .HasForeignKey(u => u.FarmId) 
                 .OnDelete(DeleteBehavior.NoAction);  

            modelBuilder.Entity<Farm>()
                .HasOne(f => f.Owner) 
                .WithMany()  
                .HasForeignKey(f => f.OwnerId)  
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Animal>()
                 .HasOne(a => a.Stable)
                 .WithMany()
                 .HasForeignKey(a => a.StableId)
                 .OnDelete(DeleteBehavior.NoAction); 

            
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Farm)
                .WithMany() 
                .HasForeignKey(a => a.FarmId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder); 
        }

    }
}
