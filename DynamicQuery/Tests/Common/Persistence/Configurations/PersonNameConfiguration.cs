using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tests.Common.Model;

namespace Tests.Common.Persistence.Configurations
{
    public class PersonNameConfiguration : IEntityTypeConfiguration<PersonName>
    {
        public void Configure(EntityTypeBuilder<PersonName> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();
        }
    }
}
