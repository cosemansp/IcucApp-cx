using System;

namespace IcucApp.Core.Configuration
{
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException(string message)
            : base(message)
        {
        }
        public InvalidConfigurationException(Exception exception)
            : base("Invalid configuration", exception)
        {
        }
    }
}