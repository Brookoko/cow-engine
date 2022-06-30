namespace CowEngine.ImageWorker
{
    using System;
    using System.Linq;
    using Cowject;

    internal interface IImageEncoderProvider
    {
        IImageEncoder FindEncoder(string extension);
    }

    public class ImageEncoderProvider : IImageEncoderProvider
    {
        [Inject]
        public IImageEncoder[] Encoders { get; set; }

        public IImageEncoder FindEncoder(string extension)
        {
            var encoder = Encoders.FirstOrDefault(en => en.CanWorkWith(extension));
            if (encoder == null)
            {
                throw new Exception("Cannot encode image. Format is not supported");
            }
            return encoder;
        }
    }
}
