using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.VisaExpiry
{
    public class TM_Remark_VisaService : ServiceBase<TM_Remark_Visa>
    {
        public TM_Remark_VisaService(IRepository<TM_Remark_Visa> repo) : base(repo)
        {
            //
        }


        public TM_Remark_Visa Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_Remark_Visa> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }


        public IEnumerable<TM_Remark_Visa> GetDataForSelectByID(string Id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.Id.ToString() == Id);
            return sQuery.ToList();
        }

     
    }
}
