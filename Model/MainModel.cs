using System;
using System.Numerics;
using TPW.Logic;

namespace TPW.Presentation.Model
{
   public class OnPositionChangeUiAdapterEventArgs : EventArgs
   {
      public readonly int Id;
      public readonly Vector2 Position;
      public readonly float Radius;

      public OnPositionChangeUiAdapterEventArgs(int id, Vector2 position, float radius)
      {
         Position = position;
         Radius = radius;
         Id = id;
      }
   }

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

      public event EventHandler<OnPositionChangeUiAdapterEventArgs>? BallPositionChange;

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
            BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(args.Ball.Id, args.Ball.Position, args.Ball.Radius));
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

      public void OnBallPositionChange(OnPositionChangeUiAdapterEventArgs args)
      {
         BallPositionChange?.Invoke(this, args);
      }
   }
}