using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.Utilities.LogUtil;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ligg.Infrastructure.DataModels;


namespace Ligg.Uwa.Mvc.Controllers.SYS
{
    [Area("sys")]
    public class ConfigController : BaseController
    {
        private readonly ConfigService _service;

        public ConfigController(ConfigService service)
        {
            _service = service;
        }

        //*page
        public IActionResult Manage(int index)
        {
            var indexName = EnumHelper.GetNameById<ConfigIndex>(index);
            var isOwner = false;
            var ownerCfgItem = new ConfigHandler().GetFirstConfigItem((int)OrpConfigSubType.ConfigOwner + "", indexName);
            if (ownerCfgItem != null)
            {
                var owner = ownerCfgItem.Attribute1;
                var currentOprt = CurrentOperator.Instance.GetCurrent();
                if (currentOprt != null)
                {
                    if (currentOprt.IsRoot) isOwner = true;
                    else if (currentOprt.ActorId == owner) isOwner = true;
                }
            }
            var isManager = false;
            var permissionHandler = new PermissionHandler();
            isManager = permissionHandler.IsManger(PermissionType.GrantAsManagerForConfig, index.ToString());
            ViewBag.IsOwner = isOwner;
            ViewBag.IsManager = isManager;

            if (index == (int)ConfigIndex.DevConfig | index == (int)ConfigIndex.OrpConfig | index == (int)ConfigIndex.FuncConfig)
            {
                ViewBag.ConfigIndex = index;

                return View("ManageSysConfig");
            }
            else if (index == (int)ConfigIndex.CustConfig)
            {
                ViewBag.ConfigIndex = index;
                return View("ManageCustConfig");
            }
            else
            {
                var errCode = GetErrorCode();
                return Redirect("/Home/Error/notExist/index/" + index + "/" + errCode + "-1");
            }
        }

        //*modal
        public IActionResult AddEditModal()
        {
            return View();
        }

        //*get
        [HttpGet]
        public async Task<IActionResult> GetPagedManageDtosJson(int id, CommonReqArgs param, Pagination pagination)
        {
            if (id == (int)ConfigIndex.DevConfig | id == (int)ConfigIndex.OrpConfig | id == (int)ConfigIndex.FuncConfig)
            {
                var dtos = await _service.GetPagedManageSysConfigsAsync(id, param, pagination);
                var rst = new TResult<List<ManageConfigsDto>>(1, dtos);
                rst.Total = pagination.Total;
                ViewBag.Index = id;
                return Json(rst);
            }
            else if (id == (int)ConfigIndex.CustConfig)
            {
                var dtos = await _service.GetPagedManageCustConfigsAsync(param, pagination);
                var rst = new TResult<List<ManageConfigsDto>>(1, dtos);
                rst.Total = pagination.Total;
                return Json(rst);
            }

            return null;

        }


        [HttpGet]
        public async Task<IActionResult> GetEditDtoJson(string id)
        {
            var errCode = GetErrorCode();
            var dto = await _service.GetAddEditDtoAsync(id);
            return Json(new TResult<AddEditConfigDto>(1, dto));
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSequenceNoJson(int type)
        {
            var errCode = GetErrorCode();
            if (!judgeOperator())
                return Json((0, new TextHandler().GetErrorMessage("noAuth", errCode, "You are not Manager ")));

            var no = await _service.GetMaxSequenceNoAsync(type);
            return Json(new TResult<int>(1, no));
        }

        //common
        [HttpGet]
        public IActionResult GetPortalConfigDetailsJsonToFrontEnd(string id)
        {
            LogHelper.Trace("GetPortalConfigDetailsJsonToFrontEnd begins");
            var dtos = new List<ConfigDetail>();
            dtos = _service.GetPortalConfigDetailsToFrontEnd(id);
            var rst = new TResult<List<ConfigDetail>>(1, dtos);
            LogHelper.Trace("GetPortalConfigDetailsJsonToFrontEnd ends");
            return Json(rst);
        }

        [HttpGet]
        public IActionResult GetPageConfigDetailsJsonToFrontEnd(string pageId)
        {
            var dtos = _service.GetPageConfigDetailsToFrontEnd(pageId);
            var rst = new TResult<List<ConfigDetail>>(1, dtos);
            return Json(rst);
        }

        //#post
        [HttpPost]
        public async Task<IActionResult> Add(AddEditConfigDto model)
        {
            var errCode = GetErrorCode();
            if (!judgeOperator())
                return Json((0, new TextHandler().GetErrorMessage("noAuth", errCode, "You are not Manager ")));

            var rst = new TResult(0, "");
            if (model.Type < 1 | model.Type == null)
            {
                rst.Message = "请选择类型";
                return Json(rst);
            }
            if (!StringExtension.AlphaAndNumeralExpression.IsMatch(model.Code))
            {

                rst.Message = "编号只能是英文字母和数字";
                return Json(rst);
            }

            var msg = await _service.SaveAddEditDtoAsync(model);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEditConfigDto model)
        {
            var errCode = GetErrorCode();
            if (!judgeOperator())
                return Json((0, new TextHandler().GetErrorMessage("noAuth", errCode, "You are not Manager ")));

            var rst = new TResult(0, "");

            if (model.Type < 1 | model.Type == null)
            {
                rst.Message = "请选择类型";
                return Json(rst);
            }

            if (!StringExtension.AlphaAndNumeralExpression.IsMatch(model.Code))
            {
                rst.Message = "编号只能是英文字母和数字";
                return Json(rst);
            }

            var msg = await _service.SaveAddEditDtoAsync(model);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected(string ids)
        {
            var errCode = GetErrorCode();
            if (!judgeOperator())
                return Json((0, new TextHandler().GetErrorMessage("noAuth", errCode, "You are not Manager ")));

            var msg = await _service.DeleteByIdsAsync(ids);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }

        private bool judgeOperator()
        {
            var index = (int)ConfigIndex.CustConfig + "";
            var permissionHandler = new PermissionHandler();
            var isManager = permissionHandler.IsManger(PermissionType.GrantAsManagerForConfig, index.ToString());
            return isManager;
        }


    }
}