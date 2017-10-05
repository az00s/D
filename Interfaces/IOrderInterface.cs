using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IOrderInterface
    {
         DateTime? OrderDate { get; set; }

        decimal? AmountVat { get; set; }

        int OrderID { get; set; }
        int? ClientID { get; set; }

        int? PersonnelNumber { get; set; }

        string OrderStatus { get; set; }

        CustomerEnt CustomerEnt { get; set; }

         Employee Employee { get; set; }

         ICollection<OrderPayment> OrderPayments { get; set; }

         ICollection<Ordering> Orderings { get; set; }
         void AddtoTable(IdbInterface db, IOrderInterface p);
    }
}