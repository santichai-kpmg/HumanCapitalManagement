using HumanCapitalManagement.Models.VisaExpiry;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Data.VisaExpiry
{
    public class TM_Type_DocumentRepository : RepositoryBase<TM_Type_Document>
    {
        public TM_Type_DocumentRepository(DbContext context) : base(context)
        {
            //
        }
    }
}
