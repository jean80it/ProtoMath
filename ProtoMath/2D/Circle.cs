namespace ProtoMath._2D
{
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    public class Circle<T, M> : ICircle<T>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public Point<T, M> CenterReference { get; protected set; }
        public Scalar<T, M> RadiusReference { get; protected set; }

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

        public Circle(Point<T, M> center, Scalar<T, M> radius)
        {
            CenterReference = center;
            RadiusReference = radius;
        }

        public Circle(IPoint<T> center, T radius)
            :this(center.X, center.Y, radius)
        { }

        public Circle()
            : this(new Point<T, M>(), new Scalar<T, M>())
        { }

        public Circle(T x, T y, T r)
            : this(new Point<T, M>(x, y), new Scalar<T, M>(r))
        { }

        public override string ToString()
        {
            return "{center: " + CenterReference + ", radius: " + Radius + " }";
        }
    }
}
