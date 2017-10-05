using D.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace D.Models
{
    public interface IdbInterface
    {
         //DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
         DbSet<AspNetRole> AspNetRoles { get; set; }
         DbSet<AspNetUser> AspNetUsers { get; set; }
         DbSet<MoneyReceipt> MoneyReceipts { get; set; }
         DbSet<Order> Orders { get; set; }
         DbSet<CustomerEnt> CustomerEnts { get; set; }
         DbSet<CustomerInd> CustomerInds { get; set; }

        DbSet<OrderPayment> OrderPayments { get; set; }
         DbSet<Ordering> Orderings { get; set; }
         DbSet<Supplier> Suppliers { get; set; }
         DbSet<SupplierPrice> SupplierPrices { get; set; }
         DbSet<Employee> Employees { get; set; }
         DbSet<Product> Products { get; set; }

         int SaveChanges();
         DbEntityEntry Entry(object entity);
         void Dispose();
    }

}