
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.VisaExpiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.VisaExpiry
{
    public class TM_Type_DocumentService : ServiceBase<TM_Type_Document>
    {
        public TM_Type_DocumentService(IRepository<TM_Type_Document> repo) : base(repo)
        {
            //
        }
        public IEnumerable<TM_Type_Document> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
    }
}
