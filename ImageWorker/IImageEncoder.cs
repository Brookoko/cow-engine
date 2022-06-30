namespace CowEngine.ImageWorker
{
    using CowLibrary;

    public interface IImageEncoder
    {
        byte[] Encode(in Image image);

        bool CanWorkWith(string extension);
    }
}
