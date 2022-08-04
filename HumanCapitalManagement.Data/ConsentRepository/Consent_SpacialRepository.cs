
using HumanCapitalManagement.Models.ConsentModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class Consent_SpacialRepository : RepositoryBase<Consent_Spacial>
    {
        public Consent_SpacialRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
