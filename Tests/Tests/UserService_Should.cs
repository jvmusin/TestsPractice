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
        public void RegisterCorrectly_WhenDataIsCorrect()
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
            var pass = "pass";
            var hash = "hash";
            var guid = Guid.NewGuid();
            var entity = new UserEntity(login, guid, hash);
            var model = new UserModel(login, guid);

            A.CallTo(() => userRepository.Find(login)).Returns(entity).Once();
            A.CallTo(() => passwordHasher.Hash(pass)).Returns(hash).Once();

            userService.Login(login, pass).ShouldBeEquivalentTo(model);
        }

        [Test]
        public void ReturnNullOnLogin_WhenLoginNotFound()
        {
            var login = "login";
            var pass = "pass";

            A.CallTo(() => userRepository.Find(login)).Returns(null).Once();

            userService.Login(login, pass).Should().BeNull();
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

        [Test]
        public void FailOnRegister_WhenLoginIsNull()
        {
            Action registerWithNullLogin = () => userService.Register(null, "pass");
            registerWithNullLogin.ShouldThrow<Exception>();
        }

        [Test]
        public void FailOnRegister_WhenPasswordIsNull()
        {
            Action registerWithNullPassword = () => userService.Register("login", null);
            registerWithNullPassword.ShouldThrow<Exception>();
        }

        [Test]
        public void FailOnRegister_WhenLoginAndPasswordAreNull()
        {
            Action registerWithNullLoginAndPassword = () => userService.Register(null, null);
            registerWithNullLoginAndPassword.ShouldThrow<Exception>();
        }

        [Test]
        public void FailOnLogin_WhenLoginIsNull()
        {
            Action registerWithNullLogin = () => userService.Login(null, "pass");
            registerWithNullLogin.ShouldThrow<Exception>();
        }

        [Test]
        public void FailOnLogin_WhenPasswordIsNull()
        {
            Action registerWithNullPassword = () => userService.Login("login", null);
            registerWithNullPassword.ShouldThrow<Exception>();
        }

        [Test]
        public void FailOnLogin_WhenLoginAndPasswordAreNull()
        {
            Action registerWithNullLoginAndPassword = () => userService.Login(null, null);
            registerWithNullLoginAndPassword.ShouldThrow<Exception>();
        }

        #endregion
    }
}
