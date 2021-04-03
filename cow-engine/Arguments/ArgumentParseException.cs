namespace CowEngine
{
    using System;

    public class ArgumentParseException : Exception
    {
        public ArgumentParseException(string message) : base(message)
        {
        }
    }
}