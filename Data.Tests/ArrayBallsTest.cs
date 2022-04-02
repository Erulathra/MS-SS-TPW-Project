using System.Numerics;
using NUnit.Framework;

namespace TPW.Data.Tests
{
    public class Tests
    {
        private DataLayerAbstractApi balls;
        private IBall testBall1;
        private IBall testBall2;
        private IBall testBall3;

        [SetUp]
        public void Setup()
        {
            balls = DataLayerAbstractApi.CreateBallsList();
            testBall1 = DataLayerAbstractApi.CreateBall(new Vector2(5, 10));
            testBall2 = DataLayerAbstractApi.CreateBall(new Vector2(8, 4));
            testBall3 = DataLayerAbstractApi.CreateBall(new Vector2(2, 9));
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