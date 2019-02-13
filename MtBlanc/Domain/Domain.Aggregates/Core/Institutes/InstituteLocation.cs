using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakAway.Domain.Core.Institutes
{
    public class InstituteLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InstituteId { get; set; }
    }
}
