using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IEmployeeInterface
    {
        
         int PersonnelNumber { get; set; }

         string LastName { get; set; }

         string Name { get; set; }

        string Patronymic { get; set; }

        string Position { get; set; }

         string Telephone { get; set; }

        string Address { get; set; }

         string PassportID { get; set; }

         DateTime? BirstDate { get; set; }

         ICollection<Order> Orders { get; set; }

        void AddtoTable(IdbInterface db, IEmployeeInterface p);

        
    }
}