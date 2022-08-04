using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.HCMServiceClass;

namespace HumanCapitalManagement.Service.MainService
{
    public class PersonnelRequestService : ServiceBase<PersonnelRequest>
    {
        public PersonnelRequestService(IRepository<PersonnelRequest> repo) : base(repo)
        {
            //
        }
        public PersonnelRequest Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.active_status == "Y");
        }

        public PersonnelRequest FindByRefNo(string ref_no)
        {
            return Query().SingleOrDefault(s => s.RefNo == ref_no && s.active_status == "Y");
        }

        public PersonnelRequest FindPRAdmin(int id)
        {
            List<int> nStatus = new List<int>();
            //Save_Draft = 1,
            //Awaiting_Approval = 2,
            //Recruiting = 3,
            //Completed = 4,
            //Reject = 5,
            //Cancel = 6,

            nStatus.Add((int)StatusPR.Cancel);
            nStatus.Add((int)StatusPR.Awaiting_Approval);
            nStatus.Add((int)StatusPR.Recruiting);
            nStatus.Add((int)StatusPR.Reject);
            nStatus.Add((int)StatusPR.Completed);
            return Query().SingleOrDefault(s => s.Id == id && s.active_status == "Y" && nStatus.Contains(s.TM_PR_Status.Id));
        }
        public bool CheckDuplicateReplace(ref string id)
        {
            bool bReturn = false;
            List<int> nStatus = new List<int>();
            nStatus.Add((int)StatusPR.Reject);
            nStatus.Add((int)StatusPR.Cancel);
            string user = id;
            if (Query().Any(a => !nStatus.Contains(a.TM_PR_Status.Id) && a.user_replaced == (user + "").Trim().ToLower() && a.active_status == "Y"))
            {
                bReturn = true;
                id = Query().Where(a => !nStatus.Contains(a.TM_PR_Status.Id) && a.user_replaced == (user + "").Trim().ToLower() && a.active_status == "Y").FirstOrDefault().RefNo + "";
            }
            return bReturn;
        }
        public PersonnelRequest FindForCancel(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id && s.active_status == "Y");
        }
        public PersonnelRequest FindForApprove(int id)
        {
            int[] aStatus = new int[] { 2, 3, 4 };
            return Query().SingleOrDefault(w => w.Id == id && (w.BUApprove_status + "" != "Y"
            || w.HeadApprove_status + "" != "Y"
            || (w.CeoApprove_status + "" != "Y" && w.need_ceo_approve + "" == "Y"))
            && aStatus.Contains(w.TM_PR_Status.Id)
            && w.active_status == "Y"
            );
        }

        public IEnumerable<PersonnelRequest> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        public PersonnelRequest FindForAddCadidate(int id)
        {
            int[] aStatus = new int[] { 3, 2 };
            //return Query().SingleOrDefault(w => w.Id == id && (w.BUApprove_status + "" == "Y"
            //&& w.HeadApprove_status + "" == "Y"
            //&& (w.CeoApprove_status + "" == "Y" || w.need_ceo_approve + "" != "Y"))
            //&& aStatus.Contains(w.TM_PR_Status.Id)
            //);
            return Query().SingleOrDefault(w => w.Id == id && w.active_status == "Y");
        }
        public IEnumerable<PersonnelRequest> GetPersonnelRqListData(string group_code,
            string[] agroup_code,
            string status,
            int pr_status,
            string ref_no,
            int supgroup_id,
            int position_id,
            bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.active_status == "Y");

            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            if (!string.IsNullOrEmpty(ref_no))
            {
                sQuery = sQuery.Where(w => ((w.RefNo + "").Trim()).ToLower() == ((ref_no + "").Trim()).ToLower());
            }
            if (pr_status != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PR_Status.Id == pr_status);
            }
            if (supgroup_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_SubGroup.Id == supgroup_id);
            }
            if (position_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_Position.Id == position_id);
            }
            if (!isAdmin)
            {
                if ((agroup_code != null && agroup_code.Length > 0))
                {
                    sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_Divisions.division_code));
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
            }

            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public IEnumerable<PersonnelRequest> GetPersonnelPRListData(string group_code,
        string[] agroup_code,
        string status,
        int pr_status,
        string ref_no,
        int supgroup_id,
        int position_id,
        string emp_no,
        bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.active_status == "Y");

            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            if (!string.IsNullOrEmpty(ref_no))
            {
                sQuery = sQuery.Where(w => ((w.RefNo + "").Trim()).ToLower() == ((ref_no + "").Trim()).ToLower());
            }
            if (pr_status != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PR_Status.Id == pr_status);
            }
            if (supgroup_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_SubGroup.Id == supgroup_id);
            }
            if (position_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_Position.Id == position_id);
            }
            if (!isAdmin)
            {
                sQuery = sQuery.Where(w => w.request_user == emp_no);
                if ((agroup_code != null && agroup_code.Length > 0))
                {
                    sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_Divisions.division_code));
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
            }

            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public IEnumerable<PersonnelRequest> GetPRForApprove(string group_code, string[] agroup_code, string user_no, string ref_no, bool isAdmin = false)//,bool isAdmin
        {
            int[] aStatus = new int[] { 2, 3, 4 };
            var sQuery = Query().Where(w => w.active_status == "Y" && aStatus.Contains(w.TM_PR_Status.Id)
            );
            if (isAdmin)
            {
                sQuery = sQuery.Where(w => (w.BUApprove_status + "" != "Y")
                     || (w.HeadApprove_status + "" != "Y")
                     || (w.CeoApprove_status + "" != "Y" && w.need_ceo_approve + "" == "Y"));
            }
            else
            {
                sQuery = sQuery.Where(w => (w.Req_BUApprove_user == user_no && w.BUApprove_status + "" != "Y")
                     || (w.Req_HeadApprove_user == user_no && w.HeadApprove_status + "" != "Y")
                     || (w.Req_CeoApprove_user == user_no && w.CeoApprove_status + "" != "Y" && w.need_ceo_approve + "" == "Y"));
            }
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_Divisions.division_code == group_code);
            }

            if (!string.IsNullOrEmpty(ref_no))
            {
                sQuery = sQuery.Where(w => ((w.RefNo + "").Trim()).ToLower() == ((ref_no + "").Trim()).ToLower());
            }


            if ((agroup_code != null && agroup_code.Length > 0))
            {
                sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_Divisions.division_code));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public IEnumerable<PersonnelRequest> GetPRListForAddCandidate(string group_code,
            string[] agroup_code,
            string status,
            int pr_status,
            string ref_no,
            int supgroup_id,
            int position_id,
            int rank_id,
            string sName,
            bool isAdmin = false)//,bool isAdmin
        {
            //var sQuery = Query().Where(w => w.active_status == "Y" && (w.TM_PR_Status.Id == 3 || w.TM_PR_Status.Id == 2));
            var sQuery = Query().Where(w => w.active_status == "Y");
            if (!isAdmin)
            {
                if ((agroup_code != null && agroup_code.Length > 0))
                {
                    sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_Divisions.division_code));
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
            }
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            if (!string.IsNullOrEmpty(ref_no))
            {
                sQuery = sQuery.Where(w => ((w.RefNo + "").Trim()).ToLower() == ((ref_no + "").Trim()).ToLower());
            }
            if (pr_status != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PR_Status.Id == pr_status);
            }
            if (supgroup_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_SubGroup.Id == supgroup_id);
            }
            if (position_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_Position.Id == position_id);
            }
            if (rank_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_Pool_Rank.Id == rank_id);
            }
            //if (!string.IsNullOrEmpty(sName))
            //{
            //    sQuery = sQuery.Where(w => ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
            //    + (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
            //    + ((w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? (
            //    (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
            //    + (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
            //    + ((w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            //}

            if (!string.IsNullOrEmpty(sName))
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.Any(a => ((a.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (a.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((a.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (a.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? (
                (a.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (a.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((a.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (a.TM_Candidates.last_name_en + "").Trim().ToLower())))));
            }

            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public IEnumerable<PersonnelRequest> GetPRListForPRAdmin(string group_code,
        string[] agroup_code,
        string status,
        int pr_status,
        string ref_no,
        int supgroup_id,
        int position_id,
        string sName,
        bool isAdmin = false)//,bool isAdmin
        {
            List<int> nStatus = new List<int>();
            //Save_Draft = 1,
            //Awaiting_Approval = 2,
            //Recruiting = 3,
            //Completed = 4,
            //Reject = 5,
            //Cancel = 6,

            nStatus.Add((int)StatusPR.Cancel);
            nStatus.Add((int)StatusPR.Awaiting_Approval);
            nStatus.Add((int)StatusPR.Recruiting);
            nStatus.Add((int)StatusPR.Reject);
            nStatus.Add((int)StatusPR.Completed);

            //var sQuery = Query().Where(w => w.active_status == "Y" && (w.TM_PR_Status.Id == 3 || w.TM_PR_Status.Id == 2));
            var sQuery = Query().Where(w => w.active_status == "Y" && nStatus.Contains(w.TM_PR_Status.Id));
            if (!isAdmin)
            {
                if ((agroup_code != null && agroup_code.Length > 0))
                {
                    sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_Divisions.division_code));
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
            }
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            if (!string.IsNullOrEmpty(ref_no))
            {
                sQuery = sQuery.Where(w => ((w.RefNo + "").Trim()).ToLower() == ((ref_no + "").Trim()).ToLower());
            }
            if (pr_status != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PR_Status.Id == pr_status);
            }
            if (supgroup_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_SubGroup.Id == supgroup_id);
            }
            if (position_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_Position.Id == position_id);
            }
            //if (!string.IsNullOrEmpty(sName))
            //{
            //    sQuery = sQuery.Where(w => ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
            //    + (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
            //    + ((w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? (
            //    (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
            //    + (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
            //    + ((w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            //}

            if (!string.IsNullOrEmpty(sName))
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.Any(a => ((a.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (a.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((a.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (a.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? (
                (a.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (a.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((a.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (a.TM_Candidates.last_name_en + "").Trim().ToLower())))));
            }

            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }
        public IEnumerable<PersonnelRequest> GetPRListForStaffMovement(string group_code,
string[] agroup_code,
bool isAdmin = false)//,bool isAdmin
        {
            List<int> nStatus = new List<int>();
            //Save_Draft = 1,
            //Awaiting_Approval = 2,
            //Recruiting = 3,
            //Completed = 4,
            //Reject = 5,
            //Cancel = 6,

            nStatus.Add((int)StatusPR.Awaiting_Approval);
            nStatus.Add((int)StatusPR.Recruiting);
            nStatus.Add((int)StatusPR.Reject);
            nStatus.Add((int)StatusPR.Completed);

            //var sQuery = Query().Where(w => w.active_status == "Y" && (w.TM_PR_Status.Id == 3 || w.TM_PR_Status.Id == 2));
            var sQuery = Query().Where(w => w.active_status == "Y" && nStatus.Contains(w.TM_PR_Status.Id));
            if (!isAdmin)
            {
                if ((agroup_code != null && agroup_code.Length > 0))
                {
                    sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_Divisions.division_code));
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
            }
            return sQuery.ToList();
        }
        public IEnumerable<PersonnelRequest> GetAllPR()//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            return sQuery.ToList();
        }
        #region Save Edit Delect 
        public int CreateNew(ref PersonnelRequest s)
        {
            //s.Id = SelectMax();


            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int UpdateApprove(PersonnelRequest s)
        {
            var sResult = 0;

            sResult = SaveChanges();
            //var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            //if (_getData != null)
            //{

            //    sResult = SaveChanges();
            //}
            return sResult;
        }

        public int AdminUpdate(PersonnelRequest s)
        {
            var sResult = 0;

            var _getDataAD = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataAD != null)
            {
                _getDataAD.update_user = s.update_user;
                _getDataAD.update_date = s.update_date;
                _getDataAD.job_descriptions = s.job_descriptions;
                _getDataAD.qualification_experience = s.qualification_experience;
                _getDataAD.remark = s.remark;
                _getDataAD.active_status = s.active_status;
                //    _getDataAD.TM_SubGroup_Id = s.TM_SubGroup_Id;
                _getDataAD.TM_Pool_Rank = s.TM_Pool_Rank;
                _getDataAD.TM_Position = s.TM_Position;
                _getDataAD.TM_PR_Status = s.TM_PR_Status;
                _getDataAD.no_of_headcount = s.no_of_headcount;

                sResult = SaveChanges();
            }
            return sResult;

        }
        #endregion
    }
}
