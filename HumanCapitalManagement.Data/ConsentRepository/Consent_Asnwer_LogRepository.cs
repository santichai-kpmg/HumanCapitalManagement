
using HumanCapitalManagement.Models.ConsentModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class Consent_Asnwer_LogRepository : RepositoryBase<Consent_Asnwer_Log>
    {
        public Consent_Asnwer_LogRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
