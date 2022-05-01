using System;
using System.Numerics;
using System.Threading.Tasks;
using TPW.Data;

namespace TPW.Logic;

public interface ILogicBall
{
   Vector2 Position { get; }
   float Radius { get; }
   int Id { get; }
}

internal class LogicBallAdapter : ILogicBall
{
   private readonly IBall ball;

   public LogicBallAdapter(IBall ball)
   {
      this.ball = ball;
   }

   public Vector2 Position { get => ball.Position; }
   public float Radius { get => ball.Radius; }
   public int Id { get => ball.ID; }
}