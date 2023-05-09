using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ligg.Infrastructure.Extensions
{
    public static class DynamicExpressionEx
    {
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        public static Func<T, T> FieldFilter<T>(string fields = "")
        {
            string[] entityFields;
            if (fields == "")
                // get Properties of the T
                entityFields = typeof(T).GetProperties().Select(propertyInfo => propertyInfo.Name).ToArray();
            else
                entityFields = fields.Split(',');

            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(T), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(T));

            // create initializers
            var bindings = entityFields.Select(o => o.Trim())
                .Select(o =>
                {

                    // property "Field1"
                    var mi = typeof(T).GetProperty(o);

                    // original value "o.Field1"
                    var xOriginal = Expression.Property(xParameter, mi);

                    // set value "Field1 = o.Field1"
                    return Expression.Bind(mi, xOriginal);
                }
            );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda.Compile();

            //*demo
            //var db = new AppDbContext()
            //var result = db.SampleEntity.DynamicFilter(Helpers.DynamicSelectGenerator<SampleEntity>("Field1, Field2")).ToList();
            ////select all field from entity
            //var result1 = db.SampleEntity.DynamicFilter(Helpers.DynamicSelectGenerator<SampleEntity>()).ToList();

        }


        private class ExpressionVisitorEx : ExpressionVisitor
        {
            /// The ParameterExpression map
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            ExpressionVisitorEx(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ExpressionVisitorEx(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }
    }
}
