namespace CowLibrary.Mathematics.Sampler;

public interface ISamplerProvider
{
    ISampler Sampler { get; }
}

public class SamplerProvider : ISamplerProvider
{
    public ISampler Sampler { get; }

    public SamplerProvider()
    {
        Sampler = new ThreadSafeSampler();
    }
}
