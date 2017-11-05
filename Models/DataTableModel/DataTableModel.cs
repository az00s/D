using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace D.Models
{
    public class dtParam
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public dtSearch Search { get; set; }
        public dtOrder[] Order { get; set; }
        public dtColumn[] Columns { get; set; }
        public DateTime repStart { get; set; }
        public DateTime repEnd { get; set; }
    }

    public class dtSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class dtOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class dtColumn
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public dtSearch Search { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }

    }

    //Generalized class-adapter for datatable plugin (instead 7 classes dt*.cs for each entity )
    public class dtResult<S, T> where T : class
    {
        public IEnumerable<T> data;
        public int recordsFiltered;
        public int recordsTotal;
        public int draw;
        public List<T> Get_one(dtParam param, S dList)
        {

            

            if (param.Search.Value == null)
            {
                recordsFiltered = (dList as IQueryable<T>).Count();
                return (dList as IQueryable<T>).ToList();
            }
            else
            {
                List<T> rList = new List<T>();
                foreach (T obj in dList as IQueryable<T>)
                {
                    if (obj.GetType().GetProperties().
                        Where(p => !p.PropertyType.IsInterface).
                        Any(pr => pr.GetValue(obj) == null ? false : pr.GetValue(obj).ToString().ToLower().Contains(param.Search.Value.ToLower())))
                    {
                        rList.Add(obj);

                    }

                }
                recordsFiltered = rList.Count();

                return rList;
            }


        }



        public void GetData(dtParam param, S src,S dList)
        {
            recordsTotal = (src as IQueryable<T>).Count();
            draw = param.Draw;


            if (param.Order != null)
            {

                //---------------------------------------------------------------------------------------------------------------------------------------------
                //fixing sorting issue (can't sort when table consists CustomerEnt and CustomerInd orders ) on Order table, column Customer(Клиент). 
                if (typeof(T).Equals(typeof(Order)) && param.Columns[param.Order[0].Column].Data == "CustomerEnt.Name")
                {

                    if (param.Order[0].Dir == "asc")
                        data = (Get_one(param, dList) as List<Order>)
                            .OrderBy(o => o.CustomerEnt != null ? o.CustomerEnt.Name : o.CustomerInd.FirstName)
                                                  .Skip(param.Start)
                                                  .Take(param.Length == -1 ? 2147483647 : param.Length) as IEnumerable<T>;


                    else data = (Get_one(param, dList) as List<Order>)
                                                     .OrderByDescending(o => o.CustomerEnt != null ? o.CustomerEnt.Name : o.CustomerInd.FirstName)

                                              .Skip(param.Start)
                                              .Take(param.Length == -1 ? 2147483647 : param.Length) as IEnumerable<T>;


                }
                //-----------------------------------------------------------------------------------------------------------------------------------------------
                else
                {
                    if (param.Order[0].Dir == "asc")
                        data = Get_one(param, dList).AsQueryable()
                                                  .SortBy(param.Columns[param.Order[0].Column].Data)
                                                  .Skip(param.Start)
                                                  .Take(param.Length == -1 ? 2147483647 : param.Length);


                    else data = Get_one(param, dList).AsQueryable()
                                              .SortBy(param.Columns[param.Order[0].Column].Data + " " + param.Order[0].Dir)
                                              .Skip(param.Start)
                                              .Take(param.Length == -1 ? 2147483647 : param.Length);
                }
                                          
                                          
            }
            else data= Get_one(param, dList).AsQueryable()
                                      .Skip(param.Start)
                                      .Take(param.Length == -1 ? 2147483647 : param.Length);


        }












    }

}