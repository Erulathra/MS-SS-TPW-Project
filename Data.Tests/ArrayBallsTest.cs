using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Newtonsoft.Json.Linq;
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
        public void Test()
        {
           string test = "";
           var array = JArray.Parse(test);
        }
    }
}