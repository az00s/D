using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IMoneyReceiptInterface
    {
         decimal? Amount { get; set; }

         DateTime? ReceiptDate { get; set; }
        int ReceiptID { get; set; }
         int ClientID { get; set; }

          ICollection<OrderPayment> OrderPayments { get; set; }

        CustomerEnt CustomerEnt { get; set; }
        void AddtoTable(IdbInterface db, IMoneyReceiptInterface p);
    }
}