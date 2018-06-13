using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCatalog.Models
{
    public class ServiceResponse
    {
        public bool IsSuccessfull { get; set; }
        public string ResultMessage { get; set; }
    }
}