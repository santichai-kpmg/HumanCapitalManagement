using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.Common
{

    public class TM_Trainee_HiringRatingService : ServiceBase<TM_Trainee_HiringRating>
    {
        public TM_Trainee_HiringRatingService(IRepository<TM_Trainee_HiringRating> repo) : base(repo)
        {
            //
        }

        public IEnumerable<TM_Trainee_HiringRating> GetDataForall()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public TM_Trainee_HiringRating Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<TM_Trainee_HiringRating> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }


        public IEnumerable<TM_Trainee_HiringRating> GetDataForSelectByID(string Id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.Id.ToString() == Id);
            return sQuery.ToList();
        }

        public IEnumerable<TM_Trainee_HiringRating> GetGenderForSave(int[] aID)//,bool isAdmin
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
