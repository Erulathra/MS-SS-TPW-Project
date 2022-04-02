using System.Numerics;
using TPW.Data;

namespace TPW.Logic;

public interface ILogicBall
{
	Vector2 Position { get; set; }
}

public class LogicBallAdapter : ILogicBall
{
	private readonly IBall ball;

	public LogicBallAdapter(IBall ball)
	{
		this.ball = ball;
	}

	//todo: to trzeba zmienić na api
	public LogicBallAdapter(Vector2 position)
	{
		this.ball = BallsDataLayerAbstractApi.CreateBall(position);
	}

	public Vector2 Position
	{
		get => ball.Position;
		set => ball.Position = value;
	}

}