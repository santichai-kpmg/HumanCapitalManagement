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
    public class TM_Candidate_Status_CycleService : ServiceBase<TM_Candidate_Status_Cycle>
    {
        public TM_Candidate_Status_CycleService(IRepository<TM_Candidate_Status_Cycle> repo) : base(repo)
        {
            //
        }
        public TM_Candidate_Status_Cycle Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Candidate_Status_Cycle FindForRollback(int map_id, int Status_id)
        {
            return Query().FirstOrDefault(s => s.TM_Candidate_Status.Id == Status_id && s.TM_PR_Candidate_Mapping.Id == map_id && s.active_status == "Y");
        }
        public DateTime? FindInterviewDate(int id)
        {
            var _getData = Query().Where(w => w.TM_PR_Candidate_Mapping.Id == id && w.active_status == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.Interview).FirstOrDefault();
            if (_getData != null)
            {
                return _getData.action_date;
            }
            else
            {
                return null;
            }

        }
        public IEnumerable<TM_Candidate_Status_Cycle> GetCandidateForInterview(string group_code, string[] agroup_code, string status, bool isAdmin = false)
        {
            //&& w.TM_Candidate_Status.Id == (int)StatusCandidate.Interview
            List<int> nStatus = new List<int>();
            nStatus.Add((int)StatusPR.Awaiting_Approval);
            nStatus.Add((int)StatusPR.Recruiting);
            List<int> nCanStatus = new List<int>();
            nCanStatus.Add((int)StatusCandidate.AddNew);
            nCanStatus.Add((int)StatusCandidate.Interview);
            nCanStatus.Add((int)StatusCandidate.Shortlist_BU);
            nCanStatus.Add((int)StatusCandidate.BU_Response);
            nCanStatus.Add((int)StatusCandidate.Passed);
            nCanStatus.Add((int)StatusCandidate.Passed_NotOffer);
            nCanStatus.Add((int)StatusCandidate.Comparing);
            nCanStatus.Add((int)StatusCandidate.Pending);
            nCanStatus.Add((int)StatusCandidate.Send_Hiring);
            var GetMax = (from sQ in Query().Where(w => w.active_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y" && nStatus.Contains(w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_PR_Status.Id) && nCanStatus.Contains(w.TM_Candidate_Status.Id))
                          group new { sQ } by new { sQ.TM_PR_Candidate_Mapping.Id } into grp

                          select new
                          {
                              nId = grp.Key.Id,
                              value = (grp.Max(p => p.sQ.seq) == grp.Where(w => w.sQ.TM_Candidate_Status.Id == (int)StatusCandidate.Interview).Max(p => p.sQ.seq)) ? grp.Max(p => p.sQ.seq) : 0,
                          }
                         ).ToList();


            if (GetMax.Any())
            {
                int[] nID = GetMax.Where(w => w.value != 0).Select(s => s.nId).ToArray();
                var sQuery = Query().Where(w => w.active_status == "Y"
                && nID.Contains(w.TM_PR_Candidate_Mapping.Id) && w.TM_PR_Candidate_Mapping.active_status == "Y"
                && nStatus.Contains(w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_PR_Status.Id)
                && w.TM_Candidate_Status.Id == (int)StatusCandidate.Interview);


                if (!isAdmin)

                {
                    if ((agroup_code != null && agroup_code.Length > 0))
                    {
                        sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code));
                    }
                    else
                    {
                        sQuery = sQuery.Take(0);
                    }
                }
                if (!string.IsNullOrEmpty(group_code))
                {
                    sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
                }
                return sQuery.ToList();
            }
            else
            {
                return Query().Take(0).ToList();
            }

        }
        //public IEnumerable<TM_Candidate_Status_Cycle> GetCandidateForInterview(string group_code, string[] agroup_code, string status, bool isAdmin = false)
        //{
        //    //&& w.TM_Candidate_Status.Id == (int)StatusCandidate.Interview
        //    List<int> nStatus = new List<int>();
        //    nStatus.Add((int)StatusPR.Awaiting_Approval);
        //    nStatus.Add((int)StatusPR.Recruiting);
        //    var GetMax = (from sQ in Query().Where(w => w.active_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y" && nStatus.Contains(w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_PR_Status.Id))
        //                  group new { sQ } by new { sQ.TM_PR_Candidate_Mapping.Id } into grp

        //                  select new
        //                  {
        //                      nId = grp.Key.Id,
        //                      value = (grp.Max(p => p.sQ.seq) == grp.Where(w => w.sQ.TM_Candidate_Status.Id == (int)StatusCandidate.Interview).Max(p => p.sQ.seq)) ? grp.Max(p => p.sQ.seq) : 0,
        //                  }
        //                 ).ToList();


        //    if (GetMax.Any())
        //    {
        //        int[] nID = GetMax.Where(w => w.value != 0).Select(s => s.nId).ToArray();
        //        var sQuery = Query().Where(w => w.active_status == "Y"
        //        && nID.Contains(w.TM_PR_Candidate_Mapping.Id) && w.TM_PR_Candidate_Mapping.active_status == "Y"
        //        && nStatus.Contains(w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_PR_Status.Id)
        //        && w.TM_Candidate_Status.Id == (int)StatusCandidate.Interview);


        //        if (!isAdmin)

        //        {
        //            if ((agroup_code != null && agroup_code.Length > 0))
        //            {
        //                sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code));
        //            }
        //            else
        //            {
        //                sQuery = sQuery.Take(0);
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(group_code))
        //        {
        //            sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
        //        }
        //        return sQuery.ToList();
        //    }
        //    else
        //    {
        //        return Query().Take(0).ToList();
        //    }

        //}
        public int CreateNew(TM_Candidate_Status_Cycle s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Candidate_Status_Cycle s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
    }
}
