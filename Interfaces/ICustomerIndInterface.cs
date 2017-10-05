using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface ICustomerIndInterface
    {
        string PassportId { get; set; }

         string FirstName { get; set; }

        string Patronymic { get; set; }

         string LastName { get; set; }

        string Address { get; set; }

         string Telephone { get; set; }

         string Email { get; set; }

        DateTime? BirstDate { get; set; }

        DateTime RegisteredDate { get; set; }

         string Description { get; set; }

         int CustomerIndId { get; set; }
        void AddtoTable(IdbInterface db, ICustomerIndInterface p);
    }
}