namespace D.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Interfaces;

    public partial class Денежное_поступление: IДенежное_поступлениеInterface
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Денежное_поступление()
        {
            Оплата_заказа = new HashSet<Оплата_заказа>();
        }

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
        [Display(Name ="Номер")]
        [Key]
        public int ID_поступления { get; set; }
        [Required]
        public int ID_клиента { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Оплата_заказа> Оплата_заказа { get; set; }

        public virtual Клиент Клиент { get; set; }

        public void AddtoTable(IdbInterface db, IДенежное_поступлениеInterface p)
        {

            db.Денежное_поступление.Add(p as Денежное_поступление);
        }
    }
}
