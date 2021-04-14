namespace CowRenderer.Raycasting.Impl
{
    using CowLibrary;

    public class SimpleRaycaster : IRaycaster
    {
        public bool Raycast(Scene scene, Ray ray, out Surfel surfel)
        {
            var closestSurfelDistanceSqr = float.PositiveInfinity;
            Surfel closestSurfel = null;
            foreach (var renderableObject in scene.objects)
            {
                if (renderableObject.mesh.Intersect(ray, out var hitSurfel))
                {
                    var distanceSqr = (hitSurfel.point - ray.origin).LengthSquared();
                    if (distanceSqr < closestSurfelDistanceSqr)
                    {
                        closestSurfel = hitSurfel;
                        closestSurfelDistanceSqr = distanceSqr;
                    }
                }
            }

            surfel = closestSurfel;
            return closestSurfel != null;
        }
    }
}