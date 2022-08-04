using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_TechnicalTestService : ServiceBase<TM_TechnicalTest>
    {

        public TM_TechnicalTestService(IRepository<TM_TechnicalTest> repo) : base(repo)
        {
            //
        }
        public TM_TechnicalTest Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_TechnicalTest> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_TechnicalTest> GetTestNameForSave(int[] aID)//,bool isAdmin
        {
            var sQuery = Query();
            if (aID != null)
            {
                sQuery = sQuery.Where(w => aID.Contains(w.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public int Update(TM_TechnicalTest s)
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
