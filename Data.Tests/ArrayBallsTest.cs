using System.Numerics;
using NUnit.Framework;

namespace Data.Tests
{
    public class Tests
    {
        private IBalls balls;
        private Ball testBall1;
        private Ball testBall2;
        private Ball testBall3;

        [SetUp]
        public void Setup()
        {
            balls = DataLayerFactory.CreateBallsList();
            testBall1 = new Ball(new Vector2(5, 10));
            testBall2 = new Ball(new Vector2(8, 4));
            testBall3 = new Ball(new Vector2(2, 9));
        }

        [Test]
        public void AddBallTest()
        {
            balls.Add(testBall1);
            Assert.AreEqual(1, balls.GetBallCount());
            balls.Add(testBall2);
            Assert.AreEqual(2, balls.GetBallCount());
            balls.Add(testBall3);
            Assert.AreEqual(3, balls.GetBallCount());

            Assert.AreEqual(testBall1, balls.Get(0));
            Assert.AreEqual(testBall2, balls.Get(1));
            Assert.AreEqual(testBall3, balls.Get(2));
        }
    }
}