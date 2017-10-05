using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace D.Models.DataTableModel
{
    public class dtCustomerInd
    {
        public IQueryable<CustomerInd> data;
        public int recordsFiltered;
        public int recordsTotal;
        public int draw;
        public IQueryable<CustomerInd> Get_one(dtParam param, IQueryable<CustomerInd> src)
        {

            return src.AsNoTracking().Where(o => param.Search.Value != null ?
                                                                                                              o.Address.ToString().Contains(param.Search.Value) ||
                                                                                                              o.BirstDate.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Description.Contains(param.Search.Value) ||
                                                                                                              o.Email.Contains(param.Search.Value) ||
                                                                                                              o.FirstName.Contains(param.Search.Value) ||
                                                                                                              o.LastName.Contains(param.Search.Value) ||
                                                                                                              o.PassportId.Contains(param.Search.Value) ||
                                                                                                              o.Patronymic.Contains(param.Search.Value) ||
                                                                                                              o.Telephone.Contains(param.Search.Value) 
                                                                                                              : true);

        }



        public IQueryable<CustomerInd> GetData(dtParam param, IQueryable<CustomerInd> src)
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
            else return Get_one(param, src).OrderBy(o => o.CustomerIndId)
                                      .Skip(param.Start)
                                      .Take(param.Length == -1 ? 2147483647 : param.Length);


        }






        public int Count(dtParam param, IQueryable<CustomerInd> src)
        {
            return Get_one(param, src).Count();

        }



    }

}