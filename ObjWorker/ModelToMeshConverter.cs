namespace CowEngine
{
    using System.Numerics;
    using CowLibrary;
    using ObjLoader.Loader.Data.Elements;
    using ObjLoader.Loader.Data.VertexData;
    using ObjLoader.Loader.Loaders;

    public interface IModelToObjectConverter
    {
        IMesh Convert(LoadResult result, int id);
    }

    public class ModelToMeshConverter : IModelToObjectConverter
    {
        public IMesh Convert(LoadResult result, int id)
        {
            return ExtractMesh(result, result.Groups[0], id);
        }

        private IMesh ExtractMesh(LoadResult result, Group group, int id)
        {
            var triangles = new Triangle[group.Faces.Count];
            for (var i = 0; i < group.Faces.Count; i++)
            {
                var face = group.Faces[i];
                var (v0, v1, v2) = GetVertices(result, face);
                Vector3 n0;
                Vector3 n1;
                Vector3 n2;
                if (result.Normals.Count == 0)
                {
                    (n0, n1, n2) = CalculateNormal(v0, v1, v2);
                }
                else
                {
                    (n0, n1, n2) = GetNormals(result, face);
                }
                var t = new Triangle(v0, v1, v2, n0, n1, n2, id);
                triangles[i] = t;
            }
            return new OptimizedMesh(triangles, id);
        }

        public (Vector3 n0, Vector3 n1, Vector3 n2) CalculateNormal(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            var v0v1 = v1 - v0;
            var v0v2 = v2 - v0;
            var n = Vector3.Cross(v0v2, v0v1);
            return (n, n, n);
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
