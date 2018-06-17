using BookCatalog.Data.Entities;
using BookCatalog.Data.Repositories.Interfaces;
using BookCatalog.Models;
using BookCatalog.Models.Enums;
using BookCatalog.Services.Interfaces;
using BookCatalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BookCatalog.Services
{
    public class BookService : IBookService
    {
        private readonly IBookCatalogRepository bookCatalogRepository;

        public BookService(IBookCatalogRepository bookCatalogRepository)
        {
            this.bookCatalogRepository = bookCatalogRepository;
        }

        public IEnumerable<AuthorSearchViewModel> GetAuthorsList()
        {
            var authors = bookCatalogRepository.GetAuthorsList();
            var authorViews = new List<AuthorSearchViewModel>();

            for (int i = 0; i < authors.Count(); i++)
            {
                var authorView = new AuthorSearchViewModel();
                AutoMapper.Mapper.Map<Author, AuthorSearchViewModel>(authors[i], authorView);
                authorViews.Add(authorView);
            }

            return authorViews;
        }

        public Book GetBook(int bookId)
        {
            return bookCatalogRepository.GetBook(bookId);
        }

        public IEnumerable<BookSearchViewModel> GetFilteredBooks(DataTablePaginationModel model)
        {
            var books = bookCatalogRepository.GetBooks(GetOrderType(model.iSortCol_0), model.sSortDir_0, model.iDisplayStart, model.iDisplayLength, model.sSearch_2);

            var booksViews = new List<BookSearchViewModel>();

            for (int i = 0; i < books.Count(); i++)
            {
                var bookView = new BookSearchViewModel();
                AutoMapper.Mapper.Map<Book, BookSearchViewModel>(books[i], bookView);
                booksViews.Add(bookView);
            }

            return booksViews;
        }

        public int GetTotalBooksAmount(string searchOption)
        {
            return bookCatalogRepository.GetBooksAmount(searchOption);
        }

        public ServiceResponse RemoveBook(int bookId)
        {
            var book = bookCatalogRepository.GetBook(bookId);
            var response = new ServiceResponse();
            if (book != null)
            {
                try
                {
                    bookCatalogRepository.RemoveBook(book);
                    response = CreateResponse(true, Resources.Resources.CompletedSuccessfully);
                }
                catch (Exception ex)
                {
                    response = CreateResponse(false, $"{Resources.Resources.ErrorOccured}. {ex.Message}");
                }
            }
            else
            {
                response = CreateResponse(false, Resources.Resources.BookIsAbsent);
            }
            return response;
        }

        public ServiceResponse SaveAuthor(AuthorViewModel authorViewModel)
        {
            var author = new Author();
            AutoMapper.Mapper.Map<AuthorViewModel, Author>(authorViewModel, author);

            var response = new ServiceResponse();
            try
            {
                if (authorViewModel.Id.HasValue)
                {
                    bookCatalogRepository.UpdateAuthor(author);
                }
                else
                {
                    bookCatalogRepository.SaveAuthor(author);
                }
                response = CreateResponse(true, Resources.Resources.CompletedSuccessfully);
            }
            catch (Exception ex)
            {
                response = CreateResponse(false, $"{Resources.Resources.ErrorOccured}. {ex.Message}");
            }
            return response;
        }

        public ServiceResponse SaveBook(BookViewModel bookViewModel)
        {
            var book = new Book();
            bookViewModel.AuthorsCollection = bookCatalogRepository.GetAuthorsById(bookViewModel.Authors);
            AutoMapper.Mapper.Map<BookViewModel, Book>(bookViewModel, book);

            ServiceResponse response;
            try
            {
                if (bookViewModel.Id.HasValue)
                {
                    bookCatalogRepository.UpdateBook(book);
                }
                else
                {
                    bookCatalogRepository.SaveBook(book);
                }
                response = CreateResponse(true, Resources.Resources.CompletedSuccessfully);
            }
            catch (Exception ex)
            {
                response = CreateResponse(false, $"{Resources.Resources.ErrorOccured}. {ex.Message}");
            }
            return response;
        }

        private Expression<Func<Book, int>> GetOrderType(string nameOrderType)
        {
            Expression<Func<Book, int>> orderType = book => book.Id;
            var orderResult = default(int);
            if (!string.IsNullOrEmpty(nameOrderType) && Int32.TryParse(nameOrderType, out orderResult))
            {
                switch (orderResult)
                {
                    case (int)BookOrderType.Id: orderType = book => book.Id; break;
                    case (int)BookOrderType.PublishingYear: orderType = book => book.PublishingYear; break;
                    case (int)BookOrderType.PagesAmount: orderType = book => book.PagesAmount; break;
                    case (int)BookOrderType.Rate: orderType = book => book.Rate; break;
                    default: orderType = book => book.Id; break;
                }
            }
            return orderType;
        }

        private ServiceResponse CreateResponse(bool isSuccessfull, string message)
        {
            return new ServiceResponse
            {
                IsSuccessfull = isSuccessfull,
                ResultMessage = message
            };
        }
    }
}