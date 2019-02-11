using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreakAway.Entities;
using BreakAway.Models.Contact;

namespace BreakAway.Services
{
    public interface IContactFilterService
    {
        IQueryable<Contact> FilterContact(IQueryable<Contact> contacts, ContactFilterItem filterItem);
    }
}
