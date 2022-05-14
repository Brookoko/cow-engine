namespace CowRenderer.Raycasting
{
    using CowLibrary;

    public class TreeRaycaster : IRaycaster
    {
        private SceneTree tree;

        public void Init(Scene scene)
        {
            tree = new SceneTree(scene.objects);
        }

        public bool Raycast(in Ray ray, out Surfel surfel)
        {
            var intersect = tree.Intersect(ray);
            if (intersect.HasValue)
            {
                surfel = intersect.Value;
                return true;
            }
            surfel = new Surfel() { ray = ray.direction };
            return false;
        }
    }
}
