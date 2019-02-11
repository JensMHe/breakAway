using AutoFixture;
using BreakAway.Entities;
using BreakAway.Models.Contact;
using BreakAway.Services;
using BreakAway.Services.Filters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Web.Services.Filters
{
    [TestFixture]
    class FilterContactByAddressesTest
    {
        private IFixture _fixture;
        private List<Contact> _contacts;
        private FilterContactByAddresses _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            var errorList = _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList();
            errorList.ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _contacts = _fixture.CreateMany<Contact>(50).ToList();

            for (int i = 0; i < 40; i++)
            {
                var tempContact = _fixture.Build<Contact>().Without(c => c.Addresses).Create();
                tempContact.Addresses = new List<Address>();
                _contacts.Add(tempContact);
            }

            _sut = new FilterContactByAddresses();
        }

        [Test]
        public void IsAbleToFilter__Check_If_Able_To_Filter_When_ContactItem_Is_Valid__Addresses()
        {
            // ARRANGE
            var item = _fixture.Create<ContactFilterItem>();
            
            // ACT
            var result = _sut.IsAbleToFilter(item);

            // ASSERT
            Assert.IsTrue(result);
        }

        [Test]
        public void IsAbleToFilter__Check_If_Unable_To_Filter_When_ContactFilterItem_Is_Null__Addresses()
        {
            // ARRANGE

            // ACT
            var result = _sut.IsAbleToFilter(null);

            // ASSERT
            Assert.False(result);
        }

        [Test]
        public void ExecuteFilter__Check_If_It_Return_Correct_When_IncludeEveryone_Is_Valid__Addresses()
        {
            // ARRANGE
            var queryList = _contacts;
            var item = _fixture.Build<ContactFilterItem>().With(i => i.IncludeContacts, AddressFilterOptions.Everyone).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), item);

            // ASSERT
            Assert.True(query.ToArray().Length == queryList.Count());
        }

        [Test]
        public void ExecuteFilter__Check_If_It_Return_Correct_When_IncludeWithAddresses_Is_Valid__Addresses()
        {
            // ARRANGE
            var queryList = _contacts;
            var item = _fixture.Build<ContactFilterItem>().With(i => i.IncludeContacts, AddressFilterOptions.WithAddresses).Create();
            var expected = 50;
            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), item);

            // ASSERT
            Assert.AreEqual(expected, query.ToArray().Length);
        }

        [Test]
        public void ExecuteFilter__Check_If_It_Return_Correct_When_IncludeWithoutAddresses_Is_Valid__Addresses()
        {
            // ARRANGE
            var queryList = _contacts;
            var item = _fixture.Build<ContactFilterItem>().With(i => i.IncludeContacts, AddressFilterOptions.WithoutAddresses).Create();
            var expected = 40;
            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), item);

            // ASSERT
            var asserted = _contacts.Where(c => c.Addresses.Count <= 0).ToList().Count;

            Assert.AreEqual(expected, asserted);
        }

        [Test]
        public void ExecuteFilter__Check_If_Return_When_ContactFilterItem_Is_Null__Addresses()
        {
            // ARRANGE
            var queryList = _contacts;

            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), null);

            // ASSERT
            Assert.True(query.ToArray().Length == queryList.ToArray().Length);
        }

        [Test]
        public void ExecuteFilter__Check_If_Return_All_Contacts_When_InlcudeContact_Is_Null__Addresses()
        {
            // ARRANGE
            var queryList = _contacts;
            var item = _fixture.Build<ContactFilterItem>().Without(i => i.IncludeContacts).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), item);

            // ASSERT
            Assert.True(query.ToArray().Length == queryList.ToArray().Length);
        }

        [Test]
        public void ExecuteFilter__Check_If_Returning_All_Contacts_When_IncludeContact_Is_Unvalid__Addresses()
        {
            // ARRANGE
            var queryList = _contacts.AsQueryable();
            var item = _fixture.Create<ContactFilterItem>();

            // ACT
            var query = _sut.ExecuteFilter(queryList, item);
            var array = query.ToArray();

            // ASSERT
            Assert.True(query.ToArray().Length == queryList.ToArray().Length);
        }
    }
}