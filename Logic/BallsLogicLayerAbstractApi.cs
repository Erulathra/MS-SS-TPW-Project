using System;
using System.Collections.Generic;
using System.Numerics;
using TPW.Data;

namespace TPW.Logic;

public class OnPositionChangeEventArgs : EventArgs
{
	public ILogicBall ball;

	public OnPositionChangeEventArgs(ILogicBall ball)
	{
		this.ball = ball;
	}
}

public abstract class BallsLogicLayerAbstractApi
{
	public event EventHandler<OnPositionChangeEventArgs>? PositionChange;
	public abstract void AddBalls(int howMany);
	public abstract void AddBall(Vector2 position);
	public abstract void StartSimulation();
	public abstract void StopSimulation();

	public abstract int GetBallsCount();
	public abstract IList<ILogicBall> GetBalls();

	protected virtual void OnPositionChange(OnPositionChangeEventArgs args)
	{
		PositionChange?.Invoke(this, args);
	}

	public static BallsLogicLayerAbstractApi CreateBallsLogic(Vector2 boardSize, BallsDataLayerAbstractApi dataApi = default(BallsDataLayerAbstractApi))
	{
		if (dataApi == null)
		{
			dataApi = BallsDataLayerAbstractApi.CreateBallsList();
		}
		return new BallsLogic(dataApi, boardSize);
	}
}