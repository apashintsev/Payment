using Payment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;

namespace Payment.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserAccount> UsersAccount { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Activity> Activitys { get; set; }
    public DbSet<Referral> Referrals { get; set; }
    public DbSet<ReferralProfit> Profits { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }
    public DbSet<WithdrawalHistory> WithdrawalHistories { get; set; }
    public DbSet<PlatformSettings> PlatformSettings { get; set; }
    public DbSet<Merchant> Merchants { get; set; }
    public DbSet<ClientWallet> ClientWallets { get; set; }

    public DbSet<Domain.Entities.Payment> Payments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Configure primary key as Guid for all entities
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.BaseType == null && entityType.FindPrimaryKey() != null)
            {
                entityType.FindPrimaryKey().Properties
                    .Where(p => p.ClrType == typeof(Guid))
                    .ToList()
                    .ForEach(p => p.ValueGenerated = ValueGenerated.OnAdd);
            }
        }

        builder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            t => t.GetInterfaces().Any(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
        );
        base.OnModelCreating(builder);
    }
}
