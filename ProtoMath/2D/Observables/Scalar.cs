namespace ProtoMath._2D.Observables
{
    using ProtoMath._2D.Maths;

    public class ObservableScalar<T, M> : IObservableScalar<T>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public event ValueChangedHandler ValueChanged;

        T _value;

        public ObservableScalar()
        {
            _value = default(T);
        }

        public ObservableScalar(T value)
        {
            _value = value;
        }

        public virtual T Value
        {
            get 
            {
                return _value;
            }
            set
            {
                _value = value;
                OnValueChanged();
            }
        }

        protected void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this);
            }
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (_value == null)
            {
                return (obj == null);
            }

            if (obj is T)
            {
                return _value.Equals(obj);
            }

            IScalar<T> typedObj = obj as IScalar<T>;

            if (obj != null)
            {
                return _value.Equals(typedObj.Value);
            }

            return false;
        }


        #region operators

        public static implicit operator ObservableScalar<T, M>(T a)
        {
            return new ObservableScalar<T, M>(a);
        }

        public static implicit operator T(ObservableScalar<T, M> a)
        {
            return a.Value;
        }

        public static ObservableScalar<T, M> operator +(ObservableScalar<T, M> a, ObservableScalar<T, M> b)
        {
            return scalarMath.Sum(a.Value, b.Value);
        }

        public static ObservableScalar<T, M> operator -(ObservableScalar<T, M> a, ObservableScalar<T, M> b)
        {
            return scalarMath.Difference(a.Value, b.Value);
        }

        public static ObservableScalar<T, M> operator *(ObservableScalar<T, M> a, ObservableScalar<T, M> b)
        {
            return scalarMath.Multiply(a.Value, b.Value);
        }

        public static ObservableScalar<T, M> operator /(ObservableScalar<T, M> a, ObservableScalar<T, M> b)
        {
            return scalarMath.Divide(a.Value, b.Value);
        }

        public static ObservableScalar<T, M> operator ++(ObservableScalar<T, M> a)
        {
            return scalarMath.Increment(a);
        }

        public static ObservableScalar<T, M> operator --(ObservableScalar<T, M> a)
        {
            return scalarMath.Decrement(a);
        }

        public static ObservableScalar<T, M> operator -(ObservableScalar<T, M> a)
        {
            return scalarMath.Negate(a);
        }

        // TODO: implement ==, !=, <, >, <=, >= 
 
        #endregion // operators
    }
}
