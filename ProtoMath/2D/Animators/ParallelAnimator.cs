
namespace ProtoMath._2D.Animators
{
    using System;
    using System.Collections.Generic;

    public class ParallelAnimations : IAnimator
    {
        public enum TerminationStrategies
        { 
            Never,
            Any,
            All,
            Single
        }

        private TerminationStrategies _terminationStrategy = TerminationStrategies.Any;

        bool _done = false;

        public List<IAnimator> Animators { get; private set; }

        IAnimator _terminatingAnimator = null;

        public ParallelAnimations(TerminationStrategies terminationStrategy)
        {
            Animators = new List<IAnimator>();
            _terminationStrategy = terminationStrategy;
        }

        public ParallelAnimations(IAnimator terminatingAnimator)
        {
            Animators = new List<IAnimator>();
            _terminationStrategy = TerminationStrategies.Single;
            _terminatingAnimator = terminatingAnimator;
        }

        public void Update(float time)
        {
            foreach(IAnimator animator in Animators)
            {
                animator.Update(time);
                switch (_terminationStrategy)
                {
                    case TerminationStrategies.Never:
                        break;
                    case TerminationStrategies.Any:
                        _done |= animator.IsDone;
                        break;
                    case TerminationStrategies.All:
                        // TODO: implement AND
                        break;
                    case TerminationStrategies.Single:
                        if (animator == _terminatingAnimator)
                        {
                            _done = animator.IsDone;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public bool IsDone
        {
            get { return _done; }
        }

        public void Rewind()
        {
            _done = false;
            foreach(IAnimator animator in Animators)
            {
                animator.Rewind();
            }
        }
    }
}
