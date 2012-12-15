namespace ProtoMath._2D.Animators
{
    using ProtoMath._2D.Maths;

    public class Command : IAnimator
    {
        public delegate void ExecuteCommandHandler();

        ExecuteCommandHandler _executeCommandHandler;

        bool _done = false;

        public Command(ExecuteCommandHandler executeCommandHandler)
        {
            _executeCommandHandler = executeCommandHandler;
        }

        public void Update(float time)
        {
            if (_done)
            {
                return;
            }

            _executeCommandHandler();

            _done = true;
        }
        
        public bool IsDone
        {
            get { return _done; }
        }

        public void Rewind()
        {
            _done = false;
        }
    }
}