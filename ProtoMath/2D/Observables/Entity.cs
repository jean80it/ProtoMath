namespace ProtoMath._2D.Observables
{
    using ProtoMath._2D.Maths;

    public abstract class Entity<T, M> : IObservableEntity<T>
        where M : IScalarMath<T>, new()
    {
        public event ValueChangedHandler ValueChanged;

        public Scalar<T, M> XReference { get; protected set; }
        public Scalar<T, M> YReference { get; protected set; }

        public T X { get { return XReference.Value; } set { XReference.Value = value; } }
        public T Y { get { return YReference.Value; } set { YReference.Value = value; } }

        public Entity()
        {
            XReference = new Scalar<T, M>();
            YReference = new Scalar<T, M>();

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

        public Entity(Scalar<T, M> x, Scalar<T, M> y)
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

        public Entity(T x, T y)
            : this(new Scalar<T, M>(x), new Scalar<T, M>(y))
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
