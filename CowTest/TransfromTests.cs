namespace CowTest
{
    using System.Numerics;
    using CowLibrary;
    using NUnit.Framework;

    public class TransfromTests
    {
        [Test]
        public void _01TestDefaultTransform()
        {
            var transform = new Transform();
            Assert.AreEqual(Matrix4x4.Identity, transform.LocalToWorldMatrix);
            Assert.AreEqual(Matrix4x4.Identity, transform.WorldToLocalMatrix);
            Assert.AreEqual(Vector3.Zero, transform.LocalPosition);
            Assert.AreEqual(Vector3.One, transform.LocalScale);
            Assert.AreEqual(Quaternion.Identity, transform.LocalRotation);
            Assert.AreEqual(Vector3.Zero, transform.Position);
            Assert.AreEqual(Vector3.One, transform.LossyScale);
            Assert.AreEqual(Quaternion.Identity, transform.Rotation);
        }

        [Test]
        public void _02TestWorldPositionNoParent()
        {
            var transform = new Transform();
            transform.Position = Vector3.One;
            Assert.AreEqual(Vector3.One, transform.Position);
            Assert.AreEqual(Vector3.One, transform.LocalPosition);
        }
        
        [Test]
        public void _03TestWorldPosition()
        {
            var transform = new Transform();
            var parent = transform.Parent = new Transform();
            parent.Position = Vector3.One;
            transform.Position = Vector3.One;
            Assert.AreEqual(Vector3.One, transform.Position);
            Assert.AreEqual(Vector3.Zero, transform.LocalPosition);
        }
        
        [Test]
        public void _04TestLocalPositionNoParent()
        {
            var transform = new Transform();
            transform.LocalPosition = Vector3.One;
            Assert.AreEqual(Vector3.One, transform.Position);
            Assert.AreEqual(Vector3.One, transform.LocalPosition);
        }
        
        [Test]
        public void _05TestLocalPosition()
        {
            var transform = new Transform();
            var parent = transform.Parent = new Transform();
            parent.Position = Vector3.One;
            transform.LocalPosition = Vector3.One;
            Assert.AreEqual(Vector3.One * 2, transform.Position);
            Assert.AreEqual(Vector3.One, transform.LocalPosition);
        }
        
        [Test]
        public void _06TestWorldRotationNoParent()
        {
            var transform = new Transform();
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            transform.Rotation = rot;
            Assert.AreEqual(rot, transform.Rotation);
            Assert.AreEqual(rot, transform.LocalRotation);
        }
        
        [Test]
        public void _07TestWorldRotation()
        {
            var transform = new Transform();
            var parent = transform.Parent = new Transform();
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            parent.Rotation = rot;
            transform.Rotation = rot;
            Assert.AreEqual(rot, transform.Rotation);
            Assert.AreEqual(Quaternion.Identity, transform.LocalRotation);
        }
        
        [Test]
        public void _08TestLocalRotationNoParent()
        {
            var transform = new Transform();
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            transform.LocalRotation = rot;
            Assert.AreEqual(rot, transform.Rotation);
            Assert.AreEqual(rot, transform.LocalRotation);
        }
        
        [Test]
        public void _09TestLocalRotation()
        {
            var transform = new Transform();
            var parent = transform.Parent = new Transform();
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            parent.Rotation = rot;
            transform.LocalRotation = rot;
            Assert.AreEqual(rot * rot, transform.Rotation);
            Assert.AreEqual(rot, transform.LocalRotation);
        }
        
        [Test]
        public void _10TestWorldScaleNoParent()
        {
            var transform = new Transform();
            transform.LossyScale = Vector3.One * 2;
            Assert.AreEqual(Vector3.One * 2, transform.LossyScale);
            Assert.AreEqual(Vector3.One * 2, transform.LocalScale);
        }
        
        [Test]
        public void _11TestWorldScale()
        {
            var transform = new Transform();
            var parent = transform.Parent = new Transform();
            parent.LossyScale = Vector3.One * 2;
            transform.LossyScale = Vector3.One * 2;
            Assert.AreEqual(Vector3.One * 2, transform.LossyScale);
            Assert.AreEqual(Vector3.One, transform.LocalScale);
        }
        
        [Test]
        public void _12TestLocalScaleNoParent()
        {
            var transform = new Transform();
            transform.LocalScale = Vector3.One * 2;
            Assert.AreEqual(Vector3.One * 2, transform.LossyScale);
            Assert.AreEqual(Vector3.One * 2, transform.LocalScale);
        }
        
        [Test]
        public void _13TestLocalScale()
        {
            var transform = new Transform();
            var parent = transform.Parent = new Transform();
            parent.LossyScale = Vector3.One * 2;
            transform.LocalScale = Vector3.One * 2;
            Assert.AreEqual(Vector3.One * 4, transform.LossyScale);
            Assert.AreEqual(Vector3.One * 2, transform.LocalScale);
        }
        
        [Test]
        public void _14TestComplexTransformation()
        {
            var transform = new Transform();
            var parent = transform.Parent = new Transform();
            parent.LocalPosition = Vector3.One;
            parent.LocalScale = Vector3.One * 2;
            transform.LocalScale = Vector3.One * 2;
            transform.LocalPosition = Vector3.One;
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            transform.LocalRotation = rot;
            Assert.AreEqual(Vector3.One, transform.LocalPosition);
            Assert.AreEqual(Vector3.One * 2, transform.LocalScale);
            Assert.AreEqual(rot, transform.LocalRotation);
            Assert.AreEqual(Vector3.One * 3, transform.Position);
            Assert.AreEqual(Vector3.One * 4, transform.LossyScale);
            Assert.AreEqual(rot, transform.Rotation);
        }
    }
}