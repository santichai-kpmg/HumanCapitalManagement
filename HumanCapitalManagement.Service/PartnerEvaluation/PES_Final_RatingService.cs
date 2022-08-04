using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PES_Final_RatingService : ServiceBase<PES_Final_Rating>
    {
        public PES_Final_RatingService(IRepository<PES_Final_Rating> repo) : base(repo)
        {
            //
        }
        public PES_Final_Rating FindForYearRate(string user_no, int nYear)
        {
            if (Query().Any(s => s.user_no == user_no && s.PES_Final_Rating_Year.evaluation_year.Value.Year == nYear && s.active_status == "Y"))
            {
                return Query().Where(s => s.user_no == user_no && s.PES_Final_Rating_Year.evaluation_year.Value.Year == nYear && s.active_status == "Y").FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        public int Update(PES_Final_Rating s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int CreateNewOrUpdate(PES_Final_Rating s)
        {
            var sResult = 0;
            var objSave = s;
            var _getDataTIF = Query().Where(w => w.PES_Final_Rating_Year.Id == objSave.PES_Final_Rating_Year.Id && w.user_no == s.user_no).FirstOrDefault();
            if (_getDataTIF == null)
            {
                Add(s);
            }
            else
            {

                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                _getDataTIF.active_status = s.active_status;
                _getDataTIF.Final_TM_Annual_Rating_Id = s.Final_TM_Annual_Rating_Id;

                s = _getDataTIF;
            }
            sResult = SaveChanges();
            return sResult;
        }
    }
}
