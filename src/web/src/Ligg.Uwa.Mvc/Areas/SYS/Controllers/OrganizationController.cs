using Ligg.EntityFramework.Entities;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.Mvc.Controllers.SYS
{
    [Area("Sys")]
    public class OrganizationController : BaseController
    {
        private readonly OrganizationService _service;
        private readonly UserService _userService;
        public OrganizationController(OrganizationService service, UserService userService)
        {
            _service = service;
            _userService = userService;
        }

        //*page
        public IActionResult Manage()
        {
            var defId = new Organization().Id;
            ViewBag.DefaultId = defId.ToString();
            return View();
        }

        //*modal
        public IActionResult AddEditModal()
        {
            var defId = new Category().Id;
            ViewBag.DefaultId = defId.ToString();
            return View();
        }
        public IActionResult AddTopModal()
        {
            var defId = new Category().Id;
            ViewBag.DefaultId = defId.ToString();
            return View("AddEditModal");
        }

        //*get
        [HttpGet]
        public async Task<IActionResult> GetManageDtosJson(CommonReqArgs param)
        {
            var dtos = await _service.GetManageDtosAsync(param);
            var rst = new TResult<List<ManageOrganizationsDto>>(1, dtos);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrganizationTreeJsonForSelectParent()
        {
            var parentId = new Organization().Id;
            var dtos = await _service.GetListDtoTreeByParenId(parentId.ToString(), false);

            var rst = new TResult<List<TreeItem>>(1, dtos);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTreeJsonForSelectOwner()
        {
            var orgId = new Organization().Id.ToString();
            var dtos = await _userService.GetOrganizationAndSubUserTreeByOrgId(orgId, true);
            var rst = new TResult<List<TreeItem>>(1, dtos);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditDtoJson(string id)
        {
            var dto = await _service.GetAddEditDtoAsync(id);
            var rst = new TResult<AddEditOrganizationDto>(1, dto);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSequenceNoJson(string parentId)
        {
            var no = await _service.GetMaxSequenceNoAsync(parentId);
            var rst = new TResult<int>(1, no);
            return Json(rst);
        }

        //*post
        [HttpPost]
        public async Task<IActionResult> Add(AddEditOrganizationDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            if (model.ParentId == null) model.ParentId = new Organization().Id.ToString();
            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEditOrganizationDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            if (model.ParentId == null) model.ParentId = new Organization().Id.ToString();
            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected(string id)
        {
            var msg = await _service.DeleteByIdAsync(id);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }




    }
}