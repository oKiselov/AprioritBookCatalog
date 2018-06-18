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
    }
}