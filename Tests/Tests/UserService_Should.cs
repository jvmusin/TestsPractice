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

        [Test, Pairwise]
        public void FailOnRegister_WhenLoginOrPasswordIsNull(
            [Values("login",    null)] string login,
            [Values("password", null)] string password)
        {
            if (login != null && password != null)
                return;

            Action register = () => userService.Register(login, password);
            register.ShouldThrow<Exception>();
        }

        [Test, Pairwise]
        public void FailOnLogin_WhenLoginOrPasswordIsNull(
            [Values("login",    null)] string login,
            [Values("password", null)] string password)
        {
            if (login != null && password != null)
                return;

            Action register = () => userService.Login(login, password);
            register.ShouldThrow<Exception>();
        }

        #endregion
    }
}
