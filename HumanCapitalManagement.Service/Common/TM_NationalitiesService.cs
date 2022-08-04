using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class TM_NationalitiesService : ServiceBase<TM_Nationalities>
    {
        public TM_NationalitiesService(IRepository<TM_Nationalities> repo) : base(repo)
        {
            //
        }


        public TM_Nationalities Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_Nationalities> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }


        public IEnumerable<TM_Nationalities> GetDataForSelectByID(string Id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.Id.ToString() == Id);
            return sQuery.ToList();
        }

        public IEnumerable<TM_Nationalities> GetGenderForSave(int[] aID)//,bool isAdmin
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
    }
}
