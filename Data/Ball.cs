using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;

namespace TPW.Data;
public class OnPositionChangeEventArgs : EventArgs
{
   public readonly IBall Ball;

   public OnPositionChangeEventArgs(IBall ball)
   {
      this.Ball = ball;
   }
}

public interface IBall
{
   Vector2 Position { get; }
   float Radius { get; }
   float Weight { get; }
   Vector2 Velocity { get; }
   
   int ID { get; }
   public void Simulate();

   public event EventHandler<OnPositionChangeEventArgs>? PositionChange;
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
   public float Radius { get; private set; }
   public float Weight { get; private set; }
   public Vector2 Velocity { get; private set; }
   public int ID { get; }
   public event EventHandler<OnPositionChangeEventArgs>? PositionChange;

   public async void Simulate()
   {
      var sw = new Stopwatch();
      var deltaTime = 0f;
      while (!owner.CancelSimulationSource.Token.IsCancellationRequested)
      {
         sw.Start();

         Position += Vector2.Multiply(Velocity, deltaTime);
         PositionChange?.Invoke(this, new OnPositionChangeEventArgs(this));
         
			await Task.Delay(32, owner.CancelSimulationSource.Token).ContinueWith(_ => { });
         // Delta time calculation
         sw.Stop();
         deltaTime = sw.ElapsedMilliseconds / 1000f;
         sw.Reset();
      }
   }
}