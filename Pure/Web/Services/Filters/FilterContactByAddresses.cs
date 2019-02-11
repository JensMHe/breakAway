using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BreakAway.Entities;
using BreakAway.Models.Contact;

namespace BreakAway.Services.Filters
{
    public class FilterContactByAddresses : IFilterBy
    {
        public bool IsAbleToFilter(ContactFilterItem item)
        {
            if (item == null)
                return false;
            else
                return true;
        }

        public IQueryable<Contact> ExecuteFilter(IQueryable<Contact> query, ContactFilterItem item)
        {
            if (item == null)
                return query;

            switch (item.IncludeContacts)
            {
                case AddressFilterOptions.Everyone:
                    return query;
                case AddressFilterOptions.WithAddresses:
                    return query.Where(q => q.Addresses.Count() > 0);
                case AddressFilterOptions.WithoutAddresses:
                    return query.Where(q => q.Addresses.Count() <= 0);
                default:
                    return query;
            }
        }
    }
}