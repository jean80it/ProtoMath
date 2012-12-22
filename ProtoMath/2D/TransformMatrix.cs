namespace ProtoMath._2D
{
    using ProtoMath._2D.Maths;
    using ProtoMath._2D.Observables;

    public class TransformMatrix<T, M>
        where M : IScalarMath<T>, new()
    {
        private static readonly M scalarMath = new M();

        public T A, B, C,
                D, E, F,
                G, H, I;

        // matrix constructor
        //
        // [A, B, C]
        // [D, E, F]
        // [G, H, I]
        //
        public TransformMatrix(T a11, T a21, T a31, T a12, T a22, T a32, T a13, T a23, T a33)
        {
            A = a11; B = a21; C = a31;
            D = a12; E = a22; F = a32;
            G = a13; H = a23; I = a33;
        }


        // identity matrix
        //
        // [1, 0, 0]
        // [0, 1, 0]
        // [0, 0, 1]
        //
        static public readonly TransformMatrix<T, M> Identity = new TransformMatrix<T, M>(
            scalarMath.One, scalarMath.Zero, scalarMath.Zero,
            scalarMath.Zero, scalarMath.One, scalarMath.Zero,
            scalarMath.Zero, scalarMath.Zero, scalarMath.One);


        // translation matrix
        //
        // [1, 0, x]
        // [0, 1, y]
        // [0, 0, 1]
        //
        public static TransformMatrix<T, M> Translation(IVector<T> translation)
        {
            return new TransformMatrix<T, M>(
                scalarMath.One, scalarMath.Zero, translation.X,
                scalarMath.Zero, scalarMath.One, translation.Y,
                scalarMath.Zero, scalarMath.Zero, scalarMath.One);
        }

        // translation matrix
        //
        // [1, 0, -x]
        // [0, 1, -y]
        // [0, 0,  1]
        //
        public static TransformMatrix<T, M> TranslationBack(IVector<T> translation)
        {
            return new TransformMatrix<T, M>(
                scalarMath.One, scalarMath.Zero, scalarMath.Negate(translation.X),
                scalarMath.Zero, scalarMath.One, scalarMath.Negate(translation.Y),
                scalarMath.Zero, scalarMath.Zero, scalarMath.One);
        }

        // rotation matrix
        //
        // [c, -s, 0]
        // [s,  c, 0]
        // [0,  0, 1]
        //
        public static TransformMatrix<T, M> Rotation(T angle)
        {
            T c = scalarMath.Cos(angle);
            T s = scalarMath.Sin(angle);

            return new TransformMatrix<T, M>(
                c, scalarMath.Negate(s), scalarMath.Zero,
                s, c, scalarMath.Zero,
                scalarMath.Zero, scalarMath.Zero, scalarMath.One);
        }

        // rotation matrix
        //
        // [x, -y, 0]
        // [y,  x, 0]
        // [0,  0, 1]
        //
        public static TransformMatrix<T, M> Rotation(IVector<T> rightVersor)
        {
            T c = rightVersor.X;
            T s = rightVersor.Y;

            return new TransformMatrix<T, M>(
                c, scalarMath.Negate(s), scalarMath.Zero,
                s, c, scalarMath.Zero,
                scalarMath.Zero, scalarMath.Zero, scalarMath.One);
        }

        // rotation matrix
        //
        // [ x, y, 0]
        // [-y, x, 0]
        // [ 0, 0, 1]
        //
        public static TransformMatrix<T, M> RotationBack(IVector<T> rightVersor)
        {
            T c = rightVersor.X;
            T s = scalarMath.Negate(rightVersor.Y);

            return new TransformMatrix<T, M>(
                c, scalarMath.Negate(s), scalarMath.Zero,
                s, c, scalarMath.Zero,
                scalarMath.Zero, scalarMath.Zero, scalarMath.One);
        }

        // scaling matrix (different x,y coefficients)
        //
        // [x, 0, 0]
        // [0, y, 0]
        // [0, 0, 1]
        //
        public TransformMatrix<T, M> Scaling(IVector<T> scale)
        {
            return new TransformMatrix<T, M>(
                scale.X, scalarMath.Zero, scalarMath.Zero,
                scalarMath.Zero, scale.Y, scalarMath.Zero,
                scalarMath.Zero, scalarMath.Zero, scalarMath.One);
        }

        // scaling matrix (different x,y coefficients)
        //
        // [x, 0, 0]
        // [0, y, 0]
        // [0, 0, 1]
        //
        public TransformMatrix<T, M> Scaling(T scaleX, T scaleY)
        {
            return new TransformMatrix<T, M>(
                scaleX, scalarMath.Zero, scalarMath.Zero,
                scalarMath.Zero, scaleY, scalarMath.Zero,
                scalarMath.Zero, scalarMath.Zero, scalarMath.One);
        }

        // scaling matrix
        //
        // [k, 0, 0]
        // [0, k, 0]
        // [0, 0, 1]
        //
        public TransformMatrix<T, M> Scaling(T scale)
        {
            return new TransformMatrix<T, M>(
                scale, scalarMath.Zero, scalarMath.Zero,
                scalarMath.Zero, scale, scalarMath.Zero,
                scalarMath.Zero, scalarMath.Zero, scalarMath.One);
        }

        // multiply translation matrix by this
        //
        // [1, 0, x] [A, B, C]   [ A+X*G, B+X*H, C+X*I]
        // [0, 1, y] [D, E, F] = [ D+Y*G, E+Y*H, F+Y*I]
        // [0, 0, 1] [G, H, I]   [ G    , H    , I    ]
        //
        public TransformMatrix<T, M> Translate(IEntity<T> translation)
        {
            A = scalarMath.Sum(A, scalarMath.Multiply(translation.X, G));
            B = scalarMath.Sum(B, scalarMath.Multiply(translation.X, H));
            C = scalarMath.Sum(C, scalarMath.Multiply(translation.X, I));

            D = scalarMath.Sum(D, scalarMath.Multiply(translation.Y, G));
            E = scalarMath.Sum(E, scalarMath.Multiply(translation.Y, H));
            F = scalarMath.Sum(F, scalarMath.Multiply(translation.Y, I));

            // G, H, I stays the same

            return this;
        }

        // multiply translation matrix by this
        //
        // [1, 0, -x] [A, B, C]   [ A-X*G, B-X*H, C-X*I]
        // [0, 1, -y] [D, E, F] = [ D-Y*G, E-Y*H, F-Y*I]
        // [0, 0,  1] [G, H, I]   [ G    , H    , I    ]
        //
        public TransformMatrix<T, M> TranslateBack(IEntity<T> translation)
        {
            A = scalarMath.Difference(A, scalarMath.Multiply(translation.X, G));
            B = scalarMath.Difference(B, scalarMath.Multiply(translation.X, H));
            C = scalarMath.Difference(C, scalarMath.Multiply(translation.X, I));

            D = scalarMath.Difference(D, scalarMath.Multiply(translation.Y, G));
            E = scalarMath.Difference(E, scalarMath.Multiply(translation.Y, H));
            F = scalarMath.Difference(F, scalarMath.Multiply(translation.Y, I));

            // G, H, I stays the same

            return this;
        }

        // multiply rotation matrix by this
        //
        // [x, -s, 0] [A, B, C]   [A*x-D*y, B*x-E*y, C*x-F*y]
        // [s,  c, 0] [D, E, F] = [A*y+D*x, B*y+E*x, C*y+F*x]
        // [0,  0, 1] [G, H, I]   [G      , H      , I      ]
        //
        public TransformMatrix<T, M> Rotate(IVector<T> rightVersor)
        {
            T c = rightVersor.X;
            T s = rightVersor.Y;

            T t;
            
            t = scalarMath.Difference(scalarMath.Multiply(A, c), scalarMath.Multiply(D, s));
            D = scalarMath.Sum(scalarMath.Multiply(A, s), scalarMath.Multiply(D, c));
            A = t;

            t = scalarMath.Difference(scalarMath.Multiply(B, c), scalarMath.Multiply(E, s));
            E = scalarMath.Sum(scalarMath.Multiply(B, s), scalarMath.Multiply(E, c));
            B = t;

            t = scalarMath.Difference(scalarMath.Multiply(C, c), scalarMath.Multiply(F, s));
            F = scalarMath.Sum(scalarMath.Multiply(C, s), scalarMath.Multiply(F, c));
            C = t;

            return this;
        }

        // multiply rotation matrix by this
        //
        // [c, -s, 0] [A, B, C]   [ A*c+D*s,  B*c+E*s,  C*c+F*s]
        // [s,  c, 0] [D, E, F] = [-A*s+D*c, -B*s+E*c, -C*s+F*c]
        // [0,  0, 1] [G, H, I]   [ G      ,  H      ,  I      ]
        //
        public TransformMatrix<T, M> RotateBack(IVector<T> rightVersor)
        {
            T c = rightVersor.X;
            T ns = scalarMath.Negate(rightVersor.Y);

            T t;

            t = scalarMath.Difference(scalarMath.Multiply(A, c), scalarMath.Multiply(D, ns));
            D = scalarMath.Sum(scalarMath.Multiply(A, ns), scalarMath.Multiply(D, c));
            A = t;

            t = scalarMath.Difference(scalarMath.Multiply(B, c), scalarMath.Multiply(E, ns));
            E = scalarMath.Sum(scalarMath.Multiply(B, ns), scalarMath.Multiply(E, c));
            B = t;

            t = scalarMath.Difference(scalarMath.Multiply(C, c), scalarMath.Multiply(F, ns));
            F = scalarMath.Sum(scalarMath.Multiply(C, ns), scalarMath.Multiply(F, c));
            C = t;

            return this;
        }

        // multiply rotation matrix by this
        //
        // [c, -s, 0] [A, B, C]   [A*c-D*s, B*c-E*s, C*c-F*s]
        // [s,  c, 0] [D, E, F] = [A*s+D*c, B*s+E*c, C*s+F*c]
        // [0,  0, 1] [G, H, I]   [G      , H      , I      ]
        //
        public TransformMatrix<T, M> Rotate(T angle)
        {
            T c = scalarMath.Cos(angle);
            T s = scalarMath.Sin(angle);

            T tA = scalarMath.Difference(scalarMath.Multiply(A, c), scalarMath.Multiply(D, s));
            T tB = scalarMath.Difference(scalarMath.Multiply(B, c), scalarMath.Multiply(E, s));
            T tC = scalarMath.Difference(scalarMath.Multiply(C, c), scalarMath.Multiply(F, s));

            D = scalarMath.Sum(scalarMath.Multiply(A, s), scalarMath.Multiply(D, c));
            E = scalarMath.Sum(scalarMath.Multiply(B, s), scalarMath.Multiply(E, c));
            F = scalarMath.Sum(scalarMath.Multiply(C, s), scalarMath.Multiply(F, c));

            A = tA;
            B = tB;
            C = tC;

            // G, H, I stay the same

            return this;
        }

        // multiply rotation matrix by this
        //
        // [ c, s, 0] [A, B, C]   [ A*c+D*s,  B*c+E*s,  C*c+F*s]
        // [-s, c, 0] [D, E, F] = [-A*s+D*c, -B*s+E*c, -C*s+F*c]
        // [ 0, 0, 1] [G, H, I]   [ G      ,  H      ,  I      ]
        //
        public TransformMatrix<T, M> RotateBack(T angle)
        {
            T c = scalarMath.Cos(angle);
            T ns = scalarMath.Negate(scalarMath.Sin(angle));

            T tA = scalarMath.Difference(scalarMath.Multiply(A, c), scalarMath.Multiply(D, ns));
            T tB = scalarMath.Difference(scalarMath.Multiply(B, c), scalarMath.Multiply(E, ns));
            T tC = scalarMath.Difference(scalarMath.Multiply(C, c), scalarMath.Multiply(F, ns));

            D = scalarMath.Sum(scalarMath.Multiply(A, ns), scalarMath.Multiply(D, c));
            E = scalarMath.Sum(scalarMath.Multiply(B, ns), scalarMath.Multiply(E, c));
            F = scalarMath.Sum(scalarMath.Multiply(C, ns), scalarMath.Multiply(F, c));

            A = tA;
            B = tB;
            C = tC;

            // G, H, I stay the same

            return this;
        }

        // multiply scaling matrix by this
        //
        // [x, 0, 0] [A, B, C]   [x*A, x*B, x*C]
        // [0, y, 0] [D, E, F] = [y*A, y*B, y*C]
        // [0, 0, 1] [G, H, I]   [G  , H  , I  ]
        //
        public TransformMatrix<T, M> Scale(IVector<T> scale)
        {
            A = scalarMath.Multiply(A, scale.X);
            B = scalarMath.Multiply(B, scale.X);
            C = scalarMath.Multiply(C, scale.X);

            D = scalarMath.Multiply(D, scale.Y);
            E = scalarMath.Multiply(E, scale.Y);
            F = scalarMath.Multiply(F, scale.Y);

            // G, H, I stay the same

            return this;
        }

        // multiply scaling matrix by this
        //
        // [x, 0, 0] [A, B, C]   [x*A, x*B, x*C]
        // [0, y, 0] [D, E, F] = [y*A, y*B, y*C]
        // [0, 0, 1] [G, H, I]   [G  , H  , I  ]
        //
        public TransformMatrix<T, M> Scale(T scaleX, T scaleY)
        {
            A = scalarMath.Multiply(A, scaleX);
            B = scalarMath.Multiply(B, scaleX);
            C = scalarMath.Multiply(C, scaleX);

            D = scalarMath.Multiply(D, scaleY);
            E = scalarMath.Multiply(E, scaleY);
            F = scalarMath.Multiply(F, scaleY);

            // G, H, I stay the same

            return this;
        }

        // multiply scaling matrix by this
        //
        // [k, 0, 0] [A, B, C]   [k*A, k*B, k*C]
        // [0, k, 0] [D, E, F] = [k*D, k*E, k*F]
        // [0, 0, 1] [G, H, I]   [G  , H  , I  ]
        //
        public TransformMatrix<T, M> Scale(T scale)
        {
            A = scalarMath.Multiply(A, scale);
            B = scalarMath.Multiply(B, scale);
            C = scalarMath.Multiply(C, scale);

            D = scalarMath.Multiply(D, scale);
            E = scalarMath.Multiply(E, scale);
            F = scalarMath.Multiply(F, scale);

            // G, H, I stay the same

            return this;
        }
        
        // multiply scaling matrix by this
        //
        // [x, 0, 0] [A, B, C]   [x*A, x*B, x*C]
        // [0, 1, 0] [D, E, F] = [D  , E  , F  ]
        // [0, 0, 1] [G, H, I]   [G  , H  , I  ]
        //
        public TransformMatrix<T, M> ScaleX(T scale)
        {
            A = scalarMath.Multiply(A, scale);
            B = scalarMath.Multiply(B, scale);
            C = scalarMath.Multiply(C, scale);

            // D, E, F, G, H, I stay the same

            return this;
        }

        // multiply scaling matrix by this
        //
        // [1, 0, 0] [A, B, C]   [A  , B  , C  ]
        // [0, y, 0] [D, E, F] = [D*y, E*y, F*y]
        // [0, 0, 1] [G, H, I]   [G  , H  , I  ]
        //
        public TransformMatrix<T, M> ScaleY(T scale)
        {
            D = scalarMath.Multiply(D, scale);
            E = scalarMath.Multiply(E, scale);
            F = scalarMath.Multiply(F, scale);

            // D, E, F, G, H, I stay the same

            return this;
        }

        // multiply scaling by x:-1 y:-1 matrix by this
        //
        // [-1,  0, 0] [A, B, C]   [-A, -B, -C]
        // [0 , -1, 0] [D, E, F] = [-D, -E, -F]
        // [0 ,  0, 1] [G, H, I]   [ G,  H,  I]
        //
        public TransformMatrix<T, M> Mirror()
        {
            A = scalarMath.Negate(A);
            B = scalarMath.Negate(B);
            C = scalarMath.Negate(C);

            D = scalarMath.Negate(D);
            E = scalarMath.Negate(E);
            F = scalarMath.Negate(F);

            // G, H, I stay the same

            return this;
        }

        // multiply scaling by x:-1 matrix by this
        //
        // [-1, 0, 0] [A, B, C]   [-A, -B, -C]
        // [0 , 1, 0] [D, E, F] = [ D,  E,  F]
        // [0 , 0, 1] [G, H, I]   [ G,  H,  I]
        //
        public TransformMatrix<T, M> MirrorX()
        {
            A = scalarMath.Negate(A);
            B = scalarMath.Negate(B);
            C = scalarMath.Negate(C);

            // D, E, F, G, H, I stay the same

            return this;
        }

        // multiply scaling by y:-1 matrix by this
        //
        // [1,  0, 0] [A, B, C]   [ A,  B,  C]
        // [0, -1, 0] [D, E, F] = [-D, -E, -F]
        // [0,  0, 1] [G, H, I]   [ G,  H,  I]
        //
        public TransformMatrix<T, M> MirrorY()
        {
            D = scalarMath.Negate(D);
            E = scalarMath.Negate(E);
            F = scalarMath.Negate(F);

            // D, E, F, G, H, I stay the same

            return this;
        }

        // apply this matrix to 2D entity
        //
        // [A B C] [X]   [A*X+B*Y+C]
        // [C D E] [Y] = [C*X+D*Y+E]
        // [G H I] [1]   [don't care]
        //
        public void Apply(IEntity<T> e)
        {
            e.SetValue(
                scalarMath.SumMany(scalarMath.Multiply(e.X, this.A) , scalarMath.Multiply(e.Y, this.B) , this.C),
                scalarMath.SumMany(scalarMath.Multiply(e.X , this.D) , scalarMath.Multiply(e.Y , this.E) , this.F)
                );
        }
    }
}
