#pragma checksum "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bd11040e0384aae9be2a7d5ddfdce11bed323613"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_SYS_Views_User_Manage), @"mvc.1.0.view", @"/Areas/SYS/Views/User/Manage.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bd11040e0384aae9be2a7d5ddfdce11bed323613", @"/Areas/SYS/Views/User/Manage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6cc5d010ab9bfc05a4b62199a196885be56ed38e", @"/Areas/SYS/Views/_ViewImports.cshtml")]
    public class Areas_SYS_Views_User_Manage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
  
    Layout = "~/Views/Shared/_SysPage.cshtml";

#line default
#line hidden
#nullable disable
            DefineSection("header", async() => {
                WriteLiteral("\r\n    ");
#nullable restore
#line 6 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/lib/zTree/v3/css/metroStyle/metroStyle.min.css")));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n    ");
#nullable restore
#line 7 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/lib/zTree/v3/js/ztree.min.js")));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n    ");
#nullable restore
#line 9 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
Write(BundlerHelper.Render(HostingEnvironment.ContentRootPath, Url.Content("~/lib/jquery.layout/1.4.4/jquery.layout-latest.min.css")));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n    ");
#nullable restore
#line 10 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
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
            <div class=""box-header"">
                <div class=""box-title"">
                    组织机构
                </div>

                <div class=""box-tools pull-right"">
                    <a type=""button"" class=""btn btn-box-tool menuItem"" href=""#"" onclick=""showManageOrganization()"" title=""管理组织""><i class=""fa fa-edit""></i></a>
                    <button type=""button"" class=""btn btn-box-tool"" id=""btnExpandTree"" title=""展开"" style=""display:none;""><i class=""fa fa-chevron-up""></i></button>
                    <button type=""button"" class=""btn btn-box-tool"" id=""btnCollapseTree"" title=""折叠""><i class=""fa fa-chevron-down""></i></button>
                    <button type=""button"" class=""btn btn-box-tool"" id=""btnRefreshMaster"" title=""刷新组织""><i class=""fa fa-refresh""></i></button>

                </div>
            </div>
            <div class=""ui-layout-content"">
                <div id=""masterTree"" class");
            WriteLiteral(@"=""ztree""></div>
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
                        状态：<span id=""status"" col=""Status""></span>
                    </li>
                    <li>
                        姓名或描述：<input id=""text"" col=""Text"" type=""text"" />
                    </li>
                    <li>
                        账号/手机/邮件/微信：<input id=""mark"" col=""Mark"" type=""text"" />
                    </li>

                    <li class=""se");
            WriteLiteral(@"lect-time"">
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
            <a id=""btnEdit"" class=""btn btn-primary disabled"" onclick=""showAddEditDialog(false)""><i class=""fa fa-edit""></i> 修改</a>
            <a id=""btnManageRoles"" class=""btn btn-primary disabled""");
            BeginWriteAttribute("onclick", " onclick=\"", 3714, "\"", 3786, 3);
            WriteAttributeValue("", 3724, "showUpdateBelongedUserGroupsDialog(", 3724, 35, true);
#nullable restore
#line 74 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
WriteAttributeValue("", 3759, (int)UserGroupType.Role, 3759, 26, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3785, ")", 3785, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fa fa-edit\"></i> 管理角色</a>\r\n            <a id=\"btnManageAuthGroups\" class=\"btn btn-primary disabled\"");
            BeginWriteAttribute("onclick", " onclick=\"", 3897, "\"", 3983, 3);
            WriteAttributeValue("", 3907, "showUpdateBelongedUserGroupsDialog(", 3907, 35, true);
#nullable restore
#line 75 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
WriteAttributeValue("", 3942, (int)UserGroupType.AuthorizationGroup, 3942, 40, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3982, ")", 3982, 1, true);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fa fa-edit\"></i> 管理权限组</a>\r\n            <a id=\"btnManageCommGroups\" class=\"btn btn-primary disabled\"");
            BeginWriteAttribute("onclick", " onclick=\"", 4095, "\"", 4181, 3);
            WriteAttributeValue("", 4105, "showUpdateBelongedUserGroupsDialog(", 4105, 35, true);
#nullable restore
#line 76 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
WriteAttributeValue("", 4140, (int)UserGroupType.CommunicationGroup, 4140, 40, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 4180, ")", 4180, 1, true);
            EndWriteAttribute();
            WriteLiteral(@"><i class=""fa fa-edit""></i> 管理通讯组</a>
            <a id=""btnUploadThumbnail"" class=""btn btn-info disabled"" onclick=""showUploadThumbnailDialog()""><i class=""fa fa-upload""></i> 上传缩略图</a>
            <a id=""btnDelete"" class=""btn btn-danger disabled"" onclick=""deleteSelected()""><i class=""fa fa-remove""></i> 删除</a>
        </div>

        <div class=""col-sm-12 select-table table-striped"">
            <table id=""gridTable"" data-mobile-responsive=""true""></table>
        </div>
    </div>
</div>

<script type=""text/javascript"">
    $(function () {
        $(""#status"").ysComboBox({data: ys.getJson(");
#nullable restore
#line 89 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                                             Write(Html.Raw(typeof(StatusType).EnumToDictionaryJson()));

#line default
#line hidden
#nullable disable
            WriteLiteral(") });\r\n        $(\"#recursiveSearch\").ysComboBox({class: \'no-additional-option\', data: ys.getJson(");
#nullable restore
#line 90 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                                                                                     Write(Html.Raw(typeof(YesOrNo).EnumToDictionaryJson()));

#line default
#line hidden
#nullable disable
            WriteLiteral(@")});
        initTree();
        initGrid();
        $(""#gridTable"").on(""check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table"", function () {
            var selectedRows = $(""#gridTable"").bootstrapTable(""getSelections"");
            if ($('#btnManageRoles')) {
                $('#btnManageRoles').toggleClass('disabled', selectedRows.length != 1);
            }
            if ($('#btnManageAuthGroups')) {
                $('#btnManageAuthGroups').toggleClass('disabled', selectedRows.length != 1);
            }
            if ($('#btnManageCommGroups')) {
                $('#btnManageCommGroups').toggleClass('disabled', selectedRows.length != 1);
            }
            if ($('#btnUploadThumbnail')) {
                $('#btnUploadThumbnail').toggleClass('disabled', selectedRows.length != 1);
            }
        });

        $('body').layout({ west__size: 185 });
        laydate.render({ elem: '#startCreationTime', format: 'yyyy-MM-dd' });
        laydate.render({ ele");
            WriteLiteral(@"m: '#endCreationTime', format: 'yyyy-MM-dd' });

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

        $('#btnRefreshMaster').click(function () {
            initTree();
        });

    });

    function initTree() {
        $('#masterTree').ysTree({
            url: '");
#nullable restore
#line 135 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
             Write(Url.Content("~/Sys/User/GetOrganizationTreeJsonForManage"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"',
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
#line 149 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                   Write(Url.Content("~/sys/User/GetPagedManageDtosJson"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"';
        $('#gridTable').ysTable({
            url: queryUrl,
            sortName: 'Account',
            sortOrder: 'Asc',
            toolbar: '#toolbar',
            columns: [
                { checkbox: true, visible: true },

                { field: 'Account', title: '账号', sortable: true },
                { field: 'Name', title: '姓名' , sortable: true },
                { field: 'MasterCascadePath', title: '组织' },
                { field: 'Mobile', title: '手机', width: '100px'},
                {
                    field: 'HasThumbnail', title: '缩略图', width: '60px',
                    formatter: function (value, row, index) {
                        if (row.HasThumbnail == """);
#nullable restore
#line 165 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                                             Write((int)HasOrNone.Has);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"badge badge-primary\">\' + \"");
#nullable restore
#line 166 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                                                                      Write(HasOrNone.Has.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("\" + \'</span>\';\r\n                        } else {\r\n                            return \'<span class=\"badge badge-warning\">\' + \"");
#nullable restore
#line 168 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                                                                      Write(HasOrNone.None.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral(@""" + '</span>';
                        }
                    }
                },
                {
                    field: 'Status', title: '状态', width: '40px', sortable: true,
                    formatter: function (value, row, index) {
                        if (row.Status == """);
#nullable restore
#line 175 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                                       Write((int)StatusType.Enabled);

#line default
#line hidden
#nullable disable
            WriteLiteral("\") {\r\n                            return \'<span class=\"badge badge-primary\">\' + \"");
#nullable restore
#line 176 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                                                                      Write(StatusType.Enabled.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral("\" + \'</span>\';\r\n                        } else {\r\n                            return \'<span class=\"badge badge-warning\">\' + \"");
#nullable restore
#line 178 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                                                                      Write(StatusType.Disabled.GetDescription());

#line default
#line hidden
#nullable disable
            WriteLiteral(@""" + '</span>';
                        }
                    }
                },
                {
                    field: 'ModificationTime', title: '修改时间', width:'150px',sortable: true,
                    formatter: function (value, row, index) {
                        return ys.formatDate(value, ""yyyy-MM-dd HH:mm:ss"");
                    }
                },
                { field: 'Id', title: 'Id', width: '100px',visible: false },
                {
                    title: '操作',
                    align: 'center', width: '100px',
                    formatter: function (value, row, index) {
                        var actions = [];
                        actions.push('<a class=""btn btn-warning btn-xs"" href=""#"" onclick=""showResetPasswordDialog(\'' + row.Id + '\')""><i class=""fa fa-key""></i>重置密码</a>');
                        return actions.join('');
                    }
                }
            ],
            queryParams: function (params) {
                var pagin");
            WriteLiteral(@"ation = $('#gridTable').ysTable('getPagination', params);
                queryString = $(""#searchDiv"").getWebControls(pagination);
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
        var masterId = '';
        if (add) { masterId = $(""#masterId"").val();}
        else {
            var selectedRows = $(""#gridTable"").bootstrapTable(""getSelections"");
            if (!ys.checkRowEdit(selectedRows)) {
                return;
            }
            else {
                id = selectedRows[0].Id;
            }
        }
        ys.openDialog({
            title: id == '' ? ""新建用户"" : ""修改用户"",
            height:'580px',
            content: '");
#nullable restore
#line 228 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                 Write(Url.Content("~/Sys/User/AddEditModal"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?id=' + id + '&masterId='+masterId,
            callback: function (index, layero) {
                var frame = window[layero.find('iframe')[0]['name']];
                frame.saveForm(index);
            }
        });
    }

    function showUpdateBelongedUserGroupsDialog(type) {
        var id = '';
        var selectedRows = $(""#gridTable"").bootstrapTable(""getSelections"");
        if (!ys.checkRowEdit(selectedRows)) {
            return;
        }

        id = selectedRows[0].Id;
         ys.openDialog({
            title: ""角色选择"",
             content: '");
#nullable restore
#line 246 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                  Write(Url.Content("~/Sys/User/SelectBelongedUserGroupsModal"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?type=' + type+ '&id=' + id,
            width: ""600px"",
            height: ""490px"",
            shadeClose: true,
            callback: function (index, layero) {
                var childFrame = window[layero.find('iframe')[0]['name']];
                var ids = childFrame.getSelectedIds();
                ys.ajax({
                    url: '");
#nullable restore
#line 254 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                     Write(Url.Content("~/Sys/User/UpdateBelongedUserGroups"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?type=' + type + '&id='+id+'&relatedIds=' + ids,
                    type: ""post"",
                    success: function (rst) {
                        if (rst.Flag == 1) {
                            ys.msgSuccess(rst.Message);
                        }
                        else {
                            ys.msgError(rst.Message);
                        }
                    }
                });
                layer.close(index);
            }
        });
    }

    function showUploadThumbnailDialog() {
        var id = '';
        var selectedRows = $(""#gridTable"").bootstrapTable(""getSelections"");
        if (!ys.checkRowEdit(selectedRows)) {
            return;
        }
        else {
            id = selectedRows[0].Id;
        }

        ys.openDialog({
            title: ""上传Logo缩略图"",
            height: '580px',
            btn: ['关闭'],
            content: '");
#nullable restore
#line 284 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                 Write(Url.Content("~/Sys/User/UploadAttachmentModal"));

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
#line 298 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                     Write(Url.Content("~/Sys/User/DeleteSelected"));

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
                        else {
                            ys.msgError(rst.Message);
                        }
                    }
                });
            });
        }
    }

    function showManageOrganization() {
        var url = '");
#nullable restore
#line 316 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
              Write(Url.Content("~/Sys/Organization/Manage"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\';\r\n        createMenuItem(url, \"组织管理\");\r\n    }\r\n\r\n    function showResetPasswordDialog(id) {\r\n        ys.openDialog({\r\n            title: \"重置密码\",\r\n            content: \'");
#nullable restore
#line 323 "D:\Doing\coding\coding-doing\arch\Ligg.Web\src-sharing\src\web\src\Ligg.Uwa.Mvc\Areas\SYS\Views\User\Manage.cshtml"
                 Write(Url.Content("~/Sys/User/ResetPasswordModal"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"' + '?id=' + id,
            height: ""220px"",
            callback: function (index, layero) {
                var frame = window[layero.find('iframe')[0]['name']];
                frame.saveForm(index);
            }
        });
    }

</script>
");
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
