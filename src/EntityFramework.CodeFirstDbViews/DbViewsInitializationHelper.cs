using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EntityFramework.CodeFirst.DbViews
{
    internal class DbViewsInitializationHelper
    {
        internal static IEnumerable<string> DeleteTables<TContext>(TContext context) where TContext : DbContext
        {
            var deletedTables = new List<string>();

            var propertyInfos = typeof(TContext).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var propertyInfo in propertyInfos)
            {
                if (IsDbView(propertyInfo.PropertyType))
                {
                    var entityType = propertyInfo.PropertyType.GenericTypeArguments[0];
                    var tableName = context.GetTableName(entityType);
                    deletedTables.Add(tableName);
                    DeleteTable(context.Database, tableName);
                }
            }
            return deletedTables;
        }

        private static bool IsDbView(Type type)
        {
            if (!type.IsGenericType) return false;
            var entityType = type.GenericTypeArguments[0];

            return typeof(DbSet<>).MakeGenericType(entityType).IsAssignableFrom(type) &&
                   entityType.GetCustomAttributes(typeof(DbView)).Any();
        }

        private static void DeleteTable(Database db, string tableName)
        {
            db.ExecuteSqlCommand($@"IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
            WHERE TABLE_NAME = N'{tableName}')
            BEGIN
                DROP TABLE {tableName}
            END");
        }

        internal static void CreateViews(Database db, string pathToDirWithScripts)
        {
            var sqls = Directory.GetFiles(pathToDirWithScripts, "*.sql").OrderBy(x => x);
            foreach (var sql in sqls)
            {
                db.ExecuteSqlCommand(File.ReadAllText(sql));
            }
        }

        internal static void ValidateIfAllViewsHaveBeenCreated(Database db, IEnumerable<string> dbViewNames)
        {
            foreach (var dbViewName in dbViewNames)
            {
                var result = db.SqlQuery<int>($@"IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = '{dbViewName}')
                    SELECT 1
                ELSE
                    SELECT 0").First();
                if (result == 0)
                {
                    throw new IncorrectModelAfterCreatingViewsException($"view {dbViewName} does not exists");
                }
            }
        }

        internal static void DropAllDbViews(Database db)
        {
            db.ExecuteSqlCommand($@"DECLARE @viewName varchar(500) 
                    DECLARE cur CURSOR FOR SELECT [name] FROM sys.objects WHERE TYPE = 'v' 
                    OPEN cur 
                    FETCH NEXT FROM cur INTO @viewName 
                    WHILE @@fetch_status = 0 
                    BEGIN 
                     EXEC('drop view ' + @viewName) 
                     FETCH NEXT FROM cur INTO @viewName 
                    END
                    CLOSE cur 
                    DEALLOCATE cur");
        }

    }
}
