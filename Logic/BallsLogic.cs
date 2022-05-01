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
      dataBalls.PositionChange += (_, argv) =>
      {
         this.OnPositionChange(new OnPositionChangeEventArgs(new LogicBallAdapter(argv.Ball)));
      };
      dataBalls.StartSimulation();
   }

   public override void StopSimulation()
   {
      dataBalls.StopSimulation();
   }
}