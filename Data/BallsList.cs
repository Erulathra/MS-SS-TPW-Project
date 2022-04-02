using System.Collections.Generic;
using System.Numerics;

namespace TPW.Data
{
    internal class BallsList : BallsDataLayerAbstractApi
    {
        private readonly List<IBall> ballsList;

        public BallsList()
        {
            this.ballsList = new List<IBall>();
        }

        public override void Add(IBall ball)
        {
            ballsList.Add(ball);
        }

        public override IBall Get(int index)
        {
            return ballsList[index];
        }

        public override int GetBallCount()
        {
            return ballsList.Count;
        }
    }
}
