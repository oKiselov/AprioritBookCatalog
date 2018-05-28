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

        public Book GetBook()
        {
            return bookCatalogRepository.GetBook();
        }

        public IEnumerable<BookSearchViewModel> GetFilteredBooks(DataTablePaginationModel model)
        {
            var books = bookCatalogRepository.GetBooks(GetOrderType(model.iSortCol_0), model.sSortDir_0, model.iDisplayStart, model.iDisplayLength);

            var booksViews = new List<BookSearchViewModel>();

            for (int i = 0; i < books.Count(); i++)
            {
                var bookView = new BookSearchViewModel();
                AutoMapper.Mapper.Map<Book, BookSearchViewModel>(books[i], bookView);
                booksViews.Add(bookView);
            }

            return booksViews;
        }

        public int GetTotalBooksAmount()
        {
            return bookCatalogRepository.GetBooksAmount();
        }

        private Expression<Func<Book, int>> GetOrderType(string nameOrderType)
        {
            Expression<Func<Book, int>> orderType = book => book.Id;
            var orderResult = default(int);
            if (!string.IsNullOrEmpty(nameOrderType) && Int32.TryParse(nameOrderType, out orderResult))
            {
                switch (orderResult)
                {
                    case (int)BookOrderType.Id: orderType = book => book.Id;break;
                    case (int)BookOrderType.PublishingYear: orderType = book => book.PublishingYear;break;
                    case (int)BookOrderType.PagesAmount: orderType = book => book.PagesAmount; break;
                    case (int)BookOrderType.Rate:orderType = book => book.Rate; break;
                    default: orderType = book => book.Id;break;
                }
            }
            return orderType;
        }
    }
}