namespace ProtoMath._2D.Animators
{
    using ProtoMath._2D.Maths;

    public class Pause : IAnimator
    {
        bool _done = false;

        float _elapsedTime = -1;
        float _amount = -1;

        public bool AllowRewind { get; set; }

        public Pause(float amount)
        {
            _amount = amount;
        }

        public void Update(float time)
        {
            if (_done)
            {
                return;
            }

            if (_elapsedTime < 0)
            {
                _elapsedTime = time;
            }
            else
            {
                _elapsedTime += time; 
                _done = (_elapsedTime >= _amount);
            }
        }

        public bool IsDone
        {
            get { return _done; }
        }

        public void Rewind()
        {
            if (!AllowRewind)
            {
                return;
            }

            _done = false;
            _elapsedTime = -1;
        }
    }
}