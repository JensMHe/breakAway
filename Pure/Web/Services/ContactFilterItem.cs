using BreakAway.Models.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BreakAway.Services
{
    public class ContactFilterItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public int? Id { get; set; }
        public AddressFilterOptions IncludeContacts { get; set; }
    }
}