using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.VisaExpiry
{
    public class TM_Prefix_VisaService : ServiceBase<TM_Prefix_Visa>
    {
        public TM_Prefix_VisaService(IRepository<TM_Prefix_Visa> repo) : base(repo)
        {
            //
        }


        public TM_Prefix_Visa Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_Prefix_Visa> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }


        public IEnumerable<TM_Prefix_Visa> GetDataForSelectByID(string Id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.Id.ToString() == Id);
            return sQuery.ToList();
        }

        public IEnumerable<TM_Prefix_Visa> GetGenderForSave(int[] aID)//,bool isAdmin
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
