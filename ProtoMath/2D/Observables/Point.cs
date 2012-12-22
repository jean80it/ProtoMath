namespace ProtoMath._2D.Observables
{
    using System;
    using ProtoMath._2D.Maths;

    public class Point<T, M> : Entity<T, M>, IObservablePoint<T>, ICircleCollidee<T,M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();
        //...

        public static Vector<T, M> Subtract(IPoint<T> p1, IPoint<T> p2)
        {
            return new Vector<T, M>(scalarMath.Difference(p1.X, p2.Y), scalarMath.Difference(p1.Y, p2.Y)); 
        }

        public static T Distance(IPoint<T> p1, IPoint<T> p2)
        {
            return (Subtract(p1, p2)).Length;
        }

        public Point()
        { }

        public Point(IPoint<T> p)
        {
            SetValue(p);
        }

        public Point(T x, T y)
        {
            SetValue(x, y);
        }

        public static Point<T, M> Random(T minX, T minY, T maxX, T maxY)
        {
            Random rnd = new Random();

            return new Point<T, M>(scalarMath.Random(minX, maxX), scalarMath.Random(minY, maxY));
        }

        public Point<T, M> Clone() // should clone copy event handlers, too??-->original java didn't do this
        {
            return new Point<T, M>(this);
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
            VectorMath<T, M>.Difference(circleCenter, this, pq);
            //VectorMath<T, M>.Difference(this, circleCenter, pq);
            T a = scalarMath.Sqr(movement.Length);
            T b = scalarMath.MultiplyF(scalarMath.Sum(scalarMath.Multiply(pq.X, movement.X), scalarMath.Multiply(pq.Y, movement.Y)), 2);
            T c = scalarMath.Difference(scalarMath.Sqr(VectorMath<T,M>.GetLenght(pq)), scalarMath.Sqr(circleRadius)); // !!!!!!!!!

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

            if (scalarMath.IsLessThan(t1, scalarMath.Zero) || scalarMath.IsLessThan(t2, scalarMath.Zero))
            {
                // if both times are positive, then we're going to collide with nearest one
                // if both times are negative, we're leaving the collidee behind and must not worry 
                // if a time is negative and the other is positive, then the point is inside the circle, and we should avoid considering collisions in this case
                // ...hence, we're going to detect a collision only when both times are positive
                return false;
            }

            T t;
            if (scalarMath.IsLessThan(t1, t2))
            {
                t = t1;
            }
            else
            {
                t = t2;
            }

            if (scalarMath.IsMoreThan(t, scalarMath.One)) // nearest time is > 1, so we should not worry about a collision
            {
                return false; 
            }

            if ((lastCollisionContext == null) || (scalarMath.IsMoreThan( lastCollisionContext.PercentPathCovered, t)))
            {
                PointCollisionContext<T, M> newCollisionContext = new PointCollisionContext<T, M>(); // -> pointCollisionContext?
                newCollisionContext.Collidee = this;
                newCollisionContext.PercentPathCovered = t;
                // take current position, sum movement*percentPathCovered
                // we can use 'movement' as intermediate since we don't need it anymore; 
                // newCollisionContext.CollisionPoint should be [0,0] at this time
                newCollisionContext.CollisionCircleCenter.SetValue(circleCenter);
                VectorMath<T, M>.MultiplyAndAccumulate(movement, newCollisionContext.CollisionCircleCenter, newCollisionContext.PercentPathCovered);
                lastCollisionContext = newCollisionContext;
            }

            return true;
        }

        public TransformMatrix<T, M> ComputeResponseMatrix()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
