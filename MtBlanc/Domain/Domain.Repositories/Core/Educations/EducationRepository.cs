using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentum.Infrastructure.Repository;

namespace BreakAway.Domain.Core.Educations
{
    public interface IEducationRepository : IRepository<Education>
    {

    }

    public class EducationRepository : RepositoryBase<Education>, IEducationRepository    {
        public EducationRepository(IRepositoryProviderBase repositoryProvider) : base(repositoryProvider) { }
    }
}