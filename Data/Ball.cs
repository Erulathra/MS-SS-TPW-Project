using System.Numerics;

namespace TPW.Data
{
    public interface IBall
    {
        Vector2 Position { get; set; }
        void Move(Vector2 moveVector);
    }

    internal class Ball : IBall
    {
        public Vector2 Position { get; set; }

        public Ball(Vector2 position)
        {
            this.Position = position;
        }

        public void Move(Vector2 moveVector)
        {
            this.Position += Position;
        }
    }
}
