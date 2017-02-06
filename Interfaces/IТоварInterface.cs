using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IТоварInterface
    {

         

         int ID_товара { get; set; }

        string Обозначение { get; set; }
        string Наименование { get; set; }

        string Краткое_описание { get; set; }

        string Единица_измерения { get; set; }

        int Остаток_на_складе { get; set; }

         int? Срок_поставки { get; set; }

        int? Вес { get; set; }

        decimal? Цена { get; set; }









        decimal? Цена_с_НДС { get; set; }



         ICollection<Оформление_заказа> Оформление_заказа { get; set; }

        ICollection<Поставщик_цена> Поставщик_цена { get; set; }
        void AddtoTable(IdbInterface db, IТоварInterface p);
    }
}