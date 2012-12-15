namespace ProtoMath._2D
{
    using ProtoMath._2D.Plains;
    using ProtoMath._2D.Observables;
    using ProtoMath._2D.Maths;
    using System.Collections.Generic;
    
    public interface ICollisionContext<T,M>
        where M : IScalarMath<T>, new()
    {
        void ComputeResponse(IPoint<T> position, IVector<T> speed, ResponseCache<T, M> responseCache); // UGH... just to make it compile now
        //T SquaredDistance { get; }
        T PercentPathCovered { get; set; }

        ICircleCollidee<T, M> Collidee { get; set; }
    }

    public class CollisionContext<T,M> : ICollisionContext<T,M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public IPoint<T> CollisionPoint = new Point<T>(); // the point in which collision occurred; it is used to measure squaredDistance, too
        public T PercentPathCovered { get; set; }
        public IVector<T> VersRadius = new Vector<T>(); // versor of tangent for non moving object, passing by collision point
        public IVector<T> Offset = new Vector<T>(); // x,y offset to handle "tiles"
        //public T SquaredDistance { get; set; } // squared distance from start point to collision point, used to choose nearest collision without the hassle of doing sqrt

        //===============================
        public ICircleCollidee<T,M> Collidee { get; set; }

        public void ComputeResponse(IPoint<T> position, IVector<T> speed, ResponseCache<T,M> responseCache) 
        {
            // apply transform given by cached matrix rotate->translate->scale&flip->translateBack->rotateBack
            // to speed. Next position can be computed by collisionPoint+versRadius+newSpeed*t*(1-percentPathCovered)
            responseCache.GetResponse(Collidee).Apply(speed);
            
            VectorMath<T, M>.Sum(this.CollisionPoint, this.VersRadius, position);

            VectorMath<T, M>.Sum(position, this.Offset, position);
        }
    }

    public class PointCollisionContext<T, M> : ICollisionContext<T, M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public IPoint<T> CollisionPoint = new Point<T>(); // the point in which collision occurred; it is used to measure squaredDistance, too
        
        #region ICollisionContext<T,M> Members

        public T PercentPathCovered { get; set; }

        public ICircleCollidee<T, M> Collidee { get; set; }

        public void ComputeResponse(IPoint<T> position, IVector<T> speed, ResponseCache<T, M> responseCache)
        {
            
        }

        #endregion
    }

    public interface ICircleCollidee<T, M>
        where M : IScalarMath<T>, new()
    {
        bool IsCollidingWithCircle(ICircle<T> circle, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T, M> lastCollisionContext);
        bool IsCollidingWithCircle(IPoint<T> circleCenter, T circleRadius, IVector<T> offset, IVector<T> movement, ref ICollisionContext<T, M> lastCollisionContext);
        TransformMatrix<T, M> ComputeResponseMatrix();
    }

    public class ResponseCache<T, M> 
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        Dictionary<ICircleCollidee<T,M>, TransformMatrix<T, M>> _collisionResponses = new Dictionary<ICircleCollidee<T,M>, TransformMatrix<T, M>>();

        public TransformMatrix<T, M> GetResponse(ICircleCollidee<T,M> collidee)
        {
            try
            {
                return _collisionResponses[collidee];
            }
            catch
            {
                TransformMatrix<T, M> m = collidee.ComputeResponseMatrix();

                _collisionResponses[collidee] = m;

                return m;
            }
        }
    }
}
