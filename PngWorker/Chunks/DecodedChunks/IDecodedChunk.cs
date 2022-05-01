namespace CowEngine.ImageWorker.Png
{
    internal interface IDecodedChunk
    {
        void Init(Chunk[] chunks);

        bool IsCompatible(Chunk[] chunks);

        Chunk ToChunk();
    }
}
