using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Dantas.Core;
using System.Linq.Expressions;

namespace Dantas.Data.Tests
{
    class EntityId : Identity
    {
        public EntityId() : base(0) { }
        public EntityId(int value) : base(value) { }
        public EntityId(string value) : base(value) { }
        public EntityId(Identity id) : base(id) { }
    }
    class EntityTest : AggregateRoot<EntityId>
    {
        public string Some { get; set; }
    }

    class InMemoryRepositoryTest : RepositoryBase<EntityTest>
    {
        private static int sequenceId = 0;
        private List<EntityTest> list = new List<EntityTest>();

        protected override IQueryable<EntityTest> RepositoryQueryable
        {
            get { return list.AsQueryable<EntityTest>(); }
        }

        public override void Save(EntityTest item)
        {
            sequenceId++;
            item.GetType().GetProperty("Id").SetValue(item, new EntityId(sequenceId), null);
            this.list.Add(item);
        }

        public override void Remove(EntityTest item)
        {
            this.list.Remove(item);
        }
    }

    [TestFixture]
    public class RepositoryBaseTests
    {
        private InMemoryRepositoryTest repo = null;

        [SetUp]
        public void Setup()
        {
            //Restart data and repo
            repo = new InMemoryRepositoryTest();
            repo.Save(new EntityTest() { Some = "s1" });
            repo.Save(new EntityTest() { Some = "s2" });
            repo.Save(new EntityTest() { Some = "s3" });
        }

        [Test]
        public void RepositoryShouldSelect()
        {
            //Arrange
            var qty = 3;

            //Act
            var results = from e in repo.FindAll()
                          select e;

            //Assert
            Assert.AreEqual(qty, results.Count());
        }

        [Test]
        public void RepositoryShouldFilterByWhereCondition()
        {
            //Arrange
            string someValue = "s1";

            //Act
            var results = from e in repo.FindAll()
                          where e.Some == someValue
                          select e;
            //Assert
            Assert.AreEqual(someValue, results.Single().Some);
        }

        [Test]
        public void RepositoryShouldFilterByExpressionCondition()
        {
            //Arrange
            string someValue = "s1";

            //Act
            var results = repo.FindAll(e => e.Some == someValue);
            //Assert
            Assert.AreEqual(someValue, results.Single().Some);
        }

        [Test]
        public void RepositoryShouldFilterBySpecification()
        {
            //Arrange
            string someValue = "s1";
            Specification<EntityTest> spec1 = new Specification<EntityTest>(
                e => e.Some == someValue);

            //Act
            var results = from e in repo.FindAll(spec1)
                          select e;

            //Assert
            Assert.AreEqual(someValue, results.Single().Some);
        }

        [Test]
        public void RepositoryCanBeIterable()
        {
            //Arrange
            string concatResult = string.Empty;

            //Act
            var results = from e in repo.FindAll()
                          select e;

            foreach (var item in results)
                concatResult += item.Some;
            
            //Assert
            Assert.AreEqual(concatResult, "s1s2s3");
        }
    }
}
