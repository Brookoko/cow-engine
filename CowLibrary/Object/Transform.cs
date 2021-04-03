namespace CowLibrary
{
    using System.Numerics;

    public class Transform
    {
        public Transform parent
        {
            get => _parent;
            set => SetParent(value);
        }

        public Vector3 position
        {
            get => _position;
            set
            {
                _position = value;
                CalculateWorldToLocalMatrix();
                _localPosition = worldToLocalMatrix.MultiplyPoint(value);
            }
        }

        public Quaternion rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                CalculateWorldToLocalMatrix();
                _localRotation = Quaternion.Inverse(parent.rotation) * _rotation;
            }
        }
        
        public Vector3 lossyScale
        {
            get => _lossyScale;
            set
            {
                _lossyScale = value;
                CalculateWorldToLocalMatrix();
                _localScale = worldToLocalMatrix.MultiplyVector(value);
            }
        }
        
        public Vector3 localPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;
                CalculateLocalToWorldMatrix();
                _position = localToWorldMatrix.MultiplyPoint(value);
            }
        }
        
        public Quaternion localRotation
        {
            get => _localRotation;
            set
            {
                _localRotation = value;
                CalculateLocalToWorldMatrix();
                _rotation = parent.rotation * _localRotation;
            }
        }
        
        public Vector3 localScale
        {
            get => _localScale;
            set
            {
                _localScale = value;
                CalculateLocalToWorldMatrix();
                _lossyScale = localToWorldMatrix.MultiplyVector(_localScale);
            }
        }
        
        public Matrix4x4 localToWorldMatrix;
        public Matrix4x4 worldToLocalMatrix;
        
        private Transform _parent;
        
        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _lossyScale;
        
        private Vector3 _localPosition;
        private Quaternion _localRotation;
        private Vector3 _localScale;
        
        public void SetParent(Transform newParent)
        {
            _parent = newParent;
            CalculateWorldToLocalMatrix();
            CalculateLocalToWorldMatrix();
        }
        
        private void CalculateWorldToLocalMatrix()
        {
            worldToLocalMatrix = Matrix4x4Extensions.TRS(_position, _rotation, _lossyScale);
        }
        
        private void CalculateLocalToWorldMatrix()
        {
            var localToParentMatrix = Matrix4x4Extensions.TRS(_localPosition, _localRotation, _localScale);
            localToWorldMatrix = parent.localToWorldMatrix * localToParentMatrix;
        }
    }
}