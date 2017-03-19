namespace D.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerEnt")]
    public partial class CustomerEnt
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public CustomerEnt()
        //{
        //    Заказ = new HashSet<Заказ>();
        //}

        //[Key]
        //public int ID_клиента { get; set; }

        //public int? УНП_Клиента { get; set; }

        //[StringLength(200)]
        //public string Название_организации { get; set; }

        //[StringLength(20)]
        //public string Телефон { get; set; }

        //[StringLength(200)]
        //public string Адрес { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Заказ> Заказ { get; set; }
    }
}
