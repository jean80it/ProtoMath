namespace ProtoMath._2D.Dependants
{
    using System;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    public class DependantPoint<T, M> : Entity<T, M>, IDependantPoint<T>
        where M : IScalarMath<T>, new()
    {
        protected RecomputeHandler<Entity<T, M>> _recomputeHandler;

        protected bool _valid = false;

        protected DependantPoint() { } // just to allow inheriting classes to implement their own stuff from scratch

        public DependantPoint(RecomputeHandler<Entity<T, M>> recomputeHandler)
        {
            if (recomputeHandler == null)
                throw new ArgumentNullException("recomputeHandler"); // Dependant entities ALWAYS need a method to be recomputed, 
                                                                            // and avoid null test each single time a call has to be 
                                                                            // done makes things faster

            _recomputeHandler = recomputeHandler;

            // when a scalar field needs to be recomputed, it forces to validate the whole entity
            RecomputeHandler<DependantScalar<T, M>> handler = new RecomputeHandler<DependantScalar<T, M>>(delegate(DependantScalar<T, M> scalar) { this.Validate(); });
            XReference = new DependantScalar<T, M>(handler);
            YReference = new DependantScalar<T, M>(handler);

            // when a scalar changes, it triggers notification even for observers observing the entity (and not the scalar, specifically)
            ValueChangedHandler scalarChanged = new ValueChangedHandler(delegate(object sender) { Invalidate(); });
            XReference.ValueChanged+= scalarChanged;
            YReference.ValueChanged+= scalarChanged;

            
        }

#if DEBUG
        protected string _name = "[DependantEntity " + (++ProtoMath.Debug.DebugHelpers.Ticket) + "]";
#endif

        public virtual void Invalidate()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("invalidating " + this.GetType().Name + " " + _name);
#endif
            _valid = false;
            OnValueChanged();
        }

        public void Validate()
        {
            if (!_valid)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("validating " + this.GetType().Name + " " + _name);
#endif
                _recomputeHandler(this);
                _valid = true;
            }
#if DEBUG
            else
                System.Diagnostics.Debug.WriteLine("NOT validating " + this.GetType().Name + " " + _name);
#endif


        }

        public bool IsValid
        {
            get { return _valid; }
        }

        public override string ToString()
        {
            Validate();
#if DEBUG
            return "[" + X.ToString() + ", " + Y.ToString() + "] (" + _name + ")";
#else
            return "[" + X.ToString() + ", " + Y.ToString() + "]";
#endif
        }
    }
}
