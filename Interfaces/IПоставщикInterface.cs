using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IПоставщикInterface
    {
        int УНП_поставщика { get; set; }

        string Название_организации { get; set; }

         string Адрес { get; set; }

        string Телефон { get; set; }

         string Описание { get; set; }

         ICollection<Поставщик_цена> Поставщик_цена { get; set; }

        void AddtoTable(IdbInterface db, IПоставщикInterface p);
       
    }
}