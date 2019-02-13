using Studentum.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Studentum.Infrastructure.Collections;
using BreakAway.Domain.Core.Sites;

namespace BreakAway.Domain.Core.Institutes
{
    public class Institute : ICachedEntity
    {
        public int Id { get; protected set; }

        public int Key
        {
            get { return Id; }
        }

        public string Name { get; set; }

        public string WWW { get; set; }

        public string EMail { get; set; }

        public int SiteId { get; set; }

        public virtual InstituteSite Site { get; set; }

        public virtual IList<InstituteLocation> Locations { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
