using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.HCMServiceClass;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_PR_Candidate_MappingService : ServiceBase<TM_PR_Candidate_Mapping>
    {
        public TM_PR_Candidate_MappingService(IRepository<TM_PR_Candidate_Mapping> repo) : base(repo)
        {
            //
        }
        public TM_PR_Candidate_Mapping Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_PR_Candidate_Mapping FindForHR(int id, bool isAdmin = false, bool isHRAdmin = false)
        {
            var sQuery = Query();

            if (!isAdmin && !isHRAdmin)
            {
                sQuery = sQuery.Where(w => w.PersonnelRequest.TM_Divisions.Id != (int)UnitGroup.HR);
            }

            return sQuery.SingleOrDefault(s => s.Id == id);
        }
        public TM_PR_Candidate_Mapping FindCreateEva(int id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            List<int> lstNotSelect = new List<int>();
            lstNotSelect.Add((int)StatusCandidate.Turndown);
            lstNotSelect.Add((int)StatusCandidate.NoShow);
            sQuery = sQuery.Where(w => w.TM_Candidate_Status_Cycle.Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard) &&
             !w.TM_Candidate_Status_Cycle.Any(a => lstNotSelect.Contains(a.TM_Candidate_Status.Id)));
            return sQuery.SingleOrDefault(s => s.Id == id);
        }
        public bool CanCreateMapping(TM_PR_Candidate_Mapping s)
        {
            bool sCan = false;
            var sQuery = Query();
            var _checkData = sQuery.Where(w => w.TM_Candidates.Id == s.TM_Candidates.Id && w.PersonnelRequest.Id == s.PersonnelRequest.Id).FirstOrDefault();
            if (_checkData == null)
            {
                sCan = true;
            }
            return sCan;
        }
        public IEnumerable<TM_PR_Candidate_Mapping> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_PR_Candidate_Mapping> GetForLeadtime(DateTime dTarget, string user_no, bool isAdmin = false)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && DbFunctions.TruncateTime(w.PersonnelRequest.request_date.Value) >= DbFunctions.TruncateTime(dTarget));
            sQuery = sQuery.Where(w => w.PersonnelRequest.TM_Employment_Request.TM_Employment_Type.personnel_type + "" == "S");
            sQuery = sQuery.Where(w => w.PersonnelRequest.TM_PR_Status.Id != (int)StatusPR.Cancel);
            if (!isAdmin)
            {
                if (!string.IsNullOrEmpty(user_no))
                {
                    sQuery = sQuery.Where(w => w.TM_Recruitment_Team != null && w.TM_Recruitment_Team.user_no == user_no);
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
            }

            return sQuery.ToList();
        }
        public IEnumerable<TM_PR_Candidate_Mapping> GetMajorForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<TM_PR_Candidate_Mapping> GetDataForEva(int Candidate_id, bool isAdmin = false)
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            List<int> lstNotSelect = new List<int>();

            lstNotSelect.Add((int)StatusCandidate.Withdrawn);
            lstNotSelect.Add((int)StatusCandidate.Turndown);
            lstNotSelect.Add((int)StatusCandidate.NoShow);
            lstNotSelect.Add((int)StatusCandidate.Blacklist);
            lstNotSelect.Add((int)StatusCandidate.Reject_Before_Sending_Hiring);
            lstNotSelect.Add((int)StatusCandidate.Withdraw_the_Offer_KPMG);
            lstNotSelect.Add((int)StatusCandidate.Reject_Before_Offer_Date);

            lstNotSelect.Add((int)StatusCandidate.Offer_Rejected);

            sQuery = sQuery.Where(w =>

            w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard) &&
             !w.TM_Candidate_Status_Cycle.Any(a => lstNotSelect.Contains(a.TM_Candidate_Status.Id) && a.active_status == "Y"));
            if (!isAdmin)
            {

                sQuery = sQuery.Where(w => w.TM_Candidates.Id == Candidate_id);
            }


            return sQuery.ToList();
        }
        public IEnumerable<TM_PR_Candidate_Mapping> GetDataForAcknow()
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview);

            sQuery = sQuery.Where(w => string.IsNullOrEmpty(w.PersonnelRequest.type_of_TIFForm)

            // && (w.PersonnelRequest.type_of_TIFForm + "" == "N" && w.TM_Candidate_TIF.TM_Candidate_TIF_Approv.Where(w2 => string.IsNullOrEmpty(w2.Approve_status)).Count() <= 0)
            );
            sQuery = sQuery.Where(w => w.TM_Candidate_Status_Cycle.Any(a => a.TM_Candidate_Status.Id == 2 && a.active_status == "Y"));
            return sQuery.ToList();
        }
        public IEnumerable<TM_PR_Candidate_Mapping> GetDataForTimeSheet_CDD(int Candidate_id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_Candidates.Id == Candidate_id);


            return sQuery.ToList();
        }

        public IEnumerable<TM_PR_Candidate_Mapping> GetDataForTimeSheet_id(int id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.Id == id);


            return sQuery.ToList();
        }

        public IEnumerable<TM_PR_Candidate_Mapping> GetDataForInterview(string PR_No,string Activities, string group_code, string[] agroup_code, string status, string sName, bool isAdmin = false)
        {
            List<int> nStatus = new List<int>();
            nStatus.Add((int)StatusPR.Awaiting_Approval);
            nStatus.Add((int)StatusPR.Recruiting);
            List<int> nCanStatus = new List<int>();
            nCanStatus.Add((int)StatusCandidate.Withdrawn);
            nCanStatus.Add((int)StatusCandidate.Turndown);
            nCanStatus.Add((int)StatusCandidate.NoShow);
            nCanStatus.Add((int)StatusCandidate.Blacklist);
            nCanStatus.Add((int)StatusCandidate.Reject_Before_Sending_Hiring);
            nCanStatus.Add((int)StatusCandidate.Withdraw_the_Offer_KPMG);
            nCanStatus.Add((int)StatusCandidate.Reject_Before_Offer_Date);

            nCanStatus.Add((int)StatusCandidate.Offer_Rejected);


            var sQuery = Query().Where(w => w.active_status == "Y" && !w.TM_Candidate_Status_Cycle.Any(a => nCanStatus.Contains(a.TM_Candidate_Status.Id) && a.active_status == "Y")
            && nStatus.Contains(w.PersonnelRequest.TM_PR_Status.Id));

            if (!isAdmin)
            {
                if ((agroup_code != null && agroup_code.Length > 0))
                {
                    sQuery = sQuery.Where(w => agroup_code.Contains(w.PersonnelRequest.TM_Divisions.division_code));
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
                //if ()
                //{

                //}
            }
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.PersonnelRequest.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(sName))
            {
                sQuery = sQuery.Where(w => ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            }
            if (!string.IsNullOrEmpty(PR_No))
            {
                sQuery = sQuery.Where(w => w.PersonnelRequest.Id + "" == PR_No);
            }
            if (!string.IsNullOrEmpty(Activities))
            {
                sQuery = sQuery.Where(w => w.TM_Candidates.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities.Id + "" == Activities);
            }
            return sQuery.ToList();
        }

        public IEnumerable<TM_PR_Candidate_Mapping> GetDataForPreIntern(string group_code, string[] agroup_code, string status, string sName, bool isAdmin = false)
        {
            List<int> nStatus = new List<int>();
            nStatus.Add((int)StatusPR.Awaiting_Approval);
            nStatus.Add((int)StatusPR.Recruiting);
            List<int> nCanStatus = new List<int>();
            nCanStatus.Add((int)StatusCandidate.Withdrawn);
            nCanStatus.Add((int)StatusCandidate.Turndown);
            nCanStatus.Add((int)StatusCandidate.NoShow);
            nCanStatus.Add((int)StatusCandidate.Blacklist);
            nCanStatus.Add((int)StatusCandidate.Reject_Before_Sending_Hiring);
            nCanStatus.Add((int)StatusCandidate.Withdraw_the_Offer_KPMG);
            nCanStatus.Add((int)StatusCandidate.Reject_Before_Offer_Date);

            nCanStatus.Add((int)StatusCandidate.Offer_Rejected);


            var sQuery = Query().Where(w => w.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en.Contains("Intern") && w.active_status == "Y" && !w.TM_Candidate_Status_Cycle.Any(a => nCanStatus.Contains(a.TM_Candidate_Status.Id) && a.active_status == "Y")
            && nStatus.Contains(w.PersonnelRequest.TM_PR_Status.Id));

            if (!isAdmin)
            {
                if ((agroup_code != null && agroup_code.Length > 0))
                {
                    sQuery = sQuery.Where(w => agroup_code.Contains(w.PersonnelRequest.TM_Divisions.division_code));
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
            }
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.PersonnelRequest.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(sName))
            {
                sQuery = sQuery.Where(w => ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            }
            return sQuery.ToList();
        }


        public IEnumerable<TM_PR_Candidate_Mapping> GetDataCandidateReport(string group_code, string[] agroup_code, string status,
             string ref_no,
            int supgroup_id,
            int position_id,
            string sName,
            DateTime? date_strat, DateTime? date_to, bool isAdmin = false)
        {
            var sQuery = Query().Where(w => w.active_status == "Y");

            if (!isAdmin)
            {
                if ((agroup_code != null && agroup_code.Length > 0))
                {
                    sQuery = sQuery.Where(w => agroup_code.Contains(w.PersonnelRequest.TM_Divisions.division_code));
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
            }

            if (!string.IsNullOrEmpty(ref_no))
            {
                sQuery = sQuery.Where(w => ((w.PersonnelRequest.RefNo + "").Trim()).ToLower() == ((ref_no + "").Trim()).ToLower());
            }
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.PersonnelRequest.TM_Divisions.division_code == group_code);
            }
            if (supgroup_id != 0)
            {
                sQuery = sQuery.Where(w => w.PersonnelRequest.TM_SubGroup.Id == supgroup_id);
            }
            if (position_id != 0)
            {
                sQuery = sQuery.Where(w => w.PersonnelRequest.TM_Position.Id == position_id);
            }
            if (!string.IsNullOrEmpty(sName))
            {
                sQuery = sQuery.Where(w => ((w.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (w.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? (
                (w.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (w.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((w.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            }

            return sQuery.ToList();
        }
        #region Save Edit Delect 
        public int CreateNew(TM_PR_Candidate_Mapping s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_PR_Candidate_Mapping s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
