using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface ISupplierInterface
    {
        int SupplierPAN { get; set; }

        string Name { get; set; }

         string Address { get; set; }

        string Telephone { get; set; }

         string Description { get; set; }

         ICollection<SupplierPrice> SupplierPrices { get; set; }

        void AddtoTable(IdbInterface db, ISupplierInterface p);
       
    }
}