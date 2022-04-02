using System;
using System.Numerics;

namespace TPW.Logic;

public abstract class BallsLogicLayerAbstractApi
{
	public event EventHandler<OnPositionChangeEventArgs> OnPositionChange;
	public abstract void AddBalls(int howMany);
	public abstract void AddBall(Vector2 position);
	public abstract void StartSimulation();
	public abstract void StopSimulation();

	public static BallsLogicLayerAbstractApi CreateBallsLogic(Vector2 boardSize)
	{
		return new BallsLogic(boardSize);
	}

	public static ILogicBall CreateLogicBall(Vector2 position)
	{
		return new LogicBallAdapter(position);
	}
}