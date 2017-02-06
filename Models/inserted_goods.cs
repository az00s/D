namespace D.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inserted_goods
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ID_товара { get; set; }

        [StringLength(120)]
        public string Наименование { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime Дата_добавления { get; set; }
    }
}
