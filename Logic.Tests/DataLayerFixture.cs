using System.Collections.Generic;
using TPW.Data;

namespace TPW.Logic.Tests;

public class DataLayerFixture : BallsDataLayerAbstractApi
{
	private readonly List<IBall> ballsList;

	public DataLayerFixture()
	{
		this.ballsList = new List<IBall>();
	}

	public override void Add(IBall ball)
	{
		ballsList.Add(ball);
	}

	public override IBall Get(int index)
	{
		return ballsList[index];
	}

	public override int GetBallCount()
	{
		return ballsList.Count;
	}
}