using System.Collections.Generic;
using System.Threading;
using TPW.Data;

namespace TPW.Logic;

internal class BallsLogic : BallsLogicLayerAbstractApi
{
   private readonly BallsDataLayerAbstractApi dataBalls;
   private readonly Mutex simulationPauseMutex = new(false);

   public BallsLogic(BallsDataLayerAbstractApi? dataBalls)
   {
      this.dataBalls = dataBalls;
   }

   public override void AddBalls(int howMany)
   {
      dataBalls.Add(howMany);
   }

   public override void StartSimulation()
   {
      dataBalls.PositionChange += this.OnDataBallsOnPositionChange;
      dataBalls.StartSimulation();
   }

   private void OnDataBallsOnPositionChange(object _, Data.OnPositionChangeEventArgs args)
   {
      this.HandleBallsCollisions(args.SenderBall, args.Balls);
      CollisionHandler.CollideWithWalls(args.SenderBall, dataBalls.BoardSize);
      OnPositionChangeEventArgs newArgs = new OnPositionChangeEventArgs(new LogicBallAdapter(args.SenderBall));
      this.OnPositionChange(newArgs);
   }

   private void HandleBallsCollisions(IBall ball, IList<IBall> allBalls)
   {
      simulationPauseMutex.WaitOne();
      try
      {
         IBall? collidedBall = CollisionHandler.CheckCollisions(ball, allBalls);
         if (collidedBall != null)
         {
            CollisionHandler.HandleCollision(ball, collidedBall);
         }
      }
      finally
      { 
         simulationPauseMutex.ReleaseMutex();
      }
   }

   public override void StopSimulation()
   {
      dataBalls.StopSimulation();
   }
}