#pragma checksum "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\UserGroup\AddEditModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d91fdeeebc025ca52ad38adc8fe815656c1edd8a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_SYS_Views_UserGroup_AddEditModal), @"mvc.1.0.view", @"/Areas/SYS/Views/UserGroup/AddEditModal.cshtml")]
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
#line 4 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\_ViewImports.cshtml"
using Ligg.Infrastructure.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\_ViewImports.cshtml"
using Ligg.Infrastructure.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\_ViewImports.cshtml"
using Ligg.Infrastructure.Utilities.DataParserUtil;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\_ViewImports.cshtml"
using Ligg.EntityFramework.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\_ViewImports.cshtml"
using Ligg.Uwa.Application;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\_ViewImports.cshtml"
using Ligg.Uwa.Application.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\_ViewImports.cshtml"
using Ligg.Uwa.Mvc.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\_ViewImports.cshtml"
using Ligg.Uwa.Basis.SYS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\_ViewImports.cshtml"
using Ligg.Uwa.Mvc.Controllers.SYS;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d91fdeeebc025ca52ad38adc8fe815656c1edd8a", @"/Areas/SYS/Views/UserGroup/AddEditModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6cc5d010ab9bfc05a4b62199a196885be56ed38e", @"/Areas/SYS/Views/_ViewImports.cshtml")]
    public class Areas_SYS_Views_UserGroup_AddEditModal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-horizontal m"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\UserGroup\AddEditModal.cshtml"
   Layout = "~/Views/Shared/_ModalForm.cshtml";
    string controller = ViewBag.Controller; 

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"wrapper animated fadeInRight\">\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d91fdeeebc025ca52ad38adc8fe815656c1edd8a6135", async() => {
                WriteLiteral(@"
        <input id=""type"" col=""Type"" type=""text"" class=""hidden"" />
        <div class=""form-group"">
            <label class=""col-sm-3 control-label "">名称<font class=""red""> *</font></label>
            <div class=""col-sm-8"">
                <input id=""name"" col=""Name"" type=""text"" class=""form-control"" />
            </div>
        </div>
        <div class=""form-group"">
            <label class=""col-sm-3 control-label "">排序码</label>
            <div class=""col-sm-8"">
                <input id=""sequence"" col=""Sequence"" type=""text"" class=""form-control"" />
            </div>
        </div>
        <div class=""form-group"">
            <label class=""col-sm-3 control-label "">状态</label>
            <div class=""col-sm-8"" id=""status"" col=""Status"">
            </div>
        </div>
        <div class=""form-group"">
            <label class=""col-sm-3 control-label "">描述</label>
            <div class=""col-sm-8"">
                <textarea id=""description"" col=""Description"" class=""form-control"" style=""hei");
                WriteLiteral("ght:68px\"></textarea>\r\n            </div>\r\n        </div>\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
</div>

<script type=""text/javascript"">
    var id = ys.request(""id"");
    var type = ys.request(""type"");
     var act = 'edit';
    if (id == '' | id == null) act = 'add';


    $(function () {
        $(""#status"").ysRadioBox({ data: ys.getJson(");
#nullable restore
#line 41 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\UserGroup\AddEditModal.cshtml"
                                              Write(Html.Raw(typeof(StatusType).EnumToDictionaryJson()));

#line default
#line hidden
#nullable disable
            WriteLiteral(@") });
        getForm();

        $(""#form"").validate({
            rules: {
                name: { required: true, maxlength: 63 },
                sequence: { digits: true },
            }
        });
    });

    function getForm() {
        if (act == 'add') {//add
            ys.ajax({
                url: '");
#nullable restore
#line 55 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\UserGroup\AddEditModal.cshtml"
                 Write(Url.Content("~/Sys/" + "UserGroup" + "/GetMaxSequenceNoJson"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"'+'?type='+type,
                type: ""get"",
                success: function (rst) {
                    if (rst.Flag == 1) {
                        var defaultData = {};
                        defaultData.Sequence = rst.Data;
                        defaultData.Type = type;
                        defaultData.Status = """);
#nullable restore
#line 62 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\UserGroup\AddEditModal.cshtml"
                                          Write((int)StatusType.Enabled);

#line default
#line hidden
#nullable disable
            WriteLiteral(@""";
                        $(""#form"").setWebControls(defaultData);
                    }
                    else ys.msgError(rst.Message);
                }
            });
        }
        else {//edit
            ys.ajax({
                url: '");
#nullable restore
#line 71 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\UserGroup\AddEditModal.cshtml"
                 Write(Url.Content("~/Sys/"+ "UserGroup" + "/GetEditDtoJson"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?id=' + id,
                type: ""get"",
                success: function (rst) {
                    if (rst.Flag == 1) {
                        //rst.Data.Type = type;
                        $(""#form"").setWebControls(rst.Data);
                    }
                    else ys.msgError(rst.Message);
                }
            });
        }

    }

    function saveForm(index) {
        if (!ys.checkTextLenth(""description"", ""描述"", 0, 127)) return;

        if ($(""#form"").validate().form()) {
            var postData = $(""#form"").getWebControls({ Id: id });
            //postData.MenuItemIds = $('#menuTree').ysTree(""getCheckedNodes"");
            ys.ajax({
                url: '");
#nullable restore
#line 92 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\UserGroup\AddEditModal.cshtml"
                 Write(Url.Content("~/Sys/" + "UserGroup" + "/"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + act,
                type: ""post"",
                data: postData,
                success: function (rst) {
                    if (rst.Flag == 1) {
                        ys.msgSuccess(rst.Message);
                        parent.searchGrid();
                        parent.layer.close(index);
                    }
                    else {
                        ys.msgError(rst.Message);
                    }
                }
            });
        }
    }


</script>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591