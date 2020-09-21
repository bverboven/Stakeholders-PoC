using Microsoft.EntityFrameworkCore;
using System.Linq;
using Regira.Stakeholders.Core.Entities;

namespace Regira.Stakeholders.Library.Data
{
    public class StakeholdersContext : DbContext
    {
        public StakeholdersContext(DbContextOptions<StakeholdersContext> options)
            : base(options)
        {
        }

        #region Entities
        public DbSet<Stakeholder> Stakeholders { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ContactRole> ContactRoles { get; set; }
        public DbSet<StakeholderContact> StakeholderContacts { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSnakeCaseNamingConvention();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Stakeholder
            modelBuilder.Entity<Stakeholder>(entity =>
            {
                entity
                    .HasDiscriminator(p => p.StakeholderType)
                    .HasValue<Organization>(StakeholderType.Organization)
                    .HasValue<Person>(StakeholderType.Person);
                entity.OwnsOne(e => e.Address, cb =>
                {
                    var prefix = "address";
                    cb.Property(a => a.Box).HasColumnName($"{prefix}_box");
                    cb.Property(a => a.City).HasColumnName($"{prefix}_city");
                    cb.Property(a => a.CountryCode).HasColumnName($"{prefix}_country_code");
                    cb.Property(a => a.Number).HasColumnName($"{prefix}_number");
                    cb.Property(a => a.PostBox).HasColumnName($"{prefix}_post_box");
                    cb.Property(a => a.PostalCode).HasColumnName($"{prefix}_postal_code");
                    cb.Property(a => a.Street).HasColumnName($"{prefix}_street");
                });
                entity.OwnsMany(e => e.BankAccounts, cb =>
                {
                    var prefix = "account";
                    cb.Property(a => a.AccountType)
                        //.HasConversion(new EnumToStringConverter<AccountType>())
                        .HasColumnName($"{prefix}_type");
                    cb.Property(a => a.Number).HasColumnName($"{prefix}_number");
                });
                entity.HasMany(e => e.Superiors)
                    .WithOne(c => c.RoleBearer)
                    .HasForeignKey(s => s.RoleBearerId);
                entity.HasMany(e => e.Subordinates)
                    .WithOne(c => c.RoleGiver)
                    .HasForeignKey(s => s.RoleGiverId);
            });
            // Organization
            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasBaseType<Stakeholder>();
                entity.HasIndex(e => e.Kbo);
            });
            // Person
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasBaseType<Stakeholder>();
                entity.HasIndex(e => e.Kbo);
            });
            // OrganizationContact
            modelBuilder.Entity<StakeholderContact>(entity =>
            {
                entity.HasKey(e => new { e.RoleGiverId, e.RoleBearerId, e.RoleId });
                //entity.HasOne(e => e.RoleBearer)
                //    .WithMany(s => s.Superiors)
                //    .HasForeignKey(s => s.RoleBearerId);
                //entity.HasOne(e => e.RoleGiver)
                //    .WithMany(s => s.Subordinates)
                //    .HasForeignKey(s => s.RoleGiverId);
            });

            // https://stackoverflow.com/questions/43277154/entity-framework-core-setting-the-decimal-precision-and-scale-to-all-decimal-p#answer-43282620
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .ToArray();
            foreach (var property in entityTypes
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)))
            {
                property.SetColumnType("decimal(18, 4)");
            }
        }
    }
}
