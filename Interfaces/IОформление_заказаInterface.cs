using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Models
{
    public interface IОформление_заказаInterface
    {
         int ID_товара { get; set; }

         int ID_заказа { get; set; }

         int Количество { get; set; }

         int ID { get; set; }

          Заказ Заказ { get; set; }

          Товар Товар { get; set; }

         void AddtoTable(IdbInterface db, IОформление_заказаInterface p);
    }
}