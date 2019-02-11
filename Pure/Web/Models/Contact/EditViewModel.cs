using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BreakAway.Entities;
using System.ComponentModel.DataAnnotations;

namespace BreakAway.Models.Contact
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public List<AddressItem> Addresses { get; set; }
    }

    public class AddressItem
    {
        public int Id { get; set; }
        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string City { get; set; }

        //[Required(ErrorMessage = "Address type must be specified")]
        public string AddressType { get; set; }
    }
}