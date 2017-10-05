using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace D.Models.DataTableModel
{
    public class dtCustomerEnt
    {
        public IQueryable<CustomerEnt> data;
        public int recordsFiltered;
        public int recordsTotal;
        public int draw;
        public IQueryable<CustomerEnt> Get_one(dtParam param, IQueryable<CustomerEnt> src)
        {

            return src.AsNoTracking().Where(o => param.Search.Value != null ?
                                                                                                              o.Address.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Name.Contains(param.Search.Value) ||
                                                                                                              o.ClientPAN.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Telephone.Contains(param.Search.Value)
                                                                                                             
                                                                                                              : true);

        }


        public IQueryable<CustomerEnt> GetData(dtParam param, IQueryable<CustomerEnt> src)
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
            else return Get_one(param, src).OrderBy(o => o.ClientID)
                                      .Skip(param.Start)
                                      .Take(param.Length == -1 ? 2147483647 : param.Length);


        }






        public int Count(dtParam param, IQueryable<CustomerEnt> src)
        {
            return Get_one(param, src).Count();

        }

        

    }

}