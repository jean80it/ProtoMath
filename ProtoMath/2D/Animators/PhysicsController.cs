using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoMath._2D.Animators
{
    public class PhysicsController<T> :IAnimator
    {
        IPointParticle<T>[] _particles;

        public PhysicsController(params IPointParticle<T>[] particles)
        {
            _particles = particles;
        }

        public void Update(float elapsed)
        {
            foreach (IPointParticle<T> particle in _particles)
            {
                particle.Update(elapsed);
            }
        }


        public bool IsDone
        {
            get { return false; }
        }

        public void Rewind()
        { }
    }
}
