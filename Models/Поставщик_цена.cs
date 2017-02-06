namespace D.Models
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Поставщик_цена: IПоставщик_ценаInterface
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
}
