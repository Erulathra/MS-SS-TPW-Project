using System.Numerics;

namespace TPW.Data;

public abstract class BallsDataLayerAbstractApi
{
	public abstract void Add(IBall ball);
	public abstract IBall Get(int index);
	public abstract int GetBallCount();
	public static BallsDataLayerAbstractApi CreateBallsList()
	{
		return new BallsList();
	}

	public static IBall CreateBall(Vector2 position)
	{
		return new Ball(position);
	}
}