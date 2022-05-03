using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace TPW.Data;

public class OnBallPositionChangeEventArgs
   {
      public IBall Ball;

      public OnBallPositionChangeEventArgs(IBall ball)
      {
         this.Ball = ball;
      }
   }
public interface IBall
{
   Vector2 Position { get; }
   float Radius { get; }
   float Weight { get; }
   Vector2 Velocity { get; set; }

   int ID { get; }
   public void Simulate();

   public event EventHandler<OnBallPositionChangeEventArgs>? PositionChange;
}

internal class Ball : IBall
{
   private readonly BallsDataLayerAbstractApi owner;

   public Ball(int ID, Vector2 position, float radius, float weight, Vector2 velocity, BallsDataLayerAbstractApi owner)
   {
      Position = position;
      Velocity = velocity;
      this.owner = owner;
      Weight = weight;
      Radius = radius;
      this.ID = ID;
   }

   public Vector2 Position { get; private set; }
   public float Radius { get; }
   public float Weight { get; }
   public Vector2 Velocity { get; set; }
   public int ID { get; }
   public event EventHandler<OnBallPositionChangeEventArgs>? PositionChange;

   public async void Simulate()
   {
      var sw = new Stopwatch();
      var deltaTime = 0f;
      while (!owner.CancelSimulationSource.Token.IsCancellationRequested)
      {
         sw.Start();

         var nextPosition = Position + Vector2.Multiply(Velocity, deltaTime);
         Position = this.ClampPosition(nextPosition);
         
         PositionChange?.Invoke(this, new OnBallPositionChangeEventArgs(this));

         await Task.Delay(8, owner.CancelSimulationSource.Token).ContinueWith(_ => { });
         // Delta time calculation
         sw.Stop();
         deltaTime = sw.ElapsedMilliseconds / 1000f;
         sw.Reset();
      }
   }

   private Vector2 ClampPosition(Vector2 nextPosition)
   {
      if (nextPosition.X < 0)
         nextPosition.X = -1;
      if (Radius + nextPosition.X > owner.BoardSize.X)
         nextPosition.X = owner.BoardSize.X - Radius + 1;

      if (nextPosition.Y < 0)
         nextPosition.Y = -1;
      if (Radius + nextPosition.Y > owner.BoardSize.Y)
         nextPosition.Y = owner.BoardSize.Y - Radius + 1;
      return nextPosition;
   }
}