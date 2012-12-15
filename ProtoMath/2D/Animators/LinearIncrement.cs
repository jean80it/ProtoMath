namespace ProtoMath._2D.Animators
{
    using ProtoMath._2D.Maths;

    public class LinearIncrement<T, M> : IAnimator
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        IScalar<T> _scalar;
        T _step;

        public LinearIncrement(IScalar<T> scalar, T step) 
        {
            _scalar = scalar;
            _step = step;
        }

        public void Update(float time)
        {
            _scalar.Value = scalarMath.Sum(_scalar.Value, scalarMath.MultiplyF(_step, time));
        }

        public bool IsDone
        {
            get { return false; }
        }

        public void Rewind()
        { }
    }
}