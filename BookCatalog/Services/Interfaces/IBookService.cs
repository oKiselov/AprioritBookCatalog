﻿using BookCatalog.Data.Entities;
using BookCatalog.Models;
using BookCatalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCatalog.Services.Interfaces
{
    public interface IBookService
    {
        ServiceResponse SaveBook(BookViewModel bookViewModel);
        ServiceResponse SaveAuthor(AuthorViewModel authorViewModel);
        Book GetBook();
        IEnumerable<AuthorSearchViewModel> GetAuthorsList();
        IEnumerable<BookSearchViewModel> GetFilteredBooks(DataTablePaginationModel model);
        int GetTotalBooksAmount(string searchOption);
    }
}
