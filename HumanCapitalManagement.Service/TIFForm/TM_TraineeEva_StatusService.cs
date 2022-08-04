using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_TraineeEva_StatusService : ServiceBase<TM_TraineeEva_Status>
    {
        public TM_TraineeEva_StatusService(IRepository<TM_TraineeEva_Status> repo) : base(repo)
        {
            //
        }
        public TM_TraineeEva_Status Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_TraineeEva_Status FindForSave(int id)
        {

            var sQuert = Query().FirstOrDefault(s => s.Id == id);
            if (sQuert != null)
            {
                return sQuert;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<TM_TraineeEva_Status> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y").OrderBy(o => o.seq);
            return sQuery.ToList();
        }
    }
}
