using DianaShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // Add your entities here
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Kiosk> Kiosks { get; set; }
        public DbSet<KioskAddress> KioskAddresses { get; set; }
        public DbSet<KioskImage> KioskImages { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<KioskCamera> KioskCameras { get; set; }
        public DbSet<SensorData> SensorData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .HasOne(ki => ki.Kiosk)
                .WithMany(sc => sc.Schedules)
                .HasForeignKey(ur => ur.KioskID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Schedule>()
                .HasOne(ur => ur.User)
                .WithMany(sc => sc.Schedules)
                .HasForeignKey(ur => ur.UserID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Schedule>()
                .HasOne(ad => ad.Address)
                .WithMany(sc => sc.Schedules)
                .HasForeignKey(ad => ad.AddressID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductImage>()
                .HasKey(pi => new { pi.ProductId, pi.ImageId });
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId);
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Image)
                .WithMany(i => i.ProductImages)
                .HasForeignKey(pi => pi.ImageId);

            modelBuilder.Entity<KioskImage>()
                .HasKey(ki => new { ki.KioskId, ki.ImageId });
            modelBuilder.Entity<KioskImage>()
                .HasOne(ki => ki.Kiosk)
                .WithMany(p => p.Images)
                .HasForeignKey(ki => ki.KioskId);
            modelBuilder.Entity<KioskImage>()
                .HasOne(ki => ki.Image)
                .WithMany(i => i.KioskImages)
                .HasForeignKey(ki => ki.ImageId);

            //sensor
            modelBuilder.Entity<KioskCamera>()
                .HasKey(kc => kc.Id);
            modelBuilder.Entity<KioskCamera> ()
                .HasOne(kc => kc.Kiosk)
                .WithMany(ki => ki.Cameras)
                .HasForeignKey(kc => kc.KiodID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GPSLocation>()
                .HasKey(lo => lo.Id);
            modelBuilder.Entity<GPSLocation>()
                .HasOne(lo => lo.Kiosk)
                .WithMany(ki => ki.Locations)
                .HasForeignKey(lo => lo.KioskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SensorData>()
                .HasKey(se => se.Id);
            modelBuilder.Entity<SensorData>()
                .HasOne(se => se.Kiosk)
                .WithMany(ki => ki.Sensors)
                .HasForeignKey(se => se.KioskID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartProduct>()
                .HasKey(cp => new { cp.CartId, cp.ProductId });
            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Cart)
                .WithMany(c => c.CartProducts)
                .HasForeignKey(cp => cp.CartId);
            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CartProducts)
                .HasForeignKey(cp => cp.ProductId);

            // Cấu hình Order
            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Kiosk)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.KioskID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.StageStatus)
                .WithMany(ss => ss.Orders)
                .HasForeignKey(o => o.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Voucher)
                .WithMany(v => v.Orders)
                .HasForeignKey(o => o.VoucherID)
                .OnDelete(DeleteBehavior.SetNull);

            // Cấu hình OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => od.Id);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.Details)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Size)
                .WithMany(s => s.Details)
                .HasForeignKey(od => od.SizeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình Payment
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentMethod)
                .WithMany(pm => pm.Payments)
                .HasForeignKey(p => p.PaymentMethodID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.StageStatus)
                .WithMany(ss => ss.Payments)
                .HasForeignKey(p => p.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình PaymentMethod
            modelBuilder.Entity<PaymentMethod>()
                .HasKey(pm => pm.Id);

            modelBuilder.Entity<PaymentMethod>()
                .Property(pm => pm.MethodName)
                .IsRequired()
                .HasMaxLength(100);

            // Cấu hình StageStatus
            modelBuilder.Entity<StageStatus>()
                .HasKey(ss => ss.Id);

            modelBuilder.Entity<StageStatus>()
                .Property(ss => ss.StatusName)
                .IsRequired()
                .HasMaxLength(100);


            modelBuilder.Entity<VnPayTransaction>()
                .HasKey(i => i.TransactionId);

            modelBuilder.Entity<VnPayTransaction>()
                .HasOne(u => u.User)
                .WithMany(u => u.VnPay)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VnPayTransaction>()
                .HasOne(p => p.Payment)
                .WithOne(o => o.VnPayTransaction)
                .HasForeignKey<VnPayTransaction>(p => p.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);


            //SeedData
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Staff" },
                new Role { Id = 3, Name = "Supplier" }
                );

            modelBuilder.Entity<StageStatus>().HasData(
                new StageStatus { Id = 1, StatusName = "Unpaid" },
                new StageStatus { Id = 2, StatusName = "Paid" },
                new StageStatus { Id = 3, StatusName = "Waiting" },
                new StageStatus { Id = 4, StatusName = "Completed" },
                new StageStatus { Id = 5, StatusName = "Cancelled" }
                );

            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { Id = 1, MethodName = "Cash on Delivery" },
                new PaymentMethod { Id = 2, MethodName = "VNPay" }
                );

        }
    }
}


