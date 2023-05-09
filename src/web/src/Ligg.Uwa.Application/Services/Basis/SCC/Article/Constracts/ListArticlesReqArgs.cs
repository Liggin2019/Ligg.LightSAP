using Ligg.Infrastructure.Extensions;
using Ligg.EntityFramework.Entities;
using Ligg.Uwa.Application.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ligg.Uwa.Basis.SCC
{
    public class ListArticlesReqArgs : CommonReqArgs
    {
        //public string Text { get; set; }
        public int? IncludeLinks { get; set; }
        public string MasterId { get; set; }
        public int RecursiveSearch { get; set; }
        public List<string> RecursiveChildMasterIds { get; set; }

        public static Expression<Func<Article, bool>> GetListFilter(ListArticlesReqArgs param)
        {
            var expression = GetCommonListFilter(param);
            if (param != null)
            {
                if (param.IncludeLinks > -1)
                {
                    if (param.IncludeLinks == 0)
                        expression = expression.And(x => x.Type == (int)ArticleType.RichText | x.Type == (int)ArticleType.HtmlText | x.Type == (int)ArticleType.MarkdownText);
                }

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
        public static Expression<Func<Article, bool>> GetCommonListFilter(CommonReqArgs param)
        {
            var expression = DynamicExpressionEx.True<Article>();
            if (param != null)
            {
                if (param.Status > -1)
                {
                    expression = expression.And(x => x.Status == (param.Status == 1));
                }

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
                if (!string.IsNullOrEmpty(param.StartModificationTime.ToString()))
                {
                    expression = expression.And(x => x.ModificationTime >= param.StartModificationTime);
                }
                if (!string.IsNullOrEmpty(param.EndModificationTime.ToString()))
                {
                    param.EndModificationTime = param.EndModificationTime.Value.Date.Add(new TimeSpan(23, 59, 59));
                    expression = expression.And(x => x.ModificationTime <= param.EndModificationTime);
                }

                if (!string.IsNullOrEmpty(param.Text) & !GlobalContext.SystemSetting.SupportMultiLanguages)
                {
                    expression = expression.And(x => x.Name.Contains(param.Text) | x.Description.Contains(param.Text) | x.Body.Contains(param.Text));
                }
            }

            return expression;
        }

    }


}
