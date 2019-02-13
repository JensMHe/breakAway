using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakAway.Models.Sites
{
    public class EducationViewModel
    {
        public IReadOnlyList<EducationItem> Educations { get; set; }
    }

    public class EducationItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string InstituteName { get; set; }
    }
}