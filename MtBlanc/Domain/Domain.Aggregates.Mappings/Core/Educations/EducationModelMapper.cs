using Studentum.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace BreakAway.Domain.Core.Educations
{
    public class EducationModelMapper : IEntityModelMapper
    {
        public ProviderInfo Provider
        {
            get { return ProviderInfo.SqlServer2008; }
        }

        public string AggregateName { get { return nameof(Education); } }

        public void Map(DbModelBuilder modelBuilder)
        {
            MapEducation(modelBuilder.Entity<Education>());
            MapEducationInstitute(modelBuilder.Entity<EducationInstitute>());
        }

        private void MapEducation(EntityTypeConfiguration<Education> entity)
        {
            entity.HasKey(p => p.Id).ToTable("Educations", "education");

            entity.Property(p => p.Id).HasColumnName("ID");

            entity.Property(p => p.Name);

            entity.Property(p => p.Link);

            entity.Property(p => p.InstituteId).HasColumnName("FKInstituteID");
        }

        private void MapEducationInstitute(EntityTypeConfiguration<EducationInstitute> entity)
        {
            entity.HasKey(p => p.Id).ToTable("Institutes", "institute");

            entity.Property(p => p.Id).HasColumnName("ID");

            entity.Property(p => p.Name);
        }
    }
}