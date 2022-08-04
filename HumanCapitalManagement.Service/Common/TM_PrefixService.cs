using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HumanCapitalManagement.Service.Common
{
    public class TM_PrefixService : ServiceBase<TM_Prefix>
    {
        public TM_PrefixService(IRepository<TM_Prefix> repo) : base(repo)
        {
            //
        }


        public TM_Prefix Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_Prefix> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }


        public IEnumerable<TM_Prefix> GetDataForSelectByID(string Id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.Id.ToString() == Id);
            return sQuery.ToList();
        }

        public IEnumerable<TM_Prefix> GetGenderForSave(int[] aID)//,bool isAdmin
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
