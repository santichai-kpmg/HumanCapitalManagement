using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_MassTIF_StatusService : ServiceBase<TM_MassTIF_Status>
    {
        public TM_MassTIF_StatusService(IRepository<TM_MassTIF_Status> repo) : base(repo)
        {
            //
        }
        public TM_MassTIF_Status Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_MassTIF_Status> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
