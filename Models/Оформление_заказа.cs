namespace D.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Оформление_заказа: IОформление_заказаInterface
    {
        public int ID_товара { get; set; }

        public int ID_заказа { get; set; }

        public int Количество { get; set; }

        public int ID { get; set; }

        public virtual Заказ Заказ { get; set; }

        public virtual Товар Товар { get; set; }
        public void AddtoTable(IdbInterface db, IОформление_заказаInterface p)
        {

            db.Оформление_заказа.Add(p as Оформление_заказа);
        }
    }
}
