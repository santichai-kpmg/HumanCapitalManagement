using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;

namespace HumanCapitalManagement.Service.AddressService
{
    public class TM_CityService : ServiceBase<TM_City>
    {

        public TM_CityService(IRepository<TM_City> repo) : base(repo)
        {
            //
        }

        public TM_City Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_City> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y"   );
            return sQuery.ToList();
        }

        public IEnumerable<TM_City> GetDataById(int id)
        {
            var sQuery = Query().Where(w => w.Id == id && w.active_status == "Y");
            return sQuery.ToList();
        }


        public IEnumerable<TM_City> GetCityForSave(int[] aID)//,bool isAdmin
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
