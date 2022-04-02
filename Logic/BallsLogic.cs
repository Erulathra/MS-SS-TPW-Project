using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TPW.Data;

namespace TPW.Logic;

internal class BallsLogic : BallsLogicLayerAbstractApi
{
	private readonly BallsDataLayerAbstractApi balls;
	private readonly Vector2 boardSize;
	private CancellationTokenSource cancelSimulationSource;
	private int iterationCount = 0;

	public BallsLogic(Vector2 boardSize) : this(BallsDataLayerAbstractApi.CreateBallsList(), boardSize)
	{ }

	private BallsLogic(BallsDataLayerAbstractApi balls, Vector2 boardSize)
	{
		this.balls = balls;
		this.boardSize = boardSize;
		cancelSimulationSource = new CancellationTokenSource();
		PositionChange += (sender, args) =>
		{
			iterationCount++;
		};
	}

	protected override void OnPositionChange(OnPositionChangeEventArgs args)
	{
		base.OnPositionChange(args);
	}

	public override void AddBalls(int howMany)
	{
		for (var i = 0; i < howMany; i++)
		{
			var randomPoint = GetRandomPointInsideBoard();
			balls.Add(BallsDataLayerAbstractApi.CreateBall(randomPoint));
		}
	}

	private Vector2 GetRandomPointInsideBoard()
	{
		return GetRandomPoint(boardSize);
	}

	private Vector2 GetRandomPoint(Vector2 maxSize)
	{
		var rng = new Random();
		var x = (float)rng.NextDouble() * maxSize.X;
		var y = (float)rng.NextDouble() * maxSize.Y;
		return new Vector2(x, y);
	}

	public override void AddBall(Vector2 position)
	{
		if (position.X < 0 || position.X > boardSize.X || position.Y < 0 || position.Y > boardSize.Y)
			throw new PositionIsOutOfBoardException();
		balls.Add(BallsDataLayerAbstractApi.CreateBall(position));
	}

	public override void StartSimulation()
	{
		if (cancelSimulationSource.IsCancellationRequested) return;

		cancelSimulationSource = new CancellationTokenSource();
		Task.Factory.StartNew(Simulation, cancelSimulationSource.Token);
	}

	public override void StopSimulation()
	{
		cancelSimulationSource.Cancel();
	}

	private async Task Simulation()
	{
		while (!cancelSimulationSource.IsCancellationRequested)
		{
			for (var i = 0; i < balls.GetBallCount(); i++)
			{
				balls.Get(i).Position = GetRandomPointInsideBoard();
				OnPositionChange(new OnPositionChangeEventArgs{ball = new LogicBallAdapter(balls.Get(i))});
			}
			await Task.Delay(16, cancelSimulationSource.Token);
		}
	}

	public override int GetBallsCount()
	{
		return balls.GetBallCount();
	}

	public override IList<ILogicBall> GetBalls()
	{
		var ballsList = new List<ILogicBall>();
		for (int i = 0; i < balls.GetBallCount(); i++)
		{
			ballsList.Add(new LogicBallAdapter(balls.Get(i)));
		}

		return ballsList;
	}
}