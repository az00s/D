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
         DbSet<AspNetRole> AspNetRoles { get; set; }
         DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
         DbSet<AspNetUser> AspNetUsers { get; set; }
         DbSet<sysdiagram> sysdiagrams { get; set; }
         DbSet<Денежное_поступление> Денежное_поступление { get; set; }
         DbSet<Заказ> Заказ { get; set; }
         DbSet<Клиент> Клиент { get; set; }
         DbSet<Оплата_заказа> Оплата_заказа { get; set; }
         DbSet<Оформление_заказа> Оформление_заказа { get; set; }
         DbSet<Пользователь> Пользователь { get; set; }
         DbSet<Поставщик> Поставщик { get; set; }
         DbSet<Поставщик_цена> Поставщик_цена { get; set; }
         DbSet<Сотрудник> Сотрудник { get; set; }
         DbSet<Товар> Товар { get; set; }
         DbSet<deleted_client> deleted_client { get; set; }
         DbSet<inserted_goods> inserted_goods { get; set; }

         int SaveChanges();
         DbEntityEntry Entry(object entity);
         void Dispose();
        Task SaveChangesAsync();
    }

}