#pragma checksum "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "41bdf7364386acb7cc738db56201b4f5c94627d7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_SysPagePartial), @"mvc.1.0.view", @"/Views/Shared/SysPagePartial.cshtml")]
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
#line 4 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.Infrastructure.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.Infrastructure.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.Infrastructure.Utilities.DataParserUtil;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.EntityFramework.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.Uwa.Application;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.Uwa.Application.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.Uwa.Basis.SYS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.Uwa.Basis.SCC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.Uwa.Mvc.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Ligg.Uwa.Mvc.ViewComponents;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\_ViewImports.cshtml"
using Humanizer;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"41bdf7364386acb7cc738db56201b4f5c94627d7", @"/Views/Shared/SysPagePartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"353fc76568021bcef0b679f35d70c936ed16d2f8", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_SysPagePartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("<link rel=\"stylesheet\"");
            BeginWriteAttribute("href", " href=\'", 101, "\'", 167, 1);
#nullable restore
#line 3 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
WriteAttributeValue("", 108, Url.Content("~/lib/bootstrap/3.3.7/css/bootstrap.min.css"), 108, 59, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n<link rel=\"stylesheet\"");
            BeginWriteAttribute("href", " href=\'", 193, "\'", 263, 1);
#nullable restore
#line 4 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
WriteAttributeValue("", 200, Url.Content("~/lib/fontawesome/4.7.0/css/fontawesome.min.css"), 200, 63, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n<link rel=\"stylesheet\"");
            BeginWriteAttribute("href", " href=\'", 289, "\'", 351, 1);
#nullable restore
#line 5 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
WriteAttributeValue("", 296, Url.Content("~/lib/select2/4.0.6/css/select2.min.css"), 296, 55, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n");
#nullable restore
#line 6 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/app/admin/css/admin.min.css")));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 7 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/lib/yisha/3.1/css/yisha.min.css")));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<script type=\"text/javascript\"");
            BeginWriteAttribute("src", " src=\'", 601, "\'", 655, 1);
#nullable restore
#line 9 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
WriteAttributeValue("", 607, Url.Content("~/lib/jquery/2.1.4/jquery.min.js"), 607, 48, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></script>\r\n<script type=\"text/javascript\"");
            BeginWriteAttribute("src", " src=\'", 698, "\'", 761, 1);
#nullable restore
#line 10 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
WriteAttributeValue("", 704, Url.Content("~/lib/bootstrap/3.3.7/js/bootstrap.min.js"), 704, 57, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></script>\r\n<script type=\"text/javascript\"");
            BeginWriteAttribute("src", " src=\'", 804, "\'", 856, 1);
#nullable restore
#line 11 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
WriteAttributeValue("", 810, Url.Content("~/lib/layer/3.1.1/layer.min.js"), 810, 46, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></script>\r\n<script type=\"text/javascript\"");
            BeginWriteAttribute("src", " src=\'", 899, "\'", 955, 1);
#nullable restore
#line 12 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
WriteAttributeValue("", 905, Url.Content("~/lib/laydate/5.0.9/laydate.min.js"), 905, 50, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></script>\r\n<script type=\"text/javascript\"");
            BeginWriteAttribute("src", " src=\'", 998, "\'", 1071, 1);
#nullable restore
#line 13 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
WriteAttributeValue("", 1004, Url.Content("~/lib/bootstrap.table/1.12.0/bootstrap-table.min.js"), 1004, 67, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></script>\r\n<script type=\"text/javascript\"");
            BeginWriteAttribute("src", " src=\'", 1114, "\'", 1173, 1);
#nullable restore
#line 14 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
WriteAttributeValue("", 1120, Url.Content("~/lib/select2/4.0.6/js/select2.min.js"), 1120, 53, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></script>\r\n");
#nullable restore
#line 15 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Views\Shared\SysPagePartial.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/lib/yisha/3.1/js/yisha.min.js")));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public Microsoft.AspNetCore.Hosting.IWebHostEnvironment HostingEnvironment { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
