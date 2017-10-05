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

}