using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentum.Infrastructure.Repository;

namespace BreakAway.Domain.Core.Institutes
{
    public interface IInstituteRepository: IRepository<Institute>
    {

    }

    public class InstituteRepository: RepositoryBase<Institute>, IInstituteRepository
    {
        public InstituteRepository(IRepositoryProviderBase repositoryProvider) : base(repositoryProvider) { }
    }
}
