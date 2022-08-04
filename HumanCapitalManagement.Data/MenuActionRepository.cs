using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data
{
    public class MenuActionRepository : RepositoryBase<MenuAction>
    {
        public MenuActionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
