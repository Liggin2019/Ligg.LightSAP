#pragma checksum "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "95c74dbf44183d67f254c539aae7ad264b3111a7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_SCC_Views_Article_Manage), @"mvc.1.0.view", @"/Areas/SCC/Views/Article/Manage.cshtml")]
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
#line 4 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Infrastructure.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Infrastructure.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Infrastructure.Utilities.DataParserUtil;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.EntityFramework.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Uwa.Application;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Uwa.Application.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Uwa.Basis.SCC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 13 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Uwa.Basis.SYS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Uwa.Mvc.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Uwa.Mvc.Controllers.SCC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 16 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Uwa.Mvc.Controllers.SYS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 17 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\_ViewImports.cshtml"
using Ligg.Uwa.Mvc.ViewComponents;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"95c74dbf44183d67f254c539aae7ad264b3111a7", @"/Areas/SCC/Views/Article/Manage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0bab4289abcdfc4ff87fd458561230788dfde105", @"/Areas/SCC/Views/_ViewImports.cshtml")]
    public class Areas_SCC_Views_Article_Manage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
  
    Layout = "~/Views/Shared/_SysPage.cshtml";
    string masterType = ViewBag.MasterType;
    string masterTypeName = ViewBag.MasterTypeName;
    string topMastersJsonStr = ViewBag.TopMastersJsonString;


#line default
#line hidden
#nullable disable
            DefineSection("header", async() => {
                WriteLiteral("\r\n    ");
#nullable restore
#line 10 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/lib/zTree/v3/css/metroStyle/metroStyle.min.css")));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n    ");
#nullable restore
#line 11 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/lib/zTree/v3/js/ztree.min.js")));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n    ");
#nullable restore
#line 13 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/lib/jquery.layout/1.4.4/jquery.layout-latest.min.css")));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n    ");
#nullable restore
#line 14 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/lib/jquery.layout/1.4.4/jquery.layout-latest.min.js")));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n");
            }
            );
            WriteLiteral(@"
<div class=""ui-layout-west"">
    <div class=""main-content"">
        <div class=""box box-main"">
            <div style=""padding-left:1px"">
                <span id=""top-master"" />
            </div>
            <div class=""box-header"" style=""padding: 12px 10px 0px 10px;"">
                <div class=""box-title"" style=""font-size:14px"">
                    ");
#nullable restore
#line 25 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
               Write(masterTypeName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n                <div class=\"box-tools pull-right\">\r\n                    <a type=\"button\" class=\"btn btn-box-tool menuItem\" href=\"#\" onclick=\"goToManageDirectory()\"");
            BeginWriteAttribute("title", " title=\"", 1391, "\"", 1418, 2);
            WriteAttributeValue("", 1399, "管理", 1399, 2, true);
#nullable restore
#line 28 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
WriteAttributeValue("", 1401, masterTypeName, 1401, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@"><i class=""fa fa-edit""></i></a>
                    <button type=""button"" class=""btn btn-box-tool"" id=""btnExpandTree"" title=""展开"" style=""display:none;""><i class=""fa fa-chevron-up""></i></button>
                    <button type=""button"" class=""btn btn-box-tool"" id=""btnCollapseTree"" title=""折叠""><i class=""fa fa-chevron-down""></i></button>
                    <button type=""button"" class=""btn btn-box-tool"" id=""btnRefreshMaster""");
            BeginWriteAttribute("title", " title=\"", 1846, "\"", 1873, 2);
            WriteAttributeValue("", 1854, "刷新", 1854, 2, true);
#nullable restore
#line 31 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
WriteAttributeValue("", 1856, masterTypeName, 1856, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@"><i class=""fa fa-refresh""></i></button>
                </div>
            </div>
            <div class=""ui-layout-content"">
                <div id=""masterTree"" class=""ztree""></div>
            </div>
        </div>
    </div>
</div>

<div class=""container-div ui-layout-center"">
    <div class=""row"">
        <div id=""searchDiv"" class=""col-sm-12 search-collapse"">
            <input type=""hidden"" id=""masterId"" col=""MasterId"">
            <div class=""select-list"">
                <ul>
                    <li>
                        <label style=""margin-top:4px"">多层搜索：</label>
                    </li>
                    <li style=""width:60px"">
                        <span id=""recursiveSearch"" col=""RecursiveSearch""></span>
                    </li>
                    <li>
                        类别：<span id=""type"" col=""Type"" />
                    </li>
                    <li>
                        状态：<span id=""status"" col=""Status""></span>
                    </li>
            ");
            WriteLiteral(@"        <li>
                        文本：
                        <input id=""text"" col=""Text"" type=""text"" />
                    </li>
                    <li class=""select-time"">
                        <label>创建时间： </label>
                        <input id=""startCreationTime"" col=""StartCreationTime"" type=""text"" class=""time-input"" placeholder=""开始时间"" />
                        <span>-</span>
                        <input id=""endCreationTime"" col=""EndCreationTime"" type=""text"" class=""time-input"" placeholder=""结束时间"" />
                    </li>
                    <li>
                        <a id=""btnSearch"" class=""btn btn-primary btn-sm"" onclick=""searchGrid()""><i class=""fa fa-search""></i>&nbsp;搜索</a>
                    </li>
                </ul>
            </div>
        </div>

        <div id=""toolbar"" class=""btn-group-sm"">
            <a id=""btnAdd"" class=""btn btn-success"" onclick=""showAddEditDialog(true)""><i class=""fa fa-plus""></i> 新增</a>
            <a id=""btnEdit"" class=""btn btn-pr");
            WriteLiteral(@"imary disabled"" onclick=""showAddEditDialog(false)""><i class=""fa fa-edit""></i> 修改</a>
            <a id=""btnEditBody"" class=""btn btn-primary disabled"" onclick=""showEditBodyDialog(false)""><i class=""fa fa-edit""></i> 编辑正文</a>
            <a id=""btnDelete"" class=""btn btn-danger disabled"" onclick=""deleteSelected()""><i class=""fa fa-remove""></i> 删除</a>
            <a id=""btnUploadSquareThumbnail"" class=""btn btn-primary disabled"" onclick=""showUploadSquareThumbnailDialog()""><i class=""fa fa-upload""></i> 上传缩略图</a>
        </div>

        <div class=""col-sm-12 select-table table-striped"">
            <table id=""gridTable"" data-mobile-responsive=""true""></table>
        </div>
    </div>
</div>

<script type=""text/javascript"">
    $(function () {
        $(""#top-master"").ysComboBox({ onChange: onTopMasterChange, class: 'no-additional-option', data: ys.getJson(");
#nullable restore
#line 92 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                                                              Write(Html.Raw(topMastersJsonStr));

#line default
#line hidden
#nullable disable
            WriteLiteral(") });\r\n        $(\"#status\").ysComboBox({ data: ys.getJson(");
#nullable restore
#line 93 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                              Write(Html.Raw(typeof(StatusType).EnumToDictionaryJson()));

#line default
#line hidden
#nullable disable
            WriteLiteral(") });\r\n        $(\"#type\").ysComboBox({ data: ys.getJson(");
#nullable restore
#line 94 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                            Write(Html.Raw(typeof(ArticleType).EnumToDictionaryJson()));

#line default
#line hidden
#nullable disable
            WriteLiteral(") });\r\n        $(\"#recursiveSearch\").ysComboBox({class: \'no-additional-option\', data: ys.getJson(");
#nullable restore
#line 95 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                                     Write(Html.Raw(typeof(YesOrNo).EnumToDictionaryJson()));

#line default
#line hidden
#nullable disable
            WriteLiteral(@")});
        initTree();
        initGrid();

        $(""#gridTable"").on(""check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table"", function () {
            var selectedRows = $(""#gridTable"").bootstrapTable(""getSelections"");

            if ($('#btnUploadThumbnail')) {
                $('#btnUploadThumbnail').toggleClass('disabled', selectedRows.length != 1);
            }

            if ($('#btnUploadSquareThumbnail')) {
                $('#btnUploadSquareThumbnail').toggleClass('disabled', selectedRows.length != 1);
            }
            if ($('#btnUploadImage')) {
                $('#btnUploadImage').toggleClass('disabled', selectedRows.length != 1);
            }
            if ($('#btnUploadVideo')) {
                $('#btnUploadVideo').toggleClass('disabled', selectedRows.length != 1);
            }
            if ($('#btnEditBody')) {
                $('#btnEditBody').toggleClass('disabled', selectedRows.length != 1);
            }

            if ($('#btn");
            WriteLiteral(@"ManageAttachedImages')) {
                $('#btnManageAttachedImages').toggleClass('disabled', selectedRows.length != 1);
            }
            if ($('#btnManageAttachedFiles')) {
                $('#btnManageAttachedFiles').toggleClass('disabled', selectedRows.length != 1);
            }
        });

        $('body').layout({ west__size: 185 });
        laydate.render({ elem: '#startCreationTime', format: 'yyyy-MM-dd' });
        laydate.render({ elem: '#endCreationTime', format: 'yyyy-MM-dd' });

        $('#btnExpandTree').click(function () {
            var tree = $.fn.zTree.getZTreeObj(""masterTree"");
            tree.expandAll(true);
            $(this).hide();
            $('#btnCollapseTree').show();
        });

        $('#btnCollapseTree').click(function () {
            var tree = $.fn.zTree.getZTreeObj(""masterTree"");
            tree.expandAll(false);
            $(this).hide();
            $('#btnExpandTree').show();
        });

        $('#btnRefreshMaster').clic");
            WriteLiteral(@"k(function () {
            initTree();
            initGrid();
            searchGrid();
        });

    });

    function initTree() {
        var topId = $('#top-master_select').val();
        $('#masterId').val(topId);
        $('#masterTree').ysTree({
            url: '");
#nullable restore
#line 157 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
             Write(Url.Content("~/Scc/Article/GetCategoryTreeJsonForManage"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?topMasterId=' + topId,
            async: true,
            expandLevel: 2,
            maxHeight: ""800px"",
            callback: {
                onClick: function (event, treeId, treeNode) {
                    $(""#masterId"").val(treeNode.id);
                    searchGrid();
                }
            }
        });
    }

    function initGrid() {
        var queryUrl = '");
#nullable restore
#line 171 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                   Write(Url.Content("~/Scc/Article/GetPagedManageDtosJson"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"';
        $('#gridTable').ysTable({
            url: queryUrl,
            sortName: 'Sequence',
            sortOrder: 'Asc',
            columns: [
                { checkbox: true, visible: true },
                {
                    field: 'Type', title: '类别', align: ""left"", sortable: true,
                    formatter: function (value, item, index) {
                        if (item.Type == """);
#nullable restore
#line 181 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                      Write((int)ArticleType.RichText);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"label label-success\">");
#nullable restore
#line 182 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                 Write(ArticleType.RichText.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\';\r\n                        }\r\n                        else if (item.Type == \"");
#nullable restore
#line 184 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                           Write((int)ArticleType.HtmlText);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"label label-primary\">");
#nullable restore
#line 185 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                 Write(ArticleType.HtmlText.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\';\r\n                        }\r\n                        else if (item.Type == \"");
#nullable restore
#line 187 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                           Write((int)ArticleType.MarkdownText);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"label label-info\">");
#nullable restore
#line 188 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                              Write(ArticleType.MarkdownText.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\';\r\n                        }\r\n                        else if (item.Type == \"");
#nullable restore
#line 190 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                           Write((int)ArticleType.OutLink);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"label label-danger\">");
#nullable restore
#line 191 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                Write(ArticleType.OutLink.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\';\r\n                        }\r\n                        else if (item.Type == \"");
#nullable restore
#line 193 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                           Write((int)ArticleType.ArticleShortcut);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"label label-warning\">");
#nullable restore
#line 194 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                 Write(ArticleType.ArticleShortcut.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\';\r\n                        }\r\n                        else if (item.Type == \"");
#nullable restore
#line 196 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                           Write((int)ArticleType.HttpFileShortcut);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"label warning-light\">");
#nullable restore
#line 197 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                 Write(ArticleType.HttpFileShortcut.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>';
                        }
                    }
                },
                { field: 'Name', title: '标题', sortable: true },
                { field: 'Description', title: '描述' },
                { field: 'Note', title: '说明', sortable: true },
                { field: 'MasterCascadePath', title: '路径' },
                {
                    field: 'HasThumbnail', title: '缩略图',
                    formatter: function (value, row, index) {
                        if (row.HasThumbnail == """);
#nullable restore
#line 208 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                             Write((int)HasOrNone.Has);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"badge badge-primary\">\' + \"");
#nullable restore
#line 209 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                      Write(HasOrNone.Has.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("\" + \'</span>\';\r\n                        } else {\r\n                            return \'<span class=\"badge badge-warning\">\' + \"");
#nullable restore
#line 211 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                      Write(HasOrNone.None.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("\" + \'</span>\';\r\n                        }\r\n                    }\r\n                },\r\n                {field: \'HasImage\', title: \'原图\',\r\n                    formatter: function (value, row, index) {\r\n                        if (row.HasImage == \"");
#nullable restore
#line 217 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                         Write((int)HasOrNone.Has);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"badge badge-primary\">\' + \"");
#nullable restore
#line 218 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                      Write(HasOrNone.Has.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("\" + \'</span>\';\r\n                        } else {\r\n                            return \'<span class=\"badge badge-warning\">\' + \"");
#nullable restore
#line 220 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                      Write(HasOrNone.None.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("\" + \'</span>\';\r\n                        }\r\n                    }\r\n                },\r\n                {field: \'HasVideo\', title: \'视频\',\r\n                    formatter: function (value, row, index) {\r\n                        if (row.HasVideo == \"");
#nullable restore
#line 226 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                         Write((int)HasOrNone.Has);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"badge badge-primary\">\' + \"");
#nullable restore
#line 227 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                      Write(HasOrNone.Has.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("\" + \'</span>\';\r\n                        } else {\r\n                            return \'<span class=\"badge badge-warning\">\' + \"");
#nullable restore
#line 229 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                      Write(HasOrNone.None.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral(@""" + '</span>';
                        }
                    }
                },
                { field: 'AttachedFilesNum', title: '附件数' },
                { field: 'AttachedImagesNum', title: '附图数' },
                { field: 'Sequence', title: '排序码', align: ""left"", sortable: true },
                {
                    field: 'Status', title: '状态', sortable: true,
                    formatter: function (value, row, index) {
                        if (row.Status == """);
#nullable restore
#line 239 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                       Write((int)StatusType.Enabled);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"badge badge-primary\">\' + \"");
#nullable restore
#line 240 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                      Write(StatusType.Enabled.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("\" + \'</span>\';\r\n                        } else {\r\n                            return \'<span class=\"badge badge-warning\">\' + \"");
#nullable restore
#line 242 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                                      Write(StatusType.Disabled.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral(@""" + '</span>';
                        }
                    }
                },
                {
                    field: 'ModificationTime', title: '修改时间', width: '150px', sortable: true,
                    formatter: function (value, row, index) {
                        return ys.formatDate(value, ""yyyy-MM-dd HH:mm:ss"");
                    }
                },
                { field: 'Id', title: 'Id', width: '100px', visible: false },
                {
                    title: '操作',
                    align: 'center',
                    width: '60px',
                    formatter: function (value, row, index) {
                        var actions = [];
                        actions.push('<a class=""btn btn-info btn-xs"" href=""#"" onclick=""goToDisplay(\'' + row.Id +'\'' + ','+'\''+row.Type + '\')""><i class=""fa""></i>浏览</a>');
                        //actions.push('<a class=""btn btn-info btn-xs"" href=""#"" onclick=""goToManageAttachedImages(\'' + row.Id + '\')""><i class=""fa""></i>管");
            WriteLiteral(@"理页内图片</a>');
                        //actions.push('<a class=""btn btn-info btn-xs"" style=""margin-left:10px"" href=""#"" onclick=""goToManageAttachedFiles(\'' + row.Id + '\')""><i class=""fa""></i>管理附件</a>');
                        return actions.join('');
                    }
                }
            ],
            queryParams: function (params) {
                var pagination = $('#gridTable').ysTable('getPagination', params);
                var queryString = $(""#searchDiv"").getWebControls(pagination);
                return queryString;
            }
        });
    }

    function searchGrid() {
        $('#gridTable').ysTable('search');
        resetToolbarStatus();
    }

    function showAddEditDialog(add) {
        var id = '';
        var masterId ='';
        if (!add) {
            var selectedRows = $(""#gridTable"").bootstrapTable(""getSelections"");
            if (!ys.checkRowEdit(selectedRows)) {
                return;
            }
            else {
               ");
            WriteLiteral(" id = selectedRows[0].Id;\r\n            }\r\n        }\r\n        else {\r\n            masterId = $(\"#masterId\").val();\r\n        }\r\n        ys.openDialog({\r\n            title: id == \'\' ? \"新增文章\":\"修改文章\",\r\n            content: \'");
#nullable restore
#line 296 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                 Write(Url.Content("~/Scc/Article/AddEditModal"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?id=' + id + '&masterId=' + masterId,
            height:'500px',
            callback: function (index, layero) {
                var frame = window[layero.find('iframe')[0]['name']];
                frame.saveForm(index);
            }
        });
    }

    function showEditBodyDialog() {
        var id = '';
        var type = 0;
        var selectedRows = $(""#gridTable"").bootstrapTable(""getSelections"");
        if (!ys.checkRowEdit(selectedRows)) {
            return;
        }
        else {
            id = selectedRows[0].Id;
            type = selectedRows[0].Type;
        }

        var w = type == '");
#nullable restore
#line 317 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                     Write((int)ArticleType.OutLink);

#line default
#line hidden
#nullable disable
            WriteLiteral("\' ? \'600px\' : \'100%\';\r\n        var h = type == \'");
#nullable restore
#line 318 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                     Write((int)ArticleType.OutLink);

#line default
#line hidden
#nullable disable
            WriteLiteral("\' ? \'400px\' : \'100%\';\r\n        ys.openDialog({\r\n            title: \"编辑正文-\" + selectedRows[0].Name ,\r\n            content: \'");
#nullable restore
#line 321 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                 Write(Url.Content("~/Scc/Article/EditBodyModal"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?id=' + id,
            width: w,
            height:h,
            btn: ['保存', '关闭'],
            callback: function (index, layero) {
                var frame = window[layero.find('iframe')[0]['name']];
                frame.saveForm(index);
            }
        });
    }

    function showUploadSquareThumbnailDialog() {
        var id = '';
        var selectedRows = $(""#gridTable"").bootstrapTable(""getSelections"");
        if (!ys.checkRowEdit(selectedRows)) {
            return;
        }
        else {
            id = selectedRows[0].Id;
        }

        ys.openDialog({
            title: ""上传缩略图"",
            height: '580px',
            btn: ['取消'],
            content: '");
#nullable restore
#line 346 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                 Write(Url.Content("~/Scc/Article/UploadAttachmentModal"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?index=squareThumbnail&id=' + id,
            callback: function (index, layero) {
                var frame = window[layero.find('iframe')[0]['name']];
                frame.LeaveAndRefreshParent(index);
            }
        });
    }

    function deleteSelected() {
        var selectedRows = $(""#gridTable"").bootstrapTable(""getSelections"");
        if (ys.checkRowDelete(selectedRows)) {
            ys.confirm(""确认要删除选中的"" + selectedRows.length + ""条数据吗？"", function () {
                var ids = ys.getIds(selectedRows);
                ys.ajax({
                    url: '");
#nullable restore
#line 360 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                     Write(Url.Content("~/Scc/Article/DeleteSelected"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?ids=' + ids,
                    type: ""post"",
                    error: ys.ajaxError,
                    success: function (rst) {
                        if (rst.Flag == 1) {
                            ys.msgSuccess(rst.Message);
                            searchGrid();
                        }
                        else ys.msgError(rst.Message);
                    }
                });
            });
        }
    }



    function onTopMasterChange() {
        initTree();
        initGrid();
        searchGrid();
    }

    function goToManageDirectory() {
        var url = '");
#nullable restore
#line 384 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
              Write(Url.Content("~/Scc/Directory/Manage?index="));

#line default
#line hidden
#nullable disable
            WriteLiteral("\' +\'");
#nullable restore
#line 384 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                                                               Write(masterType);

#line default
#line hidden
#nullable disable
            WriteLiteral("\';\r\n        createMenuItem(url, \"管理");
#nullable restore
#line 385 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SCC\Views\Article\Manage.cshtml"
                           Write(masterTypeName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\");\r\n    }\r\n\r\n\r\n    function goToDisplay(id, type) {\r\n        var url = \"Show?id=\" + id;\r\n        window.open(url);\r\n    }\r\n\r\n\r\n</script>\r\n");
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
