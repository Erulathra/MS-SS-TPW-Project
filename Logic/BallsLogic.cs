using System.Collections.Generic;
using TPW.Data;

namespace TPW.Logic;

internal class BallsLogic : BallsLogicLayerAbstractApi
{
   private readonly BallsDataLayerAbstractApi dataBalls;

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
      dataBalls.PositionChange += (_, args) =>
      {
         IList<ILogicBall> logicBallList = new List<ILogicBall>();
         foreach (var ball in args.Balls)
            logicBallList.Add(new LogicBallAdapter(ball));
         
         CollisionHandler.CollideWithWalls(args.SenderBall, dataBalls.BoardSize);
       
         this.OnPositionChange(new OnPositionChangeEventArgs(new LogicBallAdapter(args.SenderBall), logicBallList));
      };
      dataBalls.StartSimulation();
   }

   public override void StopSimulation()
   {
      dataBalls.StopSimulation();
   }
}