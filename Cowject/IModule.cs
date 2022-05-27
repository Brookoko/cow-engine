namespace Cowject
{
    public interface IModule
    {
        Priority Priority => Priority.Normal;

        void Prepare(DiContainer container);
    }

    public enum Priority
    {
        Highest,
        High,
        Normal,
        Low
    }
}
