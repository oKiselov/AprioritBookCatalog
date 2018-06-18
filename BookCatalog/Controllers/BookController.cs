using BookCatalog.Models;
using BookCatalog.Services.Interfaces;
using BookCatalog.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace BookCatalog.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetBookResultTable(DataTablePaginationModel paginationModel)
        { 
            return Json(new
            {
                sEcho = paginationModel.sEcho,
                iTotalDisplayRecords = bookService.GetTotalBooksAmount(paginationModel.sSearch_2),
                aaData = bookService.GetFilteredBooks(paginationModel)
            });
        }

        [HttpPost]
        public JsonResult SaveBook(BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json( new { result = new ServiceResponse() { IsSuccessfull = false, ResultMessage = ModelState.FirstOrDefault(m => m.Value.Errors.Any()).Value.Errors.FirstOrDefault().ErrorMessage } });
            }
            return Json(new { result = bookService.SaveBook(bookViewModel) });
        }

        [HttpPost]
        public JsonResult RemoveBook(int? bookId)
        {
            return Json(new { result = bookService.RemoveBook(bookId) });
        }

        [HttpPost]
        public JsonResult SaveAuthor(AuthorViewModel authorViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = new ServiceResponse() { IsSuccessfull = false, ResultMessage = ModelState.FirstOrDefault(m => m.Value.Errors.Any()).Value.Errors.FirstOrDefault().ErrorMessage } });
            }
            return Json( new { result = bookService.SaveAuthor(authorViewModel)});
        }

        [HttpGet]
        public JsonResult GetAuthorsList()
        {
            return Json(new
            {
                authors = bookService.GetAuthorsList(),

            }, JsonRequestBehavior.AllowGet);
        }
    }
}