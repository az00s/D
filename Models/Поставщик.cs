namespace D.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using D.Interfaces;

    public partial class Поставщик: IПоставщикInterface
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Поставщик()
        {
            Поставщик_цена = new HashSet<Поставщик_цена>();
        }

        
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
}
