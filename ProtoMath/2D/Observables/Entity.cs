namespace ProtoMath._2D.Observables
{
    using ProtoMath._2D.Maths;

    public abstract class ObservableEntity<T, M> : IObservableEntity<T>
        where M : IScalarMath<T>, new()
    {
        public event ValueChangedHandler ValueChanged;

        public ObservableScalar<T, M> XReference { get; protected set; }
        public ObservableScalar<T, M> YReference { get; protected set; }

        public T X { get { return XReference.Value; } set { XReference.Value = value; } }
        public T Y { get { return YReference.Value; } set { YReference.Value = value; } }

        public ObservableEntity()
        {
            XReference = new ObservableScalar<T, M>();
            YReference = new ObservableScalar<T, M>();

            var handler = new ValueChangedHandler(delegate(object sender)
            {
                this.OnValueChanged();
#if DEBUG
                System.Diagnostics.Debug.WriteLine("component changed in " + sender.GetType().Name);
#endif
            });

            XReference.ValueChanged += handler;
            YReference.ValueChanged += handler;
        }

        public ObservableEntity(ObservableScalar<T, M> x, ObservableScalar<T, M> y)
        {
            XReference = x;
            YReference = y;

            var handler = new ValueChangedHandler(delegate(object sender) { 
                this.OnValueChanged();
#if DEBUG
                System.Diagnostics.Debug.WriteLine("component changed in " + sender.GetType().Name);
#endif
            });

            XReference.ValueChanged += handler;
            YReference.ValueChanged += handler;
        }

        public ObservableEntity(T x, T y)
            : this(new ObservableScalar<T, M>(x), new ObservableScalar<T, M>(y))
        { }

        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this);
            }
        }

        public void SetValue(T x, T y) {
            //this.SuspendNotifications();
            XReference.Value = x;
            YReference.Value = y;
            //this.ResumeNotifications(true);
        }

        public void SetValue(IEntity<T> value) 
        {
            this.SetValue(value.X, value.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ X.GetHashCode();
        }

        public override string ToString()
        {
            return "[" + X.ToString() + ", " + Y.ToString() + "]";
        }
        
    }
}
