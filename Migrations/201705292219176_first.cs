namespace D.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        LoggedIn = c.Boolean(),
                        SessionId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.__MigrationHistory",
                c => new
                    {
                        MigrationId = c.String(nullable: false, maxLength: 150),
                        ContextKey = c.String(nullable: false, maxLength: 300),
                        Model = c.Binary(nullable: false),
                        ProductVersion = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => new { t.MigrationId, t.ContextKey });
            
            CreateTable(
                "dbo.CustomerEnt",
                c => new
                    {
                        ID_клиента = c.Int(nullable: false, identity: true),
                        УНП_Клиента = c.Int(),
                        Название_организации = c.String(maxLength: 200, unicode: false),
                        Телефон = c.String(maxLength: 20, unicode: false),
                        Адрес = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.ID_клиента);
            
            CreateTable(
                "dbo.Заказ",
                c => new
                    {
                        ID_заказа = c.Int(nullable: false, identity: true),
                        Дата_заказа = c.DateTime(storeType: "date"),
                        Сумма_заказа_с_НДС = c.Decimal(storeType: "money"),
                        ID_клиента = c.Int(nullable: false),
                        Табельный_номер = c.Int(),
                        Статус_заказа = c.String(maxLength: 30, unicode: false),
                        Получено = c.Decimal(storeType: "money"),
                    })
                .PrimaryKey(t => t.ID_заказа)
                .ForeignKey("dbo.CustomerInd", t => t.ID_клиента)
                .ForeignKey("dbo.Сотрудник", t => t.Табельный_номер)
                .ForeignKey("dbo.CustomerEnt", t => t.ID_клиента)
                .Index(t => t.ID_клиента)
                .Index(t => t.Табельный_номер);
            
            CreateTable(
                "dbo.CustomerInd",
                c => new
                    {
                        CustomerIndId = c.Int(nullable: false, identity: true),
                        PassportId = c.String(nullable: false, maxLength: 100),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        Patronymic = c.String(maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Adress = c.String(nullable: false, maxLength: 250),
                        Telephone = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 150),
                        BirstDate = c.DateTime(storeType: "date"),
                        RegisteredDate = c.DateTime(nullable: false),
                        Description = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.CustomerIndId);
            
            CreateTable(
                "dbo.Оплата_заказа",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Сумма = c.Decimal(storeType: "money"),
                        ID_поступления = c.Int(nullable: false),
                        ID_клиента = c.Int(nullable: false),
                        ID_заказа = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Денежное_поступление", t => t.ID_поступления, cascadeDelete: true)
                .ForeignKey("dbo.Заказ", t => t.ID_заказа)
                .Index(t => t.ID_поступления)
                .Index(t => t.ID_заказа);
            
            CreateTable(
                "dbo.Денежное_поступление",
                c => new
                    {
                        ID_поступления = c.Int(nullable: false, identity: true),
                        Сумма = c.Decimal(nullable: false, storeType: "money"),
                        Дата_поступления = c.DateTime(nullable: false, storeType: "date"),
                        ID_клиента = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID_поступления)
                .ForeignKey("dbo.CustomerEnt", t => t.ID_клиента, cascadeDelete: true)
                .Index(t => t.ID_клиента);
            
            CreateTable(
                "dbo.Оформление_заказа",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ID_товара = c.Int(nullable: false),
                        ID_заказа = c.Int(nullable: false),
                        Количество = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Заказ", t => t.ID_заказа, cascadeDelete: true)
                .ForeignKey("dbo.Товар", t => t.ID_товара)
                .Index(t => t.ID_товара)
                .Index(t => t.ID_заказа);
            
            CreateTable(
                "dbo.Товар",
                c => new
                    {
                        ID_товара = c.Int(nullable: false, identity: true),
                        Обозначение = c.String(maxLength: 50),
                        Наименование = c.String(nullable: false, maxLength: 120, unicode: false),
                        Краткое_описание = c.String(maxLength: 300, unicode: false),
                        Единица_измерения = c.String(maxLength: 20, unicode: false),
                        Остаток_на_складе = c.Int(nullable: false),
                        Срок_поставки = c.Int(),
                        Вес = c.Int(),
                        Цена = c.Decimal(storeType: "money"),
                        RegistrationDate = c.DateTime(storeType: "date"),
                        Цена_с_НДС = c.Decimal(storeType: "money"),
                    })
                .PrimaryKey(t => t.ID_товара);
            
            CreateTable(
                "dbo.Поставщик_цена",
                c => new
                    {
                        ID_товара = c.Int(nullable: false),
                        УНП_поставщика = c.Int(nullable: false),
                        Оптовая_цена = c.Decimal(storeType: "money"),
                    })
                .PrimaryKey(t => new { t.ID_товара, t.УНП_поставщика })
                .ForeignKey("dbo.Поставщик", t => t.УНП_поставщика)
                .ForeignKey("dbo.Товар", t => t.ID_товара, cascadeDelete: true)
                .Index(t => t.ID_товара)
                .Index(t => t.УНП_поставщика);
            
            CreateTable(
                "dbo.Поставщик",
                c => new
                    {
                        УНП_поставщика = c.Int(nullable: false),
                        Название_организации = c.String(maxLength: 200, unicode: false),
                        Адрес = c.String(maxLength: 200, unicode: false),
                        Телефон = c.String(maxLength: 20, unicode: false),
                        Описание = c.String(maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.УНП_поставщика);
            
            CreateTable(
                "dbo.Сотрудник",
                c => new
                    {
                        Табельный_номер = c.Int(nullable: false, identity: true),
                        Фамилия = c.String(maxLength: 20, unicode: false),
                        Имя = c.String(maxLength: 20, unicode: false),
                        Отчество = c.String(maxLength: 20, unicode: false),
                        Должность = c.String(maxLength: 200, unicode: false),
                        Телефон = c.String(maxLength: 20, unicode: false),
                        Адрес = c.String(maxLength: 250),
                        Номер_паспорта = c.String(maxLength: 50),
                        Дата_рождения = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.Табельный_номер);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Заказ", "ID_клиента", "dbo.CustomerEnt");
            DropForeignKey("dbo.Заказ", "Табельный_номер", "dbo.Сотрудник");
            DropForeignKey("dbo.Поставщик_цена", "ID_товара", "dbo.Товар");
            DropForeignKey("dbo.Поставщик_цена", "УНП_поставщика", "dbo.Поставщик");
            DropForeignKey("dbo.Оформление_заказа", "ID_товара", "dbo.Товар");
            DropForeignKey("dbo.Оформление_заказа", "ID_заказа", "dbo.Заказ");
            DropForeignKey("dbo.Оплата_заказа", "ID_заказа", "dbo.Заказ");
            DropForeignKey("dbo.Оплата_заказа", "ID_поступления", "dbo.Денежное_поступление");
            DropForeignKey("dbo.Денежное_поступление", "ID_клиента", "dbo.CustomerEnt");
            DropForeignKey("dbo.Заказ", "ID_клиента", "dbo.CustomerInd");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.Поставщик_цена", new[] { "УНП_поставщика" });
            DropIndex("dbo.Поставщик_цена", new[] { "ID_товара" });
            DropIndex("dbo.Оформление_заказа", new[] { "ID_заказа" });
            DropIndex("dbo.Оформление_заказа", new[] { "ID_товара" });
            DropIndex("dbo.Денежное_поступление", new[] { "ID_клиента" });
            DropIndex("dbo.Оплата_заказа", new[] { "ID_заказа" });
            DropIndex("dbo.Оплата_заказа", new[] { "ID_поступления" });
            DropIndex("dbo.Заказ", new[] { "Табельный_номер" });
            DropIndex("dbo.Заказ", new[] { "ID_клиента" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Сотрудник");
            DropTable("dbo.Поставщик");
            DropTable("dbo.Поставщик_цена");
            DropTable("dbo.Товар");
            DropTable("dbo.Оформление_заказа");
            DropTable("dbo.Денежное_поступление");
            DropTable("dbo.Оплата_заказа");
            DropTable("dbo.CustomerInd");
            DropTable("dbo.Заказ");
            DropTable("dbo.CustomerEnt");
            DropTable("dbo.__MigrationHistory");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetRoles");
        }
    }
}
