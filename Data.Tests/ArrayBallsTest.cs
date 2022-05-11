using System;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;

namespace TPW.Data.Tests
{
    public class Tests
    {
        private BallsDataLayerAbstractApi balls;
        private readonly Vector2 boardSize = new Vector2(800, 600);

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
            Assert.Catch<Exception>(() => balls.Add(2000));
        }
        
        [Test]
        // Tests whether event works and balls stay inside board
        public void SimulationTest()
        {
            var interactionCount = 0;
            balls.Add(10);
            Assert.AreEqual(10, balls.BallCount);

            var ballsList = new HashSet<IBall>();

            balls.PositionChange += (sender, args) =>
            {
                ballsList.Add(args.SenderBall);
                //Check if ball is inside board boundaries
                var pos = args.SenderBall.Position;
                var rad = args.SenderBall.Radius;
                Assert.GreaterOrEqual(pos.X-rad, 0);
                Assert.GreaterOrEqual(pos.Y-rad, 0);
                Assert.LessOrEqual(pos.X+rad, boardSize.X);
                Assert.LessOrEqual(pos.Y+rad, boardSize.Y);
                
                interactionCount++;
                if (interactionCount >= 100)
                {
                    balls.StopSimulation();
                }
            };
            balls.StartSimulation();
            while (interactionCount < 100)
            { }

            Assert.GreaterOrEqual(interactionCount, 99);
            Assert.GreaterOrEqual(ballsList.Count, 9);
        }
    }
}