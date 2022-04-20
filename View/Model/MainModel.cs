using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using TPW.Logic;

namespace TPW.Presentation.Model
{
    public class MainModel
    {
        private Vector2 boardSize = new Vector2(650, 400);
        private BallsLogicLayerAbstractApi ballsLogic;

        public event EventHandler<OnPositionChangeEventArgs>? BallPositionChange;

        public MainModel()
        {
            ballsLogic = BallsLogicLayerAbstractApi.CreateBallsLogic(boardSize);
            /*ballsLogic.PositionChange += (sender, args) =>
            {
                //move ball
            }*/
        }
        internal void StartSimulation(int ballsAmount)
        {
            ballsLogic.AddBalls(ballsAmount);
            ballsLogic.StartSimulation();
        }

        internal void AddBalls(int amount)
		{
            ballsLogic.AddBalls(amount);
		}

        internal void RestartBoard(int amount)
		{
            throw new NotImplementedException();
            /*ballsLogic = BallsLogicLayerAbstractApi.CreateBallsLogic(boardSize);
            ballsLogic.AddBalls(amount);
            ballsLogic.StartSimulation();*/
        }

        internal void SetBallNumber(int amount)
		{
            int amountNow = GetBallsCount();
            if (amountNow < amount)
                ballsLogic.AddBalls(amount - amountNow);
            else if(amountNow > amount)
            {
                RestartBoard(amount);
            }
        }

        internal int GetBallsCount()
		{
            return ballsLogic.GetBallsCount();
		}

        internal void OnBallPositionChange(OnPositionChangeEventArgs args)
        {
            BallPositionChange?.Invoke(this, args);
        }
    }
}
