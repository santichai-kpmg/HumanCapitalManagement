using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_Mass_ScoringService : ServiceBase<TM_Mass_Scoring>
    {
        public TM_Mass_ScoringService(IRepository<TM_Mass_Scoring> repo) : base(repo)
        {
            //
        }

        public TM_Mass_Scoring Find(int id)
        {
            var Return = Query().FirstOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }
        public TM_Mass_Scoring FindByCode(string id)
        {
            var Return = Query().FirstOrDefault(s => s.scoring_code == id);
            return Return != null ? Return : null;
        }
        public IEnumerable<TM_Mass_Scoring> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
        public IEnumerable<TM_Mass_Scoring> GetDataForSelectOld()
        {
            var sQuery = Query().Where(w => w.active_status == "N").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
