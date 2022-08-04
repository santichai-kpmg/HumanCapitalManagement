using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.TIFForm
{
    public class TM_Additional_InformationRepository : RepositoryBase<TM_Additional_Information>
    {
        public TM_Additional_InformationRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
