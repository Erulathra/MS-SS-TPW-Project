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
    }
}