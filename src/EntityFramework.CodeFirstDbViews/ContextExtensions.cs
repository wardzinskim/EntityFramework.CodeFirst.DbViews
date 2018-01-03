using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EntityFramework.CodeFirst.DbViews
{
    internal static class ContextExtensions
    {
        internal static string GetTableName(this DbContext context, Type entityType)
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            var methodInfo = typeof(ContextExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                .Where(method => method.Name == nameof(GetTableName))
                .Where(method => method.ContainsGenericParameters)
                .Where(method => method.GetParameters().FirstOrDefault()?.ParameterType == typeof(ObjectContext))
                .FirstOrDefault();

            return (string)methodInfo?.MakeGenericMethod(entityType)
                .Invoke(objectContext, new object[] {objectContext });
        }

        internal static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        private static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex(@"FROM \[dbo\].\[(?<table>.*)\] AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }
}
