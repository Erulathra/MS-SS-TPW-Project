using System.Numerics;

namespace TPW.Data
{
    public interface IBall
    {
        Vector2 Position { get; set; }
    }

    internal class Ball : IBall
    {
        public Vector2 Position { get; set; }

        public Ball(Vector2 position)
        {
            this.Position = position;
        }

    }
}
