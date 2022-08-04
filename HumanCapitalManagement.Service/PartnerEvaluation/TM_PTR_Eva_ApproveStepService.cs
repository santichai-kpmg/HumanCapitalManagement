using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class TM_PTR_Eva_ApproveStepService : ServiceBase<TM_PTR_Eva_ApproveStep>
    {
        public TM_PTR_Eva_ApproveStepService(IRepository<TM_PTR_Eva_ApproveStep> repo) : base(repo)
        {
            //
        }
        public TM_PTR_Eva_ApproveStep Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_PTR_Eva_ApproveStep> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
    }
}
