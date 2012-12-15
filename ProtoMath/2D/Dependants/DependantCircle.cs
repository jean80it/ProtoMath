namespace ProtoMath._2D.Dependants
{
    using System.Collections.Generic;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    class DependantCircle<T, M> : Circle<T, M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        private ObservablePoint<T, M>[] _points;

        protected RecomputeHandler<Circle<T, M>> _recomputeHandler;

        bool _valid = false;

        public DependantCircle(params ObservablePoint<T, M>[] points)
            : base(new ObservablePoint<T, M>(), new ObservableScalar<T, M>())
        {
            _points = points;
            _recomputeHandler = new RecomputeHandler<Circle<T, M>>(recomputeCircle);
            
            foreach (ObservablePoint<T, M> p in _points)
            {
                p.ValueChanged += new ValueChangedHandler(delegate(object sender) { this.Invalidate(); });
            }
        }

        private void recomputeCircle(Circle<T, M> e)
        { 
            // TODO: could make center dependant on points, radius dependant on [points, center]

            T x = scalarMath.Zero;
            T y = scalarMath.Zero;
            T c = scalarMath.Zero;

            foreach (ObservablePoint<T, M> p in _points)
            {
                x = scalarMath.Sum(x, p.X);
                y = scalarMath.Sum(y, p.Y);
                c = scalarMath.Sum(c, scalarMath.One);
            }

            x = scalarMath.Divide(x, c);
            y = scalarMath.Divide(y, c);

            CenterReference.SetValue(x, y);

            T squaredMaxDist = scalarMath.Zero;

            foreach (ObservablePoint<T, M> p in _points)
            {
                T d = scalarMath.SquaredDistance(p.X, p.Y, x, y); // this saves some squareRoots

                if (scalarMath.IsMoreThan(d, squaredMaxDist))
                {
                    squaredMaxDist = d;
                }
            }

            RadiusReference.Value = scalarMath.SquareRoot(squaredMaxDist);
        }

#if DEBUG
        private string _name = "[DependantCircle " + (++ProtoMath.Debug.DebugHelpers.Ticket) + "]";
#endif

        public void Invalidate()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("invalidating " + this.GetType().Name + " " + _name);
#endif
            _valid = false;

            //OnValueChanged();
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
