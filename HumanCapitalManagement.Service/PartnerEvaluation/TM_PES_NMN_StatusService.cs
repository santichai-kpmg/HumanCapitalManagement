using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class TM_PES_NMN_StatusService : ServiceBase<TM_PES_NMN_Status>
    {
        public TM_PES_NMN_StatusService(IRepository<TM_PES_NMN_Status> repo) : base(repo)
        {
            //
        }
        public TM_PES_NMN_Status Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_PES_NMN_Status> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
    }
}
