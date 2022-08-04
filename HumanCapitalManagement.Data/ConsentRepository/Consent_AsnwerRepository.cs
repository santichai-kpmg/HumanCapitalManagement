
using HumanCapitalManagement.Models.ConsentModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class Consent_AsnwerRepository : RepositoryBase<Consent_Asnwer>
    {
        public Consent_AsnwerRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
