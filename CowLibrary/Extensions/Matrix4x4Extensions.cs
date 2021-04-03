namespace CowLibrary
{
    using System.Numerics;

    public static class Matrix4x4Extensions
    {
        public static Vector4 Multiply(this Matrix4x4 m, Vector4 v)
        {
            Vector4 vector;
            vector.X = m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14 * v.W;
            vector.Y = m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24 * v.W;
            vector.Z = m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34 * v.W;
            vector.W = m.M41 * v.X + m.M42 * v.Y + m.M43 * v.Z + m.M44 * v.W;
            return vector;
        }
        
        public static Vector3 MultiplyVector(this Matrix4x4 m, Vector3 v)
        {
            Vector3 vector;
            vector.X = m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z;
            vector.Y = m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z;
            vector.Z = m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z;
            return vector;
        }
        
        public static Vector3 MultiplyPoint(this Matrix4x4 m, Vector3 v)
        {
            Vector3 vector;
            vector.X = m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z + m.M14;
            vector.Y = m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z + m.M24;
            vector.Z = m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z + m.M34;
            var num = 1f / (m.M41 * v.X + m.M42 * v.Y + m.M43 * v.Z + m.M44);
            vector.X *= num;
            vector.Y *= num;
            vector.Z *= num;
            return vector;
        }
        
        public static Matrix4x4 Translate(Vector3 vector)
        {
            Matrix4x4 matrix;
            matrix.M11 = 1f;
            matrix.M12 = 0.0f;
            matrix.M13 = 0.0f;
            matrix.M14 = vector.X;
            matrix.M21 = 0.0f;
            matrix.M22 = 1f;
            matrix.M23 = 0.0f;
            matrix.M24 = vector.Y;
            matrix.M31 = 0.0f;
            matrix.M32 = 0.0f;
            matrix.M33 = 1f;
            matrix.M34 = vector.Z;
            matrix.M41 = 0.0f;
            matrix.M42 = 0.0f;
            matrix.M43 = 0.0f;
            matrix.M44 = 1f;
            return matrix;
        }
        
        public static Matrix4x4 Rotate(Quaternion q)
        {
            var num1 = q.X * 2f;
            var num2 = q.Y * 2f;
            var num3 = q.Z * 2f;
            var num4 = q.X * num1;
            var num5 = q.Y * num2;
            var num6 = q.Z * num3;
            var num7 = q.X * num2;
            var num8 = q.X * num3;
            var num9 = q.Y * num3;
            var num10 = q.W * num1;
            var num11 = q.W * num2;
            var num12 = q.W * num3;
            Matrix4x4 matrix;
            matrix.M11 = 1.0f - (num5 + num6);
            matrix.M21 = num7 + num12;
            matrix.M31 = num8 - num11;
            matrix.M41 = 0.0f;
            matrix.M12 = num7 - num12;
            matrix.M22 = 1.0f - (num4 + num6);
            matrix.M32 = num9 + num10;
            matrix.M42 = 0.0f;
            matrix.M13 = num8 + num11;
            matrix.M23 = num9 - num10;
            matrix.M33 = 1.0f - (num4 + num5);
            matrix.M43 = 0.0f;
            matrix.M14 = 0.0f;
            matrix.M24 = 0.0f;
            matrix.M34 = 0.0f;
            matrix.M44 = 1f;
            return matrix;
        }
        
        public static Matrix4x4 Scale(Vector3 vector)
        {
            Matrix4x4 matrix;
            matrix.M11 = vector.X;
            matrix.M12 = 0.0f;
            matrix.M13 = 0.0f;
            matrix.M14 = 0.0f;
            matrix.M21 = 0.0f;
            matrix.M22 = vector.Y;
            matrix.M23 = 0.0f;
            matrix.M24 = 0.0f;
            matrix.M31 = 0.0f;
            matrix.M32 = 0.0f;
            matrix.M33 = vector.Z;
            matrix.M34 = 0.0f;
            matrix.M41 = 0.0f;
            matrix.M42 = 0.0f;
            matrix.M43 = 0.0f;
            matrix.M44 = 1f;
            return matrix;
        }
        
        public static Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
        {
            var translate = Translate(pos);
            var rotation = Rotate(q);
            var scale = Scale(s);
            return translate * rotation * scale;
        }
        
        public static Matrix4x4 LookAt(Vector3 from, Vector3 to, Vector3 up)
        {
            var forward = (from - to).Normalize();
            var right = Vector3.Cross(up, forward);
            up = Vector3.Cross(forward, right);

            Matrix4x4 matrix;

            matrix.M11 = right.X;
            matrix.M21 = right.Y;
            matrix.M31 = right.Z;
            
            matrix.M12 = up.X;
            matrix.M22 = up.Y;
            matrix.M32 = up.Z;
            
            matrix.M13 = forward.X;
            matrix.M23 = forward.Y;
            matrix.M33 = forward.Z;
            
            matrix.M14 = from.X;
            matrix.M24 = from.Y;
            matrix.M34 = from.Z;
            
            matrix.M41 = 0;
            matrix.M42 = 0;
            matrix.M43 = 0;
            matrix.M44 = 1;
            
            return matrix;
        }
    }
}