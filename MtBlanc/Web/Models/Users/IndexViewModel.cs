using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakAway.Models.Users
{
    public class IndexViewModel
    {
        public IReadOnlyList<UserModel> Users { get; set; }
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email  { get; set; }
        public string Site { get; set; }
        public IReadOnlyList<RolesListItem> Roles { get; set; }
    }

    public class RolesListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}