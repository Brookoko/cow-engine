namespace CowLibrary.Mathematics.Sampler;

public interface ISamplerProvider
{
    ISampler GetSampler();
}

public class SamplerProvider : ISamplerProvider
{
    private readonly ISampler sampler;

    public SamplerProvider()
    {
        sampler = new ThreadSafeSampler();
    }

    public ISampler GetSampler()
    {
        return sampler;
    }
}
