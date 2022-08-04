using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;

namespace HumanCapitalManagement.Service.AddressService
{
    public  class TM_CountryService : ServiceBase<TM_Country>
    {

        public TM_CountryService(IRepository<TM_Country> repo) : base(repo)
        {
            //
        }
        public TM_Country Find(int id)
        {
            var Return = Query().SingleOrDefault(s => s.Id == id);
            return Return != null ? Return : null;
        }

        public IEnumerable<TM_Country> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

    }
}
