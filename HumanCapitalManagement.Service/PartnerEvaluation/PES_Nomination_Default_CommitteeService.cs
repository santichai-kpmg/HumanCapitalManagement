using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PES_Nomination_Default_CommitteeService : ServiceBase<PES_Nomination_Default_Committee>
    {
        public PES_Nomination_Default_CommitteeService(IRepository<PES_Nomination_Default_Committee> repo) : base(repo)
        {
            //
        }
        public PES_Nomination_Default_Committee Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<PES_Nomination_Default_Committee> Get0DataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }


        public IEnumerable<PES_Nomination_Default_Committee> GetCommitteeForAD_PTR(int nID)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.committee_type == "N" && w.PES_Nomination_Year.Id == nID);
            return sQuery.ToList();
        }

        public IEnumerable<PES_Nomination_Default_Committee> GetCommitteeForShareholder(int nID)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.committee_type == "S" && w.PES_Nomination_Year.Id == nID);
            return sQuery.ToList();
        }


        #region Save Edit Delect 

        public bool CanSave(PES_Nomination_Default_Committee PES_Nomination_Default_Committee)
        {
            bool sCan = false;
            var sCheck = Query().FirstOrDefault(w => w.user_no == PES_Nomination_Default_Committee.user_no
                && w.PES_Nomination_Year.Id == PES_Nomination_Default_Committee.PES_Nomination_Year.Id
                && w.active_status == "Y"
                && w.committee_type == PES_Nomination_Default_Committee.committee_type);
            if (sCheck == null)
            {
                sCan = true;
            }

            return sCan;
        }

        public int CreateNewOrUpdate(PES_Nomination_Default_Committee s)
        {
            var sResult = 0;
            var objSave = s;
            var _getDataTIF = Query().Where(w => w.user_no == objSave.user_no && w.committee_type == s.committee_type && w.PES_Nomination_Year.Id == s.PES_Nomination_Year.Id).FirstOrDefault();
            if (_getDataTIF == null)
            {
                Add(s);
            }
            else
            {

                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                _getDataTIF.active_status = s.active_status;
                s = _getDataTIF;
            }
            sResult = SaveChanges();
            return sResult;
        }
        #endregion
    }
}
