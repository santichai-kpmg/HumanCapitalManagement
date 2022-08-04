using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class TM_PES_NMN_SignatureStepService : ServiceBase<TM_PES_NMN_SignatureStep>
    {
        public TM_PES_NMN_SignatureStepService(IRepository<TM_PES_NMN_SignatureStep> repo) : base(repo)
        {
            //
        }
        public TM_PES_NMN_SignatureStep Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_PES_NMN_SignatureStep> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
    }
}
