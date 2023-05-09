using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.Mvc.Controllers.SYS
{
    [Area("Sys")]
    public class MenuItemController : BaseController
    {
        private readonly MenuItemService _service;

        public MenuItemController(MenuItemService service)
        {
            _service = service;
        }

        //*page
        public IActionResult Manage()
        {
            var defId =new MenuItem().Id;
            ViewBag.DefaultId = defId.ToString();
            return View();
        }

        //*modal
        public IActionResult AddEditModal()
        {
            var defId = new MenuItem().Id;
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
            var rst = new TResult<List<ManageMenuItemsDto>>(1, dtos);
            return Json(rst);

        }

        [HttpGet]
        public async Task<IActionResult> GetMenuItemTreeJsonForSelectParent(string topParentId)
        {
            //var defId = new MenuItem().Id.ToString();
            var dtos = await _service.GetListDtoTreeByParentId(topParentId, (int)MenuItemType.Directory, true);
            
            var rst = new TResult<List<TreeItem>>(1, dtos);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditDtoJson(string id)
        {
            var dto= await _service.GetAddEditDtoAsync(id);
            var rst = new TResult<AddEditMenuItemDto>(1, dto);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSequenceNoJson(string parentId)
        {
            var no = await _service.GetMaxSequenceNo(parentId);
            var rst = new TResult<int>(1, no);
            return Json(rst);
        }

        //*post
        [HttpPost]
        public async Task<IActionResult> Add(AddEditMenuItemDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            if (model.MenuType==(int)MenuType.VueClient)
            {
                if(model.Code.IsNullOrEmpty()) return Json(new TResult(0, "Code can't be null"));
                if (!StringExtension.AlphaAndNumeralExpression.IsMatch(model.Code))
                {
                    return Json(new TResult(0, "Code can only be composed of English letters and numbers"));
                }
                if (model.Type==(int)MenuItemType.Directory)
                {
                    if(model.Redirect.IsNullOrEmpty()) return Json(new TResult(0, "Redirect can't be null"));
                }
                else
                {
                    model.Redirect = "";
                }
            }
            if (model.ParentId == null) model.ParentId = new MenuItem().Id.ToString();
            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEditMenuItemDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            if (model.MenuType == (int)MenuType.VueClient)
            {
                if (model.Code.IsNullOrEmpty()) return Json(new TResult(0, "Code can't be null"));
                if (!StringExtension.AlphaAndNumeralExpression.IsMatch(model.Code))
                {
                    return Json(new TResult(0, "Code can only be composed of English letters and numbers"));
                }
                if (model.Type == (int)MenuItemType.Directory)
                {
                    if (model.Redirect.IsNullOrEmpty()) return Json(new TResult(0, "Redirect can't be null"));
                }
                else
                {
                    model.Redirect = "";
                }
            }

            if (model.ParentId == null) model.ParentId = new MenuItem().Id.ToString();
            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected(string id)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var msg = await _service.DeleteByIdStringAsync(id);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-2", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }



    }
}