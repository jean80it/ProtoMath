namespace ProtoMath._2D.Animators
{
    using System;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Plains;

    public class ParticleNoise<T, M> : IAnimator
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        IPointParticle<T>[] _particles;
        T _minRadius;
        T _maxRadius;

        public ParticleNoise(T minRadius, T maxRadius, params IPointParticle<T>[] particles)
        {
            _minRadius = minRadius;
            _maxRadius = maxRadius;
            _particles = particles;
        }

        public void Update(float elapsed)
        {
            foreach (IPointParticle<T> particle in _particles)
            {
                Vector<T> v = new Vector<T>();
                VectorMath<T, M>.Random(VectorMath<T, M>.Zero, _minRadius, _maxRadius, v);
                particle.FeedForce(v);
            }
        }

        public bool IsDone
        {
            get { throw new NotImplementedException(); }
        }

        public void Rewind()
        {
            throw new NotImplementedException();
        }
    }
}
