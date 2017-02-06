namespace D.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Interfaces;

    public partial class Заказ: IЗаказInterface
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Заказ()
        {
            Оплата_заказа = new HashSet<Оплата_заказа>();
            Оформление_заказа = new HashSet<Оформление_заказа>();
        }

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

        [Display(Name ="Статус заказа")]
        [StringLength(30)]
        public string Статус_заказа { get; set; }

        //[Column(TypeName = "money")]
        //[DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        //public decimal? Получено { get; set; }

        public virtual Клиент Клиент { get; set; }

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
}
