using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakAway.Models.Sites
{
    public class InstituteViewModel
    {
        public IReadOnlyList<InstituteItem> Institutes { get; set; }
    }

    public class InstituteItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public string SiteName { get; set; }

    }
}