namespace D.Models
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerInd")]
    public partial class CustomerInd: ICustomerIndInterface
    {
        //[Required]
        //[StringLength(100)]
        //public string PassportId { get; set; }

        //[Required]
        //[StringLength(100)]
        //public string FirstName { get; set; }

        //[StringLength(100)]
        //public string Patronymic { get; set; }

        //[Required]
        //[StringLength(100)]
        //public string LastName { get; set; }

        //[Required]
        //[StringLength(250)]
        //public string Adress { get; set; }

        //[Required]
        //[StringLength(20)]
        //public string Telephone { get; set; }

        //[Required]
        //[StringLength(150)]
        //public string Email { get; set; }

        //public DateTime BirstDate { get; set; }

        //public DateTime RegisteredDate { get; set; }

        //[StringLength(300)]
        //public string Description { get; set; }

        //public int CustomerIndId { get; set; }
    }
}
