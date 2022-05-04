using System.Collections.Generic;
using System.Numerics;
using TPW.Data;

namespace TPW.Logic;

internal static class CollisionHandler
{
   public static IBall? CheckCollisions(IBall ball, IEnumerable<IBall> ballsList)
   {
      foreach (var ballTwo in ballsList)
      {
         if (ReferenceEquals(ball, ballTwo))
         {
            continue;
         }

         if (IsBallsCollides(ball, ballTwo))
         {
            return ballTwo;
         }
      }

      return null;
   }

   private static bool IsBallsCollides(IBall ballOne, IBall ballTwo)
   {
      var distSq = (ballOne.Position.X - ballTwo.Position.X) * (ballOne.Position.X - ballTwo.Position.X)
                   + (ballOne.Position.Y - ballTwo.Position.Y) * (ballOne.Position.Y - ballTwo.Position.Y);

      var radSumSq = (ballOne.Radius + ballTwo.Radius) * (ballOne.Radius + ballTwo.Radius);
      return distSq <= radSumSq;
   }

   public static void CollideWithWalls(IBall ball, Vector2 boardSize)
   {
      if (ball.Position.X <= 0 || ball.Position.X + ball.Radius + 0 >= boardSize.X)
      {
         ball.Velocity = new Vector2(-ball.Velocity.X, ball.Velocity.Y);
      }

      if (ball.Position.Y <= 0 || ball.Position.Y + ball.Radius + 0 >= boardSize.Y)
      {
         ball.Velocity = new Vector2(ball.Velocity.X, -ball.Velocity.Y);
      }
   }
}