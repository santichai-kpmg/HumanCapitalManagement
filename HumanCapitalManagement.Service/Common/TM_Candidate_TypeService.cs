using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class TM_Candidate_TypeService : ServiceBase<TM_Candidate_Type>
    {
        public TM_Candidate_TypeService(IRepository<TM_Candidate_Type> repo) : base(repo)
        {
            //
        }
        public TM_Candidate_Type Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Candidate_Type> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
    }
}
