namespace Data
{
    public interface IBalls
    {
        void Add(Ball ball);
        Ball Get(int index);
        int GetBallCount();
    }
}