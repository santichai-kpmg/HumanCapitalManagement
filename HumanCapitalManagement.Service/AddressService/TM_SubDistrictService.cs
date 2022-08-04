using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;

namespace HumanCapitalManagement.Service.AddressService
{
    public class TM_SubDistrictService : ServiceBase<TM_SubDistrict>
    {
        public TM_SubDistrictService(IRepository<TM_SubDistrict> repo) : base(repo)
        {
            //
        }

        public TM_SubDistrict Find(int id)
        {

            var Return = Query().SingleOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }

        public IEnumerable<TM_SubDistrict> FindListBySubDistrictID(int id)
        {
            return Query().Where(s => s.Id == id && s.active_status == "Y").ToList();
            //return Query().Where(s => s.TM_Universitys_Faculty.TM_Universitys.Id == id).ToList();
        }

        public IEnumerable<TM_SubDistrict> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }   

    }
}
