namespace CowEngine
{
    using System.Collections.Generic;
    using System.Numerics;
    using CowLibrary;
    using ObjLoader.Loader.Data.Elements;
    using ObjLoader.Loader.Data.VertexData;
    using ObjLoader.Loader.Loaders;

    public interface IModelToObjectConverter
    {
        Mesh Convert(LoadResult result);
    }

    public class ModelToMeshConverter : IModelToObjectConverter
    {
        public Mesh Convert(LoadResult result)
        {
            return ExtractMesh(result, result.Groups[0]);
        }

        private Mesh ExtractMesh(LoadResult result, Group group)
        {
            var triangles = new List<Triangle>();
            foreach (var face in group.Faces)
            {
                var (v0, v1, v2) = GetVertices(result, face);
                var t = new Triangle(v0, v1, v2);
                if (result.Normals.Count == 0)
                {
                    t.CalculateNormal();
                }
                else
                {
                    var (n0, n1, n2) = GetNormals(result, face);
                    t.SetNormal(n0, n1, n2);
                }
                triangles.Add(t);
            }
            return new OptimizedMesh(triangles);
        }

        private (Vector3 v0, Vector3 v1, Vector3 v2) GetVertices(LoadResult result, Face face)
        {
            var v0 = ToVector(result.Vertices[face[0].VertexIndex - 1]);
            var v1 = ToVector(result.Vertices[face[1].VertexIndex - 1]);
            var v2 = ToVector(result.Vertices[face[2].VertexIndex - 1]);
            return (v0, v1, v2);
        }

        private (Vector3 n0, Vector3 n1, Vector3 n2) GetNormals(LoadResult result, Face face)
        {
            var n0 = ToVector(result.Normals[face[0].NormalIndex - 1]);
            var n1 = ToVector(result.Normals[face[1].NormalIndex - 1]);
            var n2 = ToVector(result.Normals[face[2].NormalIndex - 1]);
            return (n0, n1, n2);
        }

        private Vector3 ToVector(Vertex v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        private Vector3 ToVector(Normal n)
        {
            return new Vector3(n.X, n.Y, n.Z);
        }
    }
}
