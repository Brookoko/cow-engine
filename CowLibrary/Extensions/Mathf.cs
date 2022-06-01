namespace CowLibrary
{
    using System;
    using System.Numerics;

    public static class Mathf
    {
        public static (float a, float b) Swap(float a, float b)
        {
            return (b, a);
        }

        public static Vector3 CosineSampleHemisphere(in Vector2 sample)
        {
            var d = ConcentricSampleDisk(sample);
            var z = (float)Math.Sqrt(Math.Max(0, 1 - d.X * d.X - d.Y * d.Y));
            return new Vector3(d.X, z, d.Y);
        }

        public static Vector2 ConcentricSampleDisk(in Vector2 sample)
        {
            var offset = 2 * sample - Vector2.One;

            if (offset.X == 0 && offset.Y == 0)
            {
                return Vector2.Zero;
            }

            float r;
            float theta;
            if (Math.Abs(offset.X) > Math.Abs(offset.Y))
            {
                r = offset.X;
                theta = Const.PiOver4 * (offset.Y / offset.X);
            }
            else
            {
                r = offset.X;
                theta = Const.PiOver2 - Const.PiOver4 * (offset.X / offset.Y);
            }

            return r * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
        }

        public static float AbsCosTheta(in Vector3 w)
        {
            return Math.Abs(CosTheta(in w));
        }

        public static float CosTheta(in Vector3 w)
        {
            return w.Y;
        }

        public static float Cos2Theta(in Vector3 w)
        {
            return w.Y * w.Y;
        }

        public static float SinTheta(in Vector3 w)
        {
            return (float)Math.Sqrt(Sin2Theta(in w));
        }

        public static float Sin2Theta(in Vector3 w)
        {
            return 1 - Cos2Theta(in w);
        }

        public static float CosPhi(in Vector3 w)
        {
            var sinTheta = SinTheta(in w);
            return CosPhi(in w, sinTheta);
        }

        public static float CosPhi(in Vector3 w, float sinTheta)
        {
            return sinTheta == 0 ? 1 : Math.Clamp(w.X / sinTheta, -1, 1);
        }

        public static float SinPhi(in Vector3 w)
        {
            var sinTheta = SinTheta(in w);
            return SinPhi(in w, sinTheta);
        }

        public static float SinPhi(in Vector3 w, float sinTheta)
        {
            return sinTheta == 0 ? 1 : Math.Clamp(w.Z / sinTheta, -1, 1);
        }

        public static float Pdf(in Vector3 wo, in Vector3 wi)
        {
            return !SameHemisphere(in wo, in wi) ? AbsCosTheta(in wi) * Const.InvPi : 0;
        }

        public static bool SameHemisphere(in Vector3 wo, in Vector3 wi)
        {
            return wo.Y * wi.Y > 0;
        }

        public static float PowerHeuristic(int nf, float fPdf, int ng, float gPdf)
        {
            var f = nf * fPdf;
            var g = ng * gPdf;
            return f * f / (f * f + g * g);
        }

        public static Vector3 ToWorld(in Vector3 normal, in Vector3 w)
        {
            var m = GetWorld(in normal, in w);
            return m.MultiplyVector(w).Normalize();
        }

        public static Vector3 ToLocal(in Vector3 normal, in Vector3 w)
        {
            var m = GetWorld(in normal, in w);
            return Invert(in m).MultiplyVector(w).Normalize();
        }

        public static (Matrix4x4 toLocal, Matrix4x4 toWorld) GetMatrices(in Vector3 normal, in Vector3 w)
        {
            var toWorld = GetWorld(in normal, in w);
            var toLocal = Invert(in toWorld);
            return (toLocal, toWorld);
        }

        private static Matrix4x4 GetWorld(in Vector3 normal, in Vector3 w)
        {
            var right = Vector3.Cross(normal, w).Normalize();
            var forward = Vector3.Cross(right, normal).Normalize();
            return Matrix4x4Extensions.FromBasis(right, normal, forward, Vector3.Zero);
        }

        private static Matrix4x4 Invert(in Matrix4x4 matrix)
        {
            var result = new Matrix4x4();
            float a = matrix.M11, b = matrix.M12, c = matrix.M13, d = matrix.M14;
            float e = matrix.M21, f = matrix.M22, g = matrix.M23, h = matrix.M24;
            float i = matrix.M31, j = matrix.M32, k = matrix.M33, l = matrix.M34;
            float m = matrix.M41, n = matrix.M42, o = matrix.M43, p = matrix.M44;

            var kp_lo = k * p - l * o;
            var jp_ln = j * p - l * n;
            var jo_kn = j * o - k * n;
            var ip_lm = i * p - l * m;
            var io_km = i * o - k * m;
            var in_jm = i * n - j * m;

            var a11 = +(f * kp_lo - g * jp_ln + h * jo_kn);
            var a12 = -(e * kp_lo - g * ip_lm + h * io_km);
            var a13 = +(e * jp_ln - f * ip_lm + h * in_jm);
            var a14 = -(e * jo_kn - f * io_km + g * in_jm);

            var det = a * a11 + b * a12 + c * a13 + d * a14;

            var invDet = 1.0f / det;

            result.M11 = a11 * invDet;
            result.M21 = a12 * invDet;
            result.M31 = a13 * invDet;
            result.M41 = a14 * invDet;

            result.M12 = -(b * kp_lo - c * jp_ln + d * jo_kn) * invDet;
            result.M22 = +(a * kp_lo - c * ip_lm + d * io_km) * invDet;
            result.M32 = -(a * jp_ln - b * ip_lm + d * in_jm) * invDet;
            result.M42 = +(a * jo_kn - b * io_km + c * in_jm) * invDet;

            var gp_ho = g * p - h * o;
            var fp_hn = f * p - h * n;
            var fo_gn = f * o - g * n;
            var ep_hm = e * p - h * m;
            var eo_gm = e * o - g * m;
            var en_fm = e * n - f * m;

            result.M13 = +(b * gp_ho - c * fp_hn + d * fo_gn) * invDet;
            result.M23 = -(a * gp_ho - c * ep_hm + d * eo_gm) * invDet;
            result.M33 = +(a * fp_hn - b * ep_hm + d * en_fm) * invDet;
            result.M43 = -(a * fo_gn - b * eo_gm + c * en_fm) * invDet;

            var gl_hk = g * l - h * k;
            var fl_hj = f * l - h * j;
            var fk_gj = f * k - g * j;
            var el_hi = e * l - h * i;
            var ek_gi = e * k - g * i;
            var ej_fi = e * j - f * i;

            result.M14 = -(b * gl_hk - c * fl_hj + d * fk_gj) * invDet;
            result.M24 = +(a * gl_hk - c * el_hi + d * ek_gi) * invDet;
            result.M34 = -(a * fl_hj - b * el_hi + d * ej_fi) * invDet;
            result.M44 = +(a * fk_gj - b * ek_gi + c * ej_fi) * invDet;

            return result;
        }
    }
}
