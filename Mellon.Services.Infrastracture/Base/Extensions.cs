using Mellon.Common.Services;
using System.Linq.Expressions;

namespace Mellon.Services.Infrastracture
{
    public static class Extensions
    {
        /// <summary>
        /// Order the IQueryable by the given property or field.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable being ordered.</typeparam>
        /// <param name="source">The IQueryable being ordered.</param>
        /// <param name="propertyOrFieldName">The name of the property or field to order by.</param>
        /// <param name="ascending">Indicates whether or not the order should be ascending (true) or descending (false.)</param>
        /// <returns>Returns an IQueryable ordered by the specified field.</returns>
        /// <seealso cref="https://schneids.net/paging-in-asp-net-web-api"/>
        public static IQueryable<T> OrderByPropertyOrField<T>(this IQueryable<T> source, string propertyOrFieldName, bool ascending = true)
        {
            var elementType = typeof(T);
            var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

            var parameterExpression = Expression.Parameter(elementType);
            var propertyOrFieldExpression = Expression.PropertyOrField(parameterExpression, propertyOrFieldName);
            var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

            var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName,
                new[] { elementType, propertyOrFieldExpression.Type }, source.Expression, selector);

            return source.Provider.CreateQuery<T>(orderByExpression);
        }

        /// <summary>
        /// Order the IQueryable by the given sort models.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable being ordered.</typeparam>
        /// <param name="queryable">The IQueryable being ordered.</param>
        /// <param name="sortModels">The collection of sort model properties to order by.</param>
        /// <returns>Returns an IQueryable ordered by the specified fields.</returns>
        /// <seealso cref="https://entityframeworkcore.com/knowledge-base/36298868/how-to-dynamically-order-by-certain-entity-properties-in-entity-framework-7--core-"/>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<ListOrder.Property> sortModels)
        {
            var expression = source.Expression;
            int count = 0;
            foreach (var item in sortModels)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                MemberExpression selector = null;

                if (item.Name.Contains("."))
                {
                    // first part goes to parameter - "x.Parameter"
                    Expression current = parameter;
                    foreach (var part in item.Name.Split('.'))
                    {
                        selector = Expression.PropertyOrField(current, part);
                        // subsequent parts go to selector itself: "x.Parameter.AnotherParameter"
                        current = selector;
                    }
                }
                else
                    selector = Expression.PropertyOrField(parameter, item.Name);

                var method = item.Ascending ?
                    (count == 0 ? "OrderBy" : "ThenBy") :
                    (count == 0 ? "OrderByDescending" : "ThenByDescending");
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                count++;
            }
            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
    }
}
