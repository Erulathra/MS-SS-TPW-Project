using System.Numerics;
using System.Threading;

namespace TPW.Data;

public abstract class BallsDataLayerAbstractApi
{

   public abstract void Add(IBall ball);
	public abstract IBall Get(int index);
	public abstract int GetBallCount();
   public CancellationTokenSource CancelSimulationSource { get; }
   
   protected BallsDataLayerAbstractApi()
   {
      CancelSimulationSource = new CancellationTokenSource();
   }
   
	public static BallsDataLayerAbstractApi CreateBallsList()
	{
		return new BallsList();
	}

	public static IBall CreateBall(Vector2 position)
	{
		//return new Ball(position);
	}
}