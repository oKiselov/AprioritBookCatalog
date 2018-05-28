using BookCatalog.Data.Entities;
using BookCatalog.Data.Entities.Mapping;
using System.Data.Entity;

namespace BookCatalog.Data.Contexts
{
    public class BookCatalogContext: DbContext
    {
        public BookCatalogContext()
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            SetBookCatalogContextSchema(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SetBookCatalogContextSchema(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AuthorMap());
            modelBuilder.Configurations.Add(new BookMap());

            modelBuilder.Entity<Author>()
                .HasMany(entity => entity.Books)
                .WithMany(entity => entity.Authors)
                .Map(m =>
                {
                    m.ToTable("BooksAuthors");
                    m.MapLeftKey("AuthorId");
                    m.MapRightKey("BookId");
                });

            modelBuilder.Entity<Book>()
                .HasMany(entity => entity.Authors)
                .WithMany(entity => entity.Books)
                .Map(m =>
                {
                    m.ToTable("BooksAuthors");
                    m.MapLeftKey("BookId");
                    m.MapRightKey("AuthorId"); 
                });
        }
    }
}
