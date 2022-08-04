using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PES_NominationService : ServiceBase<PES_Nomination>
    {
        public PES_NominationService(IRepository<PES_Nomination> repo) : base(repo)
        {
            //
        }
        public PES_Nomination Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public PES_Nomination FindFor3YearKPI(string id, int nYear)
        {

            return Query().FirstOrDefault(s => s.user_no == id && s.PES_Nomination_Year.evaluation_year.Value.Year == nYear);
        }

        public IEnumerable<PES_Nomination> GetPESReport(int nYear, int nStatus_id, string user_no, List<int> nStatus, bool isAdmin = false)//,bool isAdmin
        {

            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.PES_Nomination_Year.active_status == "Y" && nStatus.Contains(w.TM_PES_NMN_Status.Id));

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PES_Nomination_Year.Id == nYear);
            }
            if (nStatus_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PES_NMN_Status.Id == nStatus_id);
            }
            if (!isAdmin)
            {
                sQuery = sQuery.Where(w => (w.PES_Nomination_Signatures.Any(a => a.Req_Approve_user == user_no && a.active_status == "Y" )));
            }
            return sQuery.ToList();
        }


        public IEnumerable<PES_Nomination> GetNominationList(int nYear, string user_no, bool isAdmin = false)//,bool isAdmin
        {
            List<int> nStatus = new List<int>();
            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.PES_Nomination_Year.active_status == "Y");

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PES_Nomination_Year.Id == nYear);
            }
            if (isAdmin)
            {

            }
            else
            {
                sQuery = sQuery.Where(w => w.user_no == user_no);
            }
            //if (!string.IsNullOrEmpty(status))
            //{
            //    sQuery = sQuery.Where(w => w.active_status == status);
            //}
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public IEnumerable<PES_Nomination> GetPTRApprove(int nYear, string user_no, bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.PES_Nomination_Year.active_status == "Y"
            && !(w.PES_Nomination_Signatures.Any(w2 => (w2.TM_PES_NMN_SignatureStep.Id != 1) && w2.Approve_status != "Y")));

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PES_Nomination_Year.Id == nYear);
            }
            if (isAdmin)
            {

            }
            else
            {
                sQuery = sQuery.Where(w => (w.PES_Nomination_Signatures.Where(w2 => (w2.TM_PES_NMN_SignatureStep.Id != 1) && w2.Approve_status != "Y").Select(s2 => s2.Req_Approve_user).ToArray()).Contains(user_no));
            }
            //if (!string.IsNullOrEmpty(status))
            //{
            //    sQuery = sQuery.Where(w => w.active_status == status);
            //}
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }


        public bool CanSave(PES_Nomination PES_Nomination)
        {
            bool sCan = false;
            if (PES_Nomination.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => w.user_no == PES_Nomination.user_no && PES_Nomination.PES_Nomination_Year.Id == w.PES_Nomination_Year.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => w.user_no == PES_Nomination.user_no && PES_Nomination.PES_Nomination_Year.Id == w.PES_Nomination_Year.Id && w.Id != PES_Nomination.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        public int CreateNewOrUpdate(PES_Nomination s)
        {
            var sResult = 0;
            var _getDataTIF = Query().Where(w => w.PES_Nomination_Year.Id == s.PES_Nomination_Year.Id && w.user_no == s.user_no).FirstOrDefault();
            if (_getDataTIF == null)
            {
                Add(s);
            }
            else
            {
                _getDataTIF.comments = s.comments;
                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                _getDataTIF.active_status = s.active_status;
            }
            sResult = SaveChanges();
            return sResult;
        }

        public int Complect(PES_Nomination s)
        {
            var sResult = 0;
            var _getDataTIF = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataTIF != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
    }
}
