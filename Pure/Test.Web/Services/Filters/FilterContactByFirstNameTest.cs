﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BreakAway.Services.Filters;
using AutoFixture;
using BreakAway.Services;
using BreakAway.Entities;

namespace Test.Web.Filters
{
    [TestFixture]
    class FilterContactByFirstNameTest
    {
        private IFixture _fixture;
        private List<Contact> _contacts;
        private FilterContactByFirstName _sut;
        private string _text1;
        private string _text2;
        private char _letter;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            var errorList = _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList();
            errorList.ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _letter = _fixture.Create<char>();

            _text1 = _fixture.Create<Generator<string>>().FirstOrDefault(s => !s.Contains(_letter));
            _text2 = _fixture.Create<Generator<string>>().FirstOrDefault(s => !s.Contains(_letter) && s != _text1);

            _contacts = _fixture.CreateMany<Contact>(50).ToList();
            _contacts.ForEach(c => c.FirstName = _fixture.Create<Generator<string>>().FirstOrDefault(s => !s.Contains(_letter)));
            _contacts.Add(_fixture.Build<Contact>().With(i => i.FirstName, _text1).Create());
            _contacts.Add(_fixture.Build<Contact>().With(i => i.FirstName, _text2).Create());

            _sut = new FilterContactByFirstName();
        }

        [Test]
        public void IsAbleToFilter__Check_If_Able_To_Filter__FirstName()
        {
            // ARRANGE
            var item = _fixture.Create<ContactFilterItem>();

            // ACT
            var result = _sut.IsAbleToFilter(item);

            // ASSERT
            Assert.IsTrue(result);
        }

        [Test]
        public void IsAbleToFilter__Check_If_UnAbleToFilter_When_Name_Is_Null__FirstName()
        {
            // ARRANGE
            var item = _fixture.Build<ContactFilterItem>().Without(i => i.FirstName).Create();

            // ACT
            var result = _sut.IsAbleToFilter(item);

            // ASSERT
            Assert.True(result == false);
        }

        [Test]
        public void IsAbleToFilter__Check_If_Unable_To_Filter_When_ContactFilterItem_Is_Null__FirstName()
        {
            // ARRANGE

            // ACT
            var result = _sut.IsAbleToFilter(null);

            // ASSERT
            Assert.True(result == false);
        }

        [Test]
        public void IsAbleToFilter__Check_If_UnAbleToFilter_WhiteSpaces_In_Name__FirstName()
        {
            // ARRANGE
            var item = _fixture.Build<ContactFilterItem>().With(i => i.FirstName, " ").Create();

            // ACT
            var result = _sut.IsAbleToFilter(item);

            // ASSERT
            Assert.True(result == false);
        }

        [Test]
        public void ExecuteFilter__Check_If_It_Return_Correct_When_Name_Is_Valid__FirstName()
        {
            // ARRANGE
            var queryList = _contacts;
            var contact = queryList.FirstOrDefault(c => c.FirstName == _text1);

            var item = _fixture.Build<ContactFilterItem>().With(i => i.FirstName, _text1).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), item);

            // ASSERT
            Assert.True(query.ToArray()[0].Id == contact.Id);

        }

        [Test]
        public void ExecuteFilter__Check_If_Return_Correct_With_Letter_And_Returning_Multiple_Contacts__FirstName()
        {
            // ARRANGE
            var queryList = _contacts;

            var item = _fixture.Build<ContactFilterItem>().With(i => i.FirstName, _text1.Substring(0,1)).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), item);

            // ASSERT
            Assert.True(query.ToArray().Length > 1);

        }

        [Test]
        public void ExecuteFilter__Check_If_Return_When_ContactFilterItem_Is_Null__FirstName()
        {
            // ARRANGE
            var queryList = _contacts;

            // ACT
            var query = _sut.ExecuteFilter(queryList.AsQueryable(), null);

            // ASSERT
            Assert.True(query.ToArray().Length > 1);

        }

        [Test]
        public void ExecuteFilter__Check_If_Returning_All_Contacts_When_Name_Is_Blank__FirstName()
        {
            // ARRANGE
            var queryList = _contacts.AsQueryable();
            var item = _fixture.Build<ContactFilterItem>().With(i => i.FirstName, "").Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList, item);
            var array = query.ToArray();

            // ASSERT
            Assert.True(array.Length > 1);
        }

        [Test]
        public void ExecuteFilter__Check_If_Returning_All_Contacts_When_Name_Is_Null__FirstName()
        {
            // ARRANGE
            var queryList = _contacts.AsQueryable();
            var item = _fixture.Build<ContactFilterItem>().Without(i => i.FirstName).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList, item);
            var array = query.ToArray();

            // ASSERT
            Assert.True(array.Length > 1);
        }

        [Test]
        public void ExecuteFilter__Check_If_Returning_All_Contacts_When_Name_Is_Whitespaces__FirstName()
        {
            // ARRANGE
            var queryList = _contacts.AsQueryable();
            var item = _fixture.Build<ContactFilterItem>().With(i => i.FirstName, " ").Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList, item);
            var array = query.ToArray();

            // ASSERT
            Assert.True(array.Length > 1);
        }

        [Test]
        public void ExecuteFilter__Check_Not_Returning_Contacts_When_Letter_Is_Invalid__FirstName()
        {
            // ARRANGE
            var queryList = _contacts.AsQueryable();

            var item = _fixture.Build<ContactFilterItem>().With(i => i.FirstName, _letter.ToString()).Create();

            // ACT
            var query = _sut.ExecuteFilter(queryList, item);

            // ASSERT
            Assert.True(query.ToArray().Length == 0);
        }
    }
}
