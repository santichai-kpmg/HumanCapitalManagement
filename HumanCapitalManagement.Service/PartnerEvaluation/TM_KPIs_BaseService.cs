using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class TM_KPIs_BaseService : ServiceBase<TM_KPIs_Base>
    {
        public TM_KPIs_BaseService(IRepository<TM_KPIs_Base> repo) : base(repo)
        {
            //
        }
        public TM_KPIs_Base Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_KPIs_Base> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
    }
}
