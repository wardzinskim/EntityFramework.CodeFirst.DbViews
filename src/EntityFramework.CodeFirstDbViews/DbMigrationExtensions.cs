using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.CodeFirst.DbViews
{
    public static class DbMigrationExtensions
    {
        public static void DropView(this DbMigration migration, string name)
        {
            var sql = $"DROP VIEW {name}";

            var method = typeof(DbMigration).GetMethod("Sql", BindingFlags.NonPublic | BindingFlags.Instance);

            method?.Invoke(migration, new object[] { sql, false, null });
        }
    }
}
