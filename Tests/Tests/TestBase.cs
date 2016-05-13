using System;
using FakeItEasy;
using NUnit.Framework;

namespace Tests.Tests
{
    public class TestBase
    {
        protected readonly Random Rnd = new Random();

        [SetUp]
        public virtual void SetUp()
        {
        }

        protected T StrictMock<T>()
        {
            return A.Fake<T>(x => x.Strict());
        }
    }
}
