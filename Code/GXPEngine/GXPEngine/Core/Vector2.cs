using System;

namespace GXPEngine.Core
{
    public struct Vector2
    {
        public float x;
        public float y;

        public float Length { get; private set; }
        

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;

            Length = 0;
        }

        public static Vector2 operator *(Vector2 A, Vector2 B)
        {
            A.x *= B.x;
            A.y *= B.y;

            A.x = float.IsNaN(A.x) ? 0 : A.x;
            A.y = float.IsNaN(A.y) ? 0 : A.y;

            return A;
        }

        public static Vector2 operator *(Vector2 A, float B)
        {
            A.x *= B;
            A.y *= B;

            A.x = float.IsNaN(A.x) ? 0 : A.x;
            A.y = float.IsNaN(A.y) ? 0 : A.y;

            return A;
        }

        public static Vector2 operator +(Vector2 A, Vector2 B)
        {
            A.x += B.x;
            A.y += B.y;

            A.x = float.IsNaN(A.x) ? 0 : A.x;
            A.y = float.IsNaN(A.y) ? 0 : A.y;

            return A;
        }

        public static Vector2 operator +(Vector2 A, float B)
        {
            A.x += B;
            A.y += B;

            A.x = float.IsNaN(A.x) ? 0 : A.x;
            A.y = float.IsNaN(A.y) ? 0 : A.y;

            return A;
        }

        public static Vector2 operator -(Vector2 A, Vector2 B)
        {
            A.x -= B.x;
            A.y -= B.y;

            A.x = float.IsNaN(A.x) ? 0 : A.x;
            A.y = float.IsNaN(A.y) ? 0 : A.y;

            return A;
        }

        public static Vector2 operator -(Vector2 A, float B)
        {
            A.x -= B;
            A.y -= B;

            A.x = float.IsNaN(A.x) ? 0 : A.x;
            A.y = float.IsNaN(A.y) ? 0 : A.y;

            return A;
        }

        public static Vector2 operator /(Vector2 A, Vector2 B)
        {
            A.x /= B.x;
            A.y /= B.y;

            A.x = float.IsNaN(A.x) ? 0 : A.x;
            A.y = float.IsNaN(A.y) ? 0 : A.y;

            return A;
        }

        public static Vector2 operator /(Vector2 A, float B)
        {
            A.x /= B;
            A.y /= B;

            A.x = float.IsNaN(A.x) ? 0 : A.x;
            A.y = float.IsNaN(A.y) ? 0 : A.y;

            return A;
        }

        public static bool operator ==(Vector2 A, Vector2 B)
        {

            return A.Equals(B);
        }

        public static bool operator !=(Vector2 A, Vector2 B)
        {
            return !A.Equals(B);
        }

        public static float Dot(Vector2 A, Vector2 B)
        {
            return (A.x * B.x) * (A.y * B.y);
        }

        override public string ToString()
        {
            return "[Vector2 " + x + ", " + y + "]";
        }

        public float Magnitude()
        {
            //Console.WriteLine("X is " + x + " and Y is " + y);
            Length = Mathf.Sqrt(x * x + y * y);
            Length = float.IsNaN(Length) ? 0 : Length;
            //Console.WriteLine("Length is: " + Length);
            return Length;
        }

        public float SqrMagnitude()
        {
            //Console.WriteLine("X is " + x + " and Y is " + y);
            float squaredLength = x * x + y * y;
            squaredLength = float.IsNaN(squaredLength) ? 0 : squaredLength;
            //Console.WriteLine("Length is: " + Length);
            return squaredLength;
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
        }

        public Vector2 Normalize()
        {
            float length = Magnitude();
            this = new Vector2(x, y) / length;

            return this;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

