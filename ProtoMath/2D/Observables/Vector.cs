namespace ProtoMath._2D.Observables
{
    using Dependants;
    using ProtoMath._2D.Maths;

    public class ObservableVector<T, M> : ObservableEntity<T, M>, IObservableVector<T>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public DependantVersor<T, M> VersorReference { get; protected set; }

        public IVector<T> Versor { get { return VersorReference; } }

        public DependantScalar<T, M> LenghtReference { get; protected set; }

        public T Length { get { return LenghtReference.Value; } }

        public ObservableVector()
            : this(default(T), default(T))
        { }

        public ObservableVector(T x, T y) 
            :base(x, y)
        {
            VersorReference = new DependantVersor<T, M>(this);

            XReference.ValueChanged += new ValueChangedHandler(Reference_ValueChanged);

            RecomputeHandler<DependantScalar<T, M>> recomputeLenght = new RecomputeHandler<DependantScalar<T, M>>(delegate(DependantScalar<T, M> sender) { sender.Value = scalarMath.Length(XReference.Value, YReference.Value); });

            LenghtReference = new DependantScalar<T, M>(recomputeLenght);
        }

        void Reference_ValueChanged(object sender)
        {
            LenghtReference.Invalidate();
            //VersorReference.Invalidate(); // with constructor accepting an IObservableVector, versor does automatically attach invalidating event
        }
    }
}
