using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ligg.Uwa.Mvc.Controllers.SYS
{
    [Area("Sys")]
    public class UserController : BaseController
    {
        private readonly TenantService _tenantService;
        private readonly UserService _service;
        private readonly OrganizationService _organizationService;
        private readonly UserGroupService _userGroupService;
        public UserController(TenantService tenantService, UserService userService, OrganizationService organizationService, UserGroupService userGroupService)
        {
            _tenantService = tenantService;
            _service = userService;
            _organizationService = organizationService;
            _userGroupService = userGroupService;
        }

        //*page
        public IActionResult Manage()
        {
            return View();
        }
        public IActionResult ManageSelf()
        {
            var errCode = GetErrorCode();

            var oprt = CurrentOperator.Instance.GetCurrent();
            if (oprt == null)
                return Redirect("/Home/Error/noAuth/CurrentOperator/null" + "/" + errCode);
            ViewBag.CurrentUser = oprt.MapTo<ShowSelfModel>();
            return View();
        }
        public IActionResult Logon(string portal)
        {
            var errCode = GetErrorCode();
            if(portal.IsNullOrEmpty())
                return Redirect("/Home/Error/wurl/portal/" + "null" + "/" + errCode + "-1");

            var currentTenant = _tenantService.GetCurrentShowDtoAsync().Result;
            ViewBag.CurrentTenant = currentTenant;
            var configHandler = new ConfigHandler();

            ConfigItem currentPortal = null;
            var adminPortals = configHandler.GetConfigItems(((int)OrpConfigSubType.MvcAdminPortal).ToString(), GlobalContext.SystemSetting.ApplicationServer);
            currentPortal = adminPortals.Find(x => x.Attribute2.ToLower() == portal.ToLower());
            if (currentPortal == null)
            {
                var sitePortals = configHandler.GetConfigItems(((int)OrpConfigSubType.MvcSitePortal).ToString(), GlobalContext.SystemSetting.ApplicationServer);
                currentPortal = sitePortals.Find(x => x.Attribute2.ToLower() == portal.ToLower());
            }

            if (currentPortal == null)
            {
                return Redirect("/Home/Error/wurl/portal/" + (portal ?? "null") + "/" + errCode + "-2");
            }
            ViewBag.CurrentPortal = currentPortal;

            return View();
        }

        //*modal
        public IActionResult AddEditModal()
        {
            return View();
        }

        public IActionResult SelectBelongedUserGroupsModal(int type, string id)
        {
            ViewBag.Url = "/Sys/User/GetUserGroupDictItemsJson" + "?type=" + type;
            ViewBag.UpdateDataUrl = "/Sys/User/GetBelongedUserGroupIdsJson?type=" + type + "&id=" + id;
            return View("Selectors/RelatedIdsSelector");
        }

        public IActionResult ResetPasswordModal()
        {
            return View();
        }

        public IActionResult ChangeSelfPasswordModal()
        {
            var errCode = GetErrorCode();
            var oprt = CurrentOperator.Instance.GetCurrent();
            if (oprt == null)
                return Redirect("/Home/Error/wurl/CurrentOperator/null" + "/" + errCode + "-1");

            ViewBag.Account = oprt.Account.ToString();
            return View();
        }
        public IActionResult ChangeSelfModal()
        {
            var errCode = GetErrorCode();
            var oprt = CurrentOperator.Instance.GetCurrent();
            if (oprt == null)
                return Redirect("/Home/Error/wurl/CurrentOperator/null" + "/" + errCode + "-1");

            var ett = _service.GetEntityByIdStringAsync(oprt.ActorId).Result;
            var user = ett.MapTo<AddEditUserDto>();
            ViewBag.User = user;
            return View();
        }
        public IActionResult UploadAttachmentModal(string index, string id)
        {
            return UploadAttachmentModalPrivate(index, id, false);
        }


        public IActionResult UploadSelfAttachmentModal(string index)
        {
            var currentUsr = CurrentOperator.Instance.GetCurrent();
            var id = currentUsr.ActorId;
            return UploadAttachmentModalPrivate(index, id, true);
        }


        //*get
        [HttpGet]
        public async Task<IActionResult> GetPagedManageDtosJson(ListUsersReqArgs param, Pagination pagination)
        {
            var dtos = await _service.GetPagedManageDtosAsync(param, pagination);
            var rst = new TResult<List<ManageUsersDto>>(1, dtos);
            rst.Total = pagination.Total;
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrganizationTreeJsonForManage()
        {
            var orgId = new Organization().Id.ToString();
            var dtos = await _organizationService.GetListDtoTreeByParenId(orgId, true);
            var rst = new TResult<List<TreeItem>>(1, dtos);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrganizationTreeJsonForSelectMaster()
        {
            var orgId = new Organization().Id.ToString();
            var dtos = await _organizationService.GetListDtoTreeByParenId(orgId, true);
            var rst = new TResult<List<TreeItem>>(1, dtos);
            return Json(rst);
        }

        //*test 
        //can't update data by js, can be used for multiSelector
        public async Task<IActionResult> GetRoleGroupDictsJson()
        {
            var type = (int)UserGroupType.Role;
            var dtos = await _userGroupService.GetListDtoGroupDictsByType(type);
            var rst = new TResult<List<GroupDict>>(1, dtos);
            return Json(rst);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserGroupDictItemsJson(int type)
        {
            var dtos = await _userGroupService.GetListDtoDictItemsByType(type);
            var rst = new TResult<List<DictItem>>(1, dtos);
            return Json(rst);
        }
        [HttpGet]
        public async Task<IActionResult> GetBelongedUserGroupIdsJson(int type, string id)
        {
            var str = await _userGroupService.GetIdsStringByTypeAndUserIdAsync(type, id);
            var dto = new RelatedIdsDto();
            dto.RelatedIds = str;
            var rst = new TResult<RelatedIdsDto>(1, dto);
            return Json(rst);
        }
        [HttpGet]
        public async Task<IActionResult> GetEditDtoJson(string id)
        {
            var dto = await _service.GetAddEditDtoAsync(id);
            if (dto != null)
            {
                return Json(new TResult<AddEditUserDto>(1, dto));
            }
            else return Json(new TResult(0));

        }

        [HttpGet]
        public async Task<IActionResult> GetSelfShowModelJson()
        {
            var errCode = GetErrorCode();
            var oprt = CurrentOperator.Instance.GetCurrent();
            if (oprt == null)
                return Redirect("/Home/Error/wurl/CurrentOperator/null" + "/" + errCode + "-1");

            var mdl = await _service.GetShowModelAsync(oprt.ActorId);
            if (mdl != null)
            {
                return Json(new TResult<GetSelfModel>(1, mdl));
            }
            else return Json(new TResult(0));
        }

        //*post
        [HttpPost]
        public async Task<IActionResult> Add(AddEditUserDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AddEditUserDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";

            var msg = await _service.SaveAddEditDtoAsync(model);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBelongedUserGroups(int type, RelatedIdsDto model)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            //var type = (int)UserGroupType.Role;
            var msg = await _service.UpdateBelongedUserGroups(model, type);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ChangePasswordDto Dto)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var msg = await _service.ResetUserPassword(Dto);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAttachment(string index, string id, string fileName)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            if (fileName.IsNullOrEmpty())
            {
                errMsg =new TextHandler().GetErrorMessage("wurl", errCode + "-1");
                return Json(new TResult(0, errMsg));
            }
            var ett = await _service.GetEntityByIdStringAsync(id);
            if (ett == null) 
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-2");
                return Json(new TResult(0, errMsg));
            }
            var msg = await UpdateAttachmentPrivate(index, ett, fileName);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));

        }

        [HttpPost]
        public async Task<IActionResult> UpdateSelf(AddEditUserDto dto)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var oprt = CurrentOperator.Instance.GetCurrent();
            if (oprt == null)
                return Redirect("/Home/Error/wurl/CurrentOperator/null" + "/" + errCode+"-1");

            if (dto.Id != oprt.ActorId) Redirect("/Home/Error/wurl/CurrentOperator/null" + "/" + errCode + "-2");
            var ett = await _service.GetEntityByIdStringAsync(dto.Id);
            ett.Name = dto.Name;
            ett.Birthday = dto.Birthday;
            ett.Gender = dto.Gender;
            ett.Email = dto.Email;
            ett.Mobile = dto.Mobile;
            ett.Qq = dto.Qq;
            ett.WeChat = dto.WeChat;
            ett.Description = dto.Description;
            var msg = await _service.UpdateEntityAsync(ett);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeSelfPassword(ChangePasswordDto dto)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var oprt = CurrentOperator.Instance.GetCurrent();
            if (oprt == null)
                return Redirect("/Home/Error/wurl/CurrentOperator/null" + "/" + errCode + "-1");

            dto.Id = oprt.ActorId;
            var msg = await _service.ChangeSelfPassword(dto);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSelfAttachment(string index, string fileName)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var currentOprt = CurrentOperator.Instance.GetCurrent();
            var id = currentOprt.ActorId;
            if (fileName.IsNullOrEmpty()) return Json(new TResult(0, "Upload failed"));
            var ett = await _service.GetEntityByIdStringAsync(id);
            if (ett == null) return Json(new TResult(0, "Incorrect id"));
            var msg = await UpdateAttachmentPrivate(index, ett, fileName);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";

            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSelected(string ids)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var idArr = ids.Split(',');
            if (!_service.IsSameMaster(idArr))
            {
                errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1");
                return Json(new TResult(0, errMsg));
            }

            var msg = await _service.DeleteByIdsAsync(ids);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-1", msg) : "";
            return Json(new TResult(msg == Consts.OK ? 1 : 0, errMsg));
        }

        [HttpPost]
        public async Task<IActionResult> Logon(string account, string password, string captchaCode)
        {
            var rst = new TResult();
            if (string.IsNullOrEmpty(captchaCode))
            {
                rst.Message = "验证码不能为空";
                return Json(rst);
            }
            if (captchaCode != ObjectHelper.ParseToString(SessionHelper.GetSession("CaptchaCode")))
            {
                rst.Message = "验证码错误，请重新输入";
                return Json(rst);
            }

            var oprtRst = await _service.Login(account, password, (int)WebClientType.Mvc);
            if (oprtRst.Flag == 1)
            {
                var oprt = oprtRst.Data;
                CurrentOperator.Instance.AddCurrent(oprt.Id.ToString());
                var userRepo = new UserDbRepository();
                userRepo.UpdateOperator(oprt);
                RunningLogHelper.SaveEntrylog(0, account, true, "Log in");
                rst = new TResult(1, Consts.OK);
                return Json(rst);
            }
            else
            {
                RunningLogHelper.SaveEntrylog(0, account, false, oprtRst.Message);
                var msg = "账号或密码错误，请重新输入";
                rst = new TResult(0, msg);
                return Json(rst);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Logoff()
        {
            var crtOprtor = CurrentOperator.Instance.GetCurrent();
            if (crtOprtor == null) throw new Exception("Illegal request");
            if (crtOprtor.IsMachine == true) throw new Exception("Illegal request");

            RunningLogHelper.SaveEntrylog(1, crtOprtor.Account, true, "Log off");
            CurrentOperator.Instance.RemoveCurrent();
            return Json(new TResult(1));
        }

        private IActionResult UploadAttachmentModalPrivate(string index, string id, bool forSelf)
        {
            index = index.ToLower();
            if (index != "squarethumbnail")
            {
                return Redirect("~/Home/Error/NotExist/index/" + index);
            }
            var ett = _service.GetEntityByIdStringAsync(id).Result;
            if (ett == null) return Redirect("~/Home/Error/NotExist/Entity");

            ViewBag.Index = WebFileUploadType.Attachment.ToString();
            ViewBag.Object = "User" + (index == "squarethumbnail" ? "thumbnail" : index);
            ViewBag.ObjectId = id;
            ViewBag.NewFileTitle = "";
            ViewBag.UploadType = "update";
            ViewBag.SaveDatabaseUrl = (forSelf ? "Sys/User/UpdateSelfAttachment/" : "Sys/User/UpdateAttachment/") + "?index=" + index + "&id=" + id;

            if (index == "squarethumbnail")
            {
                ViewBag.ShowInitImage = !ett.ThumbnailPostfix.IsNullOrEmpty();
                ViewBag.InitFileName = "thumbnail" + ett.ThumbnailPostfix;
                ViewBag.Mode = forSelf ? "SingleMode1" : "SingleMode";
                return View("FileHandlers/UploadThumbnailsModal");
            }
            return View("~/Home/Error/NotExist/index/" + index);
        }

        private async Task<string> UpdateAttachmentPrivate(string index, User ett, string fileName)
        {
            if (index != "thumbnail" & index != "squarethumbnail")
            {
                return "Incorrect url";
            }
            if (index == "squarethumbnail") index = "thumbnail";

            var newPostfix = Path.GetExtension(fileName);
            var location = WebFileHelper.GetLocation(WebFileUploadType.Attachment);
            var relativeDir = "User\\" + WebFileHelper.GetDateTimeFlag(ett.CreationTime) + ett.Id;
            var absoluteDir = WebFileHelper.GetAbsolutePath(location, relativeDir);
            if (index == "thumbnail")
            {
                var oldThumbnailPostfix = ett.ThumbnailPostfix;
                var filePath = Path.Combine(absoluteDir, "thumbnail" + oldThumbnailPostfix);
                if (System.IO.File.Exists(filePath) & oldThumbnailPostfix != newPostfix)
                    System.IO.File.Delete(filePath);
                ett.ThumbnailPostfix = newPostfix;

                var caCheHandler = new CacheHandler();
                var currentOprt = CurrentOperator.Instance.GetCurrent();
                var id = currentOprt.ActorId;
                currentOprt.ThumbnailPostfix = newPostfix;
                caCheHandler.UpdateOperatorCache(id, currentOprt);

            }
            else
            {
                return "Incorrect url";
            }

            var msg = await _service.UpdateEntityAsync(ett);
            return msg;
        }




    }
}