﻿using BookCatalog.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BookCatalog.Data.Repositories.Interfaces
{
    public interface IBookCatalogRepository
    {
        void SaveBook(Book book);
        void UpdateBook(Book book);
        void SaveAuthor(Author author);
        void UpdateAuthor(Author author);
        Book GetBook(int? bookId);
        void RemoveBook(Book book);
        IList<Author> GetAuthorsList();
        IList<Author> GetAuthorsById(IEnumerable<int> authorsIds);
        int GetBooksAmount(string searchOption);
        IList<Book> GetBooks(Expression<Func<Book, int>> orderExpression, string destName, int displayStart, int displayLengthб, string searchOption);
    }
}
