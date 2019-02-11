using BreakAway.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakAway.Services.Filters
{
    public interface IFilterBy
    {
        bool IsAbleToFilter(ContactFilterItem item);
        IQueryable<Contact> ExecuteFilter(IQueryable<Contact> query, ContactFilterItem item);
    }
}
