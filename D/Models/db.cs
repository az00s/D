namespace D.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class db : DbContext,IdbInterface
    {
        public db()
            : base("name=db")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<CustomerEnt> CustomerEnts { get; set; }
        public virtual DbSet<CustomerInd> CustomerInds { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<MoneyReceipt> MoneyReceipts { get; set; }
        public virtual DbSet<Ordering> Orderings { get; set; }
        public virtual DbSet<OrderPayment> OrderPayments { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SupplierPrice> SupplierPrices { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<CustomerEnt>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerEnt>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerEnt>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Patronymic)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<MoneyReceipt>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderPayment>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Order>()
                .Property(e => e.AmountVat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Order>()
                .Property(e => e.OrderStatus)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.MoneyReceived)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Orderings)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderPayments)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .Property(e => e.Unit_of_measurement)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price_with_vat)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Orderings)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.SupplierPrices)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SupplierPrice>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.SupplierPrices)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);
        }
    }
}
