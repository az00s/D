using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IОплата_заказаInterface
    {
         int ID { get; set; }

        decimal? Сумма { get; set; }


         int ID_поступления { get; set; }


         int ID_клиента { get; set; }


        int ID_заказа { get; set; }
         Денежное_поступление Денежное_поступление { get; set; }

         Заказ Заказ { get; set; }
         void AddtoTable(IdbInterface db, IОплата_заказаInterface p);
    }
}