using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookCatalog.ViewModels
{
    public class BookViewModel
    {
        [Required(ErrorMessage = "Book's Title is required field")]
        [MaxLength(100)]
        [DisplayName("Book's Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Year of Publishing is required field")]
        [DisplayName("Year of Publishing")]
        public int PublishingYear { get; set; }

        [Required(ErrorMessage = "mount of Pages is required field")]
        [DisplayName("Amount of Pages")]
        public int PagesAmount { get; set; }

        [Required(ErrorMessage = "Book's Rate is required field")]
        [DisplayName("Rate")]
        public int Rate { get; set; }

        [DisplayName("Author(s)")]
        public string Authors { get; set; }
    }
}