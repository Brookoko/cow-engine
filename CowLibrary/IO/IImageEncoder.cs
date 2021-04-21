namespace CowEngine.ImageWorker
{
    using CowLibrary;
    
    public interface IImageEncoder
    {
        byte[] Encode(Image image);
        
        bool CanWorkWith(string extension);
    }
}