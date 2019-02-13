using Studentum.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace BreakAway.Domain.Core.Institutes
{
    public class InstituteModelMapper: IEntityModelMapper
    {
        public ProviderInfo Provider
        {
            get { return ProviderInfo.SqlServer2008; }
        }

        public string AggregateName { get { return nameof(Institute); } }

        public void Map(DbModelBuilder modelBuilder)
        {
            MapInstitute(modelBuilder.Entity<Institute>());
            MapInstituteSites(modelBuilder.Entity<InstituteSite>());
            MapInstituteLocation(modelBuilder.Entity<InstituteLocation>());
        }

        private void MapInstitute(EntityTypeConfiguration<Institute> entity)
        {
            entity.HasKey(p => p.Id).ToTable("Institutes", "institute");

            entity.Property(p => p.Id).HasColumnName("ID");

            entity.Property(p => p.Name);

            entity.Property(p => p.WWW);

            entity.Property(p => p.EMail);

            entity.Property(p => p.SiteId).HasColumnName("FKSiteID").IsRequired();

            entity.HasRequired(p => p.Site).WithMany().HasForeignKey(i => i.SiteId);

            entity.HasMany(p => p.Locations).WithRequired().HasForeignKey(i => i.InstituteId);
        }

        private void MapInstituteSites(EntityTypeConfiguration<InstituteSite> entity)
        {
            entity.HasKey(p => p.Id).ToTable("Sites", "site");

            entity.Property(p => p.Id).HasColumnName("ID");

            entity.Property(p => p.Domain);
        }

        private void MapInstituteLocation(EntityTypeConfiguration<InstituteLocation> entity)
        {
            entity.HasKey(p => p.Id).ToTable("InstituteLocation", "institute");

            entity.Property(p => p.Id).HasColumnName("ID");

            entity.Property(p => p.Name);
        }
    }
}
