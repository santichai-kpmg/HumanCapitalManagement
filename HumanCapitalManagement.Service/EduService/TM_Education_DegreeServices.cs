using HumanCapitalManagement.Models.EducationModels;
using HumanCapitalManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.EduService
{
    public class TM_Education_DegreeServices : ServiceBase<TM_Education_Degree>
    {
        public TM_Education_DegreeServices(IRepository<TM_Education_Degree> repo) : base(repo)
        {
            //
        }
        public TM_Education_Degree Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }


        public IEnumerable<TM_Education_Degree> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }



    }
}
