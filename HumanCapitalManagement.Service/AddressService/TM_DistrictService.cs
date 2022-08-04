using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;

namespace HumanCapitalManagement.Service.AddressService
{
    public class TM_DistrictService : ServiceBase<TM_District>
    {
        public TM_DistrictService(IRepository<TM_District> repo) : base(repo)
        {
            //
        }

        public TM_District Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_District> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        public IEnumerable<TM_District> GetDataById(int id)
        {
            return Query().Where(w => w.Id == id && w.active_status == "Y").ToList();
     
        }


    }
}
