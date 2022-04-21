using System;
using System.Numerics;
using TPW.Logic;

public class OnPositionChangeUiAdapterEventArgs : EventArgs
{
	public readonly Vector2 Position;
    public readonly int Id;

    public OnPositionChangeUiAdapterEventArgs(Vector2 position, int id)
    {
        this.Position = position;
        Id = id;
    }
}

namespace TPW.Presentation.Model
{
    public class MainModel
    {
        private readonly Vector2 boardSize;
        private int ballsAmount;
        private BallsLogicLayerAbstractApi ballsLogic;

        public event EventHandler<OnPositionChangeUiAdapterEventArgs>? BallPositionChange;

        public MainModel()
        {
            boardSize = new Vector2(650, 400);
            ballsAmount = 0;
            ballsLogic = BallsLogicLayerAbstractApi.CreateBallsLogic(boardSize);
            ballsLogic.PositionChange += (sender, args) =>
            {
                BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(args.Ball.Position, args.Ball.Id));
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
                BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(args.Ball.Position, args.Ball.Id));
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
