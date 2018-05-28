using Autofac;
using BookCatalog.Data.Contexts;
using BookCatalog.Data.Repositories;
using BookCatalog.Data.Repositories.Interfaces;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;

namespace BookCatalog.Data.Modules
{
    public class DatabaseModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<BookCatalogContext>()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder
                .Register(CreateConnection)
                .InstancePerLifetimeScope();

            builder
                .RegisterType<BookCatalogRepository>()
                .As<IBookCatalogRepository>();
        }

        private static DbConnection CreateConnection(IComponentContext context)
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["BookCatalogContext"].ConnectionString);
        }
    }
}
