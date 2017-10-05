using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IProductInterface
    {

         

         int ProductID { get; set; }

        string Designation { get; set; }
        string Name { get; set; }

        string Description { get; set; }

        string Unit_of_measurement { get; set; }

        int Balance { get; set; }

         int? Delivery_time { get; set; }

        int? Weight { get; set; }

        decimal? Price { get; set; }









        decimal? Price_with_vat { get; set; }



         ICollection<Ordering> Orderings { get; set; }

        ICollection<SupplierPrice> SupplierPrices { get; set; }
        void AddtoTable(IdbInterface db, IProductInterface p);
    }
}