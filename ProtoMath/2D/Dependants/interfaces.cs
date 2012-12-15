using ProtoMath._2D.Observables;
namespace ProtoMath._2D.Dependants
{
    public interface IDependant
    {
        void Validate();

        void Invalidate();
    }

    public interface IDependantScalar<T> : IObservableScalar<T>, IDependant
    { }

    public interface IDependantEntity<T> : IObservableEntity<T>, IDependant
    { }

    public interface IDependantPoint<T> : IDependantEntity<T>
    { }
    
    public interface IDependantVector<T> : IDependantEntity<T>, IObservableVector<T>
    { }
}
