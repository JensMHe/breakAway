using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AutoFixture;
using BreakAway.Entities;
using BreakAway.Services.Filters;
using BreakAway.Services;

namespace Test.Web.Filters
{
    [TestFixture]
    class FilterContactByIdTest
    {
        private IFixture _fixture;
        private List<Contact> _contacts;
        private FilterContactById _sut;
        private int _id;
        private int _invalidId;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            var errorList = _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList();
            errorList.ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _contacts = _fixture.CreateMany<Contact>(50).ToList();

            _id = Convert.ToInt32(_fixture.Create<Generator<int>>()
                .FirstOrDefault(n => !_contacts.Exists(c => c.Id == n)));

            _contacts.Add(_fixture.Build<Contact>().With(i => i.Id, _id).Create());

            _sut = new FilterContactById();

            _invalidId = Convert.ToInt32(_fixture.Create<Generator<int>>()
                .FirstOrDefault(n => !_contacts.Exists(c => c.Id == n) && n != _id));
        }

        [Test]
        public void IsAbleToFilter__Check_If_Able_To_Filter__Id()
        {
            // ARRANGE
            var item = _fixture.Create<ContactFilterItem>();

            // ACT
            var result = _sut.IsAbleToFilter(item);

            // ASSERT
            Assert.IsTrue(result);
        }

        [Test]
        public void IsAbleToFilter__Check_If_Unable_To_Filter_When_ContactFilterItem_Is_Null__Id()
        {
            // ARRANGE

            // ACT
            var result = _sut.IsAbleToFilter(null);

            // ASSERT
            Assert.True(result == false);
        }

        [Test]
        public void IsAbleToFilter__Check_If_Unable_To_Filter_When_Id_Is_Null__Id()
        {
            // ARRANGE
            var item = _fixture.Build<ContactFilterItem>().Without(i => i.Id).Create();

            // ACT
            var result = _sut.IsAbleToFilter(item);

            // ASSERT
            Assert.IsTrue(!result);
        }

        [Test]
        public void ExecuteFilter__Check_If_It_Return_Correct_When_Name_Is_Valid__Id()
        {
            // ARRANGE
            var queryList = _contacts;
            var contact = queryList.FirstOrDefault(c => c.Id == _id);

            var item = _fixture.Build<ContactFilterItem>().With(i => i.Id, _id).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), item);

            // ASSERT
            Assert.True(query.ToArray()[0].Id == contact.Id);

        }

        [Test]
        public void ExecuteFilter__Check_If_Return_When_ContactFilterItem_Is_Null__Id()
        {
            // ARRANGE
            var queryList = _contacts;

            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), null);

            // ASSERT
            Assert.True(query.ToArray().Length > 1);

        }

        [Test]
        public void ExecuteFilter__Check_If_Return_All_Contacts_When_Id_Is_Null__Id()
        {
            // ARRANGE
            var queryList = _contacts;
            var item = _fixture.Build<ContactFilterItem>().Without(i => i.Id).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), item);

            // ASSERT
            Assert.True(query.ToArray().Length > 1);

        }

        [Test]
        public void ExecuteFilter__Check_If_Returning_All_Contacts_When_Id_Is_Missing__Id()
        {
            // ARRANGE
            var queryList = _contacts.AsQueryable();
            var item = _fixture.Build<ContactFilterItem>().Without(i => i.Id).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList, item);
            var array = query.ToArray();

            // ASSERT
            Assert.True(array.Length > 1);
        }

        [Test]
        public void ExecuteFilter__Check_Not_Returning_Contacts_When_Letter_Is_Invalid__Id()
        {
            // ARRANGE
            var queryList = _contacts.AsQueryable();

            var item = _fixture.Build<ContactFilterItem>().With(i => i.Id, _invalidId).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList, item);

            // ASSERT
            Assert.True(query.ToArray().Length == 0);
        }
    }
}
