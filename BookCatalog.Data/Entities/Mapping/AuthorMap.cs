using System.Data.Entity.ModelConfiguration;

namespace BookCatalog.Data.Entities.Mapping
{
    public class AuthorMap: EntityTypeConfiguration<Author>
    {
        public AuthorMap()
        {
            ToTable("Authors")
                .HasKey(entity => entity.Id);

            Property(entity => entity.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(entity => entity.FirstName)
                .HasColumnName("FirstName")
                .IsRequired();

            Property(entity => entity.LastName)
                .HasColumnName("LastName")
                .IsRequired();
        }
    }
}
