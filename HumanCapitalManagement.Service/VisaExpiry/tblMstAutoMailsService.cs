using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.VisaExpiry
{
    public class tblMstAutoMailsService : ServiceBase<tblMstAutoMails>
    {
        public tblMstAutoMailsService(IRepository<tblMstAutoMails> repo) : base(repo)
        {
            //
        }
       
    }
}
