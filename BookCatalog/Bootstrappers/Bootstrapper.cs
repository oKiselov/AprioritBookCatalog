using Autofac;
using Autofac.Integration.Mvc;
using BookCatalog.Data.Entities;
using BookCatalog.ViewModels;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;

namespace BookCatalog.Bootstrappers
{
    public static class Bootstrapper
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterControllers(typeof(MvcApplication).Assembly);

            builder
                .RegisterFilterProvider();

            builder
                .RegisterSource(new ViewRegistrationSource());

            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();

            builder
                .RegisterAssemblyModules(assemblies);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static void InitializeModelMapping()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Book, BookSearchViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.PublishingYear, opt => opt.MapFrom(src => src.PublishingYear))
                .ForMember(dest => dest.PagesAmount, opt => opt.MapFrom(src => src.PagesAmount))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
                .ForMember(dest => dest.AuthorsCollection, opt => opt.MapFrom(src => src.Authors));

                cfg.CreateMap<Author, AuthorSearchViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

                cfg.CreateMap<BookViewModel, Book>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PagesAmount, opt => opt.MapFrom(src => src.PagesAmount))
                .ForMember(dest => dest.PublishingYear, opt => opt.MapFrom(src => src.PublishingYear.Year))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate));
            });

        }
    }
}