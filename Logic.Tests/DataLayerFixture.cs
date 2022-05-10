using System.Collections.Generic;
using System.Numerics;
using TPW.Data;

namespace TPW.Logic.Tests;

public class DataLayerFixture : BallsDataLayerAbstractApi
{
	private readonly List<IBall> ballsList;
    private readonly Vector2 boardSize;

    public DataLayerFixture(Vector2 boardSize) : base(boardSize)
    {
        this.boardSize = boardSize;
        this.ballsList = new List<IBall>();
    }
    //TODO: consult what to do

 //    public override void AddBalls(int howMany)
	// {
	// 	ballsList.Add(howMany);
	// }
 //
	// public override int GetBallCount()
	// {
	// 	return ballsList.Count;
	// }
    public override void Add(int howMany)
    {
        throw new System.NotImplementedException();
    }

    public override void StartSimulation()
    {
        throw new System.NotImplementedException();
    }

    public override void StopSimulation()
    {
        throw new System.NotImplementedException();
    }
}