
using ProtoMath._2D.Observables;
namespace ProtoMath._2D.Animators
{
    public interface ISprite<T>
    {
        IPoint<T> Position { get; set; }
        // TODO: add rotation, color, size, ...
    }

    public interface IObservableSprite<T>
    {
        IObservablePoint<T> Position { get; set; }
        // TODO: add rotation, color, size, ...
    }

    public interface IPointParticle<T> : IAnimator
    {
        IObservablePoint<T> Position { get; }
        IVector<T> Speed { get; }
        IVector<T> Acceleration { get; }
        T Damping { get; set; }
        
        void FeedForce(IVector<T> force);
        void FeedForceN(IVector<T> force);
    }
}
