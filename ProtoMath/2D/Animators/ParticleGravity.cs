namespace ProtoMath._2D.Animators
{
    using System;
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Plains;

    public class ParticleGravity<T, M> : IAnimator
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        IPointParticle<T>[] _particles;
        IVector<T> _g;

        public ParticleGravity(IVector<T> g, params IPointParticle<T>[] particles)
        {
            _g = g;
            _particles = particles;
        }

        public void Update(float elapsed)
        {
            foreach (IPointParticle<T> particle in _particles)
            {
                particle.FeedForce(_g);
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
