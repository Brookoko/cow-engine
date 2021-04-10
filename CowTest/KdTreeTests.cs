using NUnit.Framework;

namespace CowTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Numerics;
    using CowEngine;
    using CowEngine.ImageWorker;
    using CowLibrary;
    using ObjLoader.Loader.Loaders;

    public class KdTreeTests
    {
        private ObjWorker objWorker;
        
        [SetUp]
        public void Setup()
        {
            objWorker = new ObjWorker
            {
                IoWorker = new IoWorker(),
                ObjLoaderFactory = new ObjLoaderFactory(),
                ModelToObjectConverter = new ModelToObjectConverter()
            };
        }
        
        [Test]
        public void _01TestOneNode()
        {
            var triangles = new List<Triangle>()
            {
                new Triangle(new Vector3(-3, 0, 0), new Vector3(-2, 2, 0), new Vector3(-1, 0, 0)),
                new Triangle(new Vector3(-1, 0, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(2, 2, 0), new Vector3(3, 0, 0)),
            };
            var tree = new KdTree(triangles);
            Assert.AreEqual(new Vector3(-3, 0, 0), tree.Box.min);
            Assert.AreEqual(new Vector3(3, 2, 0), tree.Box.max);
            Assert.AreEqual(tree.root.children.Count, 0);
        }
        
        [Test]
        public void _02TestThreeNodes()
        {
            var triangles = new List<Triangle>()
            {
                new Triangle(new Vector3(-3, 0, 0), new Vector3(-2, 2, 0), new Vector3(-1, 0, 0)),
                new Triangle(new Vector3(-3, 0, 0), new Vector3(-2, 2, 0), new Vector3(-1, 0, 0)),
                new Triangle(new Vector3(-3, 0, 0), new Vector3(-2, 2, 0), new Vector3(-1, 0, 0)),
                new Triangle(new Vector3(-3, 0, 0), new Vector3(-2, 2, 0), new Vector3(-1, 0, 0)),
                new Triangle(new Vector3(-3, 0, 0), new Vector3(-2, 2, 0), new Vector3(-1, 0, 0)),
                new Triangle(new Vector3(-3, 0, 0), new Vector3(-2, 2, 0), new Vector3(-1, 0, 0)),
                new Triangle(new Vector3(-3, 0, 0), new Vector3(-2, 2, 0), new Vector3(-1, 0, 0)),
                new Triangle(new Vector3(-3, 0, 0), new Vector3(-2, 2, 0), new Vector3(-1, 0, 0)),
                new Triangle(new Vector3(-1, 0, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0)),
                new Triangle(new Vector3(-1, 0, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0)),
                new Triangle(new Vector3(-1, 0, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0)),
                new Triangle(new Vector3(-1, 0, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0)),
                new Triangle(new Vector3(-1, 0, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0)),
                new Triangle(new Vector3(-1, 0, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0)),
                new Triangle(new Vector3(-1, 0, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0)),
                new Triangle(new Vector3(-1, 0, 0), new Vector3(0, 2, 0), new Vector3(1, 0, 0)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(2, 2, 0), new Vector3(3, 0, 0)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(2, 2, 0), new Vector3(3, 0, 0)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(2, 2, 0), new Vector3(3, 0, 0)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(2, 2, 0), new Vector3(3, 0, 0)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(2, 2, 0), new Vector3(3, 0, 0)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(2, 2, 0), new Vector3(3, 0, 0)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(2, 2, 0), new Vector3(3, 0, 0)),
                new Triangle(new Vector3(1, 0, 0), new Vector3(2, 2, 0), new Vector3(3, 0, 0)),
            };
            var tree = new KdTree(triangles);
            Assert.AreEqual(new Vector3(-3, 0, 0), tree.Box.min);
            Assert.AreEqual(new Vector3(3, 2, 0), tree.Box.max);
            Assert.AreEqual(tree.root.children.Count, 3);
        }
        
        [Test]
        public void _03TestModel()
        {
            var watch = new Stopwatch();
            var obj = objWorker.Parse("C:\\Projects\\cow-engine\\assets\\cow.obj");
            var triangles = (obj.mesh as TriangleMesh).triangles;
            watch.Start();
            var tree = new KdTree(triangles);
            watch.Stop();
            var time = watch.Elapsed;
            Console.WriteLine($"{time.Milliseconds}");
            Assert.Less(time.Milliseconds, 200);
        }
    }
}