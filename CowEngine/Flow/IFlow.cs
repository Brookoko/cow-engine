namespace CowEngine
{
    public interface IFlow<in T> where T : Option
    {
        public int Process(T option);
    }
}
