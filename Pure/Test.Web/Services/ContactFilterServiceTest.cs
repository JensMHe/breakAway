using BreakAway.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BreakAway.Services.Filters;
using Moq;
using BreakAway.Entities;
using AutoFixture;

namespace Test.Web.Services
{
    [TestFixture]
    class ContactFilterServiceTest
    {
        private IFixture _fixture;
        private List<Contact> _contacts;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            var errorList = _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList();
            errorList.ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _contacts = _fixture.CreateMany<Contact>(50).ToList();
        }

        [Test]
        public void FilterContact__Check_If_All_Filters_Accept_And_Run_Filter_Once()
        {
            //ARRANGE
            var item = _fixture.Create<ContactFilterItem>();
            IQueryable<Contact> query = _contacts.AsQueryable();

            var filters = new[]
            {
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
            };

            IList<IFilterBy> filterList = new List<IFilterBy>();

            foreach (var filter in filters)
            {
                filter.Setup(f => f.IsAbleToFilter(item)).Returns(true).Verifiable();
                filter.Setup(f => f.ExecuteFilter(query, item)).Returns(query).Verifiable();
                filterList.Add(filter.Object);
            }

            IContactFilterService sut = new ContactFilterService(filterList);

            // ACT
            sut.FilterContact(query, item);

            // ASSERT
            foreach (var filter in filters)
            {
                filter.Verify(f => f.ExecuteFilter(query, item), Times.Once);
            }
        }

        [Test]
        public void FilterContact__Check_If_All_Filters_Decline_And_Dont_Run_Filter_Once()
        {
            // ARRANGE
            var item = _fixture.Create<ContactFilterItem>();
            IQueryable<Contact> query = _contacts.AsQueryable();

            var filters = new[]
            {
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
            };

            IList<IFilterBy> filterList = new List<IFilterBy>();

            foreach (var filter in filters)
            {
                filter.Setup(f => f.IsAbleToFilter(item)).Returns(false).Verifiable();
                filter.Setup(f => f.ExecuteFilter(query, item)).Returns(query).Verifiable();
                filterList.Add(filter.Object);
            }

            IContactFilterService sut = new ContactFilterService(filterList);

            // ACT
            sut.FilterContact(query, item);

            // ASSERT
            foreach (var filter in filters)
            {
                filter.Verify(f => f.ExecuteFilter(query, item), Times.Never);
            }
        }

        [Test]
        public void FilterContact__Assert_Query_Is_Unchanged_If_Able_To_Filter_Is_False()
        {
            // ARRANGE
            var item = _fixture.Create<ContactFilterItem>();

            IQueryable<Contact> resultQuery = _fixture.CreateMany<Contact>().AsQueryable();
            IQueryable<Contact> query = _contacts.AsQueryable().Concat(resultQuery);

            var filters = new[]
            {
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
            };

            IList<IFilterBy> filterList = new List<IFilterBy>();

            foreach (var filter in filters)
            {
                filter.Setup(f => f.IsAbleToFilter(item)).Returns(false).Verifiable();
                filter.Setup(f => f.ExecuteFilter(query, item)).Returns(resultQuery).Verifiable();
                filterList.Add(filter.Object);
            }

            IContactFilterService sut = new ContactFilterService(filterList);

            // ACT
            var returnQuery = sut.FilterContact(query, item);

            // ASSERT
            Assert.True(returnQuery.ToArray().Length == query.ToArray().Length);
            for (int i = 0; i < query.ToArray().Length; i++)
            {
                Assert.True(query.ToArray()[i].Id == returnQuery.ToArray()[i].Id);
            }
        }

        [Test]
        public void FilterContact__Assert_That_Return_Query_Is_Filtered_When_Able_To_Filter()
        {
            // ARRANGE
            var item = _fixture.Create<ContactFilterItem>();

            IQueryable<Contact> resultQuery = _fixture.CreateMany<Contact>().AsQueryable();
            IQueryable<Contact> query = _contacts.AsQueryable().Concat(resultQuery);

            var filters = new[]
            {
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
            };

            IList<IFilterBy> filterList = new List<IFilterBy>();

            foreach (var filter in filters)
            {
                filter.Setup(f => f.IsAbleToFilter(item)).Returns(true).Verifiable();
                filter.Setup(f => f.ExecuteFilter(query, item)).Returns(resultQuery).Verifiable();
                filterList.Add(filter.Object);
            }

            IContactFilterService sut = new ContactFilterService(filterList);

            // ACT
            var returnQuery = sut.FilterContact(query, item);

            // ASSERT
            var isUniqe = false;
            Assert.True(query.ToArray().Length > returnQuery.ToArray().Length);
            foreach(var queryItem in query)
            {
                isUniqe = !returnQuery.ToList().Exists(q => q.Id == queryItem.Id);
            }
            Assert.IsTrue(isUniqe);
        }

        [Test]
        public void FilterContact__Check_If_Exception_Is_Thrown_If_Dependency_Injection_Is_Null()
        {
            // ARRANGE

            // ACT

            // ASSERT
            Assert.Throws<ArgumentNullException>(() => new ContactFilterService(null));
        }

        [Test]
        public void FilterContact__Check_If_Query_Returns_Unchanged_If_FilterList_Is_Empty()
        {
            // ARRANGE
            var item = _fixture.Create<ContactFilterItem>();

            IQueryable<Contact> resultQuery = _fixture.CreateMany<Contact>().AsQueryable();
            IQueryable<Contact> query = _contacts.AsQueryable().Concat(resultQuery);

            IList<IFilterBy> filterList = new List<IFilterBy>();

            Mock<IFilterBy>[] filters = new Mock<IFilterBy>[0];

            foreach (var filter in filters)
            {
                filter.Setup(f => f.IsAbleToFilter(item)).Returns(true).Verifiable();
                filter.Setup(f => f.ExecuteFilter(query, item)).Returns(resultQuery).Verifiable();
                filterList.Add(filter.Object);
            }

            IContactFilterService sut = new ContactFilterService(filterList);

            // ACT
            var returnQuery = sut.FilterContact(query, item);

            // ASSERT
            Assert.True(query.ToArray().Length == returnQuery.ToArray().Length);
            for (int i = 0; i < query.ToArray().Length; i++)
            {
                Assert.True(query.ToArray()[i].Id == returnQuery.ToArray()[i].Id);
            }
        }

        [Test]
        public void FilterContact__Check_If_Query_Returns_Unchanged_When_ContactItem_Is_Null()
        {
            // ARRANGE
            IQueryable<Contact> resultQuery = _fixture.CreateMany<Contact>().AsQueryable();
            IQueryable<Contact> query = _contacts.AsQueryable().Concat(resultQuery);

            var filters = new[]
            {
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
            };

            IList<IFilterBy> filterList = new List<IFilterBy>();

            foreach (var filter in filters)
            {
                filter
                    .Setup(f => f.IsAbleToFilter(null))
                    .Returns(false).Verifiable();

                filter.Setup(f => f.ExecuteFilter(query, null)).Returns(resultQuery).Verifiable();
                filterList.Add(filter.Object);
            }

            IContactFilterService sut = new ContactFilterService(filterList);

            // ACT
            var returnQuery = sut.FilterContact(query, null);

            // ASSERT
            Assert.True(returnQuery.ToArray().Length == query.ToArray().Length);
            for (int i = 0; i < query.ToArray().Length; i++)
            {
                Assert.True(query.ToArray()[i].Id == returnQuery.ToArray()[i].Id);
            }
        }

        [Test]
        public void FilterContact__Check_If_Query_Returns_Changed_When_ContactItem_Is_Null()
        {
            // ARRANGE
            IQueryable<Contact> resultQuery = _fixture.CreateMany<Contact>().AsQueryable();
            IQueryable<Contact> query = _contacts.AsQueryable().Concat(resultQuery);

            var filters = new[]
            {
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
                new Mock<IFilterBy>(),
            };

            IList<IFilterBy> filterList = new List<IFilterBy>();

            foreach (var filter in filters)
            {
                filter
                    .Setup(f => f.IsAbleToFilter(null))
                    .Returns(true).Verifiable();

                filter.Setup(f => f.ExecuteFilter(query, null)).Returns(resultQuery).Verifiable();
                filterList.Add(filter.Object);
            }

            IContactFilterService sut = new ContactFilterService(filterList);

            // ACT
            var returnQuery = sut.FilterContact(query, null);

            // ASSERT
            Assert.True(returnQuery.ToArray().Length < query.ToArray().Length);
        }
    }
}
