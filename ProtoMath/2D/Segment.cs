namespace ProtoMath._2D
{
    using ProtoMath._2D.Dependants;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Simples;
    using ProtoMath._2D.Observables;
    using System.Collections.Generic;
    using ProtoMath._2D.Plains;
    using System;

    public class Segment<T, M> : ICircleCollidee<T,M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public IPoint<T> A { get { return AReference; } }
        public IPoint<T> B { get { return BReference; } }

        public ObservablePoint<T, M> AReference { get; protected set; }
        public ObservablePoint<T, M> BReference { get; protected set; }

        public DependantVector<T, M> Vector { get; protected set; }
        public DependantVersor<T, M> Versor { get; protected set;}

        public DependantLine<T, M> Line { get; protected set; }

        public DependantScalar<T, M> LenghtReference
        {
            get 
            {
                return Vector.LenghtReference;
            }
        }
        
        public T Length
        {
            get
            {
                return Vector.LenghtReference.Value;
            }
        }

        public Segment(ObservablePoint<T, M> a, ObservablePoint<T, M> b)
        {
            AReference = a;
            BReference = b;

            Vector = new DependantVector<T, M>(new RecomputeHandler<ObservableEntity<T,M>>(delegate(ObservableEntity<T, M> e){
                e.SetValue(
                    scalarMath.Difference(B.X, A.X), 
                    scalarMath.Difference(B.Y, A.Y)
                    );
            })); // vector = B-A

            Line = DependantLine<T, M>.PersistentFromVector(Vector, AReference);
            Versor = DependantVersor<T, M>.FromOrthogonalVector(Vector); // TODO: should take it from Line

            ValueChangedHandler valueChanged = new ValueChangedHandler(delegate(object sender) { 
                Vector.Invalidate(); });

            AReference.ValueChanged += valueChanged;
            BReference.ValueChanged += valueChanged;
        }

        public Segment()
            : this(new ObservablePoint<T, M>(), new ObservablePoint<T, M>())
        { }

        public Segment(T ax, T ay, T bx, T by)
            : this(new ObservablePoint<T, M>(ax, ay), new ObservablePoint<T, M>(bx, by))
        { }

        public override string ToString()
        {
            return "{" + A + "; " + B + "}";
        }

        public T DistanceFrom(IPoint<T> p)
        { 
            //              |    .   |
            //    case 2  . |--------|  . case 3
            //              |  .     |
            //                case 1

            //...
            throw new NotImplementedException();
        }

        
        #region ICircleCollidee<T,M> Members

        public bool IsCollidingWithCircle(ICircle<T> circle, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T,M> lastCollisionContext)
        {
            return IsCollidingWithCircle(circle.Center, circle.Radius, offset, movement, ref lastCollisionContext);
        }

        public bool IsCollidingWithCircle(IPoint<T> circleCenter, T circleRadius, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T,M> lastCollisionContext)
        {
            Point<T> offsetCenterStart = new Point<T>();

            // offset the center
            VectorMath<T, M>.Difference(circleCenter, offset, offsetCenterStart);

            // compute circle's center distance from segment's line. 
            T centerDistance = Line.SignedDistanceFrom(offsetCenterStart);
            
            // If negative, the circle is behind the segment: return false.
            // (not even a collision with extremes A, B is possible)
            //
            //       _______          ^
            //     _/       \_        |
            // ---/-----------\--------
            //   |      .      |
            //
            if (scalarMath.IsLessThan(centerDistance, scalarMath.Zero))
            {
                return false;
            }

            CollisionContext<T, M> newCollisionContext = new CollisionContext<T, M>();
            newCollisionContext.Offset = offset; // could be setted later  // use 'setValue(...)', instead??                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 

            newCollisionContext.Collidee = this;

            // now: if there's a collision "with the line part" we do not need to 
            // check for collision with extremities; conversely, if after this point
            // no collision is detected, we need to add A and B to a
            // "points to check" list and let subsequent code check them out

            // colliding point will be distant [radius] from center
            VectorMath<T, M>.Multiply(this.Versor, circleRadius, newCollisionContext.VersRadius);

            // segment versor always points towards potential colliders, 
            // so let's get a reversed one to find first point on circle
            // to eventually collide with line
            Vector<T> revVersRadius = new Vector<T>();
            VectorMath<T, M>.GetNegative(newCollisionContext.VersRadius, revVersRadius);

            // center + revVersRadius = 
            // first point on circle to eventually collide with line
            // as seen at the BEGINNING of motion that could lead to
            // a collision
            Point<T> towardLineStart = new Point<T>();
            VectorMath<T, M>.Sum(offsetCenterStart, revVersRadius, towardLineStart); // i do not overwrite offsetCenterStart because i could need it later (TODO: check i actually need it)

            // start + movement = 'end of collision' point
            // first point on circle to eventually collide with line
            // as seen at the END of motion that could end in a collision
            Point<T> end = new Point<T>(); // was in collisionContext
            VectorMath<T, M>.Sum(towardLineStart, movement, end);

            // notice p is the point _of_segment_ in wich collision occurs
            // to find the position of circle's center when collision occurs
            // we should sum p to segment's versor * circle radius
            T percentPathCovered;
            if (!this.Line.IntersectsWith(towardLineStart, end, newCollisionContext.CollisionPoint, out percentPathCovered))
            {
                // here i should signal to check against A and B (or just the nearest one)
                // as the following could be happening:
                //    ___
                //   / . \___\
                //   \___/   / A----------B
                //
                return false;
            }
            newCollisionContext.PercentPathCovered = percentPathCovered;

            // collision with line happened; now if point is inside [A, B]
            // we have a collision; otherwise, we should look for collisions with A, B (or just the nearest one)
            // as the following could be happening:
            //   ___
            //  /   \
            //  \_|_/
            //    |
            //    V
            //    x A-------B
            // 
            // note: IsBetweenC is a function crafted explicitly for this kind of collision check
            // it checks P being between A and B, and eventually being equal to A
            // (see notes in implementation)
            // so, if A = B = P the test fails, hence the "OR" used below 
            // (think about perfectly horizontal or vertical segments)
            if (scalarMath.IsBetweenC(newCollisionContext.CollisionPoint.X, A.X, B.X) || scalarMath.IsBetweenC(newCollisionContext.CollisionPoint.Y, A.Y, B.Y))
            {
                // squaredDistance has to be computed anyway...
                //newCollisionContext.SquaredDistance = Vector2DMath<T, M>.SquaredDistance(newCollisionContext.CollisionPoint, offsetCenterStart);

                if (lastCollisionContext != null)
                {
                    //if (!scalarMath.IsLessThan(newCollisionContext.SquaredDistance, lastCollisionContext.SquaredDistance))
                    if (!scalarMath.IsLessThan(newCollisionContext.PercentPathCovered, lastCollisionContext.PercentPathCovered))
                    {
                        // there is a - sure - collision "with the segment" (computed
                        // on another segment) that is nearer than this one
                        // checking A and B is not needed (as we are "on the segment" as well)
                        return false; // !! TRUE????
                    }
                }
                // else new collision is the nearest

                lastCollisionContext = newCollisionContext;

                return true;
            }
            else
            {
                // here i should signal to check against A and B (or just the nearest one)
                // (but maybe just if it's close enough to edges)
                return false;
            }
        }

        public TransformMatrix<T, M> ComputeResponseMatrix()
        {
            // TODO: if on edges, respond appropriately
            return this.Line.ComputeResponseMatrix();
        }

        #endregion
    }
}
