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
            Assert.AreEqual(Matrix4x4.Identity, transform.localToWorldMatrix);
            Assert.AreEqual(Matrix4x4.Identity, transform.worldToLocalMatrix);
            Assert.AreEqual(Vector3.Zero, transform.localPosition);
            Assert.AreEqual(Vector3.One, transform.localScale);
            Assert.AreEqual(Quaternion.Identity, transform.localRotation);
            Assert.AreEqual(Vector3.Zero, transform.position);
            Assert.AreEqual(Vector3.One, transform.lossyScale);
            Assert.AreEqual(Quaternion.Identity, transform.rotation);
        }

        [Test]
        public void _02TestWorldPositionNoParent()
        {
            var transform = new Transform();
            transform.position = Vector3.One;
            Assert.AreEqual(Vector3.One, transform.position);
            Assert.AreEqual(Vector3.One, transform.localPosition);
        }
        
        [Test]
        public void _03TestWorldPosition()
        {
            var transform = new Transform();
            var parent = transform.parent = new Transform();
            parent.position = Vector3.One;
            transform.position = Vector3.One;
            Assert.AreEqual(Vector3.One, transform.position);
            Assert.AreEqual(Vector3.Zero, transform.localPosition);
        }
        
        [Test]
        public void _04TestLocalPositionNoParent()
        {
            var transform = new Transform();
            transform.localPosition = Vector3.One;
            Assert.AreEqual(Vector3.One, transform.position);
            Assert.AreEqual(Vector3.One, transform.localPosition);
        }
        
        [Test]
        public void _05TestLocalPosition()
        {
            var transform = new Transform();
            var parent = transform.parent = new Transform();
            parent.position = Vector3.One;
            transform.localPosition = Vector3.One;
            Assert.AreEqual(Vector3.One * 2, transform.position);
            Assert.AreEqual(Vector3.One, transform.localPosition);
        }
        
        [Test]
        public void _06TestWorldRotationNoParent()
        {
            var transform = new Transform();
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            transform.rotation = rot;
            Assert.AreEqual(rot, transform.rotation);
            Assert.AreEqual(rot, transform.localRotation);
        }
        
        [Test]
        public void _07TestWorldRotation()
        {
            var transform = new Transform();
            var parent = transform.parent = new Transform();
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            parent.rotation = rot;
            transform.rotation = rot;
            Assert.AreEqual(rot, transform.rotation);
            Assert.AreEqual(Quaternion.Identity, transform.localRotation);
        }
        
        [Test]
        public void _08TestLocalRotationNoParent()
        {
            var transform = new Transform();
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            transform.localRotation = rot;
            Assert.AreEqual(rot, transform.rotation);
            Assert.AreEqual(rot, transform.localRotation);
        }
        
        [Test]
        public void _09TestLocalRotation()
        {
            var transform = new Transform();
            var parent = transform.parent = new Transform();
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            parent.rotation = rot;
            transform.localRotation = rot;
            Assert.AreEqual(rot * rot, transform.rotation);
            Assert.AreEqual(rot, transform.localRotation);
        }
        
        [Test]
        public void _10TestWorldScaleNoParent()
        {
            var transform = new Transform();
            transform.lossyScale = Vector3.One * 2;
            Assert.AreEqual(Vector3.One * 2, transform.lossyScale);
            Assert.AreEqual(Vector3.One * 2, transform.localScale);
        }
        
        [Test]
        public void _11TestWorldScale()
        {
            var transform = new Transform();
            var parent = transform.parent = new Transform();
            parent.lossyScale = Vector3.One * 2;
            transform.lossyScale = Vector3.One * 2;
            Assert.AreEqual(Vector3.One * 2, transform.lossyScale);
            Assert.AreEqual(Vector3.One, transform.localScale);
        }
        
        [Test]
        public void _12TestLocalScaleNoParent()
        {
            var transform = new Transform();
            transform.localScale = Vector3.One * 2;
            Assert.AreEqual(Vector3.One * 2, transform.lossyScale);
            Assert.AreEqual(Vector3.One * 2, transform.localScale);
        }
        
        [Test]
        public void _13TestLocalScale()
        {
            var transform = new Transform();
            var parent = transform.parent = new Transform();
            parent.lossyScale = Vector3.One * 2;
            transform.localScale = Vector3.One * 2;
            Assert.AreEqual(Vector3.One * 4, transform.lossyScale);
            Assert.AreEqual(Vector3.One * 2, transform.localScale);
        }
        
        [Test]
        public void _14TestComplexTransformation()
        {
            var transform = new Transform();
            var parent = transform.parent = new Transform();
            parent.localPosition = Vector3.One;
            parent.localScale = Vector3.One * 2;
            transform.localScale = Vector3.One * 2;
            transform.localPosition = Vector3.One;
            var rot = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            transform.localRotation = rot;
            Assert.AreEqual(Vector3.One, transform.localPosition);
            Assert.AreEqual(Vector3.One * 2, transform.localScale);
            Assert.AreEqual(rot, transform.localRotation);
            Assert.AreEqual(Vector3.One * 3, transform.position);
            Assert.AreEqual(Vector3.One * 4, transform.lossyScale);
            Assert.AreEqual(rot, transform.rotation);
        }
    }
}