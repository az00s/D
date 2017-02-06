using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IКлиентInterface
    {
         int ID_клиента { get; set; }

        string Номер_паспорта { get; set; }

        int? УНП_Клиента { get; set; }

        string Название_организации { get; set; }

        string Телефон { get; set; }

         string Адрес { get; set; }

         string Фамилия { get; set; }

         string Имя { get; set; }

        string Отчество { get; set; }

        
         ICollection<Заказ> Заказ { get; set; }

         void AddtoTable(IdbInterface db, IКлиентInterface p);
        
    }
}