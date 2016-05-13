using System;
using NUnit.Framework;
using FluentAssertions;

namespace Tests.Tests
{
    [TestFixture]
    public class PasswordHasher_Should : TestBase
    {
        private PasswordHasher hasher;

        [SetUp]
        public override void SetUp()
        {
            hasher = new PasswordHasher();
        }

        [Test]
        public void CreateSameHashes_WhenPasswordsAreEqual()
        {
            var pass = "pass";

            var hash1 = hasher.Hash(pass);
            var hash2 = hasher.Hash(pass);

            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void CreateDifferentHashes_WhenPasswordsAreNotEqual()
        {
            var pass1 = "pass1";
            var pass2 = "pass2";

            var hash1 = hasher.Hash(pass1);
            var hash2 = hasher.Hash(pass2);

            Assert.AreNotEqual(hash1, hash2);
        }

        [Test]
        public void Fail_WhenPasswordIsNull()
        {
            Action hashing = () => hasher.Hash(null);
            hashing.ShouldThrow<Exception>();
        }
    }
}