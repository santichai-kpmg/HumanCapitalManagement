using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TIFForm
{
    public class TM_TIF_StatusRepository : RepositoryBase<TM_TIF_Status>
    {
        public TM_TIF_StatusRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
