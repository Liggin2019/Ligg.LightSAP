using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.Mvc.Controllers.SYS
{
    [Area("Sys")]
    public class UserGroupController : BaseController
    {
        private readonly UserGroupService _service;
        private readonly UserService _userService;

        public UserGroupController(UserGroupService service, UserService userService)
        {
            _service = service;
            _userService = userService;
        }

        //*page
        public IActionResult Manage(int index)
        {
            var errCode = GetErrorCode();

            if (index != (int)UserGroupType.Role & index != (int)UserGroupType.AuthorizationGroup & index != (int)UserGroupType.CommunicationGroup)
            {
                return Redirect("~/Home/Error/wurl/index/" + index + "/" + errCode + "-1");
            }

            ViewBag.Type = index;
            ViewBag.TypeName = EnumHelper.GetById(index, UserGroupType.Role).GetDescription();
            return View();
        }

        //*modal
        public IActionResult AddEditModal()
        {
            return View();
        }

        public IActionResult SelectSubUsersModal(int type, string id)
        {
            ViewBag.Url = "/Sys/UserGroup/GetUserTreeJsonForSelecSubUsers";
            var updateDataUrl = "/Sys/UserGroup/GetSubUserIdsStringJson?type=" + type + "&id=" + id;
            ViewBag.UpdateDataUrl = updateDataUrl;
            return View("Selectors/RelatedIdsTreeSelector");
        }

        //*get
        [HttpGet]
        public async Task<IActionResult> GetPagedManageDtosJson(CommonReqArgs param, Pagination pagination)
        {
            var dtos = await _service.GetPagedManageDtosAsync(param, pagination);
            var rst = new TResult<List<ManageUserGroupsDto>>(1, dtos);
            rst.Total = pagination.Total;
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTreeJsonForSelecSubUsers()
        {
            var orgId = new Organization().Id.ToString();
            var dtos = await _userService.GetOrganizationAndSubUserTreeByOrgId(orgId, true);
            var rst = new TResult<List<TreeItem>>(1, dtos);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubUserIdsStringJson(int type, string id)
        {
            var dto = await _service.GetUserIdsStringByTypeAndIdAsync(type,id);
            var rst = new TResult<string>(1, dto);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditDtoJson(string id)
        {
            var dto = await _service.GetAddEditDtoAsync(id);
            var rst = new TResult<AddEditUserGroupDto>(1, dto);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSequenceNoJson(int type)
        {
            var no = await _service.GetMaxSequenceNoAsync(type);
            var rst = new TResult<int>(1, no);
            return Json(rst);
        }

        //*post
        [HttpPost]
        public async Task<IActionResult> Add(AddEditUserGroupDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));

        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEditUserGroupDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubUsers(int type, RelatedIdsDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            //var type = (int)UserGroupType.Role;
            var msg = await _service.UpdateContainedUsers(model, type);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected(int type, string ids)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var msg = await _service.DeleteByIdsAsync(type, ids);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }




    }
}