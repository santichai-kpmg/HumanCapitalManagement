using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{
    public class TM_GenderService : ServiceBase<TM_Gender>
    {
        public TM_GenderService(IRepository<TM_Gender> repo) : base(repo)
        {
            //
        }


        public TM_Gender Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_Gender> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }


        public IEnumerable<TM_Gender> GetDataForSelectByID(string Id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.Id.ToString() == Id);
            return sQuery.ToList();
        }

        public IEnumerable<TM_Gender> GetGenderForSave(int[] aID)//,bool isAdmin
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
