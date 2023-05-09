using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Basis.SYS;
using Ligg.Infrastructure.Extensions;
using Ligg.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Ligg.Infrastructure.DataModels;

namespace Ligg.Uwa.WebApi.Controllers
{
    [Route("sys/[controller]/[action]")]
    [ApiController]
    [AuthorizationFilter]
    public class MenuItemController : BaseController
    {
        public MenuItemController(MenuItemService menuItemService)
        {
        }

        [HttpGet]
        public TResult<List<ListMenuItemsDto>> GetListModels([FromQuery] string portal, string menuId)
        {
            var errCode = GetErrorCode();
            var portalKey = portal;
            if (portalKey.IsNullOrEmpty() | menuId.IsNullOrEmpty()) return new TResult<List<ListMenuItemsDto>>(0, null, "illegal request,ErrCode=" + errCode + "-1");

            var cacheHandler = new CacheHandler();
            var configHandler = new ConfigHandler();
            var allMenuItems = cacheHandler.GetAllCachedMenuItems().FindAll(x => x.Status);

            var topMenu = allMenuItems.Find(x => x.Id.ToString() == menuId.ToString());
            if (topMenu == null) return new TResult<List<ListMenuItemsDto>>(0, null, "menu does not exist, ErrCode=" + errCode + "-2");
            var menuType = configHandler.GetFirstConfigItem(((int)OrpConfigSubType.MenuDefinition).ToString(), menuId);
            if (menuType == null) return new TResult<List<ListMenuItemsDto>>(0, null, "MenuType does not configured, ErrCode=" + errCode + "-3");
            
            var menuItems = new List<MenuItem>();
            var pmsHandler = new PermissionHandler();
            var token = JwtHelper.GetToken(Request);
            menuItems = pmsHandler.GetAvailableClientMenuItems(menuId, token);
            var outlinks = configHandler.GetConfigItems(((int)OrpConfigSubType.MenuItemOutlink).ToString());
            outlinks = outlinks.FindAll(x => x.Attribute.ToLower() == GlobalContext.SystemSetting.ApplicationServer.ToLower() & (x.Attribute1.ToLower() == portalKey.ToLower() | x.Attribute1.ToLower() == "shared"));
            var clientViews = new List<ConfigItem>();
            clientViews = configHandler.GetConfigItems(((int)DevConfigSubType.ClientView).ToString());
            var listMenuItemsDtos = new List<ListMenuItemsDto>();
            foreach (var menuItem in menuItems)
            {
                var ListMenuItemsDto = menuItem.MapTo<ListMenuItemsDto>();
                if (menuItem.Type == (int)MenuItemType.Directory)
                { }
                else if (menuItem.Type == (int)MenuItemType.OuterLink)
                {
                    var outlink = outlinks.Find(x => x.Id.ToString() == menuItem.PageId);
                    ListMenuItemsDto.Url = "remove";
                    if (outlink != null) ListMenuItemsDto.Url = outlink.Value;
                }
                else
                {
                    var clientView = clientViews.Find(x => x.Id.ToString() == menuItem.PageId);
                    ListMenuItemsDto.Url = "remove";
                    if (clientView != null) ListMenuItemsDto.Url = clientView.Attribute1;
                }

                ListMenuItemsDto.Url = ListMenuItemsDto.Url ?? "";
                ListMenuItemsDto.Icon = ListMenuItemsDto.Icon ?? "";
                if (ListMenuItemsDto.Url != "remove") listMenuItemsDtos.Add(ListMenuItemsDto);
            }

            listMenuItemsDtos = listMenuItemsDtos.OrderBy(x => x.Sequence).ToList();
            var rst = new TResult<List<ListMenuItemsDto>>(1, listMenuItemsDtos);
            return rst;
        }

    }

}
