using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.PESServiceClass;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PTR_EvaluationService : ServiceBase<PTR_Evaluation>
    {
        public PTR_EvaluationService(IRepository<PTR_Evaluation> repo) : base(repo)
        {
            //
        }
        public PTR_Evaluation Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }

        public PTR_Evaluation FindFor3YearKPI(string id, int nYear)
        {

            return Query().FirstOrDefault(s => s.user_no == id && s.PTR_Evaluation_Year.evaluation_year.Value.Year == nYear);
        }

        public PTR_Evaluation FindReport(int id)
        {
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Draft_Evaluate);
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Evaluation_Completed);
            nStatus.Add((int)Eva_Status.Draft_Plan);
            nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);
            nStatus.Add((int)Eva_Status.Planning_Completed);
            return Query().SingleOrDefault(s => s.Id == id && nStatus.Contains(s.TM_PTR_Eva_Status.Id));
        }

        public IEnumerable<PTR_Evaluation> GetPTREvaluationList(int nYear, string user_no, bool isAdmin = false)//,bool isAdmin
        {
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Draft_Evaluate);
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Waiting_for_Revised_Evaluate);
            nStatus.Add((int)Eva_Status.Evaluation_Completed);
            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.PTR_Evaluation_Year.active_status == "Y" && nStatus.Contains(w.TM_PTR_Eva_Status.Id));

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PTR_Evaluation_Year.Id == nYear);
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
        public IEnumerable<PTR_Evaluation> GetPTRPerformancePlanList(int nYear, string user_no, bool isAdmin = false)//,bool isAdmin
        {
            List<int> nStatus = new List<int>();
            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.PTR_Evaluation_Year.active_status == "Y");

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PTR_Evaluation_Year.Id == nYear);
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
        public IEnumerable<PTR_Evaluation> GetPTRApprove(int nYear, string user_no, bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.PTR_Evaluation_Year.active_status == "Y"
            && !(w.PTR_Evaluation_Approve.Any(w2 => (w2.TM_PTR_Eva_ApproveStep.Id != 1 || w2.TM_PTR_Eva_ApproveStep.Id != 8) && w2.Approve_status != "Y")));

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PTR_Evaluation_Year.Id == nYear);
            }
            if (isAdmin)
            {

            }
            else
            {
                sQuery = sQuery.Where(w => (w.PTR_Evaluation_Approve.Where(w2 => (w2.TM_PTR_Eva_ApproveStep.Id != 1 || w2.TM_PTR_Eva_ApproveStep.Id != 8) && w2.Approve_status != "Y").Select(s2 => s2.Req_Approve_user).ToArray()).Contains(user_no));
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
        public IEnumerable<PTR_Evaluation> GetPTRApproveReport(int nYear, int nStatus_id, string user_no, bool isAdmin = false)//,bool isAdmin
        {
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Evaluation_Completed);
            nStatus.Add((int)Eva_Status.Draft_Evaluate);
            nStatus.Add((int)Eva_Status.Waiting_for_Revised_Evaluate);

            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.PTR_Evaluation_Year.active_status == "Y" && nStatus.Contains(w.TM_PTR_Eva_Status.Id));

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PTR_Evaluation_Year.Id == nYear);
            }
            if (nStatus_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PTR_Eva_Status.Id == nStatus_id);
            }
            if (!isAdmin)
            {
                sQuery = sQuery.Where(w => (w.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user == user_no && a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G") 
                || w.PTR_Evaluation_AuthorizedEva.Any(a2 => a2.authorized_user == user_no && a2.active_status == "Y")));
            }
            return sQuery.ToList();
        }
        public IEnumerable<PTR_Evaluation> GetPTRPlanApproveReport(int nYear, int nStatus_id, string user_no, bool isAdmin = false)//,bool isAdmin
        {
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Evaluation_Completed);
            nStatus.Add((int)Eva_Status.Draft_Evaluate);
            nStatus.Add((int)Eva_Status.Waiting_for_Revised_Evaluate);
            nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);
            nStatus.Add((int)Eva_Status.Planning_Completed);
            nStatus.Add((int)Eva_Status.Draft_Plan);
            nStatus.Add((int)Eva_Status.Waiting_for_Revised_Plan);

            var sQuery = Query().Where(w => 1 == 1 && w.active_status == "Y" && w.PTR_Evaluation_Year.active_status == "Y" && nStatus.Contains(w.TM_PTR_Eva_Status.Id) && w.PTR_Evaluation_Year.evaluation_year.Value.Year >= 2019);

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PTR_Evaluation_Year.Id == nYear);
            }
            if (nStatus_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PTR_Eva_Status.Id == nStatus_id);
            }
            if (!isAdmin)
            {
                sQuery = sQuery.Where(w => (w.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user == user_no && a.active_status == "Y") 
                || w.PTR_Evaluation_Authorized.Any(a2 => a2.authorized_user == user_no && a2.active_status == "Y")));
            }
            return sQuery.ToList();
        }
        public bool CanSave(PTR_Evaluation PTR_Evaluation)
        {
            bool sCan = false;
            if (PTR_Evaluation.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => w.user_no == PTR_Evaluation.user_no && PTR_Evaluation.PTR_Evaluation_Year.Id == w.PTR_Evaluation_Year.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => w.user_no == PTR_Evaluation.user_no && PTR_Evaluation.PTR_Evaluation_Year.Id == w.PTR_Evaluation_Year.Id && w.Id != PTR_Evaluation.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
    
        public int CreateNewOrUpdate(PTR_Evaluation s)
        {
            var sResult = 0;
            var _getDataTIF = Query().Where(w => w.PTR_Evaluation_Year.Id == s.PTR_Evaluation_Year.Id && w.user_no == s.user_no).FirstOrDefault();
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

        public int Complect(PTR_Evaluation s)
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
