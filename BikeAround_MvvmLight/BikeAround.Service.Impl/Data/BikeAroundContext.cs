using System.Data.Entity;

namespace BikeAround.Service.Impl.Data
{
    public class BikeAroundContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Bike> Bikes { get; set; }

        public DbSet<Trip> Trips { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bike>()
                .HasRequired(v => v.OwnerUser)
                .WithMany(u => u.OwnedBikes)
                .HasForeignKey(v => v.OwnerUserID)
                .WillCascadeOnDelete(false);
        }
    }
}