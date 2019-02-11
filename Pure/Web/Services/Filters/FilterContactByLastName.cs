using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BreakAway.Entities;

namespace BreakAway.Services.Filters
{
    public class FilterContactByLastName : IFilterBy
    {
        public bool IsAbleToFilter(ContactFilterItem item)
        {
            if (item == null)
                return false;

            if (!string.IsNullOrWhiteSpace(item.LastName))
                return true;
            else
                return false;
        }

        public IQueryable<Contact> ExecuteFilter(IQueryable<Contact> query, ContactFilterItem item)
        {
            if (item == null)
                return query;

            if (string.IsNullOrWhiteSpace(item.LastName))
                return query;
            else
                return query.Where(q => q.LastName.Contains(item.LastName));
        }
    }
}