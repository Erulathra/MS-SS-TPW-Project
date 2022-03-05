using NUnit.Framework;

namespace MS_SS_TPW_Project.Logic.Tests
{
    public class Tests
    {
        private MainLogic mainLogic;

        [SetUp]
        public void Setup()
        {
            mainLogic = new MainLogic();
        }

        [Test]
        public void GetGreatingTest()
        {
            Assert.AreEqual(mainLogic.GetGreeting("World"), "Hello World!");
        }
    }
}