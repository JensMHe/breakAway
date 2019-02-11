using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BreakAway.Models.Contact
{
    public class IndexViewModel
    {
        public ContactItem[] Contacts { get; set; }
        public Search Search { get; set; }
        public int NumberOfResults { get; set; }
    }

    public class ContactItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class Search
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<SelectListItem> SelectListAddresses { get; set; }
        public AddressFilterOptions SelectedAddressesValue { get; set; }

        public Search()
        {
            SelectListAddresses = new List<SelectListItem>
            {
                new SelectListItem {Text="Everyone",Value=AddressFilterOptions.Everyone.ToString() },
                new SelectListItem {Text="With addresses",Value=AddressFilterOptions.WithAddresses.ToString() },
                new SelectListItem {Text="Without addresses",Value=AddressFilterOptions.WithoutAddresses.ToString() }
            };
        }
    }

    public enum AddressFilterOptions
    {
        Everyone,
        WithAddresses,
        WithoutAddresses
    }
}