
using HumanCapitalManagement.Models.ConsentModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class TM_Consent_FormRepository : RepositoryBase<TM_Consent_Form>
    {
        public TM_Consent_FormRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
