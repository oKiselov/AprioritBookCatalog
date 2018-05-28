using BookCatalog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalog.Data.Repositories.Interfaces
{
    public interface IBookCatalogRepository
    {
        Book GetBook();
        int GetBooksAmount();
        IList<Book> GetBooks(Expression<Func<Book, int>> orderExpression, string destName, int displayStart, int displayLength);
    }
}
