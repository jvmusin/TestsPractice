//using System;
//using NUnit.Framework;
//using Rhino.Mocks;
//
//namespace Tests.Tests
//{
//    public class UserServiceUnitTest : TestBase
//    {
//        private IPasswordHasher passwordHasher;
//        private UserService userService;
//        private IUserRepository userRepository;
//        private IGuidFactory guidFactory;
//        private IUserEntityFactory userEntityFactory;
//
//        public override void SetUp()
//        {
//            base.SetUp();
//
//            passwordHasher = NewMock<IPasswordHasher>();
//            userRepository = NewMock<IUserRepository>();
//            guidFactory = NewMock<IGuidFactory>();
//            userEntityFactory = NewMock<IUserEntityFactory>();
//            userService = new UserService(passwordHasher, userRepository, guidFactory, userEntityFactory);
//        }
//
//        [Test]
//        public void TestLogin()
//        {
//            var password = "pass";
//            var login = "login";
//            var hash = "passHash";
//            var userId = Guid.NewGuid();
//            var userEntity = new UserEntity();
//
//            using (mockRepository.Record())
//            {
//                passwordHasher.Expect(f => f.Hash(password)).Return(hash);
//                guidFactory.Expect(f => f.Create()).Return(userId);
//                userEntityFactory.Expect(f => f.Create(login, userId, hash)).Return(userEntity);
//                userRepository.Expect(f => f.Create(userEntity));
//            }
//
//            userService.Register(login, password);
//        }
//    }
//}