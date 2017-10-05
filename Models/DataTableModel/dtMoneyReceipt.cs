using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace D.Models.DataTableModel
{
    public class dtMoneyReceipt
    {
        public IQueryable<Object> data;
        public int recordsFiltered;
        public int recordsTotal;
        public int draw;
        public IQueryable<Object> Get_one(dtParam param, IQueryable<MoneyReceipt> src)
        {

            return src.AsNoTracking().Where(o => param.Search.Value != null ?
                                                                                                              o.Amount.ToString().Contains(param.Search.Value) ||
                                                                                                              o.CustomerEnt.Name.Contains(param.Search.Value) ||
                                                                                                              o.CustomerEnt.ClientPAN.ToString().Contains(param.Search.Value) ||
                                                                                                              o.ReceiptDate.ToString().Contains(param.Search.Value) ||
                                                                                                              o.ReceiptID.ToString().Contains(param.Search.Value)

                                                                                                              : true).Select(s=>new { Amount = s.Amount, Customer=s.CustomerEnt.Name, CustomerPAN=s.CustomerEnt.ClientPAN, ReceiptDate = s.ReceiptDate.ToString(), ReceiptID =s.ReceiptID });

        }



        public IQueryable<Object> GetData(dtParam param, IQueryable<MoneyReceipt> src)
        {

            if (param.Order != null)
            {
                if (param.Order[0].Dir == "asc")
                    return Get_one(param, src).SortBy(param.Columns[param.Order[0].Column].Data)
                                              .Skip(param.Start)
                                              .Take(param.Length == -1 ? 2147483647 : param.Length);
                else return Get_one(param, src).SortBy(param.Columns[param.Order[0].Column].Data + " " + param.Order[0].Dir)
                                          .Skip(param.Start)
                                          .Take(param.Length == -1 ? 2147483647 : param.Length);
            }
            else return Get_one(param, src).SortBy("ReceiptID")
                                      .Skip(param.Start)
                                      .Take(param.Length == -1 ? 2147483647 : param.Length);


        }








        public int Count(dtParam param, IQueryable<MoneyReceipt> src)
        {
            return Get_one(param, src).Count();

        }




    }

}