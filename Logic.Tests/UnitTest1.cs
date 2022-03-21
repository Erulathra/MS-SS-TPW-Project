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
            Assert.AreEqual("Hello Szymon!", mainLogic.GetGreeting("Szymon"));
        }

        [Test]
        public void GetGetHelloWorldTest()
        {
            Assert.AreEqual("Hello World!", mainLogic.GetHelloWorld());
        }
    }
}