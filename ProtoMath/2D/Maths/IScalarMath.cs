namespace ProtoMath._2D.Maths
{
    public interface IScalarMath<T>
    {
        T Zero { get; }
        T One { get; }

        T Pi { get; }
        T Pi2 { get; }
        T PiHalf { get; }
        T PiQuarter { get; }

        bool IsLikeZero(T value);

        T SumMany(params T[] p);
        T Sum(T a, T b);

        /// <summary>
        /// result = a - b;
        /// </summary>
        T Difference(T a, T b);
        T Multiply(T a, T b);
        T MultiplyAndAdd(T a, T k, T b);
        T MultiplyFAndAdd(T a, float k, T b);
        T MultiplyF(T a, float b);
        T MultiplyMany(params T[] p);
        
        /// <summary>
        /// result = a / b;
        /// </summary>
        T Divide(T a, T b);

        T Negate(T a);
        T Reciprocal(T a);

        T Increment(T a);
        T Decrement(T a);

        T SquareRoot(T a);
        T Absolute(T a);

        T Distance(T x1, T y1, T x2, T y2);
        T SquaredDistance(T x1, T y1, T x2, T y2);

        T Length(T x, T y);

        T Random(T min, T max);
        T Random(T max);

        bool IsLessThan(T a, T b);

        bool IsMoreThan(T a, T b);

        bool IsLessOrEqualTo(T a, T b);

        bool IsMoreOrEqualTo(T a, T b);

        bool IsBetweenC(T p, T a, T b);

        // uhm...
        //T Convert(float value);
        //T Convert(int value);
        T Convert(double value);

        T Cos(T angle);
        T Sin(T angle);
        //...

        T Sqr(T x);
    }
}
