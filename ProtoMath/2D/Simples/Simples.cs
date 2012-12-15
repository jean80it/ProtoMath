namespace ProtoMath._2D.Simples
{
    public struct SimpleScalar<T> : IScalar<T>
    {
        public T Value { get; set; }

        public override string ToString()
        {
            return "[" + Value.ToString() + "]";
        }
    }

    public struct SimplePoint<T> : IPoint<T>
    {
        public T X { get; set; }

        public T Y { get; set; }

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
            return "[" + X.ToString() + ", " + Y.ToString() + "]";
        }
    }

    public struct SimpleVector<T> : IVector<T>
    {
        public T X { get; set; }

        public T Y { get; set; }

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
            return "[" + X.ToString() + ", " + Y.ToString() + "]";
        }
    }

    public struct SimpleCircle<T> : ICircle<T>
    {

        public IPoint<T> Center {get; set;}
        
        public T Radius {get; set;}
    }
}
