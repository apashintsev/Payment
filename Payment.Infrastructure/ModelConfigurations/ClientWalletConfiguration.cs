using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.ModelConfigurations
{
    public class ClientWalletConfiguration : IEntityTypeConfiguration<ClientWallet>
    {
        public void Configure(EntityTypeBuilder<ClientWallet> builder)
        {
            builder.Property(rp => rp.Id).IsRequired().ValueGeneratedOnAdd();

            builder
                .HasOne(r => r.Merchant)
                .WithMany(u => u.ClientWallets)
                .HasForeignKey(r => r.MerchantId);
        }
    }
}
