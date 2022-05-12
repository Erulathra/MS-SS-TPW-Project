using System;
using System.Numerics;

namespace TPW.Data;
internal class BallFixture : IBall
{
   private readonly BallsDataLayerAbstractApi owner;

   public BallFixture(int ID, Vector2 position, float radius, float weight, Vector2 velocity, BallsDataLayerAbstractApi owner)
   {
      Position = position;
      Velocity = velocity;
      this.owner = owner;
      Mass = weight;
      Radius = radius;
      this.ID = ID;
   }

   public Vector2 Position { get; set; }
   public float Radius { get; set; }
   public float Mass { get; set; }
   public Vector2 Velocity { get; set; }
   public int ID { get; }
   public event EventHandler<OnBallPositionChangeEventArgs>? PositionChange;

   public void Simulate()
   { }
}