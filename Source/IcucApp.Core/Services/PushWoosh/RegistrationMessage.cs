namespace IcucApp.Core.Services.PushWoosh
{
    public class RegistrationMessage
    {
        public RegistrationMessage(string application, string deviceToken, string hwid)
        {
            request = new RegistrationPayload(application, deviceToken, hwid);
        }

        public RegistrationPayload request { get; set; }
    }
}