using BreakAway.Entities;
using BreakAway.Models.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BreakAway.Services.Filters;

namespace BreakAway.Services
{
    public class ContactFilterService : IContactFilterService
    {
        private IList<IFilterBy> _filterList;
        public ContactFilterService(IList<IFilterBy> filterList)
        {
            if (filterList == null)
            {
                throw new ArgumentNullException("filterList");
            }

            _filterList = filterList;
        }

        public IQueryable<Contact> FilterContact(IQueryable<Contact> contacts, ContactFilterItem filterItem)
        {
            IQueryable<Contact> query = contacts;
            foreach (var filter in _filterList)
            {
                if (filter.IsAbleToFilter(filterItem))
                {
                    query = filter.ExecuteFilter(query, filterItem);
                }
            }

            return query;
        }
    }
}