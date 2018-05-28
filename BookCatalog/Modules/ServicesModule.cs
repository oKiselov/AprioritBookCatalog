using Autofac;
using BookCatalog.Services;
using BookCatalog.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookCatalog.Modules
{
    public class ServicesModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<BookService>()
                .As<IBookService>();
        }
    }
}