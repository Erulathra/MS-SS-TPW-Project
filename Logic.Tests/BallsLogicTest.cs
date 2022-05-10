using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;

namespace TPW.Logic.Tests;

public class BallsLogicTest
{
	private BallsLogicLayerAbstractApi ballsLogic;
	private readonly Vector2 boardSize = new Vector2(150, 100);

	[SetUp]
	public void SetUp()
	{
		ballsLogic = BallsLogicLayerAbstractApi.CreateBallsLogic(boardSize);
	}

    [Test]
    public void LogicSimulationTest()
    {
        //TODO: test event and collisions
    }
}