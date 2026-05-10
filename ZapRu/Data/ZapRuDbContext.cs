using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZapRu.Entities;

namespace ZapRu.Data
{
    public class ZapRuDbContext : DbContext
    {
        public ZapRuDbContext(DbContextOptions<ZapRuDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasOne(t => t.Buyer)
                     .WithMany(u => u.BuyerTransactions)
                     .HasForeignKey(t => t.BuyerID)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Seller)
                    .WithMany(u => u.SellerTransactions)
                    .HasForeignKey(t => t.SellerID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Part)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(t => t.PartID)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasOne(b => b.Buyer)
                    .WithMany()
                    .HasForeignKey(b => b.BuyerID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Seller)
                    .WithMany()
                    .HasForeignKey(b => b.SellerID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.User)
                    .WithMany()
                    .HasForeignKey(n => n.UserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.FromUser)
                    .WithMany()
                    .HasForeignKey(n => n.FromUserID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<PartsCatalog> PartsCatalogs { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<PriceEstimate> PriceEstimates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Bid> Bids { get; set; }

    }
}



