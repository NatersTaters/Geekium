#pragma checksum "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dec43476deac69e03da5dc0fd6a9cd9bdbe032e0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SellListings_Index), @"mvc.1.0.view", @"/Views/SellListings/Index.cshtml")]
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
#line 1 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\_ViewImports.cshtml"
using Geekium;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\_ViewImports.cshtml"
using Geekium.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dec43476deac69e03da5dc0fd6a9cd9bdbe032e0", @"/Views/SellListings/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"02898dc398b8f5c89a37ec564166a018d3526085", @"/Views/_ViewImports.cshtml")]
    public class Views_SellListings_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Geekium.Models.SellListing>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "FilterProducts", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("stretched-link"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Sell Listings</h1>\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dec43476deac69e03da5dc0fd6a9cd9bdbe032e05054", async() => {
                WriteLiteral("\r\n\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dec43476deac69e03da5dc0fd6a9cd9bdbe032e05320", async() => {
                    WriteLiteral("\r\n        <input id=\"searchBox\" type=\"text\" name=\"searchProduct\" placeholder=\"Search for Products\"");
                    BeginWriteAttribute("value", " value=\"", 279, "\"", 302, 1);
#nullable restore
#line 12 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
WriteAttributeValue("", 287, ViewBag.Search, 287, 15, false);

#line default
#line hidden
#nullable disable
                    EndWriteAttribute();
                    WriteLiteral(@" />
        <a class=""btn btn-primary"" data-toggle=""collapse"" href=""#filter"" role=""button"" aria-expanded=""false"" aria-controls=""filterSearch"">
            Add Filter
        </a>
        <button class=""btn btn-primary"" type=""submit"" title=""searchButton"">Search Item</button>

        <div");
                    BeginWriteAttribute("class", " class=\"", 597, "\"", 622, 1);
#nullable restore
#line 18 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
WriteAttributeValue("", 605, ViewBag.Collapse, 605, 17, false);

#line default
#line hidden
#nullable disable
                    EndWriteAttribute();
                    WriteLiteral(" id=\"filter\">\r\n            <div class=\"card card-body\">\r\n                <div class=\"form-group\">\r\n                    Price:\r\n                    <input style=\"width:25%;\" name=\"minPrice\" id=\"minPrice\" type=\"text\" placeholder=\"From\"");
                    BeginWriteAttribute("value", " value=\"", 856, "\"", 881, 1);
#nullable restore
#line 22 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
WriteAttributeValue("", 864, ViewBag.MinPrice, 864, 17, false);

#line default
#line hidden
#nullable disable
                    EndWriteAttribute();
                    WriteLiteral("/> -\r\n                    <input style=\"width:25%;\" name=\"maxPrice\" id=\"maxPrice\" type=\"text\" placeholder=\"To\"");
                    BeginWriteAttribute("value", " value=\"", 992, "\"", 1017, 1);
#nullable restore
#line 23 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
WriteAttributeValue("", 1000, ViewBag.MaxPrice, 1000, 17, false);

#line default
#line hidden
#nullable disable
                    EndWriteAttribute();
                    WriteLiteral(" />\r\n                </div>\r\n            </div>\r\n        </div>\r\n    ");
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
#line 31 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
               var count = 0;

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
               var items = 0;

#line default
#line hidden
#nullable disable
#nullable restore
#line 33 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
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
#line 38 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
                }


#line default
#line hidden
#nullable disable
                WriteLiteral("                <div class=\"col-md-4\">\r\n                    <div class=\"card\">\r\n                        <div class=\"card-block\">\r\n                            <div class=\"card-header\">\r\n                                ");
#nullable restore
#line 44 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
                           Write(Html.DisplayFor(modelItem => item.SellTitle));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                            </div>\r\n                            <div class=\"card-body\">\r\n                                <h5 class=\"card-title\">Seller: ");
#nullable restore
#line 47 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
                                                          Write(Html.DisplayFor(modelItem => item.Seller.Account.UserName));

#line default
#line hidden
#nullable disable
                WriteLiteral("</h5>\r\n                                <p class=\"card-text\">Price: ");
#nullable restore
#line 48 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
                                                       Write(Html.DisplayFor(modelItem => item.SellPrice));

#line default
#line hidden
#nullable disable
                WriteLiteral("</p>\r\n                                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "dec43476deac69e03da5dc0fd6a9cd9bdbe032e011930", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
                }
                BeginWriteTagHelperAttribute();
#nullable restore
#line 49 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
                                                                                 WriteLiteral(item.SellListingId);

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
                WriteLiteral("\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n");
#nullable restore
#line 54 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
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
#line 60 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
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
#line 67 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
             if (items == 0)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <div><br />\r\n                    <h2>Nothing matches the search/filter</h2>\r\n                </div>\r\n");
#nullable restore
#line 72 "C:\Users\antru\Documents\GitHub\Geekium\Geekium\Views\SellListings\Index.cshtml"
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
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Geekium.Models.SellListing>> Html { get; private set; }
    }
}
#pragma warning restore 1591
