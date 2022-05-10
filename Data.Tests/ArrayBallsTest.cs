using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;

namespace TPW.Data.Tests
{
    public class Tests
    {
        private BallsDataLayerAbstractApi balls;
        private Vector2 boardSize = new Vector2(800, 600);

        [SetUp]
        public void Setup()
        {
            balls = BallsDataLayerAbstractApi.CreateBallsList(boardSize);
        }

        [Test]
        public void BoardSizeTest()
        {
            Assert.AreEqual(boardSize, balls.BoardSize);
        }

        [Test]
        public void AddBallTest()
        {
            balls.Add(2);
            Assert.AreEqual(2, balls.BallCount);
            balls.Add(5);
            Assert.AreEqual(7, balls.BallCount);
        }
        
        [Test]
        public void SimulationTest()
        {
            var interactionCount = 0;
            balls.Add(10);
            Assert.AreEqual(10, balls.BallCount);

            // var startPositionList = new List<Vector2>();
            // for (int i = 0; i < balls.BallCount; i++)
            // {
            //     startPositionList.Add(balls.GetBalls()[i].Position);
            // }

            balls.PositionChange += (_, _) =>
            {
                interactionCount++;
                if (interactionCount >= 50)
                {
                    balls.StopSimulation();
                }
            };
            balls.StartSimulation();
            while (interactionCount < 50)
            { }

            Assert.GreaterOrEqual(interactionCount, 49);
            // for (int i = 0; i < balls.GetBallsCount(); i++)
            // {
            //     if (startPositionList[i] != balls.GetBalls()[i].Position)
            //     {
            //         return;
            //     }
            // }
            // Assert.Fail();
        }
    }
}