using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Tests.Tests;

namespace Tests
{
    [TestFixture]
    public class UserRepository_Should : TestBase
    {
        private UserRepository repository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            repository = new UserRepository();
        }

        [Test]
        public void CreateUsersWithoutExceptions_WhenCreatingFirstTime()
        {
            var usersCount = 10 * 1000;
            foreach (var user in CreateUsers(usersCount))
                Assert.DoesNotThrow(() => repository.Create(user));
        }

        [Test]
        public void FindUsersCorrectly_WhenQueriesAreInSequencedOrder()
        {
            var usersCount = 10 * 1000;
            var users = CreateUsers(usersCount).ToList();

            repository.CreateAll(users);
            
            foreach (var user in users)
                repository.Find(user.Login).ShouldBeEquivalentTo(user);
        }

        [Test]
        public void FindUsersCorrectly_WhenQueriesAreInRandomOrder()
        {
            var usersCount = 10 * 1000;
            var users = CreateUsers(usersCount).ToList();

            repository.CreateAll(users);
            
            foreach (var user in users.OrderBy(user => Rnd.Next()))
                repository.Find(user.Login).ShouldBeEquivalentTo(user);
        }

        [Test]
        public void FindUserCorrectly_WhenRequestedTwoTimes()
        {
            var user = new UserEntity("login", Guid.NewGuid(), "hash");
            repository.Create(user);

            repository.Find(user.Login);
            repository.Find(user.Login).ShouldBeEquivalentTo(user);
        }

        [Test]
        public void Fail_WhenUserAlreadyExists()
        {
            var user = new UserEntity("login", Guid.NewGuid(), "hash");
            Action registration = () => repository.Create(user);
            
            registration.ShouldNotThrow();
            registration.ShouldThrow<Exception>();
        }

        private static IEnumerable<UserEntity> CreateUsers(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var login = "login" + i;
                var hash = "hash" + i;
                var userId = Guid.NewGuid();
                yield return new UserEntity(login, userId, hash);
            }
        } 
    }
}