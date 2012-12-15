namespace ProtoMath._2D.Animators
{
    using ProtoMath._2D.Maths;

    public class ConstantRotator<T, M> : IAnimator
            where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        IScalar<T> _angle;
        T _step;

        public ConstantRotator(IScalar<T> angle, T step) //  should be something like IAngle
        {
            _angle = angle;
            _step = step;
        }

        public void Update(float time)
        {
            _angle.Value = scalarMath.Sum(_angle.Value, scalarMath.MultiplyF(_step, time));
        }


        public bool IsDone
        {
            get { return false; }
        }

        public void Rewind()
        { }
    }
}
