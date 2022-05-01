using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace TPW.Data;

internal class BallsList : BallsDataLayerAbstractApi
{
   private const int MaxSpeed = 50;
   private const int MinSpeed = 20;
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
      var x = rng.Next(ballRadius, (int)(boardSize.X - ballRadius));
      var y = rng.Next(ballRadius, (int)(boardSize.Y - ballRadius));

      return new Vector2(x, y);
   }

   private Vector2 GetRandomVelocity()
   {
      var rng = new Random();
      var x = rng.Next(MinSpeed, MaxSpeed);
      var y = rng.Next(MinSpeed, MaxSpeed);

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
         ball.PositionChange += (_, argv) =>
         {
            this.OnPositionChange(argv);
         };

         Task.Factory.StartNew(ball.Simulate, CancelSimulationSource.Token);
      }
   }

   public override void StopSimulation()
   {
      CancelSimulationSource.Cancel();
   }
}