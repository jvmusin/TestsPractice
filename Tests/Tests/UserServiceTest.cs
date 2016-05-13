//using System;
//using NUnit.Framework;
//
//namespace Tests.Tests
//{
//    public class UserServiceTest : TestBase
//    {
//        private UserService userService;
//
//        public override void SetUp()
//        {            
//            userService = new UserService(new PasswordHasher(), new UserRepository(), new GuidFactory(), new UserEntityFactory());
//        }
//
//        [Test]
//        public void TestRegisterAndLogin()
//        {
//            userService.Register("login", "pass");
//            var userModel = userService.Login("login", "pass");
//
//            Assert.IsNotNull(userModel);
//            Assert.AreEqual(userModel.Login, "login");
//        }
//
//        [Test]
//        public void TestRegisterAndLoginWithIncorrectPassword()
//        {
//            userService.Register("login", "pass");
//            var userModel = userService.Login("login", "pass2");
//
//            Assert.IsNull(userModel);
//        }
//
//
//        [Test]
//        public void TestRegisterDuplicateUser()
//        {
//            userService.Register("login", "pass");
//
//            Assert.Throws<Exception>(() => userService.Register("login", "pass2"));
//        }
//    }
//}