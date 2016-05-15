using System;
using FakeItEasy;

namespace Tests.Tests
{
    public abstract class TestBase
    {
        protected Random Rnd;
        
        public virtual void SetUp()
        {
            Rnd = new Random();
        }

        protected T StrictMock<T>()
        {
            return A.Fake<T>(x => x.Strict());
        }
    }
}
