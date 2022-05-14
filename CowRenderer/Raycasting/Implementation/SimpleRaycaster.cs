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

        public bool Raycast(in Ray ray, out Surfel surfel)
        {
            Surfel? closestSurfel = null;
            foreach (var renderableObject in objects)
            {
                var hitSurfel = renderableObject.Mesh.Intersect(in ray);
                if (hitSurfel.HasValue)
                {
                    if (hitSurfel.Value.t > closestSurfel?.t)
                    {
                        continue;
                    }
                    closestSurfel = hitSurfel.Value with { material = renderableObject.Material, ray = ray.direction };
                }
            }

            if (!closestSurfel.HasValue)
            {
                surfel = new Surfel() { ray = ray.direction };
                return false;
            }
            surfel = closestSurfel.Value;
            return true;
        }
    }
}
