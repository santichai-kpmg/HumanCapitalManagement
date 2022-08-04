using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.OldTable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class DivisionRepository : RepositoryBase<TM_Divisions>
    {
        public DivisionRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
