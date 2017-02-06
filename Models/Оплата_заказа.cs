namespace D.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using D.Interfaces;
    public partial class Оплата_заказа: IОплата_заказаInterface
    {

        [Key]
        public int ID { get; set; }

        [Column(TypeName = "money")]
        public decimal? Сумма { get; set; }

       
        //[Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_поступления { get; set; }

        
        //[Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_клиента { get; set; }

        
        //[Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_заказа { get; set; }
        public virtual Денежное_поступление Денежное_поступление { get; set; }

        public virtual Заказ Заказ { get; set; }

        public void AddtoTable(IdbInterface db, IОплата_заказаInterface p)
        {

            db.Оплата_заказа.Add(p as Оплата_заказа);
        }
    }
}
