using System;
using System.Numerics;
using TPW.Logic;

namespace TPW.Presentation.Model
{
    public class MainModel
    {
        private readonly Vector2 boardSize;
        private int ballsAmount;
        private BallsLogicLayerAbstractApi ballsLogic;

        public event EventHandler<OnPositionChangeEventArgs>? BallPositionChange;

        public MainModel()
        {
            boardSize = new Vector2(650, 400);
            ballsAmount = 0;
            ballsLogic = BallsLogicLayerAbstractApi.CreateBallsLogic(boardSize);
            ballsLogic.PositionChange += (sender, args) =>
            {
                BallPositionChange?.Invoke(this, args);
            };
        }
        public void StartSimulation()
        {
            ballsLogic.AddBalls(ballsAmount);
            ballsLogic.StartSimulation();
        }

        public void StopSimulation()
		{
            ballsLogic.StopSimulation();
            ballsLogic = BallsLogicLayerAbstractApi.CreateBallsLogic(boardSize);
            ballsLogic.PositionChange += (sender, args) =>
            {
                BallPositionChange?.Invoke(this, args);
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
