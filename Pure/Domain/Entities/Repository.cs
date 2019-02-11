using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakAway.Entities
{
    public abstract class Repository
    {
        public abstract ITable<Activity> Activities { get; }
        public abstract ITable<Contact> Contacts { get; }
        public abstract ITable<Customer> Customers { get; }
        public abstract ITable<Destination> Destinations { get; }
        public abstract ITable<Equipment> Equipments { get; }
        public abstract ITable<Payment> Payments { get; }
        public abstract ITable<Reservation> Reservations { get; }
        public abstract ITable<Lodging> Lodgings { get; }
        public abstract ITable<Event> Events { get; }
        public abstract ITable<Address> Addresses { get; }

        public abstract void Save();
    }

    public interface ITable<T> : IQueryable<T> where T : class
    {
        void Add(T item);

        void Delete(T item);
    }
}
