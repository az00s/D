using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D.Interfaces
{
    public interface IСотрудникInterface
    {
        
         int Табельный_номер { get; set; }

         string Фамилия { get; set; }

         string Имя { get; set; }

        string Отчество { get; set; }

        string Должность { get; set; }

         string Телефон { get; set; }

        string Адрес { get; set; }

         string Номер_паспорта { get; set; }

         DateTime? Дата_рождения { get; set; }

         ICollection<Заказ> Заказ { get; set; }

        void AddtoTable(IdbInterface db, IСотрудникInterface p);

        
    }
}