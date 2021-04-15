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
                _localPosition = parent == null ? value : parent.localToWorldMatrix.MultiplyPoint(value);
                CalculateMatrix();
            }
        }

        public Quaternion rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _localRotation = parent == null ? value : Quaternion.Inverse(parent.rotation) * _rotation;
                CalculateMatrix();
            }
        }
        
        public Vector3 lossyScale
        {
            get => _lossyScale;
            set
            {
                _lossyScale = value;
                _localScale = parent == null ? value : parent.localToWorldMatrix.MultiplyPoint(value);
                CalculateMatrix();
            }
        }
        
        public Vector3 localPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;
                _position = parent == null ? value : parent.worldToLocalMatrix.MultiplyPoint(value);
                CalculateMatrix();
            }
        }
        
        public Quaternion localRotation
        {
            get => _localRotation;
            set
            {
                _localRotation = value;
                _rotation = parent == null ? value : parent.rotation * value;
                CalculateMatrix();
            }
        }
        
        public Vector3 localScale
        {
            get => _localScale;
            set
            {
                _localScale = value;
                _lossyScale = parent == null ? value : parent.worldToLocalMatrix.MultiplyVector(value);
                CalculateMatrix();
            }
        }

        public Matrix4x4 localToWorldMatrix
        {
            get => _localToWorldMatrix;
            set
            {
                _localToWorldMatrix = value;
                Matrix4x4.Invert(_localToWorldMatrix, out _worldToLocalMatrix);
                ExtractValuesFromMatrix();
            }
        }
        
        public Matrix4x4 worldToLocalMatrix
        {
            get => _worldToLocalMatrix;
            set
            {
                _worldToLocalMatrix = value;
                Matrix4x4.Invert(_worldToLocalMatrix, out _localToWorldMatrix);
                ExtractValuesFromMatrix();
            }
        }
        
        public Vector3 Forward => new Vector3(localToWorldMatrix.M13, localToWorldMatrix.M23, localToWorldMatrix.M33);
        public Vector3 Right => new Vector3(localToWorldMatrix.M11, localToWorldMatrix.M21, localToWorldMatrix.M31);
        public Vector3 Up => new Vector3(localToWorldMatrix.M12, localToWorldMatrix.M22, localToWorldMatrix.M32);
        
        private Transform _parent;
        
        private Vector3 _position = Vector3.Zero;
        private Quaternion _rotation = Quaternion.Identity;
        private Vector3 _lossyScale = Vector3.One;
        
        private Vector3 _localPosition = Vector3.Zero;
        private Quaternion _localRotation = Quaternion.Identity;
        private Vector3 _localScale = Vector3.One;
        
        private Matrix4x4 _localToWorldMatrix = Matrix4x4.Identity;
        private Matrix4x4 _worldToLocalMatrix = Matrix4x4.Identity;
        
        public void SetParent(Transform newParent)
        {
            _parent = newParent;
            CalculateMatrix();
        }
        
        private void CalculateMatrix()
        {
            _worldToLocalMatrix = Matrix4x4Extensions.TRS(_position, _rotation, _lossyScale);
            Matrix4x4.Invert(_worldToLocalMatrix, out _localToWorldMatrix);
        }
        
        private void ExtractValuesFromMatrix()
        {
            _position = _localToWorldMatrix.ExtractTranslation();
            _rotation = Quaternion.CreateFromRotationMatrix(_localToWorldMatrix);
            _lossyScale = _localToWorldMatrix.ExtractScale();
            _localPosition = parent == null ? _position : parent.localToWorldMatrix.MultiplyPoint(_position);
            _localRotation = parent == null ? _rotation : Quaternion.Inverse(parent.rotation) * _rotation;
            _localScale = parent == null ? _lossyScale : parent.localToWorldMatrix.MultiplyPoint(_lossyScale);
        }
    }
}