using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class TM_PTR_Eva_StatusService : ServiceBase<TM_PTR_Eva_Status>
    {
        public TM_PTR_Eva_StatusService(IRepository<TM_PTR_Eva_Status> repo) : base(repo)
        {
            //
        }
        public TM_PTR_Eva_Status Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_PTR_Eva_Status> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
    }
}
