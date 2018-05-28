using System.Collections.Generic;

namespace BookCatalog.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublishingYear { get; set; }
        public int PagesAmount { get; set; }
        public int Rate { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
    }
}
