namespace CowLibrary
{
    using System.Numerics;

    public class Transform
    {
        public Transform Parent
        {
            get => parent;
            set => SetParent(value);
        }

        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                localPosition = Parent == null ? value : Parent.WorldToLocalMatrix.MultiplyPoint(value);
                CalculateMatrix();
            }
        }

        public Quaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                localRotation = Parent == null ? value : Quaternion.Inverse(Parent.Rotation) * value;
                CalculateMatrix();
            }
        }

        public Vector3 LossyScale
        {
            get => lossyScale;
            set
            {
                lossyScale = value;
                localScale = Parent == null ? value : Parent.WorldToLocalMatrix.MultiplyVector(value);
                CalculateMatrix();
            }
        }

        public Vector3 LocalPosition
        {
            get => localPosition;
            set
            {
                localPosition = value;
                position = Parent == null ? value : Parent.LocalToWorldMatrix.MultiplyPoint(value);
                CalculateMatrix();
            }
        }

        public Quaternion LocalRotation
        {
            get => localRotation;
            set
            {
                localRotation = value;
                rotation = Parent == null ? value : Parent.Rotation * value;
                CalculateMatrix();
            }
        }

        public Vector3 LocalScale
        {
            get => localScale;
            set
            {
                localScale = value;
                lossyScale = Parent == null ? value : Parent.LocalToWorldMatrix.MultiplyVector(value);
                CalculateMatrix();
            }
        }

        public Matrix4x4 LocalToWorldMatrix
        {
            get => localToWorldMatrix;
            set
            {
                localToWorldMatrix = value;
                Matrix4x4.Invert(localToWorldMatrix, out worldToLocalMatrix);
                ExtractValuesFromMatrix();
            }
        }

        public Matrix4x4 WorldToLocalMatrix
        {
            get => worldToLocalMatrix;
            set
            {
                worldToLocalMatrix = value;
                Matrix4x4.Invert(worldToLocalMatrix, out localToWorldMatrix);
                ExtractValuesFromMatrix();
            }
        }

        public Vector3 Right => localToWorldMatrix.Right();
        public Vector3 Up => localToWorldMatrix.Up();
        public Vector3 Forward => localToWorldMatrix.Forward();

        private Transform parent;

        private Vector3 position = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 lossyScale = Vector3.One;

        private Vector3 localPosition = Vector3.Zero;
        private Quaternion localRotation = Quaternion.Identity;
        private Vector3 localScale = Vector3.One;

        private Matrix4x4 localToWorldMatrix = Matrix4x4.Identity;
        private Matrix4x4 worldToLocalMatrix = Matrix4x4.Identity;

        public void SetParent(Transform newParent)
        {
            parent = newParent;
            CalculateMatrix();
            ExtractValuesFromMatrix();
        }

        private void CalculateMatrix()
        {
            localToWorldMatrix = Matrix4x4Extensions.TRS(Position, Rotation, LossyScale);
            Matrix4x4.Invert(localToWorldMatrix, out worldToLocalMatrix);
        }

        private void ExtractValuesFromMatrix()
        {
            position = localToWorldMatrix.ExtractTranslation();
            rotation = Quaternion.CreateFromRotationMatrix(localToWorldMatrix);
            lossyScale = localToWorldMatrix.ExtractScale();
            localPosition = Parent == null ? position : Parent.WorldToLocalMatrix.MultiplyPoint(position);
            localRotation = Parent == null ? rotation : Quaternion.Inverse(Parent.Rotation) * rotation;
            localScale = Parent == null ? lossyScale : Parent.WorldToLocalMatrix.MultiplyPoint(lossyScale);
        }
    }
}
