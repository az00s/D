using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IДенежное_поступлениеInterface
    {
         decimal? Сумма { get; set; }

         DateTime? Дата_поступления { get; set; }
        int ID_поступления { get; set; }
         int ID_клиента { get; set; }

          ICollection<Оплата_заказа> Оплата_заказа { get; set; }

        CustomerEnt CustomerEnt { get; set; }
        void AddtoTable(IdbInterface db, IДенежное_поступлениеInterface p);
    }
}