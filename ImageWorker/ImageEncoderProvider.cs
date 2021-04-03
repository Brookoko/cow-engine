namespace CowEngine.ImageWorker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cowject;

    internal interface IImageEncoderProvider
    {
        IImageEncoder FindEncoder(string extension);
    }
    
    public class ImageEncoderProvider : IImageEncoderProvider
    {
        [Inject]
        internal IPluginProvider PluginProvider { get; set; }
        
        private List<IImageEncoder> encoders;
        
        [PostConstruct]
        public void Prepare()
        {
            encoders = PluginProvider.Types
                .Where(t => typeof(IImageEncoder).IsAssignableFrom(t) && !t.IsInterface)
                .Select(t => (IImageEncoder) Activator.CreateInstance(t))
                .ToList();
        }
        
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