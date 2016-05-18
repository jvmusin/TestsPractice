using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Tests
{
    [TestFixture]
    public class UserService_Should : TestBase
    {
        private UserService userService;
        private IPasswordHasher passwordHasher;
        private IUserRepository userRepository;
        private IGuidFactory guidFactory;
        private IUserEntityFactory userEntityFactory;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            passwordHasher = StrictMock<IPasswordHasher>();
            userRepository = StrictMock<IUserRepository>();
            guidFactory = StrictMock<IGuidFactory>();
            userEntityFactory = StrictMock<IUserEntityFactory>();
            userService = new UserService(passwordHasher, userRepository, guidFactory, userEntityFactory);
        }

        [Test]
        public void RegisterUserCorrectly_WhenDataIsCorrect()
        {
            var login = "login";
            var password = "pass";
            var hash = "passHash";
            var userId = Guid.NewGuid();
            var userEntity = new UserEntity();

            A.CallTo(() => passwordHasher.Hash(password)).Returns(hash).Once();
            A.CallTo(() => guidFactory.Create()).Returns(userId).Once();
            A.CallTo(() => userEntityFactory.Create(login, userId, hash)).Returns(userEntity).Once();
            A.CallTo(() => userRepository.Create(userEntity)).DoesNothing().Once();

            userService.Register(login, password);
        }

        [Test]
        public void LoginCorrectly_WhenDataIsCorrect()
        {
            var login = "login";
            var password = "pass";
            var hash = "passHash";
            var guid = Guid.NewGuid();
            var entity = new UserEntity(login, guid, hash);
            var model = new UserModel(login, guid);

            A.CallTo(() => userRepository.Find(login)).Returns(entity).Once();
            A.CallTo(() => passwordHasher.Hash(password)).Returns(hash).Once();

            userService.Login(login, password).ShouldBeEquivalentTo(model);
        }

        [Test]
        public void ReturnNullOnLogin_WhenLoginNotFound()
        {
            var login = "login";
            var password = "pass";

            A.CallTo(() => userRepository.Find(login)).Returns(null).Once();

            userService.Login(login, password).Should().BeNull();
        }

        [Test]
        public void ReturnNullOnLogin_WhenPasswordHashesDiffer()
        {
            var login = "login";
            var pass = "pass";
            var hash = "hash";
            var wrongHash = "hash2";
            var entity = new UserEntity(login, Guid.NewGuid(), hash);

            A.CallTo(() => userRepository.Find(login)).Returns(entity).Once();
            A.CallTo(() => passwordHasher.Hash(pass)).Returns(wrongHash).Once();

            userService.Login(login, pass).Should().BeNull();
        }

        #region Null checks

        [Test, TestCaseSource(nameof(NullLoginAndPasswordVariants))]
        public void FailOnRegister_WhenLoginOrPasswordIsNull(string login, string password)
        {
            Action register = () => userService.Register(login, password);
            register.ShouldThrow<Exception>();
        }

        [Test, TestCaseSource(nameof(NullLoginAndPasswordVariants))]
        public void FailOnLogin_WhenLoginOrPasswordIsNull(string login, string password)
        {
            Action loginAct = () => userService.Login(login, password);
            loginAct.ShouldThrow<Exception>();
        }

        private static IEnumerable<TestCaseData> NullLoginAndPasswordVariants
        {
            get
            {
                yield return new TestCaseData("login", null);
                yield return new TestCaseData(null, "password");
                yield return new TestCaseData(null, null);
            }
        }

        #endregion
    }
}
