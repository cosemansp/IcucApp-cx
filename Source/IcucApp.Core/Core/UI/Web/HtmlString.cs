using System.Web;

namespace IcucApp.Core.UI.Web
{
    public interface IHtmlString
    {
        string ToHtmlString();
    }

    public class HtmlString : IHtmlString
    {
        private readonly string _source;

        public HtmlString(string source)
        {
            _source = source;
        }

        public string ToHtmlString()
        {
            return _source;
        }
    }
}