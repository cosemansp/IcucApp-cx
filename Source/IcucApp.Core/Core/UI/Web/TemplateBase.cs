using System;
using System.Web;

namespace IcucApp.Core.UI.Web
{
    public abstract class TemplateBase
    {

        // This field is OPTIONAL, but used by the default implementation of Generate, Write and WriteLiteral
        //
        System.IO.TextWriter _razorWriter;

        // This method is OPTIONAL
        //
        ///<summary>Executes the template and returns the output as a string.</summary>
        public string GenerateString()
        {
            using (var sw = new System.IO.StringWriter())
            {
                Generate(sw);
                return sw.ToString();
            }
        }

        // This method is OPTIONAL, you may choose to implement Write and WriteLiteral without use of _razorWriter
        // and provide another means of invoking Execute.
        //
        ///<summary>Executes the template, writing to the provided text writer.</summary>
        public void Generate(System.IO.TextWriter writer)
        {
            _razorWriter = writer;
            Execute();
            _razorWriter = null;
        }

        // This method is REQUIRED, but you may choose to implement it differently
        //
        ///<summary>Writes literal values to the template output without HTML escaping them.</summary>
        public void WriteLiteral(string value)
        {
            _razorWriter.Write(value);
        }

        // This method is REQUIRED, but you may choose to implement it differently
        //
        ///<summary>Writes values to the template output, HTML escaping them if necessary.</summary>
        protected void Write(object value)
        {
            WriteTo(_razorWriter, value);
        }

        // This method is REQUIRED if the template uses any Razor helpers, but you may choose to implement it differently
        //
        ///<summary>Invokes the action to write directly to the template output.</summary>
        ///<remarks>This is used for Razor helpers, which already perform any necessary HTML escaping.</remarks>
        protected void Write(Action<System.IO.TextWriter> write)
        {
            write(_razorWriter);
        }

        // This method is REQUIRED if the template has any Razor helpers, but you may choose to implement it differently
        //
        ///<remarks>Used by Razor helpers to HTML escape values.</remarks>
        protected static void WriteTo(System.IO.TextWriter writer, object value)
        {
            if (value != null)
            {
                var s = value as IcucApp.Core.UI.Web.IHtmlString;
                if (s != null)
                {
                    writer.Write(s.ToHtmlString());
                    return;
                }

                writer.Write(HttpUtility.HtmlEncode(value.ToString()));
            }
        }

        // This method is REQUIRED. The generated Razor subclass will override it with the generated code.
        //
        ///<summary>Executes the template, writing output to the Write and WriteLiteral methods.</summary>.
        ///<remarks>Not intended to be called directly. Call the Generate method instead.</remarks>
        public abstract void Execute();

		public HtmlHelper Html {
			get {
				return new HtmlHelper(this);
			}
		}
    }
}