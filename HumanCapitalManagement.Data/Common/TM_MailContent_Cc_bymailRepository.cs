using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class TM_MailContent_Cc_bymailRepository : RepositoryBase<TM_MailContent_Cc_bymail>
    {
        public TM_MailContent_Cc_bymailRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
