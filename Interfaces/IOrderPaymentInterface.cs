using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IOrderPaymentInterface
    {
         int ID { get; set; }

        decimal? Amount { get; set; }


         int ReceiptID { get; set; }


         int ClientID { get; set; }


        int OrderID { get; set; }
        MoneyReceipt MoneyReceipt { get; set; }

         Order Order { get; set; }
         void AddtoTable(IdbInterface db, IOrderPaymentInterface p);
    }
}