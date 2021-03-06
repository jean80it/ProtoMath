﻿namespace ProtoMath._2D
{
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Dependants;
    using ProtoMath._2D.Simples;
    using System;
    using System.Collections.Generic;
    using ProtoMath._2D.Observables;
    using System.Text;

    public class PolyLine<T, M> : ICircleCollidee<T,M>
         where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        Point<T, M>[] _points;

        public Point<T, M>[] Points { get { return _points; } }

        List<Segment<T, M>> _segments;

        public Circle<T, M> BoundingCircle { get; protected set; }

        public PolyLine(params Point<T, M>[] points)
        {
            int len = points.Length;
            if (len < 2)
            {
                throw new Exception("PolyLine needs at least two points.");
            }

            _points = points;

            _segments = new List<Segment<T, M>>(len);

            for (int i = 1; i < len; ++i )
            {
                _segments.Add(new Segment<T, M>(_points[i - 1], _points[i]));
            }

            BoundingCircle = new DependantCircle<T, M>(_points);
        }

        public PolyLine(params T[] points)
        {
            int len = points.Length / 2;
            if (len < 2)
            {
                throw new Exception("PolyLine needs at least two points.");
            }

            _points = new Point<T,M>[len];

            for (int i = 0, p = 0; p < len; i += 2, ++p)
            {
                _points[p] = new Point<T, M>(points[i], points[i + 1]);
            }
            
            _segments = new List<Segment<T, M>>(len);

            for (int i = 1; i < len; ++i)
            {
                _segments.Add(new Segment<T, M>(_points[i - 1], _points[i]));
            }

            BoundingCircle = new DependantCircle<T, M>(_points);
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Point<T, M> p in Points)
            {
                sb.Append(p.ToString());
            }
            return sb.ToString();
        }

        #region ICircleCollidee<T,M> Members

        public bool IsCollidingWithCircle(ICircle<T> circle, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T, M> lastCollisionContext)
        {
            bool result = false;
            foreach (Segment<T, M> s in _segments)
            {
                result |= s.IsCollidingWithCircle(circle, offset, movement, ref lastCollisionContext);
            }

            return result;
        }

        public bool IsCollidingWithCircle(IPoint<T> circleCenter, T circleRadius, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T, M> lastCollisionContext)
        {
            bool result = false;
            foreach (Segment<T, M> s in _segments)
            {
                result |= s.IsCollidingWithCircle(circleCenter, circleRadius, offset, movement, ref lastCollisionContext);
            }

            return result;
        }

        public TransformMatrix<T, M> ComputeResponseMatrix()
        {
            throw new NotImplementedException(); // the collisionContext generated by "IsCollidingWithCircle" from PolyLine should never specify himself as a collidee, but a sub segment/line/point instead
        }

        #endregion // ICircleCollidee<T,M> Members
    }
}
