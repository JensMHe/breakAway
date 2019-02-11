using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BreakAway.Models.Contact;
using BreakAway.Entities;
using System.Globalization;
using BreakAway.Services;

namespace BreakAway.Controllers
{
    public class ContactController : Controller
    {
        private readonly Repository _repository;
        private readonly IContactFilterService _contactFilterService;

        public ContactController(Repository repository, IContactFilterService contactFilterService)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            if (contactFilterService == null)
            {
                throw new ArgumentNullException("filterService");
            }

            _repository = repository;
            _contactFilterService = contactFilterService;
        }

        [HttpGet]
        public ActionResult Index(string message, IndexViewModel model)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.message = message;
            }

            if (model.Search == null)
            {
                model.Search = new Search();
            }

            var query = _contactFilterService.FilterContact(_repository.Contacts,
                new ContactFilterItem
                {
                    FirstName = model.Search.FirstName,
                    LastName = model.Search.LastName,
                    Title = model.Search.Title,
                    Id = model.Search.Id,
                    IncludeContacts = model.Search.SelectedAddressesValue
                });

            var filteredContacts = query.Select(contact => new ContactItem
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Title = contact.Title,
                AddedDate = contact.AddDate,
                ModifiedDate = contact.ModifiedDate
            });

            model = new IndexViewModel
            {
                Contacts = filteredContacts.ToArray(),
                Search = new Search
                {
                    Title = model.Search.Title,
                    FirstName = model.Search.FirstName,
                    LastName = model.Search.LastName,
                    Id = model.Search.Id,
                    SelectedAddressesValue = model.Search.SelectedAddressesValue
                }
            };

            model.NumberOfResults = model.Contacts.Length;

            return View(model);
        }

        public ActionResult Edit(int id, string message)
        {
            if (!string.IsNullOrEmpty(message))
                ViewBag.message = message;

            var contact = _repository.Contacts.FirstOrDefault(c => c.Id == id);

            var viewModel = new EditViewModel
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Title = contact.Title,
            };

            var list = new List<AddressItem>();

            if (contact.Addresses != null)
            {
                foreach (var address in contact.Addresses)
                {
                    list.Add(new AddressItem
                    {
                        Id = address.Id,
                        Street1 = address.Mail.Street1,
                        Street2 = address.Mail.Street2,
                        City = address.Mail.City,
                        AddressType = address.AddressType
                    });
                }
            }

            viewModel.Addresses = list;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", "Contact", model);
            }

            var message = "";
            var succeeded = true;
            var count = 0;

            if (model.Addresses == null)
            {
                model.Addresses = new List<AddressItem>();
            }

            var contact = _repository.Contacts.FirstOrDefault(c => c.Id == model.Id);

            if (contact == null)
            {
                return RedirectToAction("Index", "Contact", new { message = "Contact with id '" + model.Id + "' was not found" });
            }

            if(contact.Addresses == null)
            {
                contact.Addresses = new List<Address>();
            }

            // UPDATE CONTACT INFO
            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.Title = model.Title;
            contact.ModifiedDate = DateTime.Now;

            // ADD ADDRESSES
            count = 0;
            var addList = model.Addresses
                .FindAll(a => !contact.Addresses.ToList().Exists(b => b.Id == a.Id));

            foreach (var address in addList)
            {
                if (string.IsNullOrEmpty(address.AddressType) ||
                    string.IsNullOrWhiteSpace(address.AddressType))
                {
                    succeeded = false;
                    count++;
                }
                else
                {
                    contact.Addresses.Add(new Address
                    {
                        ContactId = contact.Id,
                        AddressType = address.AddressType,
                        ModifiedDate = DateTime.Now,
                        Mail = new Mail
                        {
                            Street1 = address.Street1,
                            Street2 = address.Street2,
                            City = address.City
                        }
                    });
                }
            }

            if (count > 0)
                message += " * " + count + " address/addresses could not be saved * ";

            // UPDATE ADDRESSES
            count = 0;
            foreach (var address in model.Addresses)
            {
                var addr = contact.Addresses.FirstOrDefault(a => a.Id == address.Id);
                if (addr != null && address.Id != 0)
                {
                    if (string.IsNullOrEmpty(address.AddressType) ||
                        string.IsNullOrWhiteSpace(address.AddressType))
                    {
                        succeeded = false;
                        count++;
                    }
                    else
                    {
                        addr.AddressType = address.AddressType;
                        addr.Mail.Street1 = address.Street1;
                        addr.Mail.Street2 = address.Street2;
                        addr.Mail.City = address.City;
                    }
                }
            }
            if (count > 0)
                message += " * " + count + " address/addresses could not be updated * ";

            // REMOVE ADDRESSES
            count = 0;
            var removeList = contact.Addresses.ToList()
                .FindAll(a => !model.Addresses.Exists(b => b.Id == a.Id));

            foreach (var address in removeList)
            {
                if (!contact.Addresses.ToList().Exists(a => a.Id == address.Id))
                {
                    succeeded = false;
                    count++;
                }
                else
                {
                    var addr = contact.Addresses.FirstOrDefault(a => a.Id == address.Id);
                    _repository.Addresses.Delete(address);
                }
            }
            if (count > 0)
                message += " * " + count + " address/addresses could not be deleted * ";

            _repository.Save();

            if (succeeded)
            {
                message = "All changes saved successfully";
            }

            return RedirectToAction("Edit", "Contact", new { id = contact.Id, message = message });
        }

        public ActionResult Add()
        {
            return View(new AddViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Add", "Contact", new { message = "Contact not created" });
            }

            var contact = new Contact
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Title = model.Title,
                ModifiedDate = DateTime.Now,
                AddDate = DateTime.Now
            };

            _repository.Contacts.Add(contact);
            _repository.Save();

            return RedirectToAction("Index", "Contact", new { message = "Contact added successfully" });
        }

        public ActionResult Delete(int id)
        {
            var contact = _repository.Contacts.FirstOrDefault(c => c.Id == id);

            if (contact == null)
            {
                return RedirectToAction("Index", "Contact", new { message = "Unable to delete contact with id: " + id });
            }

            if (contact.Addresses != null)
            {
                foreach (var address in contact.Addresses.ToList())
                {
                    _repository.Addresses.Delete(address);
                }
            }

            _repository.Contacts.Delete(contact);

            _repository.Save();

            return RedirectToAction("Index", "Contact", new { message = "Contact deleted successfully" });
        }
    }
}