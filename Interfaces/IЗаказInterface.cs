using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IЗаказInterface
    {
         DateTime? Дата_заказа { get; set; }

        decimal? Сумма_заказа_с_НДС { get; set; }

        int ID_заказа { get; set; }
        int ID_клиента { get; set; }

        int? Табельный_номер { get; set; }

        string Статус_заказа { get; set; }

         Клиент Клиент { get; set; }

         Сотрудник Сотрудник { get; set; }

         ICollection<Оплата_заказа> Оплата_заказа { get; set; }

         ICollection<Оформление_заказа> Оформление_заказа { get; set; }
         void AddtoTable(IdbInterface db, IЗаказInterface p);
    }
}