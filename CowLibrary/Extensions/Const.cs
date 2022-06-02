namespace CowLibrary
{
    using System;

    public static class Const
    {
        public const float Deg2Rad = (float)(Math.PI / 180f);
        public const float Rad2Deg = (float)(180f / Math.PI);

        public const float Pi = (float)Math.PI;
        public const float InvPi = (float)(1 / Math.PI);
        public const float PiOver2 = (float)(Math.PI * 0.5);
        public const float PiOver4 = (float)(Math.PI * 0.25);
        public static readonly float SqrtPiInv = 1f / (float)Math.Sqrt(Math.PI);

        public const float Epsilon = 1e-10f;
        public const float OneMinusEpsilon = 0.99999994f;
        public const float Bias = 0.0001f;

        public static readonly RayHit Miss = new();

        public const int KdNodeCount = 3;
        public const int MinNumberOfTriangles = 16;
        public const int MaxDepth = 17;

        public const bool SampleVisibleArea = true;
    }
}
