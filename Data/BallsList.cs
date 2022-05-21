using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace TPW.Data;

internal class BallsList : BallsDataLayerAbstractApi
{
   private const int MaxStartSpeed = 200;
   private const int MinStartSpeed = 50;
   private const int MinRadius = 10;
   private const int MaxRadius = 30;
   private readonly List<IBall> ballsList;

   private readonly IBallListLogger ballListLogger = new FileBallListLogger();


   public BallsList(Vector2 boardSize) : base(boardSize)
   {
      ballsList = new List<IBall>();
   }

   public override void Add(int howMany)
   {
      Random rand = new Random();
      for (int i = 0; i < howMany; i++)
      {
         int radius = rand.Next(MinRadius, MaxRadius);
         int weight = radius;

         Vector2 position = this.GetRandomPointInsideBoard(radius);
         Vector2 velocity = this.GetRandomVelocity();
         IBall ball = new Ball(ballsList.Count, position, radius, weight, velocity, this);

         ballsList.Add(ball);
      }
   }

   private Vector2 GetRandomPointInsideBoard(int ballRadius)
   {
      Random rng = new Random();
      bool isPositionCorrect = false;
      float x = 0;
      float y = 0;
      int i = 0;
      while (!isPositionCorrect)
      {
         x = rng.Next(ballRadius, (int)(BoardSize.X - ballRadius));
         y = rng.Next(ballRadius, (int)(BoardSize.Y - ballRadius));

         isPositionCorrect = this.CheckIsSpaceFree(new Vector2(x, y), ballRadius);

         if (i >= 100)
            throw new NoAvailableSpaceForNewBallException();
         i++;
      }

      return new Vector2(x, y);
   }

   private bool CheckIsSpaceFree(Vector2 position, int ballRadius)
   {
      foreach (IBall? ball in ballsList)
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
      float distSq = (position1.X - position2.X) * (position1.X - position2.X) + (position1.Y - position2.Y) * (position1.Y - position2.Y);
      float radSumSq = (radius1 + radius2) * (radius1 + radius2);
      return distSq <= radSumSq;
   }

   private Vector2 GetRandomVelocity()
   {
      Random rng = new Random();
      int x = rng.Next(-MaxStartSpeed, MaxStartSpeed);
      int y = rng.Next(-MaxStartSpeed, MaxStartSpeed);
      if (Math.Abs(x) < MinStartSpeed)
      {
         x = MinStartSpeed;
      }

      if (Math.Abs(y) < MinStartSpeed)
      {
         y = MinStartSpeed;
      }

      return new Vector2(x, y);
   }

   public override void StartSimulation()
   {
      if (CancelSimulationSource.IsCancellationRequested)
      {
         return;
      }

      foreach (IBall? ball in ballsList)
      {
         ball.PositionChange += this.OnBallOnPositionChange;

         Task.Factory.StartNew(ball.Simulate, CancelSimulationSource.Token);
      }
   }

   private void OnBallOnPositionChange(object _, OnBallPositionChangeEventArgs args)
   {
      ballListLogger.AddToLogQueue(args.Ball);
      OnPositionChangeEventArgs newArgs = new OnPositionChangeEventArgs(args.Ball, new List<IBall>(ballsList));
      this.OnPositionChange(newArgs);
   }

   public override void StopSimulation()
   {
      CancelSimulationSource.Cancel();
   }
}

internal class NoAvailableSpaceForNewBallException : Exception
{ }