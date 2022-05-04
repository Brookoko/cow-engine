namespace CowLibrary
{
    public abstract class SceneObject
    {
        public int Id { get; set; }

        public Transform Transform { get; set; } = new Transform();
    }
}
