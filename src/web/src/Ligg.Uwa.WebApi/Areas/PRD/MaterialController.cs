using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Business.PRD;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ligg.Infrastructure.DataModels;
using Ligg.Uwa.Basis.SYS;
using Ligg.Infrastructure.Utilities.DataParserUtil;
using System.Linq;

namespace Ligg.Uwa.WebApi.Controllers
{
    [Route("prd/[controller]/[action]")]
    [ApiController]
    [AuthorizationFilter]
    public class MaterialController : BaseController
    {
        private readonly MaterialService _service;
        public MaterialController(MaterialService materialService)
        {
            _service = materialService;
        }

        [HttpGet]
        public async Task<TResult<List<MaterialManageDto>>> GetPagedManageDtos([FromQuery] Pagination pagination, [FromQuery] CommonReqArgs commonReqArgs)
        {
            var dtos = await _service.GetManageDtosAsync(commonReqArgs);
            var rst = new TResult<List<MaterialManageDto>>(1, dtos);
            return rst;
        }

        [HttpGet]
        public async Task<TResult<List<DictItem>>> GetMaterialTypeDictItems()
        {
            var cfgHandler = new ConfigHandler();
            var configDefinition = cfgHandler.GetConfigDefinition((int)FuncConfigSubType.MaterialType + "");
            var hasConsumerOption = configDefinition.HasConsumers;
            var objs = new List<DictItem>();
            var cfgItemsOfMaterialType = cfgHandler.GetConfigItems((int)FuncConfigSubType.MaterialType + "").FindAll(x => x.Authorization == (int)Authorization.TobePermitted);
            if (hasConsumerOption)
            {
                var pmsHandler = new PermissionHandler();
                var token = JwtHelper.GetToken(Request);
                
                foreach (var cfgItem in cfgItemsOfMaterialType)
                {
                    var canBeConsumed = pmsHandler.CanBeConsumed(PermissionType.GrantAsConsumerForConfigItem, cfgItem, token);
                    if (hasConsumerOption & !canBeConsumed) continue;
                    var dictItem = new DictItem();
                    dictItem.Key = cfgItem.Id.ToString();
                    dictItem.Value = cfgItem.Name;
                    objs.Add(dictItem);
                }
            }
            else
            {
                foreach (var cfgItem in cfgItemsOfMaterialType)
                {
                    var dictItem = new DictItem();
                    dictItem.Key = cfgItem.Id.ToString();
                    dictItem.Value = cfgItem.Name;
                    objs.Add(dictItem);
                }
            }

            var rst = new TResult<List<DictItem>>(1, objs);
            return rst;
        }

        //#post
        [HttpPost]
        public async Task<TResult> Add([FromQuery] Material ett)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var cfgHandler = new ConfigHandler();
            var configDefinition = cfgHandler.GetConfigDefinition((int)FuncConfigSubType.MaterialType + "");
            var hasConsumerOption = configDefinition.HasConsumers;
            if (hasConsumerOption)
            {
                var pmsHandler = new PermissionHandler();
                var token = JwtHelper.GetToken(Request);
                var cfgItems = cfgHandler.GetConfigItems((int)FuncConfigSubType.MaterialType + "").FindAll(x => x.Authorization == (int)Authorization.TobePermitted);
                var cfgItem = cfgItems.Find(x => x.Id.ToString() == ett.Type);
                if (cfgItem == null)
                {
                    errMsg = new TextHandler().GetErrorMessage("NotExist", errCode + "-1", "", "MaterialType", ett.Type);
                    return new TResult(0, errMsg);
                }
                else
                {
                    var canBeConsumed = pmsHandler.CanBeConsumed(PermissionType.GrantAsConsumerForConfigItem, cfgItem, token);
                    if (!canBeConsumed)
                    {
                        errMsg = new TextHandler().GetErrorMessage("NoAuth", errCode + "-2", "", "MaterialType", ett.Type);
                        return new TResult(0, errMsg);
                    }
                }
            }


            var msg = await _service.SaveAddEditDtoAsync(ett);
            errMsg = msg != Consts.OK ? new TextHandler().GetErrorMessage("err", errCode + "-3", msg) : "";
            return new TResult(msg == Consts.OK ? 1 : 0, msg == Consts.OK ? "Add Acomplished" : errMsg);
        }


        [HttpPost]
        public async Task<TResult> DeleteSelected([FromQuery] string ids)
        {
            var errCode = GetErrorCode();
            var errMsg = "";
            var cfgHandler = new ConfigHandler();
            var configDefinition = cfgHandler.GetConfigDefinition((int)FuncConfigSubType.MaterialType + "");
            var hasConsumerOption = configDefinition.HasConsumers;


            if (string.IsNullOrEmpty(ids)) return new TResult(0, "No selection");
            var arr = ids.GetLarrayArray(true, true);
            if (arr == null) return new TResult(0, "Please select at least one item");

            if (arr.Length > MaterialDb.MaterialManageDtos.Count - 1)
            {
                return new TResult(0, "Can not delete all items");
            }

            var ids1 = MaterialDb.MaterialManageDtos.Select(x => x.Id);
            var cfgItems = cfgHandler.GetConfigItems((int)FuncConfigSubType.MaterialType + "").FindAll(x => x.Authorization == (int)Authorization.TobePermitted);


            foreach (string id in arr)
            {
                var obj = MaterialDb.MaterialManageDtos.Find(x => x.Id.ToString() == id);
                if (obj == null)
                {
                    errMsg = new TextHandler().GetErrorMessage("wurl", errCode + "-1", "", "id", id);
                    return new TResult(0, errMsg);
                }

                if (hasConsumerOption)
                {
                    var pmsHandler = new PermissionHandler();
                    var token = JwtHelper.GetToken(Request);
                    var cfgItem = cfgItems.Find(x => x.Id.ToString() == obj.Type);
                    var canBeConsumed = pmsHandler.CanBeConsumed(PermissionType.GrantAsConsumerForConfigItem, cfgItem, token);
                    if (!canBeConsumed)
                    {
                        errMsg = new TextHandler().GetErrorMessage("NoAuth", errCode + "-2", "", "MaterialType", obj.Type);
                        return new TResult(0, errMsg);
                    }
                }

                MaterialDb.MaterialManageDtos.Remove(obj);
            }

            return new TResult(1, "Delete Acomplished");
        }



    }


}
