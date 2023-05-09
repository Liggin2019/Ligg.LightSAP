using Ligg.EntityFramework.Entities;
using Ligg.EntityFramework.Entities.Helpers;
using Ligg.Infrastructure.Utilities.DbUtil;
using Ligg.Infrastructure.Utilities.LogUtil;
using Ligg.Uwa.Application.Shared;
using System;

namespace Ligg.Uwa.Application.Shared
{
    public class RunningLogDbRepository
    {
        private readonly string _creatorId;
        public RunningLogDbRepository()
        {
            //_context = GlobalContext.ServiceProvider?.GetService<DbSetContext>();
            //_dbHandler = GlobalContext.ServiceProvider?.GetService<IDbHandler>();
            var crtOprt = CurrentOperator.Instance.GetCurrent();
            var initCreatorId = new User().Id.ToString();
            _creatorId = crtOprt == null ? initCreatorId : crtOprt.Id;
        }

        public void SaveActionLog(ActionLog actionLog)
        {
            actionLog.Id = EntityHelper.CreateKeyVal<long>();
            actionLog.CreatorId = _creatorId;
            actionLog.CreationTime = DateTime.Now;
            actionLog.LastModifierId = _creatorId;
            actionLog.ModificationTime = DateTime.Now;
            try
            {
                DbHelper.Create<ActionLog>(actionLog);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        public void SaveEntryLog(EntryLog entryLog)
        {
            entryLog.Id = EntityHelper.CreateKeyVal<long>();
            entryLog.CreatorId = _creatorId;
            entryLog.CreationTime = DateTime.Now;
            entryLog.LastModifierId = _creatorId;
            entryLog.ModificationTime = DateTime.Now;
            try
            {
                DbHelper.Create<EntryLog>(entryLog);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }


    }
}




