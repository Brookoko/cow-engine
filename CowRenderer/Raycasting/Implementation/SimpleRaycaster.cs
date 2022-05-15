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

        public Surfel Raycast(in Ray ray)
        {
            var bestHit = new RayHit();
            var hitIndex = 0;
            for (var i = 0; i < objects.Count; i++)
            {
                var obj = objects[i];
                var hit = obj.Mesh.Intersect(in ray);
                if (bestHit.t > hit.t)
                {
                    bestHit = hit;
                    hitIndex = i;
                }
            }
            return new Surfel()
            {
                hit = bestHit,
                material = objects[hitIndex].Material,
                ray = ray.direction
            };
        }
    }
}
