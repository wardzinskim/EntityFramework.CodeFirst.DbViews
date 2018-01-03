using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.CodeFirst.DbViews
{
    public class IncorrectModelAfterCreatingViewsException : Exception
    {
        public IncorrectModelAfterCreatingViewsException(string message) : base(message)
        {
            
        }
    }
}
