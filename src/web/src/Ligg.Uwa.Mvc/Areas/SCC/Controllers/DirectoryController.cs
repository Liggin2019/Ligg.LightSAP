﻿using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SCC;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ligg.Uwa.Mvc.Controllers.SCC
{
    [Area("Scc")]
    public class DirectoryController : BaseController
    {
        private readonly CategoryService _service;
        public DirectoryController(CategoryService service)
        {
            _service = service;
        }

        //*page
        public IActionResult Manage(int index)
        {
            var errCode = GetErrorCode();
            var type = index;
            if (type != (int)CategoryType.ArticleFolder)
            {
                return Redirect("~/Home/Error/wurl/index/" + index + "/" + errCode + "-1");
            }

            ViewBag.Controller = "Directory";
            ViewBag.Type = index;
            ViewBag.TypeName = EnumHelper.GetById(index, CategoryType.ArticleFolder).GetDescription();
            ViewBag.DefaultId = new Category().Id.ToString();
            return View("Category/Manage");

        }

        //*modal
        public IActionResult AddEditModal()
        {
            ViewBag.Controller = "Directory";
            ViewBag.DefaultId = new Category().Id.ToString();
            return View("Category/AddEditModal");
        }
        public IActionResult AddTopModal()
        {
            ViewBag.Controller = "Directory";
            ViewBag.DefaultId = new Category().Id.ToString();
            return View("Category/AddEditModal");
        }


        //*get
        [HttpGet]
        public async Task<IActionResult> GetManageDtosJson(CommonReqArgs param)
        {
            var dtos = await _service.GetManageDtosAsync(param);
            var rst = new TResult<List<ManageCategoriesDto>>(1, dtos);
            return Json(rst);

        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryTreeJsonForSelectParent(int type, string id)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            if (type != (int)CategoryType.ArticleFolder)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "Incorrect type", "type", type + "");
                return Json(new TResult(0, errMsg));
            }

            var defId = new Category().Id.ToString();
            var dtos = new List<TreeItem>();
            dtos = await _service.GetListDtoTreeByParentId(defId, false, type);

            var rst = new TResult<List<TreeItem>>(1, dtos);
            return Json(rst);
        }


        [HttpGet]
        public async Task<IActionResult> GetEditDtoJson(int type, string id)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            if (type != (int)CategoryType.ArticleFolder)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "Incorrect type", "type", type + "");
                return Json(new TResult(0, errMsg));
            }

            var dto = await _service.GetAddEditDtoAsync(type, id);
            var rst = new TResult<AddEditCategoryDto>(1, dto);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSequenceNoJson(int type, string parentId)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            if (type != (int)CategoryType.ArticleFolder)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "Incorrect type", "type", type + "");
                return Json(new TResult(0, errMsg));
            }

            var no = await _service.GetMaxSequenceNoAsync(parentId, type);
            var rst = new TResult<int>(1, no);
            return Json(rst);
        }

        //*post
        [HttpPost]
        public async Task<IActionResult> Add(AddEditCategoryDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            if (model.ParentId == null) model.ParentId = new Category().Id.ToString();
            var sGuid = System.Guid.NewGuid().GetShortGuid();
            model.ShortGuid = sGuid;
            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            if (msg != Consts.OK) return Json(new TResult(0, errMsg));

            msg = Consts.OK;
            model.Description = "";
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-2", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEditCategoryDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            if (model.ParentId == null) model.ParentId = new Category().Id.ToString();
            model.Description = model.Description ?? "";
            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-2", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected(int type, string id)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            if (type != (int)CategoryType.ArticleFolder)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "Incorrect type", "type", type + "");
                return Json(new TResult(0, errMsg));
            }

            {
                var rst = await _service.CheckChildExistence(type, id, null);
                if (rst) return Json(new TResult(0, "其下有对象存在，不能删除"));
            }

            var msg = await _service.DeleteByIdStringAsync(type, id);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-2", msg) : "";
            if (msg != Consts.OK) return Json(new TResult(0, errMsg));

            msg = Consts.OK;
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-2", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));

        }



    }
}