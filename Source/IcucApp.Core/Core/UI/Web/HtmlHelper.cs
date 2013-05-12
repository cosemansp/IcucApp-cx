using System.Web;

namespace IcucApp.Core.UI.Web
{
    public class HtmlHelper {

        public TemplateBase Template;

        public HtmlHelper(TemplateBase template) {
            Template= template;
        }

        public IHtmlString Raw(string text)
        {
            return new HtmlString(text);
        }
    }
}