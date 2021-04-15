namespace CowRenderer.Raycasting.Impl
{
    using CowLibrary;

    public class SimpleRaycaster : IRaycaster
    {
        public bool Raycast(Scene scene, Ray ray, out Surfel surfel)
        {
            Surfel closestSurfel = null;
            foreach (var renderableObject in scene.objects)
            {
                if (renderableObject.mesh.Intersect(ray, out var hitSurfel))
                {
                    if (hitSurfel.t > closestSurfel?.t)
                    {
                        continue;
                    }
                    closestSurfel = hitSurfel;
                }
            }

            surfel = closestSurfel;
            return closestSurfel != null;
        }
    }
}