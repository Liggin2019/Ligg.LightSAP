
using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class TenantService
    {
        private readonly TenantRepository _repository;
        private readonly CacheHandler _cacheHandler;
        public TenantService(TenantRepository repository)
        {
            _repository = repository;
            _cacheHandler = new CacheHandler();
        }

        //*list
        public async Task<List<ManageTenantsDto>> GetPagedManageDtosAsync(CommonReqArgs param, Pagination pagination)
        {
            var dtos = new List<ManageTenantsDto>();
            var allCachedEtts = _cacheHandler.GetAllCachedTenants();
            var pubTnt = new PublicTenant();
            var pubTenant = pubTnt.MapTo<Tenant>();
            var allEtts = new List<Tenant>();
            allEtts.Add(pubTenant);
            allEtts.AddRange(allCachedEtts);
            var exp = GetListFilter(param);
            var etts = allEtts.AsQueryable().Where(exp).OrderBy(x => x.Sequence);
            if (etts.Count() > 0)
            {
                dtos = etts.MapToList<ManageTenantsDto>();
                foreach (var dto in dtos)
                {
                    var ett = etts.Where(x => x.Id.ToString() == dto.Id).FirstOrDefault();
                    dto.HasThumbnail = ett.ThumbnailPostfix.IsNullOrEmpty() ? 0 : 1;
                    dto.HasImage = ett.ImagePostfix.IsNullOrEmpty() ? 0 : 1;
                    dto.HasIco = ett.HasIco ? 1 : 0;
                }
            }
            return CommonHelper.GetPagedList(dtos, pagination);
        }

        //*get
        public async Task<AddEditTenantDto> GetAddEditDtoAsync(string id)
        {
            var entity = await GetEntityByIdStringAsync(id);
            var dto = entity.MapTo<AddEditTenantDto>();
            return dto;
        }

        public async Task<ShowTenantDto> GetShowDtoAsync(string id)
        {
            var ett = await GetEntityByIdStringAsync(id);
            var dto = ett.MapTo<ShowTenantDto>();
            return dto;
        }
        public async Task<ShowTenantDto> GetCurrentShowDtoAsync()
        {
            var dto = new ShowTenantDto();
            if (!GlobalContext.SystemSetting.SupportMultiTenants)
            {
                var pubTnt = new PublicTenant();
                var pubTenant = pubTnt.MapTo<Tenant>();
                dto = pubTenant.MapTo<ShowTenantDto>();
            }
            else
            {
                var pubTnt = new PublicTenant();
                var pubTenant = pubTnt.MapTo<Tenant>();
                dto = pubTenant.MapTo<ShowTenantDto>();
            }
            return dto;
        }
        public async Task<ShowTenantDto> GetDefaultShowDtoAsync()
        {
            var ett = _cacheHandler.GetAllCachedTenants().Where(x => x.IsDefault).FirstOrDefault();
            var dto = ett.MapTo<ShowTenantDto>();
            return dto;
        }

        public async Task<Tenant> GetEntityByIdStringAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var entity = _cacheHandler.GetAllCachedTenants().Where(x => x.Id.ToString() == id).FirstOrDefault();
            return entity;

        }

        public async Task<int> GetMaxSequenceNoAsync()
        {
            var maxOne = CommonHelper.GetMaxOne(_cacheHandler.GetAllCachedTenants(), x => x.Sequence);
            var step = GlobalContext.SystemSetting.GetMaxSequenceStep;
            var no = maxOne == null ? step : (((maxOne.Sequence ?? 0) + step));
            return no;
        }

        //*save
        public async Task<string> SaveAddEditDtoAsync(AddEditTenantDto dto)
        {
            if (dto.Key.ToLower() == "pub") return "键已经存在";
            if (dto.Code.ToLower() == "public") return "编码已经存在";

            var entity = dto.MapTo<Tenant>();
            bool isEdit = false;
            var oldEtt = new Tenant();
            if (entity.Id != new Tenant().Id)
            {
                isEdit = true;
                oldEtt = _cacheHandler.GetAllCachedTenants().Find(x => x.Id.ToString() == dto.Id);
            }

            var allList = _cacheHandler.GetAllCachedTenants();
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Key, X => X.Key, null, true)) return "键已经存在";
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Code, X => X.Code, null, true)) return "编码已经存在";
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.ShortName, X => X.ShortName, null, true)) return "短名称已经存在";
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Name, X => X.Name, null, true)) return "名称已经存在";

            entity.Description = entity.Description ?? string.Empty;
            if (isEdit)
            {
                entity.IsDefault = oldEtt.IsDefault;
                entity.ThumbnailPostfix = oldEtt.ThumbnailPostfix;
                entity.ImagePostfix = oldEtt.ImagePostfix;
            }
            var msg=await _repository.SaveEntityAsync(entity);
            if (msg == Consts.OK)
                _cacheHandler.RemoveTenantCache();
            return msg;
        }
        public async Task<string> UpdateEntityAsync(Tenant entity)
        {
            if (entity.Key.ToLower() == "pub") return "键已经存在";
            if (entity.Code.ToLower() == "public") return "编码已经存在";

            var allList = _cacheHandler.GetAllCachedTenants();
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Key, X => X.Key, null, true)) return "键已经存在";
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Code, X => X.Code, null, true)) return "编码已经存在";
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.ShortName, X => X.ShortName, null, true)) return "短名称已经存在";
            if (CommonHelper.FieldValueExists(allList, entity.Id, entity.Name, X => X.Name, null, true)) return "名称已经存在";
            var msg=await _repository.SaveEntityAsync(entity);
            if (msg == Consts.OK)
                _cacheHandler.RemoveTenantCache();
            return msg;
        }

        //*mod
        public async Task<string> SetAsDefault(string id)
        {
            var msg = await _repository.SetAsDefault(id);
            return msg;
        }

        //*del
        public async Task<string> DeleteByIdsAsync(string ids)
        {
            var idArr = ids.Split(',');
            await _repository.DeleteEntitiesByIdArrayAsync(idArr);
            _cacheHandler.RemoveTenantCache();
            return Consts.OK;

        }




        //*private
        private static Expression<Func<Tenant, bool>> GetListFilter(CommonReqArgs param)
        {
            var expression = DynamicExpressionEx.True<Tenant>();
            if (param != null)
            {
                if (param.Type > -1)
                {
                    expression = expression.And(x => x.Type == param.Type);
                }

                if (!string.IsNullOrEmpty(param.Text))
                {
                    expression = expression.And(x => x.Name.Contains(param.Text) | x.ShortName.Contains(param.Text) | x.Description.Contains(param.Text));
                }

                if (param.Status > -1)
                {
                    expression = expression.And(x => x.Status == (param.Status == 1 ? true : false));
                }

                if (!string.IsNullOrEmpty(param.Text) & !GlobalContext.SystemSetting.SupportMultiLanguages)
                {
                    expression = expression.And(x => x.Name.Contains(param.Text) | x.Description.Contains(param.Text));
                }
            }

            return expression;
        }


    }
}
