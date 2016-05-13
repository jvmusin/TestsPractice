using NUnit.Framework;

namespace Tests.Tests
{
    public class PasswordHasherTest : TestBase
    {
        private PasswordHasher passwordHaser;

        public override void SetUp()
        {
            base.SetUp();
            passwordHaser = new PasswordHasher();

        }

        [Test]
        public void HashTest()
        {
            var password = "pass";
            var hash = passwordHaser.Hash(password);
            var equalHash = passwordHaser.Hash(password);

            Assert.AreEqual(hash, equalHash);

            var incrorrect = passwordHaser.Hash("pass2");
            
            Assert.AreNotEqual(hash, incrorrect);
        }

    }
}