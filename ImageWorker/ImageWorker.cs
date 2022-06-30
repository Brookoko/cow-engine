namespace CowEngine.ImageWorker
{
    using Cowject;
    using CowLibrary;

    public interface IImageWorker
    {
        void SaveImage(in Image image, string extension);
    }

    public class ImageWorker : IImageWorker
    {
        [Inject]
        internal IIoWorker IoWorker { get; set; }

        [Inject]
        internal IImageEncoderProvider ImageEncoderProvider { get; set; }

        public void SaveImage(in Image image, string path)
        {
            var extension = path.GetExtension();
            var encoder = ImageEncoderProvider.FindEncoder(extension);
            var bytes = encoder.Encode(in image);
            IoWorker.Write(bytes, path);
        }
    }
}
