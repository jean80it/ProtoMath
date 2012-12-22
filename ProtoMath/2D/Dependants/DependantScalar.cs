namespace ProtoMath._2D.Dependants
{
    using System;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    public class DependantScalar<T, M> : Scalar<T, M>, IDependantScalar<T>
        where M : IScalarMath<T>, new()
    {
        RecomputeHandler<DependantScalar<T, M>> _recomputeHandler = null;

        bool _valid = false;

        public DependantScalar(RecomputeHandler<DependantScalar<T, M>> recomputeHandler)
        {
            if (recomputeHandler == null)
                throw new ArgumentNullException("recomputeHandler");  // Dependant entities ALWAYS need a method to be recomputed, 
                                                                            // and avoid null test each single time a call has to be 
                                                                            // done makes things faster

            _recomputeHandler = recomputeHandler;
        }

#if DEBUG
        private string _name = "[DependantScalar " + (++ProtoMath.Debug.DebugHelpers.Ticket) + "]";
#endif
        public void Invalidate()
        {
            _valid = false;
#if DEBUG
            System.Diagnostics.Debug.WriteLine("invalidating " + this.GetType().Name + " " +_name);
#endif
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

        public override T Value
        {
            get
            {
                Validate();
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        public override string ToString()
        {
            Validate();
#if DEBUG
            return base.ToString() + " (" + _name + ")";
#else
            return base.ToString();
#endif
        }
    }
}
