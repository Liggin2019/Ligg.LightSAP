using Ligg.Infrastructure.Extensions;
using Ligg.EntityFramework.Entities;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ligg.Uwa.Basis.SYS
{
    public class ListUsersReqArgs : CommonReqArgs
    {
        public string MasterId { get; set; }
        public int RecursiveSearch { get; set; }
        public List<string> RecursiveChildMasterIds { get; set; }


        public static Expression<Func<User, bool>> GetListFilter(ListUsersReqArgs param)
        {
            var expression = GetCommonListFilter(param);
            if (param != null)
            {

                if (param.RecursiveSearch == 1)
                {
                    if (param.RecursiveChildMasterIds != null)
                    {
                        if (param.RecursiveChildMasterIds.Count > 0)
                            expression = expression.And(t => param.RecursiveChildMasterIds.Contains(t.MasterId));
                    }
                }
                else
                {
                    expression = expression.And(t => param.MasterId == t.MasterId);
                }

            }
            return expression;
        }
        public static Expression<Func<User, bool>> GetCommonListFilter(CommonReqArgs param)
        {
            var expression = DynamicExpressionEx.True<User>();
            if (param != null)
            {
                expression = expression.And(x => x.Undeleted == true);
                if (param.Status > -1)
                {
                    expression = expression.And(x => x.Status == (param.Status == 1));
                }

                if (!string.IsNullOrEmpty(param.StartCreationTime.ToString()))
                {
                    expression = expression.And(x => x.CreationTime >= param.StartCreationTime);
                }
                if (!string.IsNullOrEmpty(param.EndCreationTime.ToString()))
                {
                    param.EndCreationTime = param.EndCreationTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    expression = expression.And(x => x.CreationTime <= param.EndCreationTime);
                }

                if (!string.IsNullOrEmpty(param.StartModificationTime.ToString()))
                {
                    expression = expression.And(x => x.ModificationTime >= param.StartModificationTime);
                }

                if (!string.IsNullOrEmpty(param.EndModificationTime.ToString()))
                {
                    param.EndModificationTime = param.EndModificationTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    expression = expression.And(x => x.ModificationTime <= param.EndModificationTime);
                }

                if (!string.IsNullOrEmpty(param.Mark))
                {
                    expression = expression.And(x => x.Account.Contains(param.Mark) | x.Email.Contains(param.Mark) | x.Mobile.Contains(param.Mark)| x.WeChat.Contains(param.Mark));
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
