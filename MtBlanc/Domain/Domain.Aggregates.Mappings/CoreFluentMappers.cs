using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreakAway.Domain.Core.Sites;
using BreakAway.Domain.Core.Institutes;
using BreakAway.Domain.Core.Educations;
using BreakAway.Domain.Intranet.Users;
using Studentum.Infrastructure.Repository;

namespace BreakAway.Domain
{
    public class CoreFluentMappers : IModelMapperPackage
    {
        public IEnumerable<IEntityModelMapper> GetMappers()
        {
            return new IEntityModelMapper[]
            {
                new SiteModelMapper(),
                new UserModelMapper(),
                new InstituteModelMapper(),
                new EducationModelMapper()
            };
        }
    }
}
