using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.Common
{
    public class MailContentRepository : RepositoryBase<MailContent>
    {
        public MailContentRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
