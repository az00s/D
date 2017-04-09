using D.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace D.Models
{
    public partial class Денежное_поступление : IДенежное_поступлениеInterface
    {

        [Required]
        [Range(0, 100000000000000.00)]

        [Column(TypeName = "money")]
        public decimal? Сумма { get; set; }

        [Required]

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата поступления")]
        [Column(TypeName = "date")]
        public DateTime? Дата_поступления { get; set; }
        [Display(Name = "Номер")]
        [Key]
        public int ID_поступления { get; set; }
        [Required]
        public int ID_клиента { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Оплата_заказа> Оплата_заказа { get; set; }

        public virtual CustomerEnt CustomerEnt { get; set; }

        public void AddtoTable(IdbInterface db, IДенежное_поступлениеInterface p)
        {

            db.Денежное_поступление.Add(p as Денежное_поступление);
        }
    }

    public partial class Заказ : IЗаказInterface
    {

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата заказа")]
        [Column(TypeName = "date")]
        public DateTime? Дата_заказа { get; set; }

        [Display(Name = "Сумма заказа с НДС")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]

        [Column(TypeName = "money")]
        public decimal? Сумма_заказа_с_НДС { get; set; }

        [Display(Name = "Номер заказа")]

        [Key]
        public int ID_заказа { get; set; }
        [Display(Name = "ID клиента")]
        public int ID_клиента { get; set; }

        [Display(Name = "Табельный номер")]

        public int? Табельный_номер { get; set; }

        [Display(Name = "Статус заказа")]
        [StringLength(30)]
        public string Статус_заказа { get; set; }

        //[Column(TypeName = "money")]
        //[DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        //public decimal? Получено { get; set; }

        public virtual CustomerInd CustomerInd { get; set; }
        public virtual CustomerEnt CustomerEnt { get; set; }

        public virtual Сотрудник Сотрудник { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Оплата_заказа> Оплата_заказа { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Оформление_заказа> Оформление_заказа { get; set; }

        public void AddtoTable(IdbInterface db, IЗаказInterface p)
        {

            db.Заказ.Add(p as Заказ);
        }
    }

    public partial class CustomerEnt : IКлиентInterface
    {

        [Key]
        public int ID_клиента { get; set; }



        [Range(0, 999999999)]
        //[StringLength(9)]
        [Display(Name = "УНП клиента")]
        public int? УНП_Клиента { get; set; }

        [Display(Name = "Название организации")]

        [StringLength(200)]
        public string Название_организации { get; set; }

        [StringLength(20)]
        public string Телефон { get; set; }

        [StringLength(200)]
        public string Адрес { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Заказ> Заказ { get; set; }

        public void AddtoTable(IdbInterface db, IКлиентInterface p)
        {

            db.CustomerEnt.Add(p as CustomerEnt);
        }
    }

    public partial class Оплата_заказа : IОплата_заказаInterface
    {

        [Key]
        public int ID { get; set; }

        [Column(TypeName = "money")]
        public decimal? Сумма { get; set; }


        //[Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_поступления { get; set; }


        //[Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_клиента { get; set; }


        //[Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_заказа { get; set; }
        public virtual Денежное_поступление Денежное_поступление { get; set; }

        public virtual Заказ Заказ { get; set; }

        public void AddtoTable(IdbInterface db, IОплата_заказаInterface p)
        {

            db.Оплата_заказа.Add(p as Оплата_заказа);
        }
    }

    public partial class Оформление_заказа : IОформление_заказаInterface
    {
        public int ID_товара { get; set; }

        public int ID_заказа { get; set; }

        public int Количество { get; set; }

        public int ID { get; set; }

        public virtual Заказ Заказ { get; set; }

        public virtual Товар Товар { get; set; }
        public void AddtoTable(IdbInterface db, IОформление_заказаInterface p)
        {

            db.Оформление_заказа.Add(p as Оформление_заказа);
        }
    }

    public partial class Пользователь
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string login { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }
    }

    public partial class Поставщик : IПоставщикInterface
    {


        [Display(Name = "УНП Поставщика")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(0, 999999999)]
        public int УНП_поставщика { get; set; }

        [Display(Name = "Название организации")]
        [StringLength(200)]
        public string Название_организации { get; set; }

        [StringLength(200)]
        public string Адрес { get; set; }

        [StringLength(20)]
        public string Телефон { get; set; }

        [StringLength(200)]
        public string Описание { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Поставщик_цена> Поставщик_цена { get; set; }

        public void AddtoTable(IdbInterface db, IПоставщикInterface p)
        {

            db.Поставщик.Add(p as Поставщик);
        }
    }

    public partial class Поставщик_цена : IПоставщик_ценаInterface
    {
        //private IdbInterface db;
        //public Поставщик_цена() { }
        //public Поставщик_цена(IdbInterface dbParam) { db = dbParam; }
        [Column(TypeName = "money")]
        public decimal? Оптовая_цена { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_товара { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "УНП Поставщика")]
        public int УНП_поставщика { get; set; }

        public virtual Поставщик Поставщик { get; set; }

        public virtual Товар Товар { get; set; }

        public void AddtoTable(IdbInterface db, IПоставщик_ценаInterface p)
        {

            db.Поставщик_цена.Add(p as Поставщик_цена);
        }
    }

    public partial class Сотрудник : IСотрудникInterface
    {

        [Display(Name = "Табельный номер")]
        [Key]
        public int Табельный_номер { get; set; }

        [StringLength(20)]
        public string Фамилия { get; set; }

        [StringLength(20)]
        public string Имя { get; set; }

        [StringLength(20)]
        public string Отчество { get; set; }

        [StringLength(200)]
        public string Должность { get; set; }
        [DataType(DataType.PhoneNumber)]
        [StringLength(20)]
        public string Телефон { get; set; }

        [StringLength(250)]
        public string Адрес { get; set; }

        [StringLength(50)]
        public string Номер_паспорта { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = " Дата рождения")]
        [Column(TypeName = "date")]
        public DateTime? Дата_рождения { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Заказ> Заказ { get; set; }

        public void AddtoTable(IdbInterface db, IСотрудникInterface p)
        {

            db.Сотрудник.Add(p as Сотрудник);
        }
    }

    public partial class Товар : IТоварInterface
    {
        

        [Key]
        public int ID_товара { get; set; }

        [StringLength(50)]
        public string Обозначение { get; set; }
        [Required]
        [StringLength(120)]
        public string Наименование { get; set; }

        [Display(Name = "Краткое описание")]
        [StringLength(300)]
        public string Краткое_описание { get; set; }

        [Display(Name = "Ед.изм.")]
        [StringLength(20)]
        public string Единица_измерения { get; set; }

        [Range(0, 2147483647)]
        [Required]
        [Display(Name = "Остаток")]
        public int Остаток_на_складе { get; set; }

        [Range(0, 2147483647)]

        [Display(Name = "Поставка,\nдней")]
        public int? Срок_поставки { get; set; }

        [Range(0, 2147483647)]

        [Display(Name = "Вес,\nг")]
        public int? Вес { get; set; }

        [Range(0, 100000000000000.00)]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal? Цена { get; set; }




        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = " Дата регистрации")]
        [Column(TypeName = "date")]
        public DateTime? RegistrationDate { get; set; }








        [Range(0, 100000000000000.00)]

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Цена\nс НДС")]
        [Column(TypeName = "money")]
        public decimal? Цена_с_НДС { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Оформление_заказа> Оформление_заказа { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Поставщик_цена> Поставщик_цена { get; set; }

        public void AddtoTable(IdbInterface db, IТоварInterface p)
        {

            db.Товар.Add(p as Товар);
        }


    }

    public partial class CustomerInd
    {
        
        [Required]
        [StringLength(100)]
        public string PassportId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string Patronymic { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(250)]
        public string Adress { get; set; }

        [Display(Name ="Телефон")]
        [Required]
        [StringLength(20)]
        public string Telephone { get; set; }

        [Required]
        [StringLength(150)]
        public string Email { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date)]
        
        public DateTime BirstDate { get; set; }

        public DateTime RegisteredDate { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public int CustomerIndId { get; set; }

        public virtual ICollection<Заказ> Заказ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerInd()
        {
            Заказ = new HashSet<Заказ>();
        }

    }
}