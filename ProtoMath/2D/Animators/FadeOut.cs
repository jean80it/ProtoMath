namespace ProtoMath._2D.Animators
{
    using ProtoMath._2D.Maths;

    public class FadeOut<T, M> : IAnimator
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        bool _done = false;

        IScalar<T> _scalar;
        T _rate;

        public FadeOut(IScalar<T> scalar, T rate)
        {
            _rate = rate;
            _scalar = scalar;
        }

        public void Update(float time)
        {
            if (_done)
            {
                return;
            }
            _scalar.Value = scalarMath.Multiply(_scalar.Value, _rate);
            if (scalarMath.IsLikeZero(_scalar.Value))
            {
                _done = true;
            }

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