namespace D.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using D.Interfaces;

    public partial class Клиент: IКлиентInterface
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Клиент()
        {
            Заказ = new HashSet<Заказ>();
        }

        [Key]
        public int ID_клиента { get; set; }

        [Display(Name = "Номер паспорта")]
        [StringLength(20)]
        public string Номер_паспорта { get; set; }
        
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

        [StringLength(200)]
        public string Фамилия { get; set; }

        [StringLength(200)]
        public string Имя { get; set; }

        [StringLength(200)]
        public string Отчество { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Заказ> Заказ { get; set; }

        public void AddtoTable(IdbInterface db, IКлиентInterface p)
        {

            db.Клиент.Add(p as Клиент);
        }
    }
}
