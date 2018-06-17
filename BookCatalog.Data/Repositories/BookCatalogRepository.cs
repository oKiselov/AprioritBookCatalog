﻿using BookCatalog.Data.Entities;
using BookCatalog.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace BookCatalog.Data.Repositories
{
    public class BookCatalogRepository : IBookCatalogRepository
    {
        private readonly DbContext bookCatalogContext;
        private readonly DbSet<Book> dbSetBooks;
        private readonly DbSet<Author> dbSetAuthors;

        public BookCatalogRepository(DbContext dbContext)
        {
            this.bookCatalogContext = dbContext;
            this.dbSetBooks = dbContext.Set<Book>();
            this.dbSetAuthors = dbContext.Set<Author>();
        }

        public IList<Author> GetAuthorsById(IEnumerable<int> authorsIds)
        {
            return dbSetAuthors
                .Where(a => authorsIds.Contains(a.Id))
                .ToList();
        }

        public IList<Author> GetAuthorsList()
        {
            return dbSetAuthors.ToList();
        }

        public Book GetBook(int bookId)
        {
            return dbSetBooks.FirstOrDefault(b=>b.Id == bookId);
        }

        public void RemoveBook(Book book)
        {
            using (var tran = new TransactionScope())
            {
                dbSetBooks.Remove(book);
                bookCatalogContext.SaveChanges();
                tran.Complete();
            }
        }

        public IList<Book> GetBooks(Expression<Func<Book, int>> orderExpression, string destName, int displayStart, int displayLength, string searchOption)
        {
            return GetOrderedQuery(orderExpression, destName)
                .Where(b => string.IsNullOrEmpty(searchOption) ? true : b.Title.ToLower().Contains(searchOption.ToLower()))
                .Skip(displayStart)
                .Take(displayLength)
                .ToList();
        }

        public int GetBooksAmount(string searchOption)
        {
            return dbSetBooks.Count(b=> string.IsNullOrEmpty(searchOption) ? true : b.Title.ToLower().Contains(searchOption.ToLower()));
        }

        public IQueryable<Book> GetOrderedQuery(Expression<Func<Book, int>> orderExplression, string destName)
        {
            return destName == Resources.Resources.Descending
                ? dbSetBooks.OrderByDescending(orderExplression)
                : dbSetBooks.OrderBy(orderExplression);
        }

        public void SaveAuthor(Author author)
        {
            using (var tran = new TransactionScope())
            {
                dbSetAuthors.Add(author);
                bookCatalogContext.SaveChanges();
                tran.Complete();
            }
        }

        public void SaveBook(Book book)
        {
            using(var tran = new TransactionScope())
            {
                dbSetBooks.Add(book);
                bookCatalogContext.SaveChanges();
                tran.Complete();
            }
        }

        public void UpdateAuthor(Author author)
        {
            using (var tran = new TransactionScope())
            {
                var oldauthor = dbSetAuthors.FirstOrDefault(b => b.Id == author.Id);
                oldauthor = author;
                bookCatalogContext.SaveChanges();
                tran.Complete();
            }
        }

        public void UpdateBook(Book book)
        {
            using (var tran = new TransactionScope())
            {
                var oldBook = dbSetBooks.FirstOrDefault(b => b.Id == book.Id);
                oldBook = book;
                bookCatalogContext.SaveChanges();
                tran.Complete();
            }
        }
    }
}
