using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.Mvc.Controllers.SYS
{
    [Area("Sys")]
    public class TenantController : BaseController
    {
        private readonly TenantService _service;
        public TenantController(TenantService tenantService)
        {
            _service = tenantService;
        }

        //*page
        public IActionResult Manage()
        {
            var defId = new Tenant().Id.ToString();
            ViewBag.DefaultId = defId;
            return View();
        }

        //*modal
        public IActionResult AddEditModal()
        {
            return View();
        }

        public IActionResult UploadAttachmentModal(string index, string id)
        {
            var errCode = GetErrorCode();
            if (id == new Tenant().Id.ToString())
            {
                var rst = new TResult(0, "");
                return Redirect("~/Home/Error/wurl/Tenant/public/"+ errCode + "-1");
            }

            index = index.ToLower();
            if (index != "squarethumbnail")
            {
                return Redirect("~/Home/Error/NotExist/index/" + index);
            }
            var ett = _service.GetEntityByIdStringAsync(id).Result;
            if (ett == null) return Redirect("~/Home/Error/NotExist/Entity/null/" + errCode + "-2");

            ViewBag.Index = WebFileUploadType.Attachment.ToString();
            ViewBag.Object = "Tenant" + (index == "squarethumbnail" ? "thumbnail" : index);
            ViewBag.ObjectId = id;
            ViewBag.NewFileTitle = "";
            ViewBag.UploadType = "update";
            ViewBag.SaveDatabaseUrl = "Sys/Tenant/UpdateAttachment/" + index + "?id=" + id;

            ViewBag.ShowInitImage = !ett.ThumbnailPostfix.IsNullOrEmpty();
            ViewBag.InitFileName = "thumbnail" + ett.ThumbnailPostfix;
            ViewBag.Mode = "SingleMode";
            return View("FileHandlers/UploadThumbnailsModal");
        }

        //*get
        [HttpGet]
        public async Task<IActionResult> GetPagedManageDtosJson(CommonReqArgs param, Pagination pagination)
        {
            var dtos = await _service.GetPagedManageDtosAsync(param, pagination);
            var rst = new TResult<List<ManageTenantsDto>>(1, dtos);
            rst.Total = pagination.Total;
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditDtoJson(string id)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            if (id == new Tenant().Id.ToString())
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "公共租户不能修改");
                return Json(new TResult(0, errMsg));
                //throw new ArgumentException("公共租户不能修改");
            }

            var dto = await _service.GetAddEditDtoAsync(id);
            if (dto == null)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "租户不存在","id",id);
                return Json(new TResult(0, errMsg));
            }
            return Json(new TResult<AddEditTenantDto>(1, dto));

        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSequenceNoJson()
        {
            var no = await _service.GetMaxSequenceNoAsync();
            var rst = new TResult<int>(1, no);
            return Json(rst);
        }

        //*post
        [HttpPost]
        public async Task<IActionResult> Add(AddEditTenantDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var rst = new TResult(0, "");
            if (!StringExtension.EnglishExpression.IsMatch(model.Key))
            {
                rst.Message = "键只能是英文字母";
                return Json(rst);
            }
            if (!StringExtension.EnglishExpression.IsMatch(model.Code))
            {
                rst.Message = "编号只能是英文字母";
                return Json(rst);
            }
            model.Description = "";
            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }
        public async Task<IActionResult> Edit(AddEditTenantDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var rst = new TResult(0, "");
            if (model.Id==new Tenant().Id.ToString())
            {
                rst.Message = "公共租户不能修改";
                return Json(rst);
            }
            if (!StringExtension.EnglishExpression.IsMatch(model.Key))
            {
                rst.Message = "键只能是英文字母";
                return Json(rst);
            }
            if (!StringExtension.EnglishExpression.IsMatch(model.Code))
            {
                rst.Message = "编号只能是英文字母";
                return Json(rst);
            }


            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected(string ids)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var msg = await _service.DeleteByIdsAsync(ids);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        public async Task<IActionResult> UpdateAttachment(string index, string id, string fileName)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            if (id == new Tenant().Id.ToString())
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "公共租户不能修改");
                return Json(new TResult(0, errMsg));
            }

            index = index.ToLower();
            if (index != "thumbnail" & index != "squarethumbnail" & index != "image" & index != "ico")
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-2");
                return Json(new TResult(0, errMsg));
            }
            if (index == "squarethumbnail") index = "thumbnail";
            if (fileName.IsNullOrEmpty())
            {
                errMsg = new TextHandler().GetErrorMessage("notExist", errCode + "-3","","fileName" ,fileName);
            }

            var ett = await _service.GetEntityByIdStringAsync(id);
            if (ett == null) return Json(new TResult(0, "Incorrect url"));

            var newPostfix = Path.GetExtension(fileName);
            var location = WebFileHelper.GetLocation(WebFileUploadType.Attachment);
            var relativeDir = "Tenant\\" + WebFileHelper.GetDateTimeFlag(ett.CreationTime) + id;
            var absoluteDir = WebFileHelper.GetAbsolutePath(location, relativeDir);
            if (index == "thumbnail")
            {
                var oldThumbnailPostfix = ett.ThumbnailPostfix;
                var filePath = Path.Combine(absoluteDir, "thumbnail" + oldThumbnailPostfix);
                if (System.IO.File.Exists(filePath) & oldThumbnailPostfix != newPostfix)
                    System.IO.File.Delete(filePath);
                ett.ThumbnailPostfix = newPostfix;
            }
            else if (index == "image")
            {
                var oldImagePostfix = ett.ImagePostfix;
                var filePath = Path.Combine(absoluteDir, "image" + oldImagePostfix);
                if (System.IO.File.Exists(filePath) & oldImagePostfix != newPostfix)
                    System.IO.File.Delete(filePath);

                ett.ImagePostfix = newPostfix;
            }
            else //ico
            {
                ett.HasIco=true;
            }

            var msg = await _service.UpdateEntityAsync(ett);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-4", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> SetAsDefault(string id)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            if (id == new Tenant().Id.ToString())
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "公共租户不能修改");
                return Json(new TResult(0, errMsg));
            }
            var msg = await _service.SetAsDefault(id);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-2", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }


    }
}