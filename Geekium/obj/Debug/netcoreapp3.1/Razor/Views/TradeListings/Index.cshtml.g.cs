#pragma checksum "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ba316aaf3ec4b75d1eaa38c21da0046fed78c128"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_TradeListings_Index), @"mvc.1.0.view", @"/Views/TradeListings/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\_ViewImports.cshtml"
using Geekium;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\_ViewImports.cshtml"
using Geekium.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ba316aaf3ec4b75d1eaa38c21da0046fed78c128", @"/Views/TradeListings/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"02898dc398b8f5c89a37ec564166a018d3526085", @"/Views/_ViewImports.cshtml")]
    public class Views_TradeListings_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Geekium.Models.TradeListing>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "FilterTrades", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("displayImage"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/images/buy-icon.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("stretched-link"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ba316aaf3ec4b75d1eaa38c21da0046fed78c1285934", async() => {
                WriteLiteral("\r\n    <h2>Trade Listings</h2>\r\n\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ba316aaf3ec4b75d1eaa38c21da0046fed78c1286231", async() => {
                    WriteLiteral("\r\n        <div class=\"form-inline\">\r\n            <input class=\"form-control mr-sm-2\" id=\"searchBox\" type=\"text\" name=\"searchTrade\" placeholder=\"Search for Trades\"");
                    BeginWriteAttribute("value", " value=\"", 345, "\"", 368, 1);
#nullable restore
#line 12 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
WriteAttributeValue("", 353, ViewBag.Search, 353, 15, false);

#line default
#line hidden
#nullable disable
                    EndWriteAttribute();
                    WriteLiteral(@" />
            <a class=""btn btn-outline-secondary my-2 my-sm-0"" data-toggle=""collapse"" href=""#filter"" role=""button"" aria-expanded=""false"" aria-controls=""filterSearch"">
                Add Filter
            </a>&nbsp;
            <button class=""btn btn-outline-success my-2 my-sm-0"" type=""submit"" title=""searchButton"">Search Trade</button>
        </div>
        <div");
                    BeginWriteAttribute("class", " class=\"", 744, "\"", 781, 2);
#nullable restore
#line 18 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
WriteAttributeValue("", 752, ViewBag.Collapse, 752, 17, false);

#line default
#line hidden
#nullable disable
                    WriteAttributeValue(" ", 769, "form-inline", 770, 12, true);
                    EndWriteAttribute();
                    WriteLiteral(" id=\"filter\">\r\n            <div class=\"card card-body\">\r\n                <div class=\"form-group form-inline\">\r\n                    TBD\r\n                </div>\r\n            </div>\r\n        </div>\r\n    ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n    <div class=\"py-2\">\r\n        <div class=\"container\">\r\n");
#nullable restore
#line 29 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
               var count = 0;

#line default
#line hidden
#nullable disable
#nullable restore
#line 30 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
               var items = 0;

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
             foreach (var item in Model)
            {
                if (count == 0)
                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral("<div class=\"row\">\r\n");
#nullable restore
#line 36 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <div class=\"col-md-4\">\r\n                        <div class=\"card\">\r\n                            <div class=\"card-block\">\r\n                                <div class=\"card-header truncate\">\r\n                                    ");
#nullable restore
#line 41 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
                               Write(Html.DisplayFor(modelItem => item.TradeTitle));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                </div>\r\n                                <div class=\"card-body\">\r\n                                    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "ba316aaf3ec4b75d1eaa38c21da0046fed78c12811402", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                                    <h5 class=\"card-title\">Seller: ");
#nullable restore
#line 45 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
                                                              Write(Html.DisplayFor(modelItem => item.Seller.Account.UserName));

#line default
#line hidden
#nullable disable
                WriteLiteral("</h5>\r\n                                    <p class=\"card-text\">Trade For: ");
#nullable restore
#line 46 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
                                                               Write(Html.DisplayFor(modelItem => item.TradeFor));

#line default
#line hidden
#nullable disable
                WriteLiteral("</p>\r\n                                    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ba316aaf3ec4b75d1eaa38c21da0046fed78c12813426", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_5.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
                }
                BeginWriteTagHelperAttribute();
#nullable restore
#line 47 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
                                                                                     WriteLiteral(item.TradeListingId);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n");
#nullable restore
#line 52 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
                    items++;

                    if (count == 2)
                    {
                        count = 0;

#line default
#line hidden
#nullable disable
                WriteLiteral("                    ");
                WriteLiteral("</div><br />\r\n");
#nullable restore
#line 58 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
                }
                else
                {
                    count++;
                }
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n");
#nullable restore
#line 65 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
             if (items == 0)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <div>\r\n                    <br />\r\n                    <h2>Nothing matches the search/filter</h2>\r\n                </div>\r\n");
#nullable restore
#line 71 "C:\Users\leuna\OneDrive\Documents\GitHub\Geekium\Geekium\Views\TradeListings\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
                WriteLiteral("        </div>\r\n    </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Geekium.Models.TradeListing>> Html { get; private set; }
    }
}
#pragma warning restore 1591
