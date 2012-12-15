namespace ProtoMath._2D
{
    public interface IScalar<T>
    {
        T Value { get; set; }
    }
    
    public interface IEntity<T>
    {
        T X { get; set; }
        T Y { get; set; }

        void SetValue(T x, T y);
        void SetValue(IEntity<T> value);
    }
    
    public interface IPoint<T> : IEntity<T>
    { }

    public interface IVector<T> : IEntity<T>
    {
        T Length { get; }
        IVector<T> Versor { get; }
    }

    public interface ILine<T>
    {
        T A { get; set; }
        T B { get; set; }
        T C { get; set; }
    }

    public interface ICircle<T>
    {
        IPoint<T> Center { get; set; }
        T Radius { get; set; }
    }

    public interface ISegment<T>
    {
        IPoint<T> A { get; set; }
        IPoint<T> B { get; set; }
        // vector?
        // versor??
        // line
        // lenght
    }
}
