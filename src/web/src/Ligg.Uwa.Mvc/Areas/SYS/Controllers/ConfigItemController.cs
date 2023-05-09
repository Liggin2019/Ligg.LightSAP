using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Ligg.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.Mvc.Controllers.SYS
{
    [Area("sys")]
    public class ConfigItemController : BaseController
    {
        private readonly ConfigItemService _service;
        public ConfigItemController(ConfigItemService service)
        {
            _service = service;
        }

        //*page
        public IActionResult Manage(string masterId)
        {
            var errCode = GetErrorCode();
            if (masterId.IsNullOrEmpty())
                return Redirect("/Home/Error/wurl/masterId/null/" + errCode + "-1");

            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(masterId);
            if (!configDefinition.IsOwner & !configDefinition.IsManager & !configDefinition.IsProducer)
                return Redirect("/Home/Error/noAuth/masterId/"+masterId+"/" + errCode + "-2" + "?addlMsg=You are not Owner, Manager or Producer");

            ViewBag.ConfigDefinition = configDefinition;
            return View();
        }

        //*modal
        public IActionResult AddEditModal(string index, string masterId)
        {
            var errCode = GetErrorCode();
            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(masterId);
            if (!configDefinition.IsOwner & !configDefinition.IsManager & !configDefinition.IsProducer)
                return Redirect("/Home/Error/noAuth/masterId/"+masterId+"/" + errCode + "-1" + "?addlMsg=You are not Producer, Manager or Producer");

            if (index.IsNullOrEmpty())
            {
                ViewBag.ConfigDefinition = configDefinition;
                return View();
            }
            else if (index.ToLower() == "EditValueModal".ToLower()) return View("EditValueModal");
            else return Redirect("/Home/Error/NotExist/index/" + index + "/ " + errCode + "-2");

        }

        //*get
        [HttpGet]
        public async Task<IActionResult> GetPagedManageDtosJson(string masterId, ExpressionBuilderReqArgs param, Pagination pagination)
        {
            var errCode = GetErrorCode();
            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(masterId);
            if (!configDefinition.IsOwner & !configDefinition.IsManager & !configDefinition.IsProducer)
            {
                var errMsg = new TextHandler().GetErrorMessage("noAuth", errCode + "-1", "You are not Owner, Manager or Producer", "masterId", masterId);
                return Json(new TResult(0, errMsg));
            }

            var dtos = await _service.GetPagedManageDtosAsync(param, pagination);
            var rst = new TResult<List<ManageConfigItemsDto>>(1, dtos);
            rst.Total = pagination.Total;
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditDtoJson(string id)
        {
            var errCode = GetErrorCode();
            var dto = await _service.GetAddEditDtoAsync(id);
            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(dto.MasterId);
            if (!configDefinition.IsOwner & !configDefinition.IsManager & !configDefinition.IsProducer)
            {
                var errMsg = new TextHandler().GetErrorMessage("noAuth", errCode + "-1", "You are not Owner, Manager or Producer", "masterId", dto.MasterId);
                return Json(new TResult(0, errMsg));
            }

            return Json(new TResult<AddEditConfigItemDto>(1, dto));
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSequenceNoJson(string masterId)
        {
            var errCode = GetErrorCode();
            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(masterId);
            if (!configDefinition.IsProducer)
            {
                var errMsg = new TextHandler().GetErrorMessage("noAuth", errCode + "-1", "You are not Producer", "masterId", masterId);
                return Json(new TResult(0, errMsg));
            }

            var no = await _service.GetMaxSequenceNoAsync(masterId);
            return Json(new TResult<int>(1, no));
        }

        //#post
        [HttpPost]
        public async Task<IActionResult> Add(AddEditConfigItemDto model)
        {
            var rst = new TResult(0, "");
            if (model.Style.IsNullOrEmpty())
            {
                rst.Message = "请选择样式";
                return Json(rst);
            }
            if (!StringExtension.AlphaNumeralHyphenAndUnderscoreExpression.IsMatch(model.Key))
            {
                rst.Message = "键只能是英文、数字或下划线组成";
                return Json(rst);
            }

            var errCode = GetErrorCode();
            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(model.MasterId);
            if (!configDefinition.IsProducer)
            {
                var errMsg = new TextHandler().GetErrorMessage("noAuth", errCode + "-3", "You are not Producer", "masterId", model.MasterId);
                return Json(new TResult(0, errMsg));
            }

            var msg = await _service.SaveAddEditDtoAsync(model);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }

        [HttpPost]
        public async Task<IActionResult> Copy(string id)
        {
            var errCode = GetErrorCode();
            var ett = await _service.GetEntityByIdStringAsync(id);
            if (ett == null) Json((0, new TextHandler().GetErrorMessage("wurl", errCode+"-1", "", "id", id)));


            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(ett.MasterId);
            if (!configDefinition.IsProducer)
            {
                var errMsg = new TextHandler().GetErrorMessage("noAuth", errCode+"-2", "You are not Producer", "masterId", ett.MasterId);
                return Json(new TResult(0, errMsg));
            }

            var newEtt = ett.MapTo<ConfigItem>();
            newEtt.Id = new ConfigItem().Id;
            newEtt.Key = (ett.Key.Length > 63 - 16 ? ett.Key.Substring(0, 63 - 16) : ett.Key) + "".ToUniqueStringByDataTime(System.DateTime.Now, "-");
            var msg = await _service.UpdateEntityAsync(newEtt);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(AddEditConfigItemDto model)
        {
            var rst = new TResult(0, "");
            if (model.Style.IsNullOrEmpty())
            {
                rst.Message = "请选择样式";
                return Json(rst);
            }

            if (!StringExtension.AlphaNumeralHyphenAndUnderscoreExpression.IsMatch(model.Key))
            {
                rst.Message = "键只能是英文、数字或下划线组成";
                return Json(rst);
            }

            var errCode = GetErrorCode();
            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(model.MasterId);
            if (!configDefinition.IsProducer)
            {
                var errMsg = new TextHandler().GetErrorMessage("noAuth", errCode, "You are not Producer", "masterId", model.MasterId);
                return Json(new TResult(0, errMsg));
            }

            var msg = await _service.SaveAddEditDtoAsync(model);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }
        public async Task<IActionResult> EditValue(EditConfigItemValueDto model)
        {
            var ett = await _service.GetEntityByIdStringAsync(model.Id);

            var errCode = GetErrorCode();
            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(ett.MasterId);
            if (!configDefinition.IsProducer)
            {
                var errMsg = new TextHandler().GetErrorMessage("noAuth", errCode, "You are not Producer", "masterId", ett.MasterId);
                return Json(new TResult(0, errMsg));
            }

            ett.Value = model.Value;
            var msg = await _service.UpdateEntityAsync(ett);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }


        [HttpPost]
        public async Task<IActionResult> SetAsDefault(string id)
        {
            var errCode = GetErrorCode();
            var ett = await _service.GetEntityByIdStringAsync(id);
            if (ett == null) Json((0, new TextHandler().GetErrorMessage("wurl", errCode+"-1", "", "id", id)));

            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(ett.MasterId);
            if (!configDefinition.IsProducer)
            {
                var errMsg = new TextHandler().GetErrorMessage("noAuth", errCode+"-2", "You are not Producer", "masterId", ett.MasterId);
                return Json(new TResult(0, errMsg));
            }

            var msg = await _service.SetAsDefault(id);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected(string ids)
        {
            var errCode = GetErrorCode();
            var idArr = ids.Split(",");
            var ett = await _service.GetEntityByIdStringAsync(idArr[0]);
            if (ett == null) Json((0, new TextHandler().GetErrorMessage("wurl", errCode+"-1", "entity is null", "ids", ids)));

            if (!_service.IsSameMaster(idArr))
                return Json((0, new TextHandler().GetErrorMessage("wurl", errCode+"-2", "not same master", "ids", ids)));

            var configHandler = new ConfigHandler();
            var configDefinition = configHandler.GetConfigDefinition(ett.MasterId);
            if (!configDefinition.IsProducer)
            {
                var errMsg = new TextHandler().GetErrorMessage("noAuth", errCode, "You are not Producer", "masterId", ett.MasterId);
                return Json(new TResult( 0, errMsg));
            }
               

            var msg = await _service.DeleteByIdsAsync(ids);
            return Json(new TResult(msg == Consts.OK ? 1 : 0, msg));
        }




    }
}