using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Tests
{
    [TestFixture]
    public class UserRepository_Should : TestBase
    {
        private UserRepository userRepository;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            userRepository = new UserRepository();
        }

        [Test]
        public void CreateUsersWithoutExceptions_WhenCreatingFirstTime()
        {
            var usersCount = 100;
            foreach (var user in CreateUsers(usersCount))
                Assert.DoesNotThrow(() => userRepository.Create(user));
        }

        [Test]
        public void FindUsersCorrectly_WhenQueriesAreInSequencedOrder()
        {
            var usersCount = 100;
            var users = CreateUsers(usersCount).ToList();

            userRepository.CreateAll(users);
            
            foreach (var user in users)
                userRepository.Find(user.Login).ShouldBeEquivalentTo(user);
        }

        [Test]
        public void FindUsersCorrectly_WhenQueriesAreInRandomOrder()
        {
            var usersCount = 100;
            var users = CreateUsers(usersCount).ToList();

            userRepository.CreateAll(users);
            
            foreach (var user in users.OrderBy(user => Rnd.Next()))
                userRepository.Find(user.Login).ShouldBeEquivalentTo(user);
        }

        [Test]
        public void FindUserCorrectly_WhenRequestedTwoTimes()
        {
            var user = new UserEntity("login", Guid.NewGuid(), "hash");
            userRepository.Create(user);

            userRepository.Find(user.Login);
            userRepository.Find(user.Login).ShouldBeEquivalentTo(user);
        }

        [Test]
        public void Fail_WhenUserAlreadyExists()
        {
            var user = new UserEntity("login", Guid.NewGuid(), "hash");
            Action register = () => userRepository.Create(user);
            
            register.ShouldNotThrow();
            register.ShouldThrow<Exception>();
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