namespace D.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class db : DbContext, IdbInterface
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
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Денежное_поступление> Денежное_поступление { get; set; }
        public virtual DbSet<Заказ> Заказ { get; set; }
        public virtual DbSet<Клиент> Клиент { get; set; }
        public virtual DbSet<Оплата_заказа> Оплата_заказа { get; set; }
        public virtual DbSet<Оформление_заказа> Оформление_заказа { get; set; }
        public virtual DbSet<Пользователь> Пользователь { get; set; }
        public virtual DbSet<Поставщик> Поставщик { get; set; }
        public virtual DbSet<Поставщик_цена> Поставщик_цена { get; set; }
        public virtual DbSet<Сотрудник> Сотрудник { get; set; }
        public virtual DbSet<Товар> Товар { get; set; }
        public virtual DbSet<deleted_client> deleted_client { get; set; }
        public virtual DbSet<inserted_goods> inserted_goods { get; set; }

        

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

            modelBuilder.Entity<Денежное_поступление>()
                .Property(e => e.Сумма)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Денежное_поступление>()
                .HasMany(e => e.Оплата_заказа)
                .WithRequired(e => e.Денежное_поступление)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Заказ>()
                .Property(e => e.Сумма_заказа_с_НДС)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Заказ>()
                .Property(e => e.Статус_заказа)
                .IsUnicode(false);

            //modelBuilder.Entity<Заказ>()
            //    .Property(e => e.Получено)
            //    .HasPrecision(19, 4);

            modelBuilder.Entity<Заказ>()
                .HasMany(e => e.Оплата_заказа)
                .WithRequired(e => e.Заказ)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Клиент>()
                .Property(e => e.Номер_паспорта)
                .IsUnicode(false);

            modelBuilder.Entity<Клиент>()
                .Property(e => e.Название_организации)
                .IsUnicode(false);

            modelBuilder.Entity<Клиент>()
                .Property(e => e.Телефон)
                .IsUnicode(false);

            modelBuilder.Entity<Клиент>()
                .Property(e => e.Адрес)
                .IsUnicode(false);

            modelBuilder.Entity<Клиент>()
                .HasMany(e => e.Заказ)
                .WithRequired(e => e.Клиент)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Оплата_заказа>()
                .Property(e => e.Сумма)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Поставщик>()
                .Property(e => e.Название_организации)
                .IsUnicode(false);

            modelBuilder.Entity<Поставщик>()
                .Property(e => e.Адрес)
                .IsUnicode(false);

            modelBuilder.Entity<Поставщик>()
                .Property(e => e.Телефон)
                .IsUnicode(false);

            modelBuilder.Entity<Поставщик>()
                .Property(e => e.Описание)
                .IsUnicode(false);

            modelBuilder.Entity<Поставщик>()
                .HasMany(e => e.Поставщик_цена)
                .WithRequired(e => e.Поставщик)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Поставщик_цена>()
                .Property(e => e.Оптовая_цена)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Сотрудник>()
                .Property(e => e.Фамилия)
                .IsUnicode(false);

            modelBuilder.Entity<Сотрудник>()
                .Property(e => e.Имя)
                .IsUnicode(false);

            modelBuilder.Entity<Сотрудник>()
                .Property(e => e.Отчество)
                .IsUnicode(false);

            modelBuilder.Entity<Сотрудник>()
                .Property(e => e.Должность)
                .IsUnicode(false);

            modelBuilder.Entity<Сотрудник>()
                .Property(e => e.Телефон)
                .IsUnicode(false);

            modelBuilder.Entity<Товар>()
                .Property(e => e.Наименование)
                .IsUnicode(false);

            modelBuilder.Entity<Товар>()
                .Property(e => e.Краткое_описание)
                .IsUnicode(false);

            modelBuilder.Entity<Товар>()
                .Property(e => e.Цена)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Товар>()
                .Property(e => e.Единица_измерения)
                .IsUnicode(false);

            modelBuilder.Entity<Товар>()
                .Property(e => e.Цена_с_НДС)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Товар>()
                .HasMany(e => e.Оформление_заказа)
                .WithRequired(e => e.Товар)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<deleted_client>()
                .Property(e => e.ФИО_клиента)
                .IsUnicode(false);

            modelBuilder.Entity<deleted_client>()
                .Property(e => e.Название_организации)
                .IsUnicode(false);

            modelBuilder.Entity<inserted_goods>()
                .Property(e => e.ID_товара)
                .IsUnicode(false);

            modelBuilder.Entity<inserted_goods>()
                .Property(e => e.Наименование)
                .IsUnicode(false);
        }

        Task IdbInterface.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
