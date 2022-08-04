using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PreInternAssessment;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PreInternAssessment
{
    public class TM_PIntern_StatusService : ServiceBase<TM_PIntern_Status>
    {
        public TM_PIntern_StatusService(IRepository<TM_PIntern_Status> repo) : base(repo)
        {
            //
        }
        public TM_PIntern_Status Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_PIntern_Status> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
