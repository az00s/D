using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IПоставщик_ценаInterface
    {
         decimal? Оптовая_цена { get; set; }

        
         int ID_товара { get; set; }

        
         int УНП_поставщика { get; set; }

          Поставщик Поставщик { get; set; }

        Товар Товар { get; set; }

        void AddtoTable(IdbInterface db, IПоставщик_ценаInterface p);
    }
}