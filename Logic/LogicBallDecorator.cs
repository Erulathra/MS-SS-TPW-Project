using System;
using System.Numerics;
using System.Threading.Tasks;
using TPW.Data;

namespace TPW.Logic;

public interface ILogicBall
{
	Vector2 Position { get; set; }
}

internal class LogicBallDecorator : ILogicBall
{
	private static readonly double BallRadius = 3.0;
	private readonly IBall ball;
	private readonly BallsLogic owner;
	public event EventHandler<OnPositionChangeEventArgs>? PositionChange;

	public LogicBallDecorator(IBall ball, BallsLogic owner)
	{
		this.ball = ball;
		this.owner = owner;
	}

	public LogicBallDecorator(Vector2 position, BallsLogic owner)
	{
		ball = BallsDataLayerAbstractApi.CreateBall(position);
		this.owner = owner;
	}

	public Vector2 Position
	{
		get => ball.Position;
		set => ball.Position = value;
	}

	public async void Simulate()
	{
		while (true)
		{
			Position = GetRandomPointInsideBoard();
			PositionChange?.Invoke(this, new OnPositionChangeEventArgs(this));

			await Task.Delay(16, owner.CancelSimulationSource.Token).ContinueWith(tsk => { });
		}
	}

	private Vector2 GetRandomPointInsideBoard()
	{
		Vector2 newPosition;
		do
		{
			newPosition = Position + GetRandomNormalizedVector();
		} while (Position.X < BallRadius || Position.X > owner.BoardSize.X - BallRadius
		                                 || Position.Y < BallRadius || Position.Y > owner.BoardSize.Y - BallRadius);

		return newPosition;
	}

	private Vector2 GetRandomNormalizedVector()
	{
		var rng = new Random();
		var x = (float)rng.NextDouble();
		var y = (float)rng.NextDouble();
		var result = new Vector2(x, y);
		return Vector2.Normalize(result);
	}
}