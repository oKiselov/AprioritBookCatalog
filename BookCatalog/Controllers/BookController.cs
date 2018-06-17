using BookCatalog.Models;
using BookCatalog.Services.Interfaces;
using BookCatalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
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
                iTotalRecords = bookService.GetTotalBooksAmount(paginationModel.sSearch_2),
                iTotalDisplayRecords = bookService.GetTotalBooksAmount(paginationModel.sSearch_2),
                aaData = bookService.GetFilteredBooks(paginationModel)
            });
        }

        [HttpPost]
        public JsonResult SaveBook(BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
            {
                //return Json(new { result = new ServiceResponse() { IsSuccessfull = false, ResultMessage = Resources.Resources.ErrorOccured } });
                return Json( new { result = new ServiceResponse() { IsSuccessfull = false, ResultMessage = ModelState.FirstOrDefault(m => m.Value.Errors.Any()).Value.Errors.FirstOrDefault().ErrorMessage } });
            }
            return Json(new { result = bookService.SaveBook(bookViewModel) });
        }

        [HttpPost]
        public JsonResult RemoveBook(int bookId)
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

        //    return Json(new
        //    {
        //        IsSuccess = true,
        //        sEcho = pager.sEcho,
        //        iTotalRecords = int.MaxValue,
        //        iTotalDisplayRecords = int.MaxValue,
        //        aaData = searchResponse.Result.MaskProviderInfo(this.RequestContext.IsUSIP),
        //        IsRedirectRequired = searchResponse.IsRedirectRequired
        //    }, JsonRequestBehavior.AllowGet);

        [HttpGet]
        public JsonResult GetAuthorsList()
        {
            return Json(new
            {
                authors = bookService.GetAuthorsList(),

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void Save()
        {
            //var firstA = get model by id;

            //if (firstA == null)
            //{
            //    throw new KeyNotFoundException(string.Format("Contact #{0} not found"));
            //}

            //Mapper.Map<fromClient, fromServer>(firstAFromClient, firstA);

            //using (var tran = new TransactionScope())
            //{

            //Save();
            //    LogChanges();
            //    tran.Complete();
            //}
        }



        //Save for Controller 
        //if (ModelState.IsValid)
        //    {
        //        var getService from Autofac;
        //service.Update(fromClient);
        //        return JsonData(isSuccess: true, message: "Updated successfully", data: new
        //            {
        //                
        //            });
        //    }

        //    return JsonData(ModelState);
    }
}