namespace ProtoMath._2D.Animators
{
    using System;
    using System.Collections.Generic;

    public class TimeLine : IAnimator
    {
        List<IAnimator> Animators { get; set; }

        int _currentAnimator = 0;

        bool _done = false;

        public bool Repeat { get; set; }

        public TimeLine()
        {
            Animators = new List<IAnimator>();
            Repeat = false;
        }

        public TimeLine(params IAnimator[] animators)
        {
            Animators = new List<IAnimator>(animators);
            Repeat = false;
        }

        public void Update(float time)
        {
            if (_done)
            {
                return;
            }

            Animators[_currentAnimator].Update(time);
            
            if (Animators[_currentAnimator].IsDone)
            {
                if (++_currentAnimator >= Animators.Count)
                {
                    if (Repeat)
                    {
                        Rewind();
                    }
                    else
                    {
                        _done = true;
                    }
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
            _currentAnimator = 0;
            foreach (IAnimator animator in Animators)
            {
                animator.Rewind();
            }
        }
    }
}
