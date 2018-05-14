using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace D.Models
{
    public partial class MoneyReceipt 
    {

        [Required]
        [Display(Name = "Сумма")]
        [Column(TypeName = "money")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Пожалуйста, введите положительное значение для цены")]
        public decimal? Amount { get; set; }

        [Required]

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата поступления")]
        [Column(TypeName = "date")]
        public DateTime? ReceiptDate { get; set; }
        [Display(Name = "Номер")]
        [Key]
        public int ReceiptID { get; set; }
        [Required]
        public int ClientID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<OrderPayment> OrderPayments { get; set; }

        public  virtual CustomerEnt CustomerEnt { get; set; }

    }
    
    public partial class Order 
    {

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата заказа")]
        [Column(TypeName = "date")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Сумма заказа с НДС")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]

        [Column(TypeName = "money")]
        public decimal? AmountVat { get; set; }

        [Display(Name = "Номер заказа")]

        [Key]
        public int OrderID { get; set; }
        [Display(Name = "ID клиента")]
        public int? ClientID { get; set; }
        public int? CustomerIndId { get; set; }

        [Display(Name = "Табельный номер")]

        public int? PersonnelNumber { get; set; }

        [Display(Name = "Статус заказа")]
        [StringLength(30)]
        public string OrderStatus { get; set; }

        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? MoneyReceived { get; set; }
        
        public  CustomerInd CustomerInd { get; set; }
        
        public virtual CustomerEnt CustomerEnt { get; set; }
        
        public Employee Employee { get; set; }

        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<OrderPayment> OrderPayments { get; set; }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Ordering> Orderings { get; set; }






    }
    
    public partial class CustomerEnt 
    {

        [Key]
        public int ClientID { get; set; }



        [Range(0, int.MaxValue)]
        //[StringLength(9)]
        [Display(Name = "УНП клиента")]
        public int? ClientPAN { get; set; }

        [Display(Name = "Название организации")]

        [StringLength(200)]
        public string Name { get; set; }
        [Display(Name = "Телефон")]
        [StringLength(20)]
        public string Telephone { get; set; }
        [Display(Name = "Адрес")]
        [StringLength(200)]
        public string Address { get; set; }


        [ScriptIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Order> Orders { get; set; }
        //[NonSerialized]
        //public ICollection<Order> orders;
    }

    public partial class OrderPayment 
    {

        [Key]
        public int ID { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }


        //[Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReceiptID { get; set; }


        //[Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClientID { get; set; }


        //[Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }
        public virtual MoneyReceipt MoneyReceipt { get; set; }

        public virtual Order Order { get; set; }

    }

    public partial class Ordering 
    {
        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public int ProductQuantity { get; set; }

        public int ID { get; set; }

        public  Order Order { get; set; }

        public  Product Product { get; set; }
    }

    //public partial class Employee
    //{
    //    public int Id { get; set; }

    //    [Required]
    //    [StringLength(50)]
    //    public string login { get; set; }

    //    [Required]
    //    [StringLength(50)]
    //    public string password { get; set; }
    //}

    public partial class Supplier 
    {


        [Display(Name = "УНП Поставщика")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(0, int.MaxValue)]
        public int SupplierPAN { get; set; }

        [Display(Name = "Название организации")]
        [StringLength(200)]
        public string Name { get; set; }
        [Display(Name="Адрес")]
        [StringLength(200)]
        public string Address { get; set; }
        [Display(Name = "Телефон")]
        [StringLength(20)]
        public string Telephone { get; set; }
        [Display(Name="Описание")]
        [StringLength(200)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<SupplierPrice> SupplierPrices { get; set; }

    }

    public partial class SupplierPrice 
    {
        
        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "УНП Поставщика")]
        public int SupplierPAN { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Product Product { get; set; }

    }
    [Serializable]
    public partial class Employee 
    {

        [Display(Name = "Табельный номер")]
        [Key]
        public int PersonnelNumber { get; set; }
        [Display(Name = "Фамилия")]
        [StringLength(20)]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        [StringLength(20)]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        [StringLength(20)]
        public string Patronymic { get; set; }
        [Display(Name = "Должность")]
        [StringLength(200)]
        public string Position { get; set; }
        [Display(Name = "Телефон")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20)]
        public string Telephone { get; set; }
        [Display(Name = "Адрес")]
        [StringLength(250)]
        public string Address { get; set; }
        [Display(Name = "Паспорт")]
        [StringLength(50)]
        public string PassportID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = " Дата рождения")]
        [Column(TypeName = "date")]
        public DateTime? BirstDate { get; set; }
        [ScriptIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<Order> Orders { get; set; }
        
        
    }

    public partial class Product 
    {

        [Display(Name = "#")]
        [Key]
        public int ProductID { get; set; }
        [Display(Name = "Обозначение")]
        [StringLength(50)]
        public string Designation { get; set; }
        [Display(Name = "Наименование")]
        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        [Display(Name = "Краткое описание")]
        [StringLength(300)]
        public string Description { get; set; }

        [Display(Name = "Ед.изм.")]
        [StringLength(20)]
        public string Unit_of_measurement { get; set; }

        [Range(0, int.MaxValue)]
        [Required]
        [Display(Name = "Остаток")]
        public int Balance { get; set; }

        [Range(0, int.MaxValue)]

        [Display(Name = "Поставка,\nдней")]
        public int? Delivery_time { get; set; }

        [Range(0, int.MaxValue)]

        [Display(Name = "Вес,\nг")]
        public int? Weight { get; set; }
        [Display(Name = "Цена")]
        [Range(0.01,double.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal? Price { get; set; }




        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = " Дата регистрации")]
        [Column(TypeName = "date")]
        public DateTime? RegistrationDate { get; set; }








        [Range(0, 100000000000000.00)]

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        [Display(Name = "Цена\nс НДС")]
        [Column(TypeName = "money")]
        public decimal? Price_with_vat { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<Ordering> Orderings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  ICollection<SupplierPrice> SupplierPrices { get; set; }



    }
    [Serializable]
    public partial class CustomerInd 
    {
        [Display(Name ="Идентификационный номер")]
        [Required]
        [StringLength(100)]
        public string PassportId { get; set; }
        [Display(Name = "Имя")]
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        [StringLength(100)]
        public string Patronymic { get; set; }
        [Display(Name = "Фамилия")]
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Display(Name = "Адрес")]
        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        [Display(Name ="Телефон")]
        [Required]
        [StringLength(20)]
        public string Telephone { get; set; }

        [Required]
        [StringLength(150)]
        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = " Дата рождения")]
        [Column(TypeName = "date")]
        public DateTime? BirstDate { get; set; }

        //public string Birst { get; set; }
        //[Column(TypeName = "datetime")]
        
        public DateTime RegisteredDate { get; set; }
        [Display(Name = "Примечание")]
        [StringLength(300)]
        public string Description { get; set; }

        public int CustomerIndId { get; set; }
        [ScriptIgnore]
        public /*virtual*/ ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerInd()
        {
            Orders = new HashSet<Order>();
        }

        

       
    }
}