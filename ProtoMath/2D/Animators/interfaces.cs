namespace ProtoMath._2D.Animators
{
    public interface IAnimator
    {
        void Update(float time);
        bool IsDone { get; }
        void Rewind();
    }
}
