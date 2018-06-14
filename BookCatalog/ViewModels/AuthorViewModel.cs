using BookCatalog.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookCatalog.ViewModels
{
    public class AuthorViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "First Name is required field")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required field")]
        [MaxLength(100)]
        public string LastName { get; set; }

        //public int AmountOfBooks { get; set; }

        //public virtual ICollection<Book> BooksCollection { get; set; }
    }
}