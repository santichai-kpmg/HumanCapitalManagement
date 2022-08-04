using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class GroupListPermissionRepository : RepositoryBase<GroupListPermission>
    {
        public GroupListPermissionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
