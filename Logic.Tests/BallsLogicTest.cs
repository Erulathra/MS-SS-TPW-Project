using System.Numerics;
using NUnit.Framework;
using TPW.Data;

namespace TPW.Logic.Tests;

public class BallsLogicTest
{
   private readonly Vector2 boardSize = new(150, 100);
   private BallsLogicLayerAbstractApi ballsLogic;
   private DataLayerFixture data;

   [SetUp]
   public void SetUp()
   {
      data = new DataLayerFixture(boardSize);
      ballsLogic = BallsLogicLayerAbstractApi.CreateBallsLogic(boardSize, data);
   }

   [Test]
   public void AddBalls()
   {
      ballsLogic.AddBalls(5);
      Assert.AreEqual(5, data.BallsList.Count);
   }

   [Test]
   public void LogicSimulationTest()
   {
      Assert.AreEqual(false, data.isSimulationWorking);
      ballsLogic.StartSimulation();
      Assert.AreEqual(true, data.isSimulationWorking);
      ballsLogic.StopSimulation();
      Assert.AreEqual(false, data.isSimulationWorking);
   }

   [Test]
   public void CollideWithWallsSouth()
   {
      data.BallsList.Add(new BallFixture(1, new Vector2(100, 100), 10, 10, Vector2.One, data));

      ballsLogic.PositionChange += (_, args) =>
      {
         Assert.AreEqual(new Vector2(1, -1), data.BallsList[0].Velocity);
      };
      ballsLogic.StartSimulation();
      data.OnBallOnPositionChange();
   }

   [Test]
   public void CollideWithWallNorth()
   {
      data.BallsList.Add(new BallFixture(1, new Vector2(150, 50), 10, 10, Vector2.One, data));
      ballsLogic.PositionChange += (_, args) =>
      {
         Assert.AreEqual(new Vector2(-1, 1), data.BallsList[0].Velocity);
      };
      ballsLogic.StartSimulation();
      data.OnBallOnPositionChange();
   }

   [Test]
   public void CollideWithWallEast()
   {
      data.BallsList.Add(new BallFixture(1, new Vector2(100, 0), 10, 10, Vector2.Multiply(Vector2.One, -1), data));
      ballsLogic.PositionChange += (_, args) =>
      {
         Assert.AreEqual(new Vector2(-1, 1), data.BallsList[0].Velocity);
      };
      ballsLogic.StartSimulation();
      data.OnBallOnPositionChange();
   }

   [Test]
   public void CollideWithWallWest()
   {
      data.BallsList.Add(new BallFixture(1, new Vector2(0, 100), 10, 10, Vector2.One, data));
      ballsLogic.PositionChange += (_, args) =>
      {
         Assert.AreEqual(new Vector2(1, -1), data.BallsList[0].Velocity);
      };
      ballsLogic.StartSimulation();
      data.OnBallOnPositionChange();
   }

   [Test]
   public void NoCollideBalls()
   {
      data.BallsList.Add(new BallFixture(1, new Vector2(50, 50), 10, 10, Vector2.One, data));
      data.BallsList.Add(new BallFixture(2, new Vector2(50, 100), 15, 10, Vector2.One, data));
      ballsLogic.PositionChange += (_, args) =>
      {
         Assert.AreEqual(Vector2.One, data.BallsList[0].Velocity);
      };
      ballsLogic.StartSimulation();
      data.OnBallOnPositionChange();
   }
   
   [Test]
   public void CollideBalls()
   {
      data.BallsList.Add(new BallFixture(1, new Vector2(50, 50), 10, 10, new Vector2(0, 1), data));
      data.BallsList.Add(new BallFixture(2, new Vector2(50, 60), 15, 10, new Vector2(0, -1), data));
      ballsLogic.PositionChange += (_, args) =>
      {
         Assert.AreNotEqual(Vector2.One, data.BallsList[0].Velocity);
      };
      ballsLogic.StartSimulation();
      data.OnBallOnPositionChange();
   }
   
   [Test]
   public void CollideBallsDistanceEqualsSumOfRadius()
   {
      data.BallsList.Add(new BallFixture(1, new Vector2(50, 50), 10, 10, new Vector2(0, 1), data));
      data.BallsList.Add(new BallFixture(2, new Vector2(50, 75), 15, 10, new Vector2(0, -1), data));
      ballsLogic.PositionChange += (_, args) =>
      {
         Assert.AreNotEqual(Vector2.One, data.BallsList[0].Velocity);
      };
      ballsLogic.StartSimulation();
      data.OnBallOnPositionChange();
   }
}