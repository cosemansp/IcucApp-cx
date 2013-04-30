using System;

namespace IcucApp.Core.Net
{
    public class CommunicationException : Exception
    {
        public CommunicationException(string message) : base(message)
        {
        }

        public CommunicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}