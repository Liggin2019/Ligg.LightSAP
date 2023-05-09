
using Ligg.EntityFramework.Entities;
using Ligg.EntityFrameworkCore;
using Ligg.Infrastructure.Extensions;
using Ligg.Infrastructure.Helpers;
using Ligg.Uwa.Application;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ligg.Uwa.Basis.SYS
{
    public class UserRepository : CommonRepository<long, User, DbSetContext>
    {
        UserContainerRepository _userContainerRepository;
        OperatorRepository _operatorRepository;
        private CacheHandler _cacheHandler;
        public UserRepository(IUnitWork<DbSetContext> unitWork, IDbSetRepository<long, User, DbSetContext> repository,
            UserContainerRepository userContainerRepository, OperatorRepository operatorRepository, ICacheHandler cacheManager) : base(unitWork, repository)
        {
            _userContainerRepository = userContainerRepository;
            _operatorRepository = operatorRepository;
            _cacheHandler = new CacheHandler();
        }

        //*save
        public async Task<string> SaveAddEditDtosAsync(List<AddEditUserDto> dtos, bool overwrite)
        {

            var accts = dtos.Select(x => x.Account);
            var existedDtos = UnitWork.GetMany<User>(x => accts.Contains(x.Account));
            var existedUserList = existedDtos.ToList();

            var accts1 = existedUserList.Select(x => x.Account);
            var newDtoList = dtos.Where(x => !accts1.Contains(x.Account));

            var topOrg = UnitWork.Get<Organization>(x => x.ParentId == new Organization().Id.ToString());
            await UnitWork.ExecuteTransactionAsync(() =>
            {
                //UnitWork.CreateMany<User>()
                var userList = new List<User>();
                var actorList = new List<Operator>();
                foreach (var newDto in newDtoList)
                {
                    var entity = new User();
                    entity.Account = newDto.Account;
                    entity.Gender = newDto.Gender;
                    entity.Name = newDto.Name ?? "";

                    entity.Email = newDto.Email;
                    entity.Mobile = newDto.Mobile;


                    entity.Birthday = entity.Birthday.IsNullOrEmpty() ? DateTime.MinValue.ToString("yyyy-MM-dd") : entity.Birthday;
                    entity.Description = "";
                    entity.MasterId = topOrg.Id.ToString();
                    entity.Status = true;
                    entity.Salt = GetPasswordSalt();
                    var pw = entity.Password;
                    entity.Password = pw.IsNullOrEmpty() ? EncryptPassword(pw, entity.Salt) : "";
                    userList.Add(entity);

                    var entity1 = new Operator();
                    var oprtor = new Operator();
                    oprtor.ActorId = entity.Id.ToString();
                    oprtor.LoginCount = 0;

                }
                UnitWork.CreateMany<long, User>(userList.ToArray());

                foreach (var newDto in newDtoList)
                {
                    var obj = GetEntityByExpressionAsync(x => x.Account == newDto.Account);
                    RemoveOperatorCacheById(obj.Id);
                }

                if (existedUserList.Count() > 0)
                {
                    if (overwrite)
                    {
                        var list1 = new List<User>();
                        foreach (var existedUser in existedUserList)
                        {
                            var dto = dtos.Where(x => x.Account == existedUser.Account).FirstOrDefault();
                            existedUser.Gender = dto.Gender;
                            existedUser.Name = dto.Name ?? "";
                            existedUser.Email = dto.Email;
                            existedUser.Mobile = dto.Mobile;
                            UnitWork.Update<long, User>(existedUser);
                            RemoveOperatorCacheById(existedUser.Id);
                        }
                    }
                }

                UnitWork.Save();

            });

            return Consts.OK;
        }
        public async Task<string> SaveAddEditDtoAsync(AddEditUserDto dto)
        {
            User entity = dto.MapTo<User>();
            var isEdit = false;
            if (entity.Id != new User().Id)
            {
                isEdit = true;
                var oldEtt = await GetEntityByIdAsync(entity.Id);
                if (oldEtt == null) return "Entity doesn't exist";
            }
            //entity.Birthday = ObjectParseHelper.ParseToDateTime("1000-01-01").ToString("yyyy-MM-dd");
            entity.Birthday = entity.Birthday.IsNullOrEmpty() ? DateTime.MinValue.ToString("yyyy-MM-dd") : entity.Birthday;

            if (AccountExists(entity)) return "用户账号已存在";

            await UnitWork.ExecuteTransactionAsync(() =>
            {
                if (isEdit)
                {
                    UnitWork.RemoveMany<UserContainer>(x => x.UserId == entity.Id.ToString());
                    //*test
                    //整个实体更新,不需要ExecuteTransaction, 可以实现实现ModificationTime在Uniwork统一修改
                    //RestoreOriginalValue(entity);
                    //UnitWork.Update<long, User>(entity);

                    //局部更新, 需要ExecuteTransaction或ExecuteTransactionAsync的配合， 
                    //不能实现ModificationTime在Uniwork统一修改
                    var currentUser = CurrentOperator.Instance.GetCurrent();
                    UnitWork.Update<User>(x => x.Id == entity.Id, u => new User
                    {
                        Account = entity.Account,
                        Gender = entity.Gender,
                        Name = entity.Name,
                        Birthday = entity.Birthday,
                        Email = entity.Email,
                        Qq = entity.Qq,
                        WeChat = entity.WeChat,
                        Mobile = entity.Mobile,
                        Description = entity.Description,
                        Password = entity.Password,
                        MasterId = entity.MasterId,
                        Status = entity.Status,
                        LastModifierId = currentUser.ActorId,
                        ModificationTime = DateTime.Now
                    });
                    RemoveOperatorCacheById(entity.Id);
                }
                else
                {
                    entity.Undeleted = true;
                    entity.Salt = GetPasswordSalt();
                    entity.Password = EncryptPassword(entity.Password, entity.Salt);
                    UnitWork.Create<long, User>(entity);//对于雪花Id/Guid, 此处从BaseRepository获取新的id
                    //await UnitWork.Save(); 对于自增长id需要这一步,先保存获取id，再对关联表进行追加。

                    //同时创建Operator
                    var oprtor = new Operator();
                    oprtor.Type = (int)ActorType.User;
                    oprtor.ActorId = entity.Id.ToString();
                    oprtor.LoginCount = 0;

                    UnitWork.Create<long, Operator>(oprtor);
                    //RemoveOperatorCacheById(entity.Id);
                }
                UnitWork.Save();

            });

            return Consts.OK;
        }

        //*mod
        public async Task<string> ResetUserPasswordAsync(ChangePasswordDto dto)
        {
            if (dto.Id == new User().Id.ToString()) return "用户不存在";

            var entity = await GetEntityByIdStringAsync(dto.Id);

            entity.Salt = GetPasswordSalt();
            entity.Password = EncryptPassword(dto.Password, entity.Salt);
            await Repository.UpdateAsync(entity);
            //await RemoveOperatorCacheByIdAsync(entity.Id); 不会把密码信息带到Cache
            return Consts.OK;
        }
        public async Task<string> ChangeSelfPassword(ChangePasswordDto dto)
        {
            var id = dto.Id;
            var entity = await GetEntityByIdStringAsync(id);
            if (entity.Password != EncryptPassword(dto.Password, entity.Salt))
            {
                return "原密码不正确";
            }
            entity.Salt = GetPasswordSalt();
            entity.Password = EncryptPassword(dto.NewPassword, entity.Salt);
            await Repository.UpdateAsync(entity);
            return Consts.OK;
        }


        //*del
        public async Task<string> DeleteByIdsAsync(string ids)
        {
            await UnitWork.ExecuteTransactionAsync(() =>
            {
                var idsArr = ids.Split(',');

                UnitWork.RemoveMany<UserContainer>(x => idsArr.Contains(x.UserId));
                //UnitWork.RemoveMany<Operator>(x => idsArr.Contains(x.UserId));
                UnitWork.RemoveMany<Permission>(x => x.ActorType == (int)ActorType.User & idsArr.Contains(x.Id.ToString()));
                //fake delete
                var etts = FindEntitiesByIdArrayAsync(idsArr).Result;
                foreach (var ett in etts)
                {
                    ett.MasterId = "-1";
                    ett.Undeleted = false;
                    ett.Status = false;
                    ett.Account = (ett.Account.Length > 63 - 18 ? ett.Account.Substring(0, 63 - 18) : ett.Account) + "-" + "D".ToUniqueStringByDataTime(System.DateTime.Now, "-");
                    ett.Name = ett.Name.Length > 63 - 2 ? ett.Name.Substring(0, 63 - 2) : ett.Name + "-" + "D";
                    UnitWork.Update<long, User>(ett);
                }

                RemoveOperatorCacheByIds(ids);
                UnitWork.Save();
            });
            return Consts.OK;
        }



        //*common
        public string EncryptPassword(string password, string salt)
        {
            string md5Password = EncryptionHelper.Md5EncryptToHex(password);
            string encryptPassword = EncryptionHelper.Md5EncryptToHex(md5Password.ToLower() + salt).ToLower();
            return encryptPassword;
        }

        //*private
        private string GetPasswordSalt()
        {
            return new Random().Next(1, 100000).ToString();
        }


        private void RemoveOperatorCacheByIds(string ids)
        {
            foreach (long id in ids.Split(',').Select(p => long.Parse(p)))
            {
                RemoveOperatorCacheById(id);
            }
        }

        private void RemoveOperatorCacheById(long id)
        {
            var ett = _operatorRepository.GetEntityById(id);
            if (ett != null)
            {
                _cacheHandler.RemoveOperatorCache(ett.Id.ToString());
            }
        }


        private bool AccountExists(User entity)
        {
            var expression = DynamicExpressionEx.True<User>();
            if (entity.Id == new User().Id)//add 
            {
                expression = expression.And(t => t.Account == entity.Account);
            }
            else
            {
                expression = expression.And(t => t.Account == entity.Account);
                expression = expression.And(t => t.Id.ToString() != entity.Id.ToString());
            }

            return Repository.Any(expression);

        }


    }
}