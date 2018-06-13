using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookCatalog.ViewModels
{
    public class BookViewModel
    {
        [Required(ErrorMessage = "Book's Title is required field")]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Year of Publishing is required field")]
        public DateTime PublishingYear { get; set; }

        [Required(ErrorMessage = "Amount of Pages is required field")]
        public int PagesAmount { get; set; }

        [Required(ErrorMessage = "Book's Rate is required field")]
        public int Rate { get; set; }

        [Required(ErrorMessage ="At least one author should be selected")]
        public int[] Authors { get; set; }
    }
}