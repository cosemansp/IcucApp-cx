#pragma warning disable 1591
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace IcucApp
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#line 1 "/Users/peter/git/IcucApp-cx/Source/IcucApp.Touch/ViewControllers/FacebookDetailTemplate.cshtml"
using IcucApp.Presentation.ViewModels;

#line default
#line hidden


[System.CodeDom.Compiler.GeneratedCodeAttribute("RazorTemplatePreprocessor", "2.6.0.0")]
public partial class FacebookDetailTemplate : IcucApp.Core.UI.Web.TemplateBase
{

#line hidden

#line 2 "/Users/peter/git/IcucApp-cx/Source/IcucApp.Touch/ViewControllers/FacebookDetailTemplate.cshtml"
public FacebookDetailModel Model { get; set; }

#line default
#line hidden

public override void Execute()
{
WriteLiteral("<html>\n<style");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(">\n  body {\t\n    // background-color: #fff; \n    color:#333333; \n    font-family: " +
"\"Helvetica Neue\", Helvetica; \n  }\n  h1 {\n\tfont-size: 20;\n\tfont-weight: bold;\n  }" +
"\n  h2 {\n\tfont-size: 15;\n\tfont-weight: bold;\n  }\n</style>\n<body> \n\t<H1>");


#line 21 "/Users/peter/git/IcucApp-cx/Source/IcucApp.Touch/ViewControllers/FacebookDetailTemplate.cshtml"
Write(Model.Data.Title);


#line default
#line hidden
WriteLiteral("</H1>\n\t<p>\n");

WriteLiteral("\t\t");


#line 23 "/Users/peter/git/IcucApp-cx/Source/IcucApp.Touch/ViewControllers/FacebookDetailTemplate.cshtml"
Write(Html.Raw(Model.Data.Content));


#line default
#line hidden
WriteLiteral("\n\t</p>\n</body>\n</html>");

}
}
}
#pragma warning restore 1591
