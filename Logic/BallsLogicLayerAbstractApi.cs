using System;
using System.Collections.Generic;
using System.Numerics;

namespace TPW.Logic;

public class OnPositionChangeEventArgs : EventArgs
{
	public LogicBallAdapter ball;
}

public abstract class BallsLogicLayerAbstractApi
{
	public event EventHandler<OnPositionChangeEventArgs> PositionChange;
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
	public static BallsLogicLayerAbstractApi CreateBallsLogic(Vector2 boardSize)
	{
		return new BallsLogic(boardSize);
	}

	public static ILogicBall CreateLogicBall(Vector2 position)
	{
		return new LogicBallAdapter(position);
	}
}