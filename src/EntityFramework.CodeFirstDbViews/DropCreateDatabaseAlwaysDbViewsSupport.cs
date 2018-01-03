using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.CodeFirst.DbViews
{
    public class DropCreateDatabaseAlwaysDbViewsSupport<TContext> : DropCreateDatabaseAlways<TContext> where TContext : DbContext
    {
        private readonly string _pathToDirectoryWithSqlScript;

        public DropCreateDatabaseAlwaysDbViewsSupport(string pathToDirectoryWithSqlScript)
        {
            _pathToDirectoryWithSqlScript = pathToDirectoryWithSqlScript;
        }

        public override void InitializeDatabase(TContext context)
        {
            base.InitializeDatabase(context);

            var dbViewNames = DbViewsInitializationHelper.DeleteTables(context);
            DbViewsInitializationHelper.CreateViews(context.Database, _pathToDirectoryWithSqlScript);
            DbViewsInitializationHelper.ValidateIfAllViewsHaveBeenCreated(context.Database, dbViewNames);
        }
    }
}
