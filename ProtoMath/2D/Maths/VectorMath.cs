using ProtoMath._2D.Plains;
namespace ProtoMath._2D.Maths
{
    public class VectorMath<T, M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M(); // cannot inherit from an IScalarMath, cause its implementation have to be a struct

        public static void GetOrthoVersor(IVector<T> vector, IVector<T> versor)
        {
            versor.SetValue(
                        scalarMath.Divide(scalarMath.Negate(vector.Y), vector.Length),
                        scalarMath.Divide(vector.X, vector.Length)
                        );
        }

        public static void GetNegative(IVector<T> v, IVector<T> result)
        {
            result.SetValue(scalarMath.Negate(v.X), scalarMath.Negate(v.Y));
        }

        //point+vector = point; vector + vector = vector
        public static void Sum(IEntity<T> e, IVector<T> v, IEntity<T> result)
        {
            result.SetValue(scalarMath.Sum(e.X, v.X), scalarMath.Sum(e.Y, v.Y));
        }

        /// <summary>
        /// result = e1 - e2
        /// </summary>
        public static void Difference(IEntity<T> e1, IEntity<T> e2, IEntity<T> result)
        {
            result.SetValue(scalarMath.Difference(e1.X, e2.X), scalarMath.Difference(e1.Y, e2.Y));
        }

        public static void GetLenght(IVector<T> v, out T length)
        {
            length = scalarMath.Length(v.X, v.Y);
        }
        
        public static void GetLenght(IVector<T> v, IScalar<T> length)
        {
            length.Value = scalarMath.Length(v.X, v.Y);
        }

        public static void Multiply(IVector<T> v, T k, IVector<T> result)
        {
            result.SetValue(scalarMath.Multiply(v.X, k), scalarMath.Multiply(v.Y, k));
        }

        public static T Distance(IPoint<T> a, IPoint<T> b)
        {
            return scalarMath.Distance(a.X, a.Y, b.X, b.Y);
        }

        public static T SquaredDistance(IPoint<T> a, IPoint<T> b)
        {
            return scalarMath.SquaredDistance(a.X, a.Y, b.X, b.Y);
        }

        /// <summary>
        /// accu = accu + a * multiplier;
        /// </summary>
        public static void MultiplyAndAccumulate(IEntity<T> a, IEntity<T> accu, T multiplier)
        { 
            accu.SetValue(scalarMath.MultiplyAndAdd(a.X, multiplier, accu.X), scalarMath.MultiplyAndAdd(a.Y, multiplier, accu.Y));
        }

        /// <summary>
        /// accu = accu + a * multiplier;
        /// </summary>
        public static void MultiplyFAndAccumulate(IEntity<T> a, IEntity<T> accu, float multiplier)
        {
            accu.SetValue(scalarMath.MultiplyFAndAdd(a.X, multiplier, accu.X), scalarMath.MultiplyFAndAdd(a.Y, multiplier, accu.Y));
        }

        /// <summary>
        /// a = a * (1 - damping * elapsed);
        /// </summary>
        public static void Damp(IVector<T> a, T damping, float elapsed)
        {
            Multiply(a, scalarMath.Difference(scalarMath.One, scalarMath.MultiplyF(damping, elapsed)), a); 
        }

        static IVector<T> _zero = new Vector<T>(); // TODO: implement readonly entity

        public static IVector<T> Zero { get { return _zero; } }

        public static void Random(IEntity<T> center, T minRadius, T maxRadius, IEntity<T> entity)
        {
            T r = scalarMath.Random(minRadius, maxRadius);
            T a = scalarMath.Random(scalarMath.Pi2);
            entity.SetValue(
                    scalarMath.Sum(
                            center.X, 
                            scalarMath.Multiply(scalarMath.Cos(a), r)
                        ), 
                    scalarMath.Sum(
                            center.Y, 
                            scalarMath.Multiply(scalarMath.Sin(a), r)
                        )
                );
        }
    }
}
