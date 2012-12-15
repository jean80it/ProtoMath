namespace ProtoMath._2D.Dependants
{
    using System;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    public class DependantLine<T, M> : Line<T, M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        protected RecomputeHandler<Line<T, M>> _recomputeHandler;

        bool _valid = false;

        public DependantLine(RecomputeHandler<Line<T, M>> recomputeHandler)
        {
            if (recomputeHandler == null)
                throw new ArgumentNullException("recomputeHandler"); // Dependant entities ALWAYS need a method to be recomputed, 
                                                                            // and avoid null test each single time a call has to be 
                                                                            // done makes things faster

            _recomputeHandler = recomputeHandler;
        }
#if DEBUG
        private string _name = "[DependantLineEquation " + (++ProtoMath.Debug.DebugHelpers.Ticket) + "]";
#endif

        public void Invalidate()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("invalidating " + this.GetType().Name + " " +_name);
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

        public static DependantLine<T, M> PersistentFromOrthoVector(IObservableVector<T> vector, IObservablePoint<T> point)
        {
            DependantLine<T, M> result = new DependantLine<T, M>(new RecomputeHandler<Line<T, M>>(delegate(Line<T, M> le)
            {
                IVector<T> versor = vector.Versor;

                le.A = versor.X;
                le.B = versor.Y;
                le.C = scalarMath.Negate(
                            scalarMath.Sum(
                                scalarMath.Multiply(point.X, le.A),
                                scalarMath.Multiply(point.Y, le.B)
                            )
                        );  //-(point.X * le.A + point.Y * le.B);
            }));

            ValueChangedHandler ch = new ValueChangedHandler(delegate(object sender) { 
                result.Invalidate(); 
            });

            vector.ValueChanged += ch;
            point.ValueChanged += ch;

            return result;
        }

        public static DependantLine<T, M> PersistentFromVector(IObservableVector<T> vector, IObservablePoint<T> point)
        {
            DependantLine<T, M> le = new DependantLine<T, M>(new RecomputeHandler<Line<T, M>>(delegate(Line<T, M> line)
            {
                IVector<T> versor = vector.Versor;
                line.A = scalarMath.Negate(versor.Y); // -versor.Y
                line.B = versor.X;
                line.C = scalarMath.Negate(
                            scalarMath.Sum(
                                scalarMath.Multiply(point.X, line.A), scalarMath.Multiply(point.Y, line.B)
                            )
                        );  //-(point.X * le.A + point.Y * le.B);
            }));

            ValueChangedHandler vch = new ValueChangedHandler(delegate(object sender) { le.Invalidate(); });
            vector.ValueChanged += vch;
            point.ValueChanged += vch;

            return le;
        }

        public override T SignedDistanceFrom(IPoint<T> point)
        {
            Validate();
            return base.SignedDistanceFrom(point);
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
