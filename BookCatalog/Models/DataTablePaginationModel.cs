using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCatalog.Models
{
    public class DataTablePaginationModel
    {
        public string sEcho { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public string iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
    }
}