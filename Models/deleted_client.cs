namespace D.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class deleted_client
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_клиента { get; set; }

        public int? УНП_клиента { get; set; }

        [StringLength(200)]
        public string ФИО_клиента { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime Дата_удаления { get; set; }

        [StringLength(200)]
        public string Название_организации { get; set; }
    }
}
