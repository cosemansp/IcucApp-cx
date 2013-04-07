namespace IcucApp.Core.Services.PushWoosh
{
    public class RegistrationPayload
    {
        public RegistrationPayload(string application, string deviceToken, string hwid)
        {
            push_token = deviceToken;
            this.application = application;
            this.hwid = hwid;
#if DROID
            device_type = 3;
#elif TOUCH
            device_type = 1;
#endif
            timezone = 3600;
            language = "nl";
        }

        public string application { get; private set; }
        public int device_type { get; private set; }
        public string push_token { get; private set; }
        public string language { get; private set; }
        public string hwid { get; private set; }
        public double timezone { get; private set; }
    }
}