using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PTR_Evaluation_KPIsService : ServiceBase<PTR_Evaluation_KPIs>
    {
        public PTR_Evaluation_KPIsService(IRepository<PTR_Evaluation_KPIs> repo) : base(repo)
        {
            //
        }
        public int Update(PTR_Evaluation_KPIs s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
    }
}
