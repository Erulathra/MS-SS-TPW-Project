using System;
using System.Collections.Generic;
using System.Numerics;
using TPW.Logic;

namespace TPW.Presentation.Model
{
   public class MainModel
   {
      private readonly Vector2 boardSize;
      private int ballsAmount;
      private BallsLogicLayerAbstractApi ballsLogic;

      public MainModel()
      {
         boardSize = new Vector2(650, 400);
         ballsAmount = 0;
         this.PrepareBallsLogic();
      }

      public event EventHandler<OnPositionChangeEventArgs>? BallPositionChange;

      public void StartSimulation()
      {
         ballsLogic.AddBalls(ballsAmount);
         ballsLogic.StartSimulation();
      }

      public void StopSimulation()
      {
         ballsLogic.StopSimulation();
         this.PrepareBallsLogic();
      }

      private void PrepareBallsLogic()
      {
         ballsLogic = BallsLogicLayerAbstractApi.CreateBallsLogic(boardSize);
         ballsLogic.PositionChange += (sender, args) =>
         {
            IList<IModelBall> logicBallList = new List<IModelBall>();
            foreach (var ball in args.Balls)
               logicBallList.Add(new ModelBallAdapter(ball));
          
            BallPositionChange?.Invoke(this, new OnPositionChangeEventArgs(new ModelBallAdapter(args.SenderBall), logicBallList));
         };
      }

      public void SetBallNumber(int amount)
      {
         ballsAmount = amount;
      }

      public int GetBallsCount()
      {
         return ballsAmount;
      }

      public void OnBallPositionChange(OnPositionChangeEventArgs args)
      {
         BallPositionChange?.Invoke(this, args);
      }
   }
}