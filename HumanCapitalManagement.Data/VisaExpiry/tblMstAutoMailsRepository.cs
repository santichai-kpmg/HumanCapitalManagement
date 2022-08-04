using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.VisaExpiry
{
    public class tblMstAutoMailsRepository : RepositoryBase<tblMstAutoMails>
    {
        public tblMstAutoMailsRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
