using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface ICustomerEntInterface
    {
         int ClientID { get; set; }

       

        int? ClientPAN { get; set; }

        string Name { get; set; }

        string Telephone { get; set; }

         string Address { get; set; }

        

        
         //ICollection<Order> Orders { get; set; }

         void AddtoTable(IdbInterface db, ICustomerEntInterface p);
        
    }
}