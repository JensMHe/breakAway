using Studentum.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakAway.Domain.Core.Educations
{
    public class Education : ICachedEntity
    {
        public int Id { get; protected set; }

        public int Key
        {
            get { return Id; }
        }

        public string Name { get; set; }

        public string Link { get; set; }

        public int InstituteId { get; set; }

        public virtual EducationInstitute Institute { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
