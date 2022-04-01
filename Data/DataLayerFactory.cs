namespace Data;

public static class DataLayerFactory
{
	public static IBalls CreateBallsList()
	{
		return new BallsList();
	}
}