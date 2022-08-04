using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.HCMServiceClass;

namespace HumanCapitalManagement.Service.MainService
{
    public class TM_Candidate_TIFService : ServiceBase<TM_Candidate_TIF>
    {
        public TM_Candidate_TIFService(IRepository<TM_Candidate_TIF> repo) : base(repo)
        {
            //
        }
        public TM_Candidate_TIF Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Candidate_TIF FindByMappingID(int id)
        {
            var Return = Query().FirstOrDefault(s => s.TM_PR_Candidate_Mapping.Id == id && s.active_status == "Y");
            return Return != null ? Return : null;

            //   return Query().SingleOrDefault(s => s.TM_PR_Candidate_Mapping.Id == id && s.active_status == "Y");
        }
      
        public TM_Candidate_TIF FindByApproval(int id)
        {
            return Query().SingleOrDefault(s => s.Id == s.TM_Candidate_TIF_Approv.FirstOrDefault(f => f.Id == id).TM_Candidate_TIF.Id);
        }
        public IEnumerable<TM_Candidate_TIF> FindByMappingArrayID(int[] id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            if (id != null && id.Length > 0)
            {
                sQuery = sQuery.Where(w => id.Contains(w.TM_PR_Candidate_Mapping.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }
            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidate_TIF> FindByMappingArrayID2(int[] id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != "Intern");
            if (id != null && id.Length > 0)
            {
                sQuery = sQuery.Where(w => id.Contains(w.TM_PR_Candidate_Mapping.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }
            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidate_TIF> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidate_TIF> GetSubGroupForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<TM_Candidate_TIF> GetForAcknowledge(string group_code, string[] agroup_code, string status, string sName, bool isAdmin = false, bool isHRAdmin = false)
        {
            List<int> nStatus = new List<int>();
            nStatus.Add((int)StatusPR.Awaiting_Approval);
            nStatus.Add((int)StatusPR.Recruiting);
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_Candidate_TIF_Approv.Where(w2 => w2.active_status == "Y" && string.IsNullOrEmpty(w2.Approve_status)).Count() <= 0
             // && w.TM_Candidate_TIF_Approv.Where(w2 => w2.Approve_status + "" == "Y").Count() >= 2
             && nStatus.Contains(w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_PR_Status.Id)
            && w.submit_status == "Y"
            && w.hr_acknowledge != "Y"
            // && w.TM_PR_Candidate_Mapping.TM_Candidate_Status_Cycle.OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview
            );

            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(sName))
            {
                sQuery = sQuery.Where(w => ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? (
                (w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            }
            if (!isAdmin && !isHRAdmin)
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.Id != (int)UnitGroup.HR);
            }

            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidate_TIF> GetReportList(string group_code, string[] agroup_code, string status,
             string ref_no,
            int supgroup_id,
            int position_id,
            string sName,
            DateTime? date_strat, DateTime? date_to, bool isAdmin = false, bool isHRAdmin = false)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm == "N" && w.TM_PR_Candidate_Mapping.PersonnelRequest.active_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y");
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            }

            if (!string.IsNullOrEmpty(ref_no))
            {
                sQuery = sQuery.Where(w => ((w.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "").Trim()).ToLower() == ((ref_no + "").Trim()).ToLower());
            }

            if (supgroup_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_SubGroup.Id == supgroup_id);
            }
            if (position_id != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.Id == position_id);
            }
            if (date_strat != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.create_date.Value) >= DbFunctions.TruncateTime(date_strat.Value));
            }
            if (date_to != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.create_date.Value) <= DbFunctions.TruncateTime(date_to.Value));
            }
            if (!string.IsNullOrEmpty(sName))
            {
                sQuery = sQuery.Where(w => ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? (
                (w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            }
            if (!isAdmin && !isHRAdmin)
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.Id != (int)UnitGroup.HR);
            }


            return sQuery.ToList();
        }
        public IEnumerable<TM_Candidate_TIF> GetTifByMappingID(int?[] aMID)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm == "M" && w.TM_PR_Candidate_Mapping.PersonnelRequest.active_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y");

            if (aMID.Length > 0)
            {
                sQuery = sQuery.Where(w => aMID.Contains(w.TM_PR_Candidate_Mapping.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        #region Save Edit Delect 
        public int CreateNew(TM_Candidate_TIF s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Candidate_TIF s)
        {
            var sResult = 0;
            var _getDataTIF = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataTIF != null)
            {
                _getDataTIF.comments = s.comments;
                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                _getDataTIF.submit_status = s.submit_status;
                _getDataTIF.TM_TIF_Status = s.TM_TIF_Status;
                _getDataTIF.confidentiality_agreement = s.confidentiality_agreement;
                _getDataTIF.Recommended_Rank = s.Recommended_Rank;
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateDate(TM_Candidate_TIF s)
        {
            var sResult = 0;
            var _getDataTIF = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataTIF != null)
            {
                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int CreateNewOrUpdate(ref TM_Candidate_TIF s)
        {
            var sResult = 0;
            var objSave = s;
            var _getDataTIF = Query().Where(w => w.TM_PR_Candidate_Mapping.Id == objSave.TM_PR_Candidate_Mapping.Id).FirstOrDefault();
            if (_getDataTIF == null)
            {
                Add(s);
            }
            else
            {
                _getDataTIF.comments = s.comments;
                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                _getDataTIF.submit_status = s.submit_status;
                _getDataTIF.TM_TIF_Status = s.TM_TIF_Status;
                _getDataTIF.confidentiality_agreement = s.confidentiality_agreement;
                _getDataTIF.Recommended_Rank = s.Recommended_Rank;
                s = _getDataTIF;
            }
            sResult = SaveChanges();
            return sResult;
        }
        public int ApproveTIFForm(TM_Candidate_TIF s)
        {
            var sResult = 0;
            var _getDataTIF = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataTIF != null)
            {
                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                sResult = SaveChanges();
            }

            return sResult;
        }
        #endregion
    }
}
