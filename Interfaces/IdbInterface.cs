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
         DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
         DbSet<AspNetRoles> AspNetRoles { get; set; }
         DbSet<AspNetUsers> AspNetUsers { get; set; }
         DbSet<Денежное_поступление> Денежное_поступление { get; set; }
         DbSet<Заказ> Заказ { get; set; }
         DbSet<CustomerEnt> CustomerEnt { get; set; }
         DbSet<CustomerInd> CustomerInd { get; set; }

        DbSet<Оплата_заказа> Оплата_заказа { get; set; }
         DbSet<Оформление_заказа> Оформление_заказа { get; set; }
         DbSet<Поставщик> Поставщик { get; set; }
         DbSet<Поставщик_цена> Поставщик_цена { get; set; }
         DbSet<Сотрудник> Сотрудник { get; set; }
         DbSet<Товар> Товар { get; set; }

         int SaveChanges();
         DbEntityEntry Entry(object entity);
         void Dispose();
    }

}