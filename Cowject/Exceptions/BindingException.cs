namespace Cowject
{
    using System;

    internal class BindingException : Exception
    {
        public BindingException()
        {
        }

        public BindingException(string message) : base(message)
        {
        }
    }
}
