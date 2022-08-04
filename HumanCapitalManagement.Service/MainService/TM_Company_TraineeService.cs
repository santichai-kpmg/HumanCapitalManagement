using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Company_TraineeService : ServiceBase<TM_Company_Trainee>
    {
        public TM_Company_TraineeService(IRepository<TM_Company_Trainee> repo) : base(repo)
        {
            //
        }
        public TM_Company_Trainee Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Company_Trainee FindByUnitGroupID(string UnitGroupID)
        {
            return Query().SingleOrDefault(s => s.UnitGroupID == UnitGroupID);
        }
        public List<TM_Company_Trainee> GetDataForSelect()
        {
            return Query().ToList();
        }

        #region Save Edit Delect 
        public int CreateNew(TM_Company_Trainee s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Company_Trainee s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
             
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
