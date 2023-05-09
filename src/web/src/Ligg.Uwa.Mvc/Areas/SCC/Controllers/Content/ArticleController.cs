using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SCC;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using System.Globalization;
using System;
using Ligg.Uwa.Mvc.ViewComponents;


namespace Ligg.Uwa.Mvc.Controllers.SCC
{
    [Area("Scc")]
    public class ArticleController : BaseController
    {
        private readonly ArticleService _service;
        private readonly CategoryService _categoryService;
        private readonly TenantService _tenantService;

        public ArticleController(ArticleService articleService, CategoryService categoryService, TenantService tenantService)
        {
            _service = articleService;
            _categoryService = categoryService;
            _tenantService = tenantService;
        }

        //*page
        public IActionResult Manage()
        {
            var index = (int)CategoryType.ArticleFolder;
            ViewBag.MasterType = index.ToString();

            ViewBag.MasterTypeName = EnumHelper.GetById(index, CategoryType.ArticleFolder).GetDescription();
            var objs = _categoryService.GetEnabledTopListDtosByTypeAsync(index).Result;
            var dictionary = new Dictionary<string, string>();
            foreach (var obj in objs)
            {
                dictionary.Add(obj.Id, obj.Name);
            }
            List<KeyValuePair<string, string>> dictionaryList = dictionary.ToList();
            ViewBag.TopMastersJsonString = JsonConvert.SerializeObject(dictionaryList);
            return View();
        }
        public IActionResult Show(string id, string style)
        {
            var errCode = GetErrorCode();
            var ett = _service.GetEntityByIdStringAsync(id).Result;
            if (ett == null)
                return Redirect("~/Home/Error/NotExist/id/" + id + "/" + errCode + "-1");
            style = style ?? "article";
            if (EnumHelper.IsOutofScope<ShowArticleType>(style, true))
                return Redirect("~/Home/Error/wurl/style/" + style + "/" + errCode + "-2");

            if (ett.Type == (int)ArticleType.RichText | ett.Type == (int)ArticleType.HtmlText | ett.Type == (int)ArticleType.MarkdownText | ett.Type == (int)ArticleType.ArticleShortcut)
            {
                ViewBag.Keywords = "tags";
                ViewBag.Description = ett.Description;
                ViewBag.PageUrl = "/Scc/Article/Show" + "?id=" + ett.Id + "&style=" + style ?? "article";
                ViewBag.IcoUrl = "/favicon.ico";
                var currentTenant = _tenantService.GetCurrentShowDtoAsync().Result;
                if(currentTenant.Id=="0")
                {
                    ViewBag.IcoUrl = Url.Content("~/images/tenant/ico.ico");
                }
                else
                {
                    if (currentTenant.HasIco == 1)
                    {
                        var fileName = "ico.ico";
                        ViewBag.IcoUrl = Url.Content("~/File/GetImage/Attachment/TenantIco/" + currentTenant.Id) + "?fileName=" + (fileName) + "&random=" + new Random().Next();
                    }
                }


                ViewBag.ObjectId = ett.Type == (int)ArticleType.ArticleShortcut ? ett.Body : id;
                ViewBag.Option = style ?? "article";
                return View();
            }
            else if (ett.Type == (int)ArticleType.OutLink)
            {
                return Redirect(ett.Body);
            }

            return Redirect("~/Home/Error/Wurl/id/" + id + "/" + "/" + errCode + "-3");
        }

        //*modal
        public IActionResult AddEditModal()
        {
            return View();
        }

        public IActionResult EditBodyModal(string index, string id)
        {
            var errCode = GetErrorCode();
            var ett = _service.GetEntityByIdStringAsync(id).Result;
            if (ett == null) return Redirect("~/Home/Error/NotExist/id/" + id + "/" + errCode);

            if (index == "insertMarddownBodyImage")
            {
                ViewBag.Index = WebFileUploadType.Attachment.ToString();
                ViewBag.Object = "ArticleAttachedImage";
                ViewBag.ObjectId = id;
                ViewBag.NewFileTitle = "";
                ViewBag.ShowInitImage = false;
                ViewBag.InitFileName = "";

                ViewBag.SaveDatabaseUrl = "";
                return View("FileHandlers/UploadImageModal");
            }
            else if (index == "insertMarkdownBodyTable")
            {
                return View("insertMarkdownBodyTableModal");
            }
            else
            {
                var typeName = EnumHelper.GetNameById<ArticleType>(ett.Type);
                if (ett.Type == (int)ArticleType.HtmlText)
                {
                    ViewBag.Index = WebFileUploadType.Attachment.ToString();
                    ViewBag.Object = "ArticleAttachedImage";
                    ViewBag.ObjectId = id;
                    ViewBag.NewFileTitle = "";
                    ViewBag.ShowInitImage = false;
                    ViewBag.InitFileName = "";
                    ViewBag.UploadType = "update";
                }
                return View("Edit" + typeName + "BodyModal");
            }
        }

        public IActionResult UploadAttachmentModal(string index, string id)
        {
            var errCode = GetErrorCode();
            index = index.ToLower();
            if (index != "squarethumbnail")
            {
                return Redirect("~/Home/Error/NotExist/index/" + index + "/" + errCode + "-1");
            }
            var ett = _service.GetEntityByIdStringAsync(id).Result;
            if (ett == null) return Redirect("~/Home/Error/NotExist/id/" + id + "/" + errCode + "-2");

            ViewBag.Index = WebFileUploadType.Attachment.ToString();
            ViewBag.Object = "Article" + (index == "squarethumbnail" ? "thumbnail" : index);
            ViewBag.ObjectId = id;
            ViewBag.NewFileTitle = "";
            ViewBag.UploadType = "update";
            ViewBag.SaveDatabaseUrl = "Scc/Article/UpdateAttachment/" + index + "?id=" + id;

            ViewBag.ShowInitImage = !ett.ThumbnailPostfix.IsNullOrEmpty();
            ViewBag.InitFileName = "thumbnail" + ett.ThumbnailPostfix;
            ViewBag.Mode = "SingleMode";
            return View("FileHandlers/UploadThumbnailsModal");

        }



        //*get
        [HttpGet]
        public async Task<IActionResult> GetPagedManageDtosJson(ListArticlesReqArgs param, Pagination pagination)
        {
            var dtos = await _service.GetPagedManageDtosAsync(param, pagination, true);
            var location = WebFileHelper.GetLocation(WebFileUploadType.Attachment);
            if (!location.IsNullOrEmpty())
            {
                foreach (var dto in dtos)
                {
                    var AttachedFilesDir = "Article\\" + WebFileHelper.GetDateTimeFlag(dto.CreationTime) + "\\" + dto.Id.ToString() + "\\AttachedFiles";
                    var AttachedImagesDir = "Article\\" + WebFileHelper.GetDateTimeFlag(dto.CreationTime) + "\\" + dto.Id.ToString() + "\\AttachedImages";
                    var AttachedVideosDir = "Article\\" + WebFileHelper.GetDateTimeFlag(dto.CreationTime) + "\\" + dto.Id.ToString() + "\\AttachedVideos";
                    var dir = WebFileHelper.GetAbsolutePath(location, AttachedFilesDir);
                    if (Directory.Exists(dir))
                    {
                        dto.AttachedFilesNum = DirectoryHelper.GetSubFilesNum(dir, false);
                    }
                    dir = WebFileHelper.GetAbsolutePath(location, AttachedImagesDir);
                    if (Directory.Exists(dir))
                    {
                        dto.AttachedImagesNum = DirectoryHelper.GetSubFilesNum(dir, false);
                    }
                }
            }
            var rst = new TResult<List<ManageArticlesDto>>(1, dtos);
            rst.Total = pagination.Total;
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedListModelsJson(ListArticlesReqArgs param, Pagination pagination)
        {
            param.Status = 1;
            var dtos = await _service.GetPagedManageDtosAsync(param, pagination, false);
            var location = WebFileHelper.GetLocation(WebFileUploadType.Attachment);
            var mdls = new List<ListArticlesModel>();
            foreach (var dto in dtos)
            {
                if (!location.IsNullOrEmpty())
                {
                    var AttachedFilesDir = "Article\\" + WebFileHelper.GetDateTimeFlag(dto.CreationTime) + "\\" + dto.Id.ToString() + "\\AttachedFiles";
                    var AttachedImagesDir = "Article\\" + WebFileHelper.GetDateTimeFlag(dto.CreationTime) + "\\" + dto.Id.ToString() + "\\AttachedImages";
                    var AttachedVideosDir = "Article\\" + WebFileHelper.GetDateTimeFlag(dto.CreationTime) + "\\" + dto.Id.ToString() + "\\AttachedVideos";
                    var dir = WebFileHelper.GetAbsolutePath(location, AttachedFilesDir);
                    if (Directory.Exists(dir))
                    {
                        dto.AttachedFilesNum = DirectoryHelper.GetSubFilesNum(dir, false);
                    }
                    dir = WebFileHelper.GetAbsolutePath(location, AttachedImagesDir);
                    if (Directory.Exists(dir))
                    {
                        dto.AttachedImagesNum = DirectoryHelper.GetSubFilesNum(dir, false);
                    }
                }
                var mdl = new ListArticlesModel();
                mdl = dto.MapTo<ListArticlesModel>();
                if (!mdl.Description.IsNullOrEmpty()) mdl.Description = mdl.Description.Replace("\n", "<br>");
                mdl.Type = EnumHelper.GetById<ArticleType>(dto.Type, ArticleType.HtmlText).GetDescription();
                mdl.HumanizedModificationTime = dto.ModificationTime.Humanize(utcDate: false, culture: new CultureInfo("zh-CN"));
                mdl.CreationTime = dto.CreationTime.ToString("yy-MM-dd");
                mdl.ModificationTime = dto.ModificationTime.ToString("yy-MM-dd");
                mdl.HasThumbnail = dto.HasThumbnail == (int)HasOrNone.Has ? HasOrNone.Has.GetDescription() : HasOrNone.None.GetDescription();
                mdl.HasImage = dto.HasImage == (int)HasOrNone.Has ? HasOrNone.Has.GetDescription() : HasOrNone.None.GetDescription();
                mdl.HasVideo = dto.HasVideo == (int)HasOrNone.Has ? HasOrNone.Has.GetDescription() : HasOrNone.None.GetDescription();
                mdls.Add(mdl);
            }

            var rst = new TResult<List<ListArticlesModel>>(1, mdls);
            rst.Total = pagination.Total;
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetListModelsJson(string index, string obj, string objId, int num)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var selectedFileds = obj;
            if (selectedFileds.IsNullOrEmpty())
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "SelectedFileds can't be empty", obj, "null");
                return Json(new TResult(0, errMsg));
            }
            var dtos = new List<ManageArticlesDto>();
            if (index.ToLower() == "ByMasterId".ToLower())
            {
                var masterId = objId;
                var master = await _categoryService.GetEntityByTypeAndIdStringAsync((int)CategoryType.ArticleFolder, masterId);
                if (master == null)
                {
                    errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-2", "Incorrect masterId", "index", index);
                    return Json(new TResult(0, errMsg));
                }
                if (num == 0)
                {
                    errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-3", "Incorrect num", "num", num + "");
                    return Json(new TResult(0, errMsg));
                }
                //check masterId permission
                dtos = await _service.GetListDtosByMasterIdAsync(masterId, num);
            }
            else if (index.ToLower() == "ByIds".ToLower())
            {
                var ids = objId;
                var idArray = ids.Split(',').Wash();
                //check Ids permission
                dtos = await _service.GetListDtosByIdArrayAsync(idArray);
            }
            else
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-4", "Incorrect getDataType", "index", index);
                return Json(new TResult(0, errMsg));
            }

            var mdls = dtos.MapToList<ListArticlesModel>();
            foreach (var mdl in mdls)
            {
                if (!mdl.Description.IsNullOrEmpty())
                    mdl.Description = mdl.Description.Replace("\n", "<br>");
            }
            var data = mdls.Select(DynamicExpressionEx.FieldFilter<ListArticlesModel>(selectedFileds)).ToList();
            var rst = new TResult<List<ListArticlesModel>>(1, data);
            return Json(rst);
        }


        [HttpGet]
        public async Task<IActionResult> GetCategoryTreeJsonForManage(string topMasterId)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var topMaster = await _categoryService.GetEntityByTypeAndIdStringAsync((int)CategoryType.ArticleFolder, topMasterId);
            if (topMaster == null)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "Incorrect topMasterId", "topMasterId", topMasterId);
                return Json(0, errMsg);
            }

            var dtos = await _categoryService.GetListDtoTreeByParentId(topMasterId, true, (int)CategoryType.ArticleFolder);
            //add auth filter
            var rst = new TResult<List<TreeItem>>(1, dtos);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryTreeJsonForSelectMaster()
        {
            var catId = new Category().Id.ToString();
            var dtos = await _categoryService.GetListDtoTreeByParentId(catId, false, (int)CategoryType.ArticleFolder);
            var rst = new TResult<List<TreeItem>>(1, dtos);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditDtoJson(string id)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var dto = await _service.GetAddEditDtoAsync(id);
            if (dto == null)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "", "id", id);
                return Json(0, errMsg);
            }
            dto.Body = "";
            return Json(new TResult<AddEditArticleDto>(1, dto));


        }

        [HttpGet]
        public async Task<IActionResult> GetEditBodyDtoJson(string id)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var ett = await _service.GetEntityByIdStringAsync(id);
            if (ett == null)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "", "id", id);
                return Json(0, errMsg);
            }
            var dto = ett.MapTo<EditArticleBodyDto>();
            return Json(new TResult<EditArticleBodyDto>(1, dto));

        }

        [HttpGet]
        public async Task<IActionResult> GetShowModelJson(string index, string obj, string objId, string option)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            //check Id permission
            var selectedFileds = obj;
            var ett = await _service.GetEntityByIdStringAsync(objId);
            if (ett == null)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "Incorrect id", "objId", objId);
                return Json(0, errMsg);
            }
            if (ett.Status == false)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-2", "Incorrect id: disabled article could not be shown", "objId", objId);
                return Json(0, errMsg);
            }
            if (ett.Type != (int)ArticleType.HtmlText & ett.Type != (int)ArticleType.RichText & ett.Type != (int)ArticleType.MarkdownText)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-3", "Incorrect type: only 'HtmlText' and 'MarkdownText' can be shown", "objId", objId);
                return Json(0, errMsg);
            }

            var mdl = ett.MapTo<ShowArticleModel>();
            var mdl1 = mdl;
            if (selectedFileds.ToLower() != "none" & selectedFileds.ToLower() != "all"! & selectedFileds.IsNullOrEmpty())
            {
                var mdls = new List<ShowArticleModel>();
                mdls.Add(mdl);
                var mdls1 = mdls.Select(DynamicExpressionEx.FieldFilter<ShowArticleModel>(selectedFileds)).ToList();
                mdl1 = mdls1.FirstOrDefault();
            }

            mdl1.Type = EnumHelper.GetNameById<ArticleType>(ett.Type);
            if (!mdl1.Description.IsNullOrEmpty())
                mdl1.Description = ett.Description.Replace("\n", "<br>");
            mdl1.HumanizedModificationTime = ett.ModificationTime.Humanize(utcDate: false, culture: new CultureInfo("zh-CN"));
            mdl1.CreationTime = ett.CreationTime.ToString("yy-MM-dd");
            var newDt = ett.ModificationTime ?? DateTime.MinValue;
            mdl1.ModificationTime = ett.ModificationTime != null ? newDt.ToString("yy-MM-dd") : "";
            if (ett.Type == (int)ArticleType.RichText & !ett.Body.IsNullOrEmpty())
            {
                var arr = mdl1.Body.Split('\n');
                string[] arr1 = new string[arr.Length];
                var ct = 0;
                foreach (var txt in arr)
                {
                    var txt1 = "<p>" + txt + "</p>";
                    arr1[ct] = txt1;
                    ct++;
                }
                mdl1.Body = arr1.Unwrap();
            }

            if (!option.IsNullOrEmpty())
            {
                option = option.ToLower();
                if (option == "knownledge")
                {
                    //get tag and cat links
                }
            }

            return Json(new TResult<ShowArticleModel>(1, mdl1));
        }

        [HttpGet]
        public async Task<IActionResult> GetMaxSequenceNoJson(string masterId)
        {
            var no = await _service.GetMaxSequenceNoAsync(masterId);
            return Json(new TResult<int>(1, no));
        }

        //*post
        [HttpPost]
        public async Task<IActionResult> Add(AddEditArticleDto model)
        {
            var errCode = GetErrorCode();
            var msg = await _service.SaveAddEditDtoAsync(model);
            var errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }
        public async Task<IActionResult> Edit(AddEditArticleDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var ett = await _service.GetEntityByIdStringAsync(model.Id);
            if (ett == null)
            {
                errMsg = new TextHandler().GetErrorMessage("notExist", errCode + "-1", "", "model.Id", model.Id);
                return Json(0, errMsg);
            }

            model.Body = ett.Body;
            model.ImagePostfix = ett.ImagePostfix;
            model.ThumbnailPostfix = ett.ThumbnailPostfix;
            model.Body = ett.Body;
            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        public async Task<IActionResult> EditBody(EditArticleBodyDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var ett = await _service.GetEntityByIdStringAsync(model.Id);

            if (ett == null)
            {
                errMsg = new TextHandler().GetErrorMessage("notExist", errCode + "-1", "", "model.Id", model.Id);
                return Json(0, errMsg);

            }
            ett.Body = model.Body;
            var msg = await _service.UpdateEntityAsync(ett);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAttachment(string index, string id, string fileName)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            index = index.ToLower();
            if (index != "squarethumbnail")
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "", "index", index);
                return Json(new TResult(0, errMsg));
            }
            if (index == "squarethumbnail") index = "thumbnail";
            if (fileName.IsNullOrEmpty()) return Json(new TResult(0, "Upload failed"));
            var ett = await _service.GetEntityByIdStringAsync(id);
            if (ett == null)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-2", "", "index", index);
                return Json(new TResult(0, errMsg));
            }

            var newPostfix = Path.GetExtension(fileName);
            var location = WebFileHelper.GetLocation(WebFileUploadType.Attachment);
            var relativeDir = "Article\\" + WebFileHelper.GetDateTimeFlag(ett.CreationTime) + id;
            var absoluteDir = WebFileHelper.GetAbsolutePath(location, relativeDir);
            if (index == "thumbnail")
            {
                var oldThumbnailPostfix = ett.ThumbnailPostfix;
                var filePath = Path.Combine(absoluteDir, "thumbnail" + oldThumbnailPostfix);
                if (System.IO.File.Exists(filePath) & oldThumbnailPostfix != newPostfix)
                    System.IO.File.Delete(filePath);
                ett.ThumbnailPostfix = newPostfix;
            }

            var msg = await _service.UpdateEntityAsync(ett);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-3", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected(string ids)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            //*if not fake detetion entity
            var location = WebFileHelper.GetLocation(WebFileUploadType.Attachment);
            var idArr = ids.Split(',');
            if (!_service.IsSameMaster(idArr))
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1");
                return Json(new TResult(0, errMsg));
            }
            var etts = await _service.GetEntitiesByIdsAsync(idArr);

            var msg = await _service.DeleteByIdsAsync(ids);
            if (msg == Consts.OK)
            {
                if (!location.IsNullOrEmpty())
                {
                    foreach (var ett in etts)
                    {
                        var relativeDir = "Article\\" + WebFileHelper.GetDateTimeFlag(ett.CreationTime) + "\\" + ett.Id.ToString();
                        var dir = WebFileHelper.GetAbsolutePath(location, relativeDir);
                        if (Directory.Exists(dir))
                        {
                            Directory.Delete(dir, true);
                        }
                    }
                }
            }
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-2", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelectedAttachedFiles(string masterId, string names, string type)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var master = await _service.GetEntityByIdStringAsync(masterId);
            if (master == null)
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "masterId", masterId);
                return Json(new TResult(0, errMsg));
            }

            var location = WebFileHelper.GetLocation(WebFileUploadType.Attachment);
            var relativeDir = "Article\\" + WebFileHelper.GetDateTimeFlag(master.CreationTime) + "\\" + masterId + (type == "image" ? "\\AttachedImages" : (type == "video" ? "\\AttachedVideos" : "\\AttachedFiles"));
            var absoluteDir = WebFileHelper.GetAbsolutePath(location, relativeDir);
            var nameArr = names.Split(',');
            var msg = Consts.OK;
            try
            {
                foreach (var fileName in nameArr)
                {
                    var filePath = Path.Combine(absoluteDir, fileName);
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-2", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }



    }
}