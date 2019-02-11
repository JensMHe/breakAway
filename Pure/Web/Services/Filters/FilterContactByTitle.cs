using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BreakAway.Entities;

namespace BreakAway.Services.Filters
{
    public class FilterContactByTitle : IFilterBy
    {
        public bool IsAbleToFilter(ContactFilterItem item)
        {
            if (item == null)
                return false;

            if (!string.IsNullOrWhiteSpace(item.Title))
                return true;
              else
                return false;
        }

        public IQueryable<Contact> ExecuteFilter(IQueryable<Contact> query, ContactFilterItem item)
        {
            if (item == null)
                return query;

            if (string.IsNullOrWhiteSpace(item.Title))
                return query;
            else
                return query.Where(q => q.Title.Contains(item.Title));
        }
    }
}