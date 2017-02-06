namespace D.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using D.Interfaces;
    public partial class Сотрудник: IСотрудникInterface
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Сотрудник()
        {
            Заказ = new HashSet<Заказ>();
        }

        [Display(Name = "Табельный номер")]
        [Key]
        public int Табельный_номер { get; set; }

        [StringLength(20)]
        public string Фамилия { get; set; }

        [StringLength(20)]
        public string Имя { get; set; }

        [StringLength(20)]
        public string Отчество { get; set; }

        [StringLength(200)]
        public string Должность { get; set; }
        [DataType(DataType.PhoneNumber)]
        [StringLength(20)]
        public string Телефон { get; set; }

        [StringLength(250)]
        public string Адрес { get; set; }

        [StringLength(50)]
        public string Номер_паспорта { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = " Дата рождения")]
        [Column(TypeName = "date")]
        public DateTime? Дата_рождения { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Заказ> Заказ { get; set; }

        public void AddtoTable(IdbInterface db, IСотрудникInterface p)
        {

            db.Сотрудник.Add(p as Сотрудник);
        }
    }
}
