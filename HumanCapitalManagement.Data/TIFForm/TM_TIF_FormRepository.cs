using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TIFForm
{
    public class TM_TIF_FormRepository : RepositoryBase<TM_TIF_Form>
    {
        public TM_TIF_FormRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
