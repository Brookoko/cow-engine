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

        public Surfel Raycast(in Ray ray)
        {
            var intersect = tree.Intersect(ray);
            return intersect ?? new Surfel(ray.direction);
        }
    }
}
