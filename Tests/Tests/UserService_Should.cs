using System;
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
        public void Register_WhenDataIsCorrect()
        {
            var password = "pass";
            var login = "login";
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
        public void Fail_WhenLoginIsNull()
        {
            Action registerWithNullLogin = () => userService.Register(null, "pass");
            registerWithNullLogin.ShouldThrow<Exception>();
        }

        [Test]
        public void Fail_WhenPasswordIsNull()
        {
            Action registerWithNullPassword = () => userService.Register("login", null);
            registerWithNullPassword.ShouldThrow<Exception>();
        }

        [Test]
        public void Fail_WhenLoginAndPasswordAreNull()
        {
            Action registerWithNullLoginAndPassword = () => userService.Register(null, null);
            registerWithNullLoginAndPassword.ShouldThrow<Exception>();
        }
    }
}
