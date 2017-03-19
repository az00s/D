namespace D.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Товар
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Товар()
        //{
        //    Оформление_заказа = new HashSet<Оформление_заказа>();
        //    Поставщик_цена = new HashSet<Поставщик_цена>();
        //}

        //[Key]
        //public int ID_товара { get; set; }

        //[Required]
        //[StringLength(120)]
        //public string Наименование { get; set; }

        //[StringLength(300)]
        //public string Краткое_описание { get; set; }

        //[Column(TypeName = "money")]
        //public decimal? Цена { get; set; }

        //public int Остаток_на_складе { get; set; }

        //[StringLength(20)]
        //public string Единица_измерения { get; set; }

        //public int? Срок_поставки { get; set; }

        //public int? Вес { get; set; }

        //[Column(TypeName = "money")]
        //public decimal? Цена_с_НДС { get; set; }

        //[StringLength(50)]
        //public string Обозначение { get; set; }

        //public DateTime? RegistrationDate { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Оформление_заказа> Оформление_заказа { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Поставщик_цена> Поставщик_цена { get; set; }
    }
}
