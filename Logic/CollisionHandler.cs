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
      var centerOne = ballOne.Position + (Vector2.One * ballOne.Radius / 2) + ballOne.Velocity * (8/1000f);
      var centerTwo = ballTwo.Position + (Vector2.One * ballTwo.Radius / 2) + ballTwo.Velocity * (8/1000f);

      var distance = Vector2.Distance(centerOne, centerTwo);
      var radiusSum = (ballOne.Radius + ballTwo.Radius) / 2f;
      
      return distance <= radiusSum;
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

   public static void HandleCollision(IBall ballOne, IBall ballTwo)
   {
      var centerOne = ballOne.Position + (Vector2.One * ballOne.Radius / 2);
      var centerTwo = ballTwo.Position + (Vector2.One * ballTwo.Radius / 2);

      var unitNormalVector = Vector2.Normalize(centerTwo - centerOne);
      var unitTangentVector = new Vector2(-unitNormalVector.Y, unitNormalVector.X);

      var velocityOneNormal = Vector2.Dot(unitNormalVector, ballOne.Velocity);
      var velocityOneTangent = Vector2.Dot(unitTangentVector, ballOne.Velocity);
      var velocityTwoNormal = Vector2.Dot(unitNormalVector, ballTwo.Velocity);
      var velocityTwoTangent = Vector2.Dot(unitTangentVector, ballTwo.Velocity);

      var newNormalVelocityOne = (velocityOneNormal * (ballOne.Mass - ballTwo.Mass) + 2 * ballTwo.Mass * velocityTwoNormal) / (ballOne.Mass + ballTwo.Mass);
      var newNormalVelocityTwo = (velocityTwoNormal * (ballTwo.Mass - ballOne.Mass) + 2 * ballOne.Mass * velocityOneNormal) / (ballOne.Mass + ballTwo.Mass);

      var newVelocityOne = Vector2.Multiply(unitNormalVector, newNormalVelocityOne) + Vector2.Multiply(unitTangentVector, velocityOneTangent);
      var newVelocityTwo = Vector2.Multiply(unitNormalVector, newNormalVelocityTwo) + Vector2.Multiply(unitTangentVector, velocityTwoTangent);

      ballOne.Velocity = newVelocityOne;
      ballTwo.Velocity = newVelocityTwo;
   }
}