using System;
using System.Numerics;
using System.Threading.Tasks;
using TPW.Data;

namespace TPW.Logic;

public interface ILogicBall
{
	Vector2 Position { get; set; }
	int id { get; }
}

internal class LogicBallDecorator : ILogicBall
{
	private readonly IBall ball;
	private readonly BallsLogic owner;
	private Random rng;
	public event EventHandler<OnPositionChangeEventArgs>? PositionChange;
	public int id { get; private set; }

	public LogicBallDecorator(IBall ball, int id, BallsLogic owner)
	{
		this.ball = ball;
		this.owner = owner;
		this.id = id;
		rng = new Random();
	}

	public LogicBallDecorator(Vector2 position, int id, BallsLogic owner)
	{
		ball = BallsDataLayerAbstractApi.CreateBall(position);
		this.id = id;
		this.owner = owner;
		rng = new Random();
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

			await Task.Delay(32, owner.CancelSimulationSource.Token).ContinueWith(tsk => { });
		}
	}

	private Vector2 GetRandomPointInsideBoard()
	{
		Vector2 translationVector = GetRandomNormalizedVector();
		Vector2 newPosition = Position + translationVector;

		if(newPosition.X < BallsLogic.BallRadius || newPosition.X > owner.BoardSize.X - BallsLogic.BallRadius)
        {
			translationVector.X = - translationVector.X;
        }

		if (newPosition.Y < BallsLogic.BallRadius || newPosition.Y > owner.BoardSize.Y - BallsLogic.BallRadius)
		{
			translationVector.Y = - translationVector.Y;
		}

		return Position + translationVector;
	}

	
	private Vector2 GetRandomNormalizedVector()
	{
		var x = (float)(rng.NextDouble() - 0.5) * 2;
		var y = (float)(rng.NextDouble() - 0.5) * 2;
		var result = new Vector2(x, y);
		return Vector2.Normalize(result);
	}
}