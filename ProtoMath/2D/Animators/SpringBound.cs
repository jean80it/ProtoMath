namespace ProtoMath._2D.Animators
{
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;
    using ProtoMath._2D.Dependants;
    using ProtoMath._2D.Plains;

    public class SpringBound<T, M> : IAnimator
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        IPoint<T> _a;
        IPointParticle<T> _b;

        DependantVector<T, M> _ab;

        public T K { get; set; }
        public T Distance { get; set; }

        //public ElasticLink(IObservableEntity2D<T> a, IObservableEntity2D<T> b, T distance)
        public SpringBound(IPoint<T> a, IPointParticle<T> b, T distance)
        {
            _a = a;
            _b = b;

            _ab = DependantVector<T, M>.GetPersistentFromExtremes(a, b.Position);

            K = scalarMath.Convert(3);
            Distance = distance;
        }

        public void Update(float elapsed)
        {
            // compute force
            Vector<T> v = new Vector<T>();
            T k = scalarMath.Difference(_ab.Length, Distance);
            if (!scalarMath.IsLikeZero(_ab.Length))
            {
                VectorMath<T, M>.Multiply(_ab.Versor, scalarMath.Multiply(k, K), v); // Do I have to use the differential approximation here ?!?

                //update b
                _b.FeedForceN(v);
            }
        }

        public bool IsDone
        {
            get { return false; }
        }

        public void Rewind()
        { }
    }
}
