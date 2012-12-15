namespace ProtoMath._2D.Maths
{
    using System;

    public struct FloatMath : IScalarMath<float>
    {
        static Random _rnd;

        private Random Rnd { 
            get 
            { 
                if (_rnd == null) 
                { 
                    _rnd = new Random(); 
                } 
                return _rnd; 
            } 
        }

        #region IScalarMath<float> Members

        public float Zero { get { return 0.0f; } }
        public float One { get { return 1.0f; } }

        const float _pi2 = (float)(Math.PI * 2);
        const float _piHalf = (float)(Math.PI / 2);
        const float _piQuarter = (float)(Math.PI / 4);

        public float Pi { get { return (float)Math.PI; } }
        public float Pi2 { get { return _pi2; } }
        public float PiHalf { get { return _piHalf; } }
        public float PiQuarter { get { return _piQuarter; } }

        public const float ZeroRange = 0.0001f;

        public bool IsLikeZero(float value)
        {
            return Math.Abs(value) < ZeroRange;
        }
        
        public float Sum(float a, float b)
        {
            return a + b;
        }

        public float SumMany(params float[] p)
        {
            float result = 0;
            foreach (float f in p)
            {
                result += f;
            }
            return result;
        }

        public float Difference(float a, float b)
        {
            return a - b;
        }

        public float Multiply(float a, float b)
        {
            return a * b;
        }

        public float MultiplyAndAdd(float a, float k, float b)
        {
            return a * k + b;
        }

        public float MultiplyF(float a, float b)
        {
            return a * b;
        }

        public float MultiplyFAndAdd(float a, float k, float b)
        {
            return a * k + b;
        }

        public float MultiplyMany(params float[] p)
        {
            float result = 1;
            foreach (float f in p)
            {
                result *= f;
            }
            return result;
        }

        public float Divide(float a, float b)
        {
            return a / b;
        }

        public float Negate(float a)
        {
            return -a;
        }

        public float Reciprocal(float a)
        {
            return 1 / a;
        }

        public float Increment(float a)
        {
            return a++;
        }

        public float Decrement(float a)
        {
            return a--;
        }

        public float SquareRoot(float a)
        {
            return (float)Math.Sqrt(a);
        }

        public float Absolute(float a)
        {
            return Math.Abs(a);
        }

        public float Distance(float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public float SquaredDistance(float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;

            return (float)dx * dx + dy * dy;
        }

        public float Length(float x, float y)
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        public float Random(float min, float max)
        {
            return (float)(Rnd.NextDouble() * (max - min) + min);
        }

        public float Random(float max)
        {
            return (float)(Rnd.NextDouble() * max);
        }

        public bool IsLessThan(float a, float b)
        {
            return a < b;
        }

        public bool IsMoreThan(float a, float b)
        {
            return a > b;
        }

        public bool IsLessOrEqualTo(float a, float b)
        {
            return a <= b;
        }

        public bool IsMoreOrEqualTo(float a, float b)
        {
            return a >= b;
        }

        public bool IsBetweenC(float p, float a, float b)
        {
            // to handle correctly situations like
            // A - - - - - - A'
            // |\
            //   \
            // |  \
            //     B = P - - B'/P'
            // |  /|
            //   C - - - - - C'
            // | | |
            // A"C"B"
            //
            // in which P = B
            // we allow 'p' to be eventually equal to 'a'
            // so if in a polyline segments are tested like [A, B], [B, C], ...
            // P = B would fall in second segment only
            // in case a = b = p the test fails

            return ((p >= a) && (p < b)) || ((p <= a) && (p > b));
            

            // other ways:
            
            // return (p > Math.Min(a, b)) && (p < Math.Max(a, b));            
            
            //if (a > b)
            //{ 
            //    float c = a; 
            //    a = b; 
            //    b = c; 
            //}
            //return (p >= a) && (p <= b);
        }

        public float Convert(double value)
        {
            return (float)value;
        }

        public float Cos(float angle)
        {
            return (float)Math.Cos(angle);
        }

        public float Sin(float angle)
        {
            return (float)Math.Sin(angle);
        }

        #endregion



        public float Sqr(float x)
        {
            return x * x;
        }
    }

}
