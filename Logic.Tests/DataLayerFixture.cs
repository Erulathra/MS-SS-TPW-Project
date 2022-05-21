using System.Collections.Generic;
using System.Numerics;
using TPW.Data;

namespace TPW.Logic.Tests;

public class DataLayerFixture : BallsDataLayerAbstractApi
{
   public bool isSimulationWorking;

   public DataLayerFixture(Vector2 boardSize) : base(boardSize)
   {
      this.boardSize = boardSize;
      BallsList = new List<IBall>();
   }

   public List<IBall> BallsList { get; set; }
   public Vector2 boardSize { get; set; }

   public override void Add(int howMany)
   {
      for (int i = 0; i < howMany; i++)
      {
         BallsList.Add(new BallFixture(1, new Vector2(1, 1), 1, 1, new Vector2(1, 1), this));
      }
   }

   public override void StartSimulation()
   {
      isSimulationWorking = true;
   }

   public override void StopSimulation()
   {
      isSimulationWorking = false;
   }

   public void OnBallOnPositionChange()
   {
      IBall ball = BallsList[0];
      Data.OnPositionChangeEventArgs newArgs = new TPW.Data.OnPositionChangeEventArgs(ball, new List<IBall>(BallsList));
      this.OnPositionChange(newArgs);
   }
}