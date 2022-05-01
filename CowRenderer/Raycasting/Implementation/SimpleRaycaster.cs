namespace CowRenderer.Raycasting
{
    using System.Collections.Generic;
    using CowLibrary;

    public class SimpleRaycaster : IRaycaster
    {
        private List<RenderableObject> objects;

        public void Init(Scene scene)
        {
            objects = scene.objects;
        }

        public bool Raycast(Ray ray, out Surfel surfel)
        {
            Surfel closestSurfel = null;
            foreach (var renderableObject in objects)
            {
                if (renderableObject.Mesh.Intersect(ray, out var hitSurfel))
                {
                    if (hitSurfel.t > closestSurfel?.t)
                    {
                        continue;
                    }
                    hitSurfel.material = renderableObject.Material;
                    hitSurfel.ray = ray.direction;
                    closestSurfel = hitSurfel;
                }
            }

            if (closestSurfel == null)
            {
                surfel = new Surfel() { ray = ray.direction };
                return false;
            }
            surfel = closestSurfel;
            return true;
        }
    }
}
