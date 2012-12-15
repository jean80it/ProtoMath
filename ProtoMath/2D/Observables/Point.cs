namespace ProtoMath._2D.Observables
{
    using System;
    using ProtoMath._2D.Maths;

    public class ObservablePoint<T, M> : ObservableEntity<T, M>, IObservablePoint<T>, ICircleCollidee<T,M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();
        //...

        public static ObservableVector<T, M> Subtract(IPoint<T> p1, IPoint<T> p2)
        {
            return new ObservableVector<T, M>(scalarMath.Difference(p1.X, p2.Y), scalarMath.Difference(p1.Y, p2.Y)); 
        }

        public static T Distance(IPoint<T> p1, IPoint<T> p2)
        {
            return (Subtract(p1, p2)).Length;
        }

        public ObservablePoint()
        { }

        public ObservablePoint(IPoint<T> p)
        {
            SetValue(p);
        }

        public ObservablePoint(T x, T y)
        {
            SetValue(x, y);
        }

        public static ObservablePoint<T, M> Random(T minX, T minY, T maxX, T maxY)
        {
            Random rnd = new Random();

            return new ObservablePoint<T, M>(scalarMath.Random(minX, maxX), scalarMath.Random(minY, maxY));
        }

        public ObservablePoint<T, M> Clone() // should clone copy event handlers, too??-->original java didn't do this
        {
            return new ObservablePoint<T, M>(this);
        }

        #region ICircleCollidee<T,M> Members

        public bool IsCollidingWithCircle(ICircle<T> circle, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T, M> lastCollisionContext)
        {
            return IsCollidingWithCircle(circle.Center, circle.Radius, offset, movement, ref lastCollisionContext);
        }

        public bool IsCollidingWithCircle(IPoint<T> circleCenter, T circleRadius, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T, M> lastCollisionContext)
        {
            // actually, movement should be speed... 
            Plains.Vector<T> pq = new ProtoMath._2D.Plains.Vector<T>();
            VectorMath<T,M>.Difference(circleCenter, this, pq);
            T a = scalarMath.Sqr(movement.Length);
            T b = scalarMath.Difference(scalarMath.Multiply(movement.X, pq.Y), scalarMath.Multiply(movement.Y, pq.X));
            T c = scalarMath.Difference(scalarMath.Sqr(pq.Length), scalarMath.Sqr(circleRadius));

            // d = b^2 - 4ac
            T d = scalarMath.Difference(scalarMath.Sqr(b), scalarMath.MultiplyF(scalarMath.Multiply(a, c), 4));

            // if (d<0) then no collision
            if (scalarMath.IsLessThan(d,scalarMath.Zero))
            {
                return false;
            }

            // else
            // t12 = (-b +- sqrt(d)) / 2a
            T negB = scalarMath.Negate(b);
            T sqrtD = scalarMath.SquareRoot(d);
            T a2 = scalarMath.MultiplyF(a, 2);
            T t1 = scalarMath.Divide(scalarMath.Sum(negB, sqrtD), a2);
            T t2 = scalarMath.Divide(scalarMath.Difference(negB, sqrtD), a2);

            // take smallest 0<t<1 
            // (if movement = speed * t and we're solving for t, 
            // valid collisions are those between t=0 and t=1)
            bool t1Valid = (scalarMath.IsMoreOrEqualTo(t1, scalarMath.Zero) && (scalarMath.IsLessThan(t1, scalarMath.Zero)));
            bool t2Valid = (scalarMath.IsMoreOrEqualTo(t2, scalarMath.Zero) && (scalarMath.IsLessThan(t2, scalarMath.Zero)));
            T t;
            if (t1Valid)
            {
                if (t2Valid)
                {
                    if (scalarMath.IsLessThan(t1, t2))
                    {
                        t = t1;
                    }
                    else
                    {
                        t = t2;
                    }
                }
                else
                {
                    t = t1;
                }
            }
            else
            {
                if (t2Valid)
                {
                     t = t2;
                }
                else
                {
                    return false; 
                }
            }

            PointCollisionContext<T, M> newCollisionContext = new PointCollisionContext<T, M>(); // -> pointCollisionContext?
            newCollisionContext.Collidee = this;
            newCollisionContext.PercentPathCovered = t;
            // take current position, sum movement*percentPathCovered
            VectorMath<T, M>.Multiply(movement, newCollisionContext.PercentPathCovered, movement); // we can use 'movement' as intermediate since we don't need it anymore
            newCollisionContext.CollisionPoint.SetValue(movement);
            return true; 
        }

        public TransformMatrix<T, M> ComputeResponseMatrix()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
