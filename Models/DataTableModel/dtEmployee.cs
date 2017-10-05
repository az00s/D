using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace D.Models.DataTableModel
{
    public class dtEmployee
    {
        public IQueryable<Object> data;
        public int recordsFiltered;
        public int recordsTotal;
        public int draw;
        public IQueryable<Object> Get_one(dtParam param, IQueryable<Employee> src)
        {

            return src.AsNoTracking().Where(o => param.Search.Value != null ?
                                                                                                              o.BirstDate.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Address.Contains(param.Search.Value) ||
                                                                                                              o.LastName.Contains(param.Search.Value) ||
                                                                                                              o.Name.Contains(param.Search.Value) ||
                                                                                                              o.PassportID.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Patronymic.Contains(param.Search.Value) ||
                                                                                                              o.PersonnelNumber.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Position.Contains(param.Search.Value) ||
                                                                                                              o.Telephone.ToString().Contains(param.Search.Value)


                                                                                                              : true).Select(s=>new {
                                                                                                                  PersonnelNumber = s.PersonnelNumber,
                                                                                                                  LastName = s.LastName,
                                                                                                                  Name = s.Name,
                                                                                                                  Patronymic = s.Patronymic,
                                                                                                                  Position = s.Position,
                                                                                                                  Telephone =s.Telephone,
                                                                                                                  BirstDate =s.BirstDate.ToString()
                                                                                                              });

        }



        public IQueryable<Object> GetData(dtParam param, IQueryable<Employee> src)
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
            else return Get_one(param, src).SortBy("PersonnelNumber")
                                      .Skip(param.Start)
                                      .Take(param.Length == -1 ? 2147483647 : param.Length);


        }








        public int Count(dtParam param, IQueryable<Employee> src)
        {
            return Get_one(param, src).Count();

        }




    }

}