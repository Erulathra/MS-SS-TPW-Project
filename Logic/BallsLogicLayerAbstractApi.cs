using System;
using System.Numerics;
using TPW.Data;

namespace TPW.Logic;

public abstract class BallsLogicLayerAbstractApi
{
	public event EventHandler<OnPositionChangeEventArgs>? PositionChange;
	public abstract void AddBalls(int howMany);
	public abstract void StartSimulation();
	public abstract void StopSimulation();

	protected void OnPositionChange(OnPositionChangeEventArgs args)
	{
		PositionChange?.Invoke(this, args);
	}

	public static BallsLogicLayerAbstractApi CreateBallsLogic(Vector2 boardSize, BallsDataLayerAbstractApi? dataApi = default(BallsDataLayerAbstractApi))
   {
      dataApi ??= BallsDataLayerAbstractApi.CreateBallsList(boardSize);
      return new BallsLogic(dataApi);
   }
}