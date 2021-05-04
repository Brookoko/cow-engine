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
            return tree.Intersect(ray, out surfel);
        }
    }
}