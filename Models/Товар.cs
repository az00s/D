namespace D.Models
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Товар: IТоварInterface
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Товар()
        {
            Оформление_заказа = new HashSet<Оформление_заказа>();
            Поставщик_цена = new HashSet<Поставщик_цена>();
        }

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
}
