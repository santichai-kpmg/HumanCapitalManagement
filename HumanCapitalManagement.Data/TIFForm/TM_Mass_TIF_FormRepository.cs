using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TIFForm
{
    public class TM_Mass_TIF_FormRepository : RepositoryBase<TM_Mass_TIF_Form>
    {
        public TM_Mass_TIF_FormRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
