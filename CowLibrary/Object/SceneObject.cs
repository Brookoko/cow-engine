namespace CowLibrary
{
    public abstract class SceneObject
    {
        public int Id { get; init; }

        public Transform Transform { get; init; } = new();
    }
}
