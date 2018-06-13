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
        void SaveBook(Book book);
        void UpdateBook(Book book);
        Book GetBook();
        IList<Author> GetAuthorsList();
        IList<Author> GetAuthorsById(IEnumerable<int> authorsIds);
        int GetBooksAmount();
        IList<Book> GetBooks(Expression<Func<Book, int>> orderExpression, string destName, int displayStart, int displayLength);
    }
}
