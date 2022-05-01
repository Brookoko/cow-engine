namespace Cowject
{
    using System;

    internal class InjectionException : Exception
    {
        public InjectionException()
        {
        }

        public InjectionException(string message) : base(message)
        {
        }
    }
}
