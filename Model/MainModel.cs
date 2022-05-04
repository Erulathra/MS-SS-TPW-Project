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
         ballsLogic.PositionChange += this.OnBallsLogicOnPositionChange;
      }

      private void OnBallsLogicOnPositionChange(object sender, Logic.OnPositionChangeEventArgs args)
      {
         BallPositionChange?.Invoke(this, new OnPositionChangeEventArgs(new ModelBallAdapter(args.Ball)));
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