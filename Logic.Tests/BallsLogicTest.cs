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
    public void AddBalls()
    {
        Assert.DoesNotThrow(() => ballsLogic.AddBalls(5));
    }

    [Test]
    public void LogicSimulationTest()
    {
        var interactionCount = 0;
        ballsLogic.AddBalls(5);
        var ballsPositions = new Dictionary<int, Vector2>();

        ballsLogic.PositionChange += (sender, args) =>
        {
            if(ballsPositions.ContainsKey(args.Ball.ID))
                Assert.AreNotEqual(args.Ball.Position, args.Ball.ID);
            else
                ballsPositions[args.Ball.ID] = args.Ball.Position;
            
            interactionCount++;
            if (interactionCount >= 100)
            {
                ballsLogic.StopSimulation();
            }
        };
        ballsLogic.StartSimulation();
        while (interactionCount < 100)
        { }

        Assert.GreaterOrEqual(interactionCount, 99);
    }
}