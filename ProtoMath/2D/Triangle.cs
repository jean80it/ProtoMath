namespace ProtoMath._2D
{
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Dependants;
    using ProtoMath._2D.Observables;

    public class Triangle<T, M>
         where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        ObservablePoint<T, M> A, B, C;

        Segment<T, M> s1, s2, s3;

        public Circle<T, M> BoundingCircle { get; protected set; }

        public Triangle(ObservablePoint<T, M> a, ObservablePoint<T, M> b, ObservablePoint<T, M> c)
        {
            A = a;
            B = b;
            C = c;

            s1 = new Segment<T, M>(A, B);
            s2 = new Segment<T, M>(B, C);
            s3 = new Segment<T, M>(C, A);

            BoundingCircle = new DependantCircle<T, M>(A, B, C);
        }

        public bool Contains(IPoint<T> p)
        {
            return scalarMath.IsLessThan(s1.Line.SignedDistanceFrom(p), scalarMath.Zero) &&
                    scalarMath.IsLessThan(s2.Line.SignedDistanceFrom(p), scalarMath.Zero) &&
                    scalarMath.IsLessThan(s3.Line.SignedDistanceFrom(p), scalarMath.Zero);
        }


    }
}
