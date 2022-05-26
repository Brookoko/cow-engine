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
            var bestHit = Const.Miss;
            foreach (var obj in objects)
            {
                obj.Mesh.Intersect(in ray, ref bestHit);
            }
            if (bestHit.HasHit)
            {
                return new Surfel(bestHit, ray.direction, objects[bestHit.id].Material);
            }
            return new Surfel(ray.direction);
        }
    }
}
