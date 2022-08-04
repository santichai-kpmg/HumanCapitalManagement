using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TIFForm
{

    public class TM_Mass_Question_TypeRepository : RepositoryBase<TM_Mass_Question_Type>
    {
        public TM_Mass_Question_TypeRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
