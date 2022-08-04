using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TraineeSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Time_TypeService : ServiceBase<TM_Time_Type>
    {
        public TM_Time_TypeService(IRepository<TM_Time_Type> repo) : base(repo)
        {
            //
        }
        public TM_Time_Type Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.active_status == "Y");


        }
        public TM_Time_Type Find2(int id)
        {
            var sQuery = Query().FirstOrDefault(s => s.Id == id && s.active_status == "Y");
            if (sQuery != null)
            {
                return sQuery;
            }
            else
            {
                return null;
            }



        }
        public TM_Time_Type FindByName(string name)
        {
            return Query().SingleOrDefault(s => s.type_name_en == name && s.active_status == "Y");
        }
        public TM_Time_Type FindByShortName(string shortname)
        {
            return Query().SingleOrDefault(s => s.type_short_name_en == shortname && s.active_status == "Y");
        }

        public IEnumerable<TM_Time_Type> GetDataForSelect()
        {
            var sQuery = Query();
            return sQuery.ToList();
        }

    }
}
