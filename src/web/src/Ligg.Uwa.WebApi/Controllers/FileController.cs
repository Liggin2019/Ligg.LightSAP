using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SCC;
using Ligg.Uwa.Basis.SYS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Ligg.Uwa.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class FileController : BaseController
    {
        private readonly TenantService _tenantService;
        private readonly UserService _userService;
        private readonly ArticleService _articleService;
        private readonly CategoryService _categoryService;

        public FileController(TenantService tenantService, UserService userService, ArticleService articleService, CategoryService categoryService)
        {
            _tenantService = tenantService;
            _userService = userService;
            _articleService = articleService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(string index, string obj, string objId, string fileName, string avCode)
        {
            if (index.IsNullOrEmpty()) return BadRequest("Incorrect url");
            var path = GetAbsolutePathAsync(index, obj, objId, fileName).Result;
            if (path == null) return BadRequest("Incorrect url");
            if (path == string.Empty) return BadRequest("You have no authorization");
            if (!System.IO.File.Exists(path)) return BadRequest("Incorrect url");

            var bytes = WebFileHelper.GetFileBytes(path);
            if (bytes == null) return BadRequest("Incorrect url");
            return new FileContentResult(bytes, "image/jpeg");
        }

        private async Task<string> GetAbsolutePathAsync(string index, string obj, string objId, string fileName, UploadType uploadType = UploadType.None, string uploadedFileName = null)
        {
            if (obj == null) obj = "";
            obj = obj.ToLower();

            if (index.ToLower() == WebFileUploadType.TempFile.ToString().ToLower())
            {
                var location = WebFileHelper.GetLocation(WebFileUploadType.TempFile);
                return Path.Combine(location, fileName);
            }
            else if (index.ToLower() == WebFileUploadType.RootFile.ToString().ToLower())
            {
                var location = WebFileHelper.GetLocation(WebFileUploadType.RootFile);
                return Path.Combine(location, fileName);
            }
            else if (index.ToLower() == WebFileUploadType.ContentFile.ToString().ToLower())
            {
                var location = WebFileHelper.GetLocation(WebFileUploadType.ContentFile);
                return Path.Combine(location, fileName);
            }
            else if (index.ToLower() == WebFileUploadType.ResourceFile.ToString().ToLower())
            {
                var location = WebFileHelper.GetLocation(WebFileUploadType.ResourceFile);
                return Path.Combine(location, fileName);
            }

            else if (index.ToLower() == WebFileUploadType.Attachment.ToString().ToLower())
            {
                var location = WebFileHelper.GetLocation(WebFileUploadType.Attachment);
                if (obj == "tenantthumbnail" | obj == "tenantimage" | obj == "tenantico")
                {
                    var ett = new Tenant();
                    if (objId == new Tenant().Id.ToString()) ett = new PublicTenant().MapTo<Tenant>();
                    else ett = await _tenantService.GetEntityByIdStringAsync(objId);
                    if (ett == null) return null;
                    var dir = location + "\\Tenant\\" + WebFileHelper.GetDateTimeFlag(ett.CreationTime) + "\\" + objId;
                    var thumbnailPostfix = uploadType != UploadType.None ? Path.GetExtension(uploadedFileName) : ett.ThumbnailPostfix;
                    if (obj == "tenantthumbnail" & thumbnailPostfix.IsNullOrEmpty()) return null;
                    var imgPostfix = uploadType != UploadType.None ? Path.GetExtension(uploadedFileName) : ett.ImagePostfix;
                    if (obj == "tenantimage" & imgPostfix.IsNullOrEmpty()) return null;

                    var icoPostfix = ".ico";
                    if (obj == "tenantico")
                    {
                        if (uploadType == UploadType.None) //get
                        {
                            var icoPath = Path.Combine(dir, "ico" + icoPostfix);
                            if (!System.IO.File.Exists(icoPath)) return null; ;
                        }
                    }

                    var fileName1 = obj == "tenantthumbnail" ? "thumbnail" + thumbnailPostfix : "image" + imgPostfix;
                    if (obj == "tenantthumbnail") fileName1 = "thumbnail" + thumbnailPostfix;
                    else if (obj == "tenantimage") fileName1 = "image" + imgPostfix;
                    else if (obj == "tenantico") fileName1 = "ico" + icoPostfix;
                    else return null;
                    return Path.Combine(dir, fileName1);
                }
                if (obj == "userthumbnail")
                {
                    if (objId == new User().Id.ToString()) return null;
                    var ett = await _userService.GetEntityByIdStringAsync(objId);
                    if (ett == null) return null;
                    var dir = location + "\\User\\" + WebFileHelper.GetDateTimeFlag(ett.CreationTime) + "\\" + objId;
                    var thumbnailPostfix = uploadType != UploadType.None ? Path.GetExtension(uploadedFileName) : ett.ThumbnailPostfix;
                    if (obj == "userthumbnail" & thumbnailPostfix.IsNullOrEmpty()) return null;
                    var fileName1 = "thumbnail" + Path.GetExtension(thumbnailPostfix);
                    return Path.Combine(dir, fileName1);
                }
                else if (obj == "articlethumbnail" | obj == "articleimage" | obj == "articlevideo" | obj == "articleattachedfile" | obj == "articleattachedimage")
                {
                    if (objId == new Article().Id.ToString()) return null;
                    var ett = await _articleService.GetEntityByIdStringAsync(objId);
                    if (ett == null) return null;
                    var dir = location + "\\Article\\" + WebFileHelper.GetDateTimeFlag(ett.CreationTime) + "\\" + objId;
                    if (obj == "articleattachedimage") dir = dir + "\\AttachedImages";
                    if (obj == "articleattachedfile") dir = dir + "\\AttachedFiles";

                    var fileName1 = "";
                    if (obj == "articlethumbnail")
                    {
                        var thumbnailPostfix = uploadType != UploadType.None ? Path.GetExtension(uploadedFileName) : ett.ThumbnailPostfix;
                        if (thumbnailPostfix.IsNullOrEmpty()) return null;
                        fileName1 = "thumbnail" + thumbnailPostfix;
                    }
                    else if (obj == "articleimage")
                    {
                        var imgPostfix = uploadType != UploadType.None ? Path.GetExtension(uploadedFileName) : ett.ImagePostfix;
                        if (imgPostfix.IsNullOrEmpty()) return null;
                        fileName1 = "image" + imgPostfix;
                    }
                    else if (obj == "articlevideo")
                    {
                        var VideoPostfix = uploadType != UploadType.None ? Path.GetExtension(uploadedFileName) : ett.VideoPostfix;
                        if (VideoPostfix.IsNullOrEmpty()) return null;
                        fileName1 = "video" + VideoPostfix;
                    }
                    else
                    {
                        if (uploadType == UploadType.Upload)
                            fileName1 = uploadedFileName;
                        else fileName1 = fileName; //update
                    }
                    return Path.Combine(dir, fileName1);
                }
            }
            return "";
        }

    }
}
