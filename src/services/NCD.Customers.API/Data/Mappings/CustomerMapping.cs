using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCD.Customers.API.Model;
using NCD.Customers.API.Model.ValueObjects;

namespace NCD.Costumers.API.Data.Mappings
{
    public class CostumerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(c => c.Document)
                  .IsRequired()
                  .HasColumnType("varchar(100)");

            builder.OwnsOne(c => c.Email, tf =>
            {
                tf.Property(c => c.EmailAddress)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.EmailAddressMaxLength})");
            }).Navigation(c => c.Email).IsRequired();

            // 1 : 1 => Aluno : Endereco
            builder.HasOne(c => c.Address)
                .WithOne(c => c.Customer);

            builder.ToTable("Customers");
        }
    }
}
