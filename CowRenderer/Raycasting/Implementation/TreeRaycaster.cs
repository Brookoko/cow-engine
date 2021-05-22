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
        
        public bool Raycast(Ray ray, out Surfel surfel)
        {
            if (tree.Intersect(ray, out surfel))
            {
                return true;
            }
            surfel = new Surfel() {ray = ray.direction};
            return false;
        }
    }
}