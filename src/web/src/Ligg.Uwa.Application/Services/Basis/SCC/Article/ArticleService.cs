using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.Extensions;
using Ligg.Uwa.Application.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SCC
{
    public class ArticleService
    {
        private readonly ArticleRepository _repository;
        private readonly CategoryRepository _categoryRepository;
        public ArticleService(ArticleRepository repository, CategoryRepository categoryRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
        }

        //*list
        public async Task<List<ManageArticlesDto>> GetPagedManageDtosAsync(ListArticlesReqArgs param, Pagination pagination, bool manage)
        {
            var dtos = new List<ManageArticlesDto>();
            if (param?.MasterId != null)
            {
                if (param.RecursiveSearch == 1)
                    param.RecursiveChildMasterIds = _categoryRepository.GetRecursiveChildIdsById<Category, long>(param.MasterId, true);

                var exp = ListArticlesReqArgs.GetListFilter(param);
                var etts = await _repository.FindPagedEntitiesAsync(exp, pagination);
                dtos = etts.MapToList<ManageArticlesDto>();
                var articleIdArray = dtos.Select(x => x.Id).ToArray();
                foreach (var dto in dtos)
                {
                    dto.MasterCascadePath = _categoryRepository.GetCascadedPathById<Category>(dto.MasterId);
                    var ett = etts.Where(x => x.Id.ToString() == dto.Id).FirstOrDefault();
                    dto.HasThumbnail = ett.ThumbnailPostfix.IsNullOrEmpty() ? 0 : 1;
                    dto.HasImage = ett.ImagePostfix.IsNullOrEmpty() ? 0 : 1;
                    dto.HasVideo = ett.VideoPostfix.IsNullOrEmpty() ? 0 : 1;
                }

                return dtos;
            }
            else
            {
                return dtos;
            }
        }

        public async Task<List<ManageArticlesDto>> GetListDtosByMasterIdAsync(string masterId, int num, bool includeLinks = false)
        {
            var dtos = new List<ManageArticlesDto>();
            var etts = await _repository.FindEntitiesByExpressionAsync(x => x.MasterId == masterId & x.Status);
            if (includeLinks == false) etts = etts.Where(x => x.Type == (int)ArticleType.RichText | x.Type == (int)ArticleType.HtmlText | x.Type == (int)ArticleType.MarkdownText).ToList();
            if (etts.Count > 0)
            {
                var list1 = etts.OrderBy(x => x.Sequence).Skip(0).Take(num);
                dtos = list1.MapToList<ManageArticlesDto>();
            }
            return dtos;
        }

        public async Task<List<ManageArticlesDto>> GetListDtosByIdArrayAsync(string[] idArray, bool includeLinks = false)
        {
            var etts = await _repository.FindEntitiesByIdArrayAsync(idArray);
            var list1 = etts.FindAll(x => x.Status).OrderBy(x => x.Sequence);
            var dtos = list1.MapToList<ManageArticlesDto>();
            return dtos;
        }


        //for delete selected
        public async Task<List<Article>> GetEntitiesByIdsAsync(string[] ids)
        {
            var etts = await _repository.FindEntitiesByIdArrayAsync(ids);
            return etts;
        }

        //*get
        public async Task<AddEditArticleDto> GetAddEditDtoAsync(string id)
        {
            var entity = await _repository.GetEntityByIdStringAsync(id);
            var dto = entity.MapTo<AddEditArticleDto>();
            return dto;

        }

        public async Task<Article> GetEntityByIdStringAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var entity = await _repository.GetEntityByIdStringAsync(id);
            return entity;
        }

        public async Task<int> GetMaxSequenceNoAsync(string masterId)
        {
            var maxOne = await _repository.GetMaxOneAsync(x => x.Sequence, x => x.MasterId == masterId);
            var step = GlobalContext.SystemSetting.GetMaxSequenceStep;
            var no = maxOne == null ? step : (((maxOne.Sequence ?? 0) + step));
            return no;
        }


        //*save
        public async Task<string> SaveAddEditDtoAsync(AddEditArticleDto dto)
        {
            if (dto.MasterId == new Category().Id.ToString()) return "根目录不能关联数据";
            var entity = dto.MapTo<Article>();
            bool isEdit = false;
            var oldEtt = new Article();
            if (entity.Id!=new Article().Id)
            {
                isEdit = true;
            }
            //if (_repository.FieldValueExists(entity.Id, entity.Name, x => x.Name, x => (x.MasterId == entity.MasterId), true)) return "同目录下名称不能重复！";

            entity.Description = entity.Description ?? string.Empty;
            entity.Body = entity.Body ?? string.Empty;
            entity.Note = entity.Note ?? string.Empty;
            entity.Author = entity.Author ?? string.Empty;
            entity.Resource = entity.Resource ?? string.Empty;
            if (isEdit)
            {
                entity.ThumbnailPostfix = oldEtt.ThumbnailPostfix;
                entity.ImagePostfix = oldEtt.ImagePostfix;
                entity.VideoPostfix = oldEtt.VideoPostfix;
            }
            return await _repository.SaveEntityAsync(entity);
        }

        public async Task<string> UpdateEntityAsync(Article entity)
        {
            return await _repository.SaveEntityAsync(entity);
        }


        //*del
        public async Task<string> DeleteByIdsAsync(string ids)
        {
            var idArr = ids.Split(',');
            await _repository.DeleteEntitiesByIdArrayAsync(idArr);
            return Consts.OK;
        }

        public bool IsSameMaster(string[] idArr)
        {
            var etts = _repository.FindEntitiesByIdArrayAsync(idArr).Result;
            if (etts.Count == 0) return false;
            var masterId = etts.FirstOrDefault().MasterId;
            foreach (var ett in etts)
            {
                if (masterId != ett.MasterId)
                    return false;
            }
            return true;
        }



    }
}
