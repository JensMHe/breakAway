using System;
using System.Text;
using System.Threading.Tasks;

namespace BreakAway.Entities
{
    public class SqlRepository : Repository
    {
        private readonly IBreakAwayContext _context;

        public SqlRepository(IBreakAwayContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;

            _activities = new DbSetTable<Activity>(_context.Activities);
            _contacts = new DbSetTable<Contact>(_context.Contacts);
            _customers = new DbSetTable<Customer>(_context.Customers);
            _destinations = new DbSetTable<Destination>(_context.Destinations);
            _events = new DbSetTable<Event>(_context.Events);
            _equipments = new DbSetTable<Equipment>(_context.Equipments);
            _payments = new DbSetTable<Payment>(_context.Payments);
            _reservations = new DbSetTable<Reservation>(_context.Reservations);
            _lodgings = new DbSetTable<Lodging>(_context.Lodgings);
            _addresses = new DbSetTable<Address>(_context.Addresses);
        }

        private readonly ITable<Activity> _activities;
        private readonly ITable<Contact> _contacts;
        private readonly ITable<Customer> _customers;
        private readonly ITable<Destination> _destinations;
        private readonly ITable<Event> _events;
        private readonly ITable<Equipment> _equipments;
        private readonly ITable<Payment> _payments;
        private readonly ITable<Reservation> _reservations;
        private readonly ITable<Lodging> _lodgings;
        private readonly ITable<Address> _addresses;

        public override ITable<Activity> Activities
        {
            get { return _activities; }
        }

        public override ITable<Contact> Contacts
        {
            get { return _contacts; }
        }

        public override ITable<Customer> Customers
        {
            get { return _customers; }
        }

        public override ITable<Destination> Destinations
        {
            get { return _destinations; }
        }

        public override ITable<Equipment> Equipments
        {
            get { return _equipments; }
        }

        public override ITable<Event> Events
        {
            get { return _events; }
        }

        public override ITable<Payment> Payments
        {
            get { return _payments; }
        }

        public override ITable<Reservation> Reservations
        {
            get { return _reservations; }
        }

        public override ITable<Lodging> Lodgings
        {
            get { return _lodgings; }
        }

        public override ITable<Address> Addresses
        {
            get { return _addresses; }
        }

        public override void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch(Exception e)
            {
                
            }
        }
    }
}
