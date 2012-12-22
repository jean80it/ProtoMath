namespace ProtoMath._2D.Dependants
{
    using System;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    public class DependantVersor<T, M> : DependantVector<T, M>
       where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

#if DEBUG
        private string _name = "[DependantVersor2D " + (++ProtoMath.Debug.DebugHelpers.Ticket) + "]";
#endif

        public DependantVersor(RecomputeHandler<Entity<T, M>> recomputeHandler)
        {
            if (recomputeHandler == null)
                throw new ArgumentNullException("recomputeHandler"); // Dependant entities ALWAYS need a method to be recomputed, 
                                                                            // and avoid null test each single time a call has to be 
                                                                            // done makes things faster

            _recomputeHandler = recomputeHandler;

            RecomputeHandler<DependantScalar<T, M>> recomputeComponent = new RecomputeHandler<DependantScalar<T, M>>(delegate(DependantScalar<T, M> sender) { this.Validate(); });
            XReference = new DependantScalar<T, M>(recomputeComponent); // component are recomputed forcing entity to validate
            YReference = new DependantScalar<T, M>(recomputeComponent);

            this.LenghtReference = new DependantScalar<T, M>(delegate(DependantScalar<T, M> scalar) { scalar.Value = scalarMath.One; });
            this.VersorReference = this;
        }

        public DependantVersor(IObservableVector<T> vector)
            :this(delegate(Entity<T, M> e)
            { // or, "how to recompute versor when needed"...
                IVector<T> v = (IVector<T>)e;
                if (!scalarMath.IsLikeZero(v.Length))
                {
                    v.SetValue(
                        scalarMath.Divide(vector.X, vector.Length),
                        scalarMath.Divide(vector.Y, vector.Length)
                        );
                }
            })
        {
            vector.ValueChanged += new ValueChangedHandler(delegate(object sender) { this.Invalidate(); });
        }

        public static DependantVersor<T, M> FromOrthogonalVector(IObservableVector<T> vector)
        {
            DependantVersor<T, M> versor = new DependantVersor<T, M>(delegate(Entity<T, M> e)
            { // or, "how to recompute versor when needed"...
                IVector<T> v = (IVector<T>)e;
                if (!scalarMath.IsLikeZero(v.Length))
                {
                    VectorMath<T, M>.GetOrthoVersor(vector, v);
                }
            });

            vector.ValueChanged += new ValueChangedHandler(delegate(object sender) { 
                versor.Invalidate(); });

            return versor;
         }

        // could simply make Length virtual, and override it here to always return "1", but 
        // this, alone, is not a good reason to make a method virtual (with all performance issues following)

        public override void Invalidate()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("invalidating " + this.GetType().Name + " " + _name);
#endif
            this._valid = false;

            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ((DependantScalar<T, M>)XReference).Invalidate();
            ((DependantScalar<T, M>)YReference).Invalidate();

            OnValueChanged(); // in case someone is observing the versor
        }
    }
}
