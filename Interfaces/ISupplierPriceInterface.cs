using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface ISupplierPriceInterface
    {
         decimal? Price { get; set; }

        
         int ProductID { get; set; }

        
         int SupplierPAN { get; set; }

          Supplier Supplier { get; set; }

        Product Product { get; set; }

        void AddtoTable(IdbInterface db, ISupplierPriceInterface p);
    }
}