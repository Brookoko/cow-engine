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
            var bestHit = new RayHit() { t = float.MaxValue };
            var intersected = false;
            var hitIndex = 0;
            for (var i = 0; i < objects.Count; i++)
            {
                var obj = objects[i];
                var hit = obj.Mesh.Intersect(in ray);
                if (!hit.HasValue)
                {
                    continue;
                }
                if (!intersected || bestHit.t > hit.Value.t)
                {
                    bestHit = hit.Value;
                    intersected = true;
                    hitIndex = i;
                }
            }
            return new Surfel()
            {
                hasHit = intersected,
                hit = bestHit,
                material = objects[hitIndex].Material,
                ray = ray.direction
            };
        }
    }
}
