using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCatalog.ViewModels
{
    // TODO: MAYBE INHERITANCE ???
    public class AuthorSearchViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + LastName;
            }
        }
    }
}