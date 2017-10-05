using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;

namespace D.Models.DataTableModel
{
    public class dtProduct
    {
        public IQueryable<Product> data;
        public int recordsFiltered;
        public int recordsTotal;
        public int draw;
        public IQueryable<Product> Get_one(dtParam param, IQueryable<Product> src)
        {

            return src.AsNoTracking().Where(o => param.Search.Value != null ?
                                                                                                              o.RegistrationDate.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Name.Contains(param.Search.Value) ||
                                                                                                              o.Designation.Contains(param.Search.Value) ||
                                                                                                              o.Weight.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Description.Contains(param.Search.Value) ||
                                                                                                              o.Balance.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Delivery_time.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Price.ToString().Contains(param.Search.Value) ||
                                                                                                              o.Price_with_vat.ToString().Contains(param.Search.Value)
                                                                                                              : true);

        }


        public IQueryable<Product> GetData(dtParam param, IQueryable<Product> src)
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
            else return Get_one(param, src).OrderBy(o => o.ProductID)
                                      .Skip(param.Start)
                                      .Take(param.Length == -1 ? 2147483647 : param.Length);


        }




        public int Count(dtParam param, IQueryable<Product> src)
        {
            return Get_one(param, src).Count();

        }



    }

}