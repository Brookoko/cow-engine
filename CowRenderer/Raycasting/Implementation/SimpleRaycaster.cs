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
                if (renderableObject.mesh.Intersect(ray, out var hitSurfel))
                {
                    if (hitSurfel.t > closestSurfel?.t)
                    {
                        continue;
                    }
                    hitSurfel.material = renderableObject.material;
                    hitSurfel.ray = ray.direction;
                    closestSurfel = hitSurfel;
                }
            }

            surfel = closestSurfel;
            return closestSurfel != null;
        }
    }
}