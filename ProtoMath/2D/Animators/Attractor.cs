namespace ProtoMath._2D.Animators
{
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Plains;

    public class Attractor<T, M> : IAnimator
            where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        IPoint<T> _attracted;
        IPoint<T> _attractor;

        bool _done = false;

        public Attractor(IPoint<T> attracted, IPoint<T> attractor) 
        {
            _attracted = attracted;
            _attractor = attractor;
        }

        public void Update(float time)
        {
            Vector<T> v = new Vector<T>();
            VectorMath<T, M>.Difference(_attractor, _attracted, v); 
            //...
            // done = isLikeZero(Lenght);
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