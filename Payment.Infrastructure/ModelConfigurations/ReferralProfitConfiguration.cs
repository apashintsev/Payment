using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.ModelConfigurations
{
    public class ReferralProfitConfiguration : IEntityTypeConfiguration<ReferralProfit>
    {
        public void Configure(EntityTypeBuilder<ReferralProfit> builder)
        {
            builder
                .HasOne(rp => rp.Merchant)
                .WithMany(u => u.ReferralProfits)
                .HasForeignKey(rp => rp.MerchantId);

            builder
                .HasOne(rp => rp.Referral)
                .WithMany()
                .HasForeignKey(rp => rp.ReferralId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(rp => rp.Id).IsRequired().ValueGeneratedOnAdd();
        }
    }
}
