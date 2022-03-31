using System.Collections.Generic;
using System.Numerics;

namespace Data
{
    public class ArrayBalls : IBalls
    {
        private List<Ball> ballsList;

        public ArrayBalls()
        {
            this.ballsList = new List<Ball>();
        }

        public void Add(Ball ball)
        {
            ballsList.Add(ball);
        }

        public Ball Get(int index)
        {
            return ballsList[index];
        }

        public int GetBallCount()
        {
            return ballsList.Count;
        }
    }
}
