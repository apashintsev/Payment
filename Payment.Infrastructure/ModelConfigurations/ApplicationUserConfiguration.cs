using Payment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.Infrastructure.ModelConfigurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x=>x.Id).IsRequired().ValueGeneratedNever();
        builder.HasOne<Merchant>()
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.UserId)
                .IsRequired();

    }
}
