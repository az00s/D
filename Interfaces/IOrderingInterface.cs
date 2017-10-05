using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Models
{
    public interface IOrderingInterface
    {
         int ProductID { get; set; }

         int OrderID { get; set; }

         int ProductQuantity { get; set; }

         int ID { get; set; }

          Order Order { get; set; }

          Product Product { get; set; }

         void AddtoTable(IdbInterface db, IOrderingInterface p);
    }
}