using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using TPW.Data;

namespace TPW.Logic;

internal class BallsLogic : BallsLogicLayerAbstractApi
{
	private readonly BallsDataLayerAbstractApi dataBalls;
	public CancellationTokenSource CancelSimulationSource { get; private set; }

	public BallsLogic(BallsDataLayerAbstractApi dataBalls, Vector2 boardSize)
	{
		this.dataBalls = dataBalls;
		BoardSize = boardSize;
		CancelSimulationSource = new CancellationTokenSource();
	}

	public Vector2 BoardSize { get; }

	protected override void OnPositionChange(OnPositionChangeEventArgs args)
	{
		base.OnPositionChange(args);
	}

	public override void AddBalls(int howMany)
	{
		for (var i = 0; i < howMany; i++)
		{
			var randomPoint = GetRandomPointInsideBoard();
			dataBalls.Add(BallsDataLayerAbstractApi.CreateBall(randomPoint));
		}
	}

	private Vector2 GetRandomPointInsideBoard()
	{
		return GetRandomPoint(BoardSize);
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
		if (position.X < 0 || position.X > BoardSize.X || position.Y < 0 || position.Y > BoardSize.Y)
			throw new PositionIsOutOfBoardException();
		dataBalls.Add(BallsDataLayerAbstractApi.CreateBall(position));
	}

	public override void StartSimulation()
	{
		if (CancelSimulationSource.IsCancellationRequested) return;

		CancelSimulationSource = new CancellationTokenSource();
		for (var i = 0; i < dataBalls.GetBallCount(); i++)
		{
			var ball = new LogicBallDecorator(dataBalls.Get(i), this);
			ball.PositionChange += (sender, args) => OnPositionChange(args);
			Task.Factory.StartNew(ball.Simulate, CancelSimulationSource.Token);
		}
	}

	public override void StopSimulation()
	{
		CancelSimulationSource.Cancel();
	}

	public override int GetBallsCount()
	{
		return dataBalls.GetBallCount();
	}

	public override IList<ILogicBall> GetBalls()
	{
		var ballsList = new List<ILogicBall>();
		for (var i = 0; i < dataBalls.GetBallCount(); i++) ballsList.Add(new LogicBallDecorator(dataBalls.Get(i), this));

		return ballsList;
	}
}