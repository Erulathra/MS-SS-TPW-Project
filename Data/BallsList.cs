using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace TPW.Data;

internal class BallsList : BallsDataLayerAbstractApi
{
   private const int MaxStartSpeed = 200;
   private const int MinStartSpeed = -200;
   private readonly List<IBall> ballsList;

   public BallsList(Vector2 boardSize) : base(boardSize)
   {
      ballsList = new List<IBall>();
   }

   public override void Add(int howMany)
   {
      for (var i = 0; i < howMany; i++)
      {
         var rand = new Random();

         var radius = rand.Next(25, 50);
         var weight = rand.Next(25, 50);

         var position = this.GetRandomPointInsideBoard(radius);
         var velocity = this.GetRandomVelocity();
         IBall ball = new Ball(ballsList.Count, position, radius, weight, velocity, this);

         ballsList.Add(ball);
      }
   }

   private Vector2 GetRandomPointInsideBoard(int ballRadius)
   {
      var rng = new Random();
      var isPositionCorrect = false;
      var x = 0;
      var y = 0;
      while (!isPositionCorrect)
      {
         x = rng.Next(ballRadius, (int)(boardSize.X - ballRadius));
         y = rng.Next(ballRadius, (int)(boardSize.Y - ballRadius));

         isPositionCorrect = this.CheckIsSpaceFree(new Vector2(x, y), ballRadius);
      }
      return new Vector2(x, y);
   }

   private bool CheckIsSpaceFree(Vector2 position, int ballRadius)
   {
      foreach (var ball in ballsList)
      {
         if (this.IsCirclesCollides(ball.Position, ball.Radius, position, ballRadius))
         {
            return false;
         }
      }

      return true;
   }

   private bool IsCirclesCollides(Vector2 position1, float radius1, Vector2 position2, float radius2)
   {
      var distSq = (position1.X - position2.X) * (position1.X - position2.X) + (position1.Y - position2.Y) * (position1.Y - position2.Y);
      var radSumSq = (radius1 + radius2) * (radius1 + radius2);
      return distSq <= radSumSq;
   }

   private Vector2 GetRandomVelocity()
   {
      var rng = new Random();
      var x = rng.Next(MinStartSpeed, MaxStartSpeed);
      var y = rng.Next(MinStartSpeed, MaxStartSpeed);

      return new Vector2(x, y);
   }

   public override void StartSimulation()
   {
      if (CancelSimulationSource.IsCancellationRequested)
      {
         return;
      }

      foreach (var ball in ballsList)
      {
         ball.PositionChange += (_, args) =>
         {
            this.OnPositionChange(new OnPositionChangeEventArgs(args.Ball, new List<IBall>(ballsList)));
         };

         Task.Factory.StartNew(ball.Simulate, CancelSimulationSource.Token);
      }
   }

   public override void StopSimulation()
   {
      CancelSimulationSource.Cancel();
   }
}