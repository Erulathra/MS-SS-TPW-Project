using System.Numerics;
using TPW.Logic;

namespace TPW.Presentation.Model;

public interface IModelBall
{
   Vector2 Position { get; }
   float Radius { get; }
   int ID { get; }
}

internal class ModelBallAdapter : IModelBall
{
   private readonly ILogicBall ball;

   public ModelBallAdapter(ILogicBall ball)
   {
      this.ball = ball;
   }

   public Vector2 Position { get => ball.Position; }
   public float Radius { get => ball.Radius; }
   public int ID { get => ball.ID; }
}