namespace CowEngine
{
    public interface IFlow
    {
        public bool CanWorkWithProcess(Option option);

        public int Process(Option option);
    }
}
