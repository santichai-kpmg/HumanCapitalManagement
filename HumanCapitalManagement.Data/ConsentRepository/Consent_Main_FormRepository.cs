
using HumanCapitalManagement.Models.ConsentModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.MainRepository
{
    public class Consent_Main_FormRepository : RepositoryBase<Consent_Main_Form>
    {
        public Consent_Main_FormRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
