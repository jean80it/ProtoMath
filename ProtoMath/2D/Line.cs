namespace ProtoMath._2D
{
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;
    using ProtoMath._2D.Plains;
    using ProtoMath._2D;

    public class Line<T, M> : ILine<T>, ICircleCollidee<T,M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        class LineOrthoVersor : IVector<T>
        {
            Line<T, M> _line;

            public LineOrthoVersor(Line<T, M> line)
            {
                _line = line;
            }

            public T Length
            {
                get { return scalarMath.One; }
            }

            public IVector<T> Versor
            {
                get { return this; }
            }

            public T X
            {
                get
                {
                    return _line.A;
                }
                set
                { }
            }

            public T Y
            {
                get
                {
                    return _line.B;
                }
                set
                { }
            }

            public void SetValue(T x, T y) { }

            public void SetValue(IEntity<T> value) { }

        }

        class LineLongitudinalVersor : IVector<T>
        {
            Line<T, M> _line;

            public LineLongitudinalVersor(Line<T, M> line)
            {
                _line = line;
            }

            public T Length
            {
                get { return scalarMath.One; }
            }

            public IVector<T> Versor
            {
                get { return this; }
            }

            public T X
            {
                get
                {
                    return _line.B;
                }
                set
                { }
            }

            public T Y
            {
                get
                {
                    return scalarMath.Negate(_line.A);
                }
                set
                { }
            }

            public void SetValue(T x, T y) { }

            public void SetValue(IEntity<T> value) { }
        }

        // ax + by + c = 0
        // v= [Vx, Vy] vettore (segmento giacente)
        // p= [Px, Py] punto di passaggio (qualunque)

        // a =  Vy
        // b = -Vx
        // c = -(Px a + Py b)

        public T A { get; set; }
        public T B { get; set; }
        public T C { get; set; }

        public IVector<T> OrthoVersor { get; protected set; }
        public IVector<T> LongitudinalVersor { get; protected set; }

        protected Line()
        {
            OrthoVersor = new LineOrthoVersor(this);
            LongitudinalVersor = new LineLongitudinalVersor(this);
        }

        protected Line(T a, T b, T c)
            :this()
        {
            A = a;
            B = b;
            C = c;
        }

        public Line<T, M> GetBaseLine()
        {
            return new Line<T, M>(this.A, this.B, scalarMath.Zero);
        }

        public static Line<T, M> FromVector(IVector<T> vector, IPoint<T> point)
        {
            Line<T, M> le = new Line<T, M>();
            IVector<T> versor = vector.Versor;
            le.A = scalarMath.Negate(versor.Y);
            le.B = versor.X; // -versor.X
            le.C =  scalarMath.Negate(
                        scalarMath.Sum(
                            scalarMath.Multiply(point.X, le.A), scalarMath.Multiply(point.Y, le.B)
                        )
                    );  //-(point.X * le.A + point.Y * le.B);

            return le;
        }

        public virtual T SignedDistanceFrom(IPoint<T> point)
        {
            return  scalarMath.Sum(
                        scalarMath.Sum(
                            scalarMath.Multiply(A, point.X), scalarMath.Multiply(B, point.Y)
                        ),
                    C); // Ax + By + C
        }

        public override string ToString()
        {
            return "["+A + " x + " + B + " y + " + C + " = 0]";
        }

        public bool IntersectsWith(Segment<T, M> s, IPoint<T> result)
        {
            return IntersectsWith(s.A, s.B, result);
        }

        public bool IntersectsWith(IPoint<T> a, IPoint<T> b, IPoint<T> result)
        {
            T da = this.SignedDistanceFrom(a);
            T db = this.SignedDistanceFrom(b);

            if (!(scalarMath.IsLessThan(da, scalarMath.Zero) ^ scalarMath.IsLessThan(db, scalarMath.Zero))) // IsLessOrEqualTo????
            {
                return false;
            }

            da = scalarMath.Absolute(da);
            db = scalarMath.Absolute(db);

            T d = scalarMath.Sum(da, db);

            da = scalarMath.Divide(da, d);
            db = scalarMath.Divide(db, d);

            result.SetValue( scalarMath.Sum(
                            scalarMath.Multiply(a.X, db),
                            scalarMath.Multiply(b.X, da)
                        ),
                        scalarMath.Sum(
                            scalarMath.Multiply(a.Y, db),
                            scalarMath.Multiply(b.Y, da)
                        )
                    );

            return true;

        }

        public bool IntersectsWith(IPoint<T> a, IPoint<T> b, IPoint<T> result, out T percentPathCovered)
        {
            T da = this.SignedDistanceFrom(a);
            T db = this.SignedDistanceFrom(b);

            if (!(scalarMath.IsMoreOrEqualTo(da, scalarMath.Zero) && scalarMath.IsLessOrEqualTo(db, scalarMath.Zero)))
            {
                percentPathCovered = scalarMath.One;
                return false;
            }

            da = scalarMath.Absolute(da);
            db = scalarMath.Absolute(db);

            T d = scalarMath.Sum(da, db);

            da = scalarMath.Divide(da, d);
            percentPathCovered = da;
            db = scalarMath.Divide(db, d);

            result.SetValue(scalarMath.Sum(
                            scalarMath.Multiply(a.X, db),
                            scalarMath.Multiply(b.X, da)
                        ),
                        scalarMath.Sum(
                            scalarMath.Multiply(a.Y, db),
                            scalarMath.Multiply(b.Y, da)
                        )
                    );

            return true;

        }

        public Line<T, M> TakeOrthoPassingBy(IPoint<T> point)
        {
            Line<T, M> le = new Line<T, M>();
            le.A = this.B;
            le.B = scalarMath.Negate(this.A);
            le.C = scalarMath.Negate(
                        scalarMath.Sum(
                            scalarMath.Multiply(point.X, le.A), scalarMath.Multiply(point.Y, le.B)
                        )
                    );  //-(point.X * le.A + point.Y * le.B);

            return le;
        }

        
        #region ICircleCollidee<T,M> Members

        public bool IsCollidingWithCircle(ICircle<T> circle, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T, M> lastCollisionContext)
        {
            return IsCollidingWithCircle(circle.Center, circle.Radius, offset, movement, ref lastCollisionContext);
        }

        public bool IsCollidingWithCircle(IPoint<T> circleCenter, T circleRadius, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T, M> lastCollisionContext)
        {
            Point<T> offsetCenterStart = new Point<T>();

            // offset the center
            VectorMath<T, M>.Difference(circleCenter, offset, offsetCenterStart);

            // compute circle's center distance from segment's line. 
            T centerDistance = this.SignedDistanceFrom(offsetCenterStart);

            // If negative, the circle is behind the line: return false.
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
            VectorMath<T, M>.Multiply(this.OrthoVersor, circleRadius, newCollisionContext.VersRadius);

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
            if (!this.IntersectsWith(towardLineStart, end, newCollisionContext.CollisionPoint, out percentPathCovered))
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

            //newCollisionContext.SquaredDistance = Vector2DMath<T, M>.SquaredDistance(newCollisionContext.CollisionPoint, offsetCenterStart);

            if (lastCollisionContext != null)
            {
                //if (!scalarMath.IsLessThan(newCollisionContext.SquaredDistance, lastCollisionContext.SquaredDistance))
                if (!scalarMath.IsLessThan(newCollisionContext.PercentPathCovered, lastCollisionContext.PercentPathCovered))
                {
                    // there is a - sure - collision that is nearer than this one
                    return false;
                }
            }
            // else new collision is the nearest

            lastCollisionContext = newCollisionContext;

            return true;
        }

        public TransformMatrix<T, M> ComputeResponseMatrix()
        {
            T orthoK = scalarMath.Convert(0.5); // restitution // TODO: get it from block model
            T longK = scalarMath.Convert(0.99); // friction // TODO: get it from block model

            Vector<T, M> v = new Vector<T, M>();
            VectorMath<T, M>.GetNegative(this.LongitudinalVersor, v);

            TransformMatrix<T, M> m = TransformMatrix<T, M>
                .RotationBack(v)
                .MirrorY()
                .Scale(longK, orthoK)
                .Rotate(v); // no need to translate, since speed is a vector (0 base)

            return m;
        }

        #endregion
    }

}
