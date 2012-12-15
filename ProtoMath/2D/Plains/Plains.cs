namespace ProtoMath._2D.Plains
{
    public class Scalar<T> : IScalar<T>
    {
        public T Value { get; set; }

        public override string ToString()
        {
            return "[" + Value + "]";
        }
    }

    public class Point<T> : IPoint<T>
    {
        public T X { get; set; }

        public T Y { get; set; }

        public Point()
            :this(default(T),default(T))
        { }

        public Point(T x, T y)
        {
            //SetValue(x, y);
            X = x;
            Y = y;
        }

        public void SetValue(T x, T y)
        {
            X = x;
            Y = y;
        }

        public void SetValue(IEntity<T> value) 
        {
            X = value.X;
            Y = value.Y;
        }

        public override string ToString()
        {
            return "[" + X + ", " + Y + "]";
        }
    }

    public class Vector<T> : IVector<T>
    {
        public T X { get; set; }

        public T Y { get; set; }

        public Vector()
        { }

        public Vector(T x, T y)
        {
            SetValue(x, y);
        }

        public void SetValue(T x, T y)
        {
            X = x;
            Y = y;
        }

        public void SetValue(IEntity<T> value)
        {
            X = value.X;
            Y = value.Y;
        }

        public T Length { get; set; }

        public IVector<T> Versor { get; set; }

        public override string ToString()
        {
            return "[" + X + ", " + Y + "]";
        }
    }

    public class Circle<T> : ICircle<T>
    {

        public IPoint<T> Center { get; set; }

        public T Radius { get; set; }

        public override string ToString()
        {
            return "{" + Center + ", " + Radius + "}";
        }
    }
}
