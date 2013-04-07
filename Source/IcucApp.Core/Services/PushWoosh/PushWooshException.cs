using System;
using System.Collections.Generic;

namespace IcucApp.Core.Services.PushWoosh
{
    public class PushWooshException : Exception
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public PushWooshException(string message, string statusCode, string statusMessage) 
            : base(message)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }
    }

    public class PushWooshSetTagException : Exception
    {
        public List<TagError> TagErrors { get; set; }

        public PushWooshSetTagException(string message, List<TagError> tagErrors)
            : base(message)
        {
            TagErrors = tagErrors;
        }
    }
}