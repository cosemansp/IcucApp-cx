namespace IcucApp.Services.PushWoosh
{
    public class RegistrationMessage
    {
        public RegistrationMessage(string application, string deviceToken, string hwid)
        {
            request = new RegistrationPayload(application, deviceToken, hwid);
        }

        // ReSharper disable InconsistentNaming
        public RegistrationPayload request { get; set; }
        // ReSharper restore InconsistentNaming
    }
}