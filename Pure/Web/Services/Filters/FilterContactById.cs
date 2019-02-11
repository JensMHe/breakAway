using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BreakAway.Entities;

namespace BreakAway.Services.Filters
{
    public class FilterContactById : IFilterBy
    {
        public bool IsAbleToFilter(ContactFilterItem item)
        {
            if (item == null)
                return false;

            if (item.Id != null)
                return true;
            else
                return false;
        }

        public IQueryable<Contact> ExecuteFilter(IQueryable<Contact> query, ContactFilterItem item)
        {
            if (item == null)
                return query;

            if (item.Id != null)
                return query.Where(q => q.Id == item.Id);
            else
                return query;
        }
    }
}