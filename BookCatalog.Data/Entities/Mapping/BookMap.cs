using System.Data.Entity.ModelConfiguration;

namespace BookCatalog.Data.Entities.Mapping
{
    public class BookMap: EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            ToTable("Books")
                .HasKey(entity => entity.Id);

            Property(entity => entity.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(entity => entity.Title)
                .HasColumnName("Title")
                .IsRequired();

            Property(entity => entity.PublishingYear)
                .HasColumnName("PublishingYear")
                .IsRequired();

            Property(entity => entity.PagesAmount)
                .HasColumnName("PagesAmount")
                .IsRequired();

            Property(entity => entity.Rate)
                .HasColumnName("Rate")
                .IsRequired();
        }
    }
}
