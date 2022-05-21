using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Serialization;
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
public interface IBall : ISerializable
{
   Vector2 Position { get; }
   float Radius { get; }
   float Mass { get; }
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
      Mass = weight;
      Radius = radius;
      this.ID = ID;
   }

   public Vector2 Position { get; private set; }
   public float Radius { get; }
   public float Mass { get; }
   public Vector2 Velocity { get; set; }
   public int ID { get; }
   public event EventHandler<OnBallPositionChangeEventArgs>? PositionChange;

   public async void Simulate()
   {
      Stopwatch sw = new Stopwatch();
      float deltaTime = 0f;
      while (!owner.CancelSimulationSource.Token.IsCancellationRequested)
      {
         sw.Start();
         OnBallPositionChangeEventArgs newArgs = new OnBallPositionChangeEventArgs(this);
         PositionChange?.Invoke(this, newArgs);
         
         Vector2 nextPosition = Position + Vector2.Multiply(Velocity, deltaTime);
         Position = this.ClampPosition(nextPosition);

         await Task.Delay(4, owner.CancelSimulationSource.Token).ContinueWith(_ => { });
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

   public void GetObjectData(SerializationInfo info, StreamingContext context)
   {
      info.AddValue("ID", ID);
      info.AddValue("Radius", Radius);
      info.AddValue("Mass", Mass);
      info.AddValue("Position", Position);
      info.AddValue("Velocity", Velocity);
   }
}