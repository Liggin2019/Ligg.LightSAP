using Ligg.Infrastructure.Extensions;
using Ligg.EntityFramework.Entities;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ligg.Uwa.Basis.SYS
{
    public class ListPermissionsReqArgs:CommonReqArgs
    {
        public string MasterId { get; set; }
        public int ActorType { get; set; }

        public static Expression<Func<Permission, bool>> GetListFilter(ListPermissionsReqArgs param)
        {
            var expression = GetCommonListFilter(param);
            if (param != null)
            {
                if (param.ActorType > -1)
                {
                    expression = expression.And(x => x.ActorType ==param.ActorType);
                }

                if (!string.IsNullOrEmpty(param.MasterId)&param.MasterId!="-1")
                {
                    expression = expression.And(x => x.MasterId == param.MasterId);
                }

            }
            return expression;
        }
        public static Expression<Func<Permission, bool>> GetCommonListFilter(CommonReqArgs param)
        {
            var expression = DynamicExpressionEx.True<Permission>();
            if (param != null)
            {
                if (param.Type > -1)
                {
                    expression = expression.And(x => x.Type == param.Type);
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
            }

            return expression;
        }

    }


}
