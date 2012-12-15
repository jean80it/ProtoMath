namespace ProtoMath._2D.Observables
{
    public interface IObservable
    {
        event ValueChangedHandler ValueChanged;
    }

    public interface IObservableScalar<T> : IScalar<T>, IObservable
    { }
    
    public interface IObservableEntity<T> : IEntity<T>, IObservable
    { }

    public interface IObservablePoint<T> : IObservableEntity<T>, IPoint<T>
    { }

    public interface IObservableVector<T> : IVector<T>, IObservable
    { }

    public interface IObservableCircle<T> : ICircle<T>, IObservable
    { }
}
