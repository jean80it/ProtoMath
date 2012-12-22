namespace ProtoMath._2D.Animators
{
    using System;
    using ProtoMath._2D.Plains;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    public class PointParticleWrapper<T, M> : IPointParticle<T>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public IVector<T> Acceleration { get; set; }
        public IVector<T> Speed { get; set; }

        IObservablePoint<T> _position;

        public IObservablePoint<T> Position { get { return _position; } }

        public T Damping { get; set; }

        public void FeedForce(IVector<T> force)
        {
            VectorMath<T, M>.Sum(Acceleration, force, Acceleration);
        }

        public void FeedForceN(IVector<T> force)
        {
            VectorMath<T, M>.Difference(Acceleration, force, Acceleration);
        }

        public delegate void UpdateAccelerationHandler(PointParticleWrapper<T, M> sender);

        public UpdateAccelerationHandler UpdateAcceleration;

        public virtual void OnUpdateAcceleration()
        {
            if (UpdateAcceleration!= null)
            {
                UpdateAcceleration(this);
            }
        }

        public PointParticleWrapper(IObservablePoint<T> position)
        {
            _position = position;

            Speed = new Vector<T>(); // TODO: ensure we don't need observables or something similar
            Acceleration = new Vector<T>();
            Damping = scalarMath.Convert(0.1);
        }

        public void Update(float elapsed)
        {
            OnUpdateAcceleration();

            
            // Speed += Acceleration * elapsed time
            VectorMath<T, M>.MultiplyFAndAccumulate(Acceleration, Speed, elapsed);

            // Position += Speed * elapsed time
            VectorMath<T, M>.MultiplyFAndAccumulate(Speed, _position, elapsed);

            // speed *= (1 - damping * elapsed) // approximate differential
            VectorMath<T, M>.Damp(Speed, Damping, elapsed);

            // reset acceleration
            Acceleration.SetValue(VectorMath<T, M>.Zero);
        }

        public IVector<T> ElasticForceTo(IEntity<T> p, T mult)
        {
            Vector<T> v = new Vector<T>();
            VectorMath<T, M>.Difference(p, _position, v);
            VectorMath<T, M>.Multiply(v, mult, v);
            return v;
        }

        public IVector<T> GravitationalForceTo(IEntity<T> p, T mult)
        {
            IVector<T> v = new Vector<T,M>();
            VectorMath<T, M>.Difference(p, _position, v);
            VectorMath<T, M>.Multiply(v.Versor, scalarMath.Multiply(scalarMath.Sqr(v.Length), mult), v);
            return v;
        }

        public bool IsDone
        {
            get { return false; }
        }

        public void Rewind()
        { }
    }
}
