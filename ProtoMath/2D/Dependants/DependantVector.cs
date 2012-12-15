namespace ProtoMath._2D.Dependants
{
    using System;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    public class DependantVector<T, M> : DependantPoint<T, M>, IDependantVector<T>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public DependantVersor<T, M> VersorReference { get; protected set; }

        public IVector<T> Versor { get { return VersorReference; } }

        public DependantScalar<T, M> LenghtReference { get; protected set; }
        
        public T Length
        {
            get { return LenghtReference.Value; }
        }

        protected void setRecomputeHandlers()
        {
            RecomputeHandler<DependantScalar<T, M>> recomputeLenght = new RecomputeHandler<DependantScalar<T, M>>(delegate(DependantScalar<T, M> sender) { sender.Value = scalarMath.Length(XReference.Value, YReference.Value); });

            LenghtReference = new DependantScalar<T, M>(recomputeLenght);
            this.ValueChanged += new ValueChangedHandler(delegate(object sender) { LenghtReference.Invalidate(); });
            
            VersorReference = new DependantVersor<T, M>(this);

        }

        protected DependantVector() { }

        public DependantVector(RecomputeHandler<ObservableEntity<T, M>> recomputeHandler)
        {
            if (recomputeHandler == null)
                throw new ArgumentNullException("recomputeHandler"); // Dependant entities ALWAYS need a method to be recomputed, 
                                                                           // and avoid null test each single time a call has to be 
                                                                           // done makes things faster
            _recomputeHandler = recomputeHandler;

            RecomputeHandler<DependantScalar<T, M>> recomputeComponent = new RecomputeHandler<DependantScalar<T, M>>(delegate(DependantScalar<T, M> sender) { this.Validate(); });
            XReference = new DependantScalar<T, M>(recomputeComponent); // component are recomputed forcing entity to validate
            YReference = new DependantScalar<T, M>(recomputeComponent);

            setRecomputeHandlers();
        }

        public DependantVector(RecomputeHandler<DependantScalar<T, M>> recomputeXHandler, RecomputeHandler<DependantScalar<T, M>> recomputeYHandler) 
        {
            if ((recomputeXHandler == null) || (recomputeYHandler == null))
                throw new ArgumentNullException("recomputeXHandler or recomputeYHandler"); // Dependant entities ALWAYS need a method to be recomputed, 
                                                                           // and avoid null test each single time a call has to be 
                                                                           // done makes things faster


            XReference = new DependantScalar<T, M>(recomputeXHandler);
            YReference = new DependantScalar<T, M>(recomputeYHandler);

            _recomputeHandler = new RecomputeHandler<ObservableEntity<T, M>>(delegate(ObservableEntity<T, M> sender) {
                ((DependantScalar<T, M>)XReference).Validate(); 
                ((DependantScalar<T, M>)YReference).Validate(); 
            });

            setRecomputeHandlers();
        }

        /// <summary>
        /// gets a vector that is always b - a
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static DependantVector<T, M> GetPersistentFromExtremes(IObservableEntity<T> a, IObservableEntity<T> b)
        {
            DependantVector<T, M> dv = new DependantVector<T, M>(new RecomputeHandler<ObservableEntity<T, M>>(delegate(ObservableEntity<T, M> e)
            {
                VectorMath<T, M>.Difference(b, a, e);
            })); // could use two different DependantScalar for the axes

            ValueChangedHandler vch = delegate(object sender) { dv.Invalidate(); };
            
            a.ValueChanged += vch;
            b.ValueChanged += vch;

            return dv;
        }

        /// <summary>
        /// gets a vector that is always b - a 
        /// being a a fixed point
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static DependantVector<T, M> GetPersistentFromExtremes(IEntity<T> a, IObservableEntity<T> b)
        {
            DependantVector<T, M> dv = new DependantVector<T, M>(new RecomputeHandler<ObservableEntity<T, M>>(delegate(ObservableEntity<T, M> e)
            {
                VectorMath<T, M>.Difference(b, a, e);
            })); // could use two different DependantScalar for the axes

            ValueChangedHandler vch = delegate(object sender) { dv.Invalidate(); };

            b.ValueChanged += vch;

            return dv;
        }

        public static DependantVector<T, M> GetPersistentOrthogonalTo(IObservableVector<T> v) // could make it accept IObservableVector
        { 
            DependantVector<T, M> dv = new DependantVector<T, M>(new RecomputeHandler<ObservableEntity<T,M>>(delegate(ObservableEntity<T, M> e){
                e.SetValue(v.Y, v.X);    
                })); // could use two different DependantScalar for the axes
            return dv;
        }

        public override void Invalidate()
        {
            base.Invalidate();
            
            ((DependantScalar<T, M>)XReference).Invalidate();
            ((DependantScalar<T, M>)YReference).Invalidate();
        }

        public override string ToString()
        {
            Validate();
            return base.ToString();
        }
    }
}
