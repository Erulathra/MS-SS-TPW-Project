using System;
using System.Numerics;
using TPW.Data;

namespace TPW.Logic;

public class BallsLogic
{
	private readonly IBalls balls;
	private readonly Vector2 boardSize;

	public BallsLogic(IBalls balls, Vector2 boardSize)
	{
		this.balls = balls;
		this.boardSize = boardSize;
	}

	public void AddBalls(int howMany)
	{
		for (var i = 0; i < howMany; i++)
		{
			var rng = new Random();
			var x = (float)rng.NextDouble() * boardSize.X;
			var y = (float)rng.NextDouble() * boardSize.Y;
			var randomPoint = new Vector2(x, y);
			balls.Add(new Ball(randomPoint));
		}
	}

	public void AddBall(Vector2 position)
	{
		if (position.X < 0 || position.X > boardSize.X || position.Y < 0 || position.Y > boardSize.Y)
			throw new PositionIsOutOfBoardException();
		balls.Add(new Ball(position));
	}
}