using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace D.Models.DataTableModel
{
    public class dtSupplier
    {
        public IQueryable<Object> data;
        public int recordsFiltered;
        public int recordsTotal;
        public int draw;
        public IQueryable<Object> Get_one(dtParam param, IQueryable<Supplier> src)
        {

            return src.AsNoTracking().Where(o => param.Search.Value != null ?
                                                                                                              o.Address.Contains(param.Search.Value) ||
                                                                                                              o.Description.Contains(param.Search.Value) ||
                                                                                                              o.Name.Contains(param.Search.Value) ||
                                                                                                              o.SupplierPAN.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Telephone.Contains(param.Search.Value)

                                                                                                              : true).Select(s=>new { SupplierPAN=s.SupplierPAN, Name=s.Name, Address=s.Address,Telephone=s.Telephone });

        }



        public IQueryable<Object> GetData(dtParam param, IQueryable<Supplier> src)
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
            else return Get_one(param, src).SortBy("SupplierPAN")
                                      .Skip(param.Start)
                                      .Take(param.Length == -1 ? 2147483647 : param.Length);


        }








        public int Count(dtParam param, IQueryable<Supplier> src)
        {
            return Get_one(param, src).Count();

        }




    }

}