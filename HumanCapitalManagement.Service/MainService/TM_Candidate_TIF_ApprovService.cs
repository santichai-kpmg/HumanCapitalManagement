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
    public class TM_Candidate_TIF_ApprovService : ServiceBase<TM_Candidate_TIF_Approv>
    {
        public TM_Candidate_TIF_ApprovService(IRepository<TM_Candidate_TIF_Approv> repo) : base(repo)
        {
            //
        }
        public TM_Candidate_TIF_Approv Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Candidate_TIF_Approv> GetTIFFormForApprove(string group_code, string[] agroup_code, string user_no, string sName, bool isAdmin = false)//,bool isAdmin
        {

            List<int> nStatus = new List<int>();
            nStatus.Add((int)StatusPR.Awaiting_Approval);
            nStatus.Add((int)StatusPR.Recruiting);

            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_Candidate_TIF.active_status == "Y" && w.TM_Candidate_TIF.submit_status == "Y"
                 && w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.active_status == "Y"
                 //&& nStatus.Contains(w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_PR_Status.Id)
            /*&& string.IsNullOrEmpty(w.Approve_status)*/
            );

            if (!isAdmin)
            {
                if ((agroup_code != null && agroup_code.Length > 0))
                {
                    sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code));
                }
                else
                {
                    sQuery = sQuery.Take(0);
                }
                sQuery = sQuery.Where(w => user_no == w.Req_Approve_user);
            }

            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(sName))
            {
                sQuery = sQuery.Where(w => ((w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((sName + "").Trim().ToLower() ?? (
                (w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower()
                + (w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()
                + ((w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            }
            return sQuery.ToList();
        }

        public IEnumerable<TM_Candidate_TIF_Approv> GetTIFApprovebyNameCan(string firstname,string lastname)
        {

            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en.Trim().ToLower()== firstname.Trim().ToLower()
            && w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en.Trim().ToLower() == lastname.Trim().ToLower());
           
            return sQuery.ToList();
        }

        public int UpdateApprove(List<TM_Candidate_TIF_Approv> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.TM_Candidate_TIF.Id == nEmID).ToList();
            ////set old to inactive
            //_getAdv.Where(w => !s.Select(s2 => s2.TM_TIF_Form_Question.Id).ToArray().Contains(w.TM_TIF_Form_Question.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            //{
            //    ed.active_status = "N";
            //    ed.update_user = UserUpdate;
            //    ed.update_date = dNow;
            //});
            ////set old to active

            //foreach (var item in s.Where(w => w.TM_Candidate_TIF != null))
            //{
            //    var Addnew = _getAdv.Where(w => w.TM_TIF_Form_Question.Id == item.TM_TIF_Form_Question.Id).FirstOrDefault();
            //    if (Addnew == null)
            //    {
            //        Add(item);
            //    }
            //    else
            //    {
            //        _getAdv.Where(w => w.TM_TIF_Form_Question.Id == item.TM_TIF_Form_Question.Id).ToList().ForEach(ed =>
            //        {

            //            ed.update_user = UserUpdate;
            //            ed.update_date = dNow;
            //            ed.answer = item.answer + "";
            //            ed.TM_TIF_Rating = item.TM_TIF_Rating;

            //        });
            //    }
            //}

            sResult = SaveChanges();
            return sResult;
        }
        public int CreateNewByList(List<TM_Candidate_TIF_Approv> s)
        {
            var sResult = 0;
            if (s != null && s.Any())
            {
                var _updateUser = s.Select(f => f.update_user).FirstOrDefault();
                List<TM_Candidate_TIF_Approv> lstEdit = new List<TM_Candidate_TIF_Approv>();
                foreach (var item in s.Where(w => w.TM_Candidate_TIF != null))
                {
                    var _checkDuplicate = Query().Where(w => w.Req_Approve_user == item.Req_Approve_user && w.TM_Candidate_TIF.Id == item.TM_Candidate_TIF.Id).FirstOrDefault();
                    if (_checkDuplicate == null)
                    {
                        int? nSeq = Query().Any(w => w.TM_Candidate_TIF.Id == item.TM_Candidate_TIF.Id) ? Query().Where(w => w.TM_Candidate_TIF.Id == item.TM_Candidate_TIF.Id).Max(m => m.seq) + 1 : 1;

                        item.seq = Query().Any(w => w.TM_Candidate_TIF.Id == item.TM_Candidate_TIF.Id) ? nSeq : item.seq;
                        Add(item);
                    }
                    else if (_checkDuplicate.active_status != "Y")
                    {
                        lstEdit.Add(_checkDuplicate);
                    }
                }
                if (lstEdit.Any())
                {
                    lstEdit.ForEach(ed =>
                    {
                        ed.active_status = "Y";
                        ed.Approve_date = null;
                        ed.Approve_status = "";
                        ed.update_user = _updateUser;
                        ed.update_date = DateTime.Now;
                    });
                }
            }
            sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Candidate_TIF_Approv s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int RollBackStatus(int nTIFID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;

            if (nTIFID != 0)
            {
                var _getData = Query().Where(w => w.TM_Candidate_TIF.Id == nTIFID && w.active_status == "Y").ToList();
                if (_getData.Any())
                {
                    _getData.ForEach(ed =>
                    {
                        ed.Approve_date = null;
                        ed.Approve_status = "";
                        ed.Approve_user = "";
                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                    });
                }
            }
            sResult = SaveChanges();
            return sResult;
        }

    }
}
