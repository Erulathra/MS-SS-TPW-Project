using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using TPW.Logic;

namespace TPW.Presentation.Model
{
    public class MainModel
    {
        private Vector2 boardSize;
        private int ballsAmount;
        private BallsLogicLayerAbstractApi ballsLogic;

        public event EventHandler<OnPositionChangeEventArgs>? BallPositionChange;

        public MainModel()
        {
            boardSize = new Vector2(650, 400);
            ballsAmount = 0;
            ballsLogic = BallsLogicLayerAbstractApi.CreateBallsLogic(boardSize);
            /*ballsLogic.PositionChange += (sender, args) =>
            {
                //move ball
            }*/
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
