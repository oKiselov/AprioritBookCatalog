using BookCatalog.Data.Entities;
using BookCatalog.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace BookCatalog.Data.Repositories
{
    public class BookCatalogRepository : IBookCatalogRepository
    {
        private readonly DbContext bookCatalogContext;
        private readonly DbSet<Book> dbSetBooks;

        public BookCatalogRepository(DbContext dbContext)
        {
            this.bookCatalogContext = dbContext;
            this.dbSetBooks = dbContext.Set<Book>();
        }

        public Book GetBook()
        {
            return dbSetBooks.FirstOrDefault();
        }

        public IList<Book> GetBooks(Expression<Func<Book, int>> orderExpression, string destName, int displayStart, int displayLength)
        {
            return GetOrderedQuery(orderExpression, destName)
                .Skip(displayStart)
                .Take(displayLength)
                .ToList();
        }

        public int GetBooksAmount()
        {
            return dbSetBooks.Count();
        }

        public IQueryable<Book> GetOrderedQuery(Expression<Func<Book, int>> orderExplression, string destName)
        {
            return destName == Resources.Resources.Descending
                ? dbSetBooks.OrderByDescending(orderExplression)
                : dbSetBooks.OrderBy(orderExplression);
        }
    }
}
