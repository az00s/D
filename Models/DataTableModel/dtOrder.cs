using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace D.Models.DataTableModel
{
    public class dtOrder
    {
        public IQueryable<Object> data;
        public int recordsFiltered;
        public int recordsTotal;
        public int draw;
        public IQueryable<Object> Get_one(dtParam param, IQueryable<Order> src)
        {

            return src.AsNoTracking().Where(o => param.Search.Value != null ?
                                                                                                              o.AmountVat.ToString().Contains(param.Search.Value) ||
                                                                                                              o.CustomerEnt.Name.Contains(param.Search.Value) ||
                                                                                                              o.CustomerInd.LastName.Contains(param.Search.Value) ||
                                                                                                              o.OrderDate.ToString().Contains(param.Search.Value) ||
                                                                                                              o.OrderID.ToString().Contains(param.Search.Value) ||
                                                                                                              o.OrderStatus.Contains(param.Search.Value) ||
                                                                                                              o.PersonnelNumber.ToString().Contains(param.Search.Value)

                                                                                                              : true).Select(s => 
                                                                                                              new { OrderID = s.OrderID,
                                                                                                                  OrderDate = s.OrderDate.ToString(),
                                                                                                                  Customer = s.CustomerInd.LastName+" "+s.CustomerInd.FirstName+s.CustomerEnt.Name,
                                                                                                                  AmountVat =s.AmountVat,
                                                                                                                  OrderStatus =s.OrderStatus,
                                                                                                                  Employee =s.Employee.LastName });

        }




        public IQueryable<Object> GetData(dtParam param, IQueryable<Order> src)
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
            else return Get_one(param, src).SortBy("OrderID")
                                      .Skip(param.Start)
                                      .Take(param.Length == -1 ? 2147483647 : param.Length);


        }







        public int Count(dtParam param, IQueryable<Order> src)
        {
            return Get_one(param, src).Count();

        }



    }

}