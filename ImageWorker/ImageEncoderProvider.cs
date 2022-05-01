namespace CowEngine.ImageWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cowject;
    using Png;

    internal interface IImageEncoderProvider
    {
        IImageEncoder FindEncoder(string extension);
    }
    
    public class ImageEncoderProvider : IImageEncoderProvider
    {
        private readonly List<IImageEncoder> encoders = new()
        {
            new PngWorker()
        };
        
        public IImageEncoder FindEncoder(string extension)
        {
            var encoder = encoders.FirstOrDefault(en => en.CanWorkWith(extension));
            if (encoder == null)
            {
                throw new Exception("Cannot encode image. Format is not supported");
            }
            return encoder;
        }
    }
}