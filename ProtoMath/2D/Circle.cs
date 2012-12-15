namespace ProtoMath._2D
{
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    public class Circle<T, M> : ICircle<T>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public ObservablePoint<T, M> CenterReference { get; protected set; }
        public ObservableScalar<T, M> RadiusReference { get; protected set; }

        public IPoint<T> Center 
        {
            get
            {
                return CenterReference;
            }

            set 
            {
                CenterReference.SetValue(value);
            }
        }

        public T Radius
        {
            get
            {
                return RadiusReference.Value;
            }

            set
            {
                RadiusReference.Value = value;
            }
        }

        public Circle(ObservablePoint<T, M> center, ObservableScalar<T, M> radius)
        {
            CenterReference = center;
            RadiusReference = radius;
        }

        public Circle(IPoint<T> center, T radius)
            :this(center.X, center.Y, radius)
        { }

        public Circle()
            : this(new ObservablePoint<T, M>(), new ObservableScalar<T, M>())
        { }

        public Circle(T x, T y, T r)
            : this(new ObservablePoint<T, M>(x, y), new ObservableScalar<T, M>(r))
        { }

        public override string ToString()
        {
            return "{center: " + CenterReference + ", radius: " + Radius + " }";
        }
    }
}
