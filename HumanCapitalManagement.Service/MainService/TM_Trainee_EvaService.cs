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
    public class TM_Trainee_EvaService : ServiceBase<TM_Trainee_Eva>
    {
        public TM_Trainee_EvaService(IRepository<TM_Trainee_Eva> repo) : base(repo)
        {
            //
        }
        public TM_Trainee_Eva Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Trainee_Eva FindForApprove(int id, string user_no, bool isAdmin = false)
        {
            var sQuery = Query().Where(w => w.Id == id);
            if (!isAdmin)
            {
                sQuery = sQuery.Where(w => user_no == w.req_mgr_Approve_user || user_no == w.req_incharge_Approve_user);
            }
            return sQuery.FirstOrDefault();

        }
        public TM_Trainee_Eva FindByMappingID(int id)
        {
            return Query().SingleOrDefault(s => s.TM_PR_Candidate_Mapping.Id == id);
        }
        public IEnumerable<TM_Trainee_Eva> FindByMappingIDList(int id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y");
            if (id > 0)
            {
                sQuery = sQuery.Where(s => s.TM_PR_Candidate_Mapping.Id == id);
            }

            return sQuery.ToList();
        }
        public IEnumerable<TM_Trainee_Eva> FindByCandidateID(int id)
        {
            return Query().Where(s => s.TM_PR_Candidate_Mapping.TM_Candidates.Id == id).ToList();
        }
        public IEnumerable<TM_Trainee_Eva> FindByMappingArrayID(int[] id)
        {
            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y");
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
        public IEnumerable<TM_Trainee_Eva> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Trainee_Eva> GetSubGroupForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<TM_Trainee_Eva> GetEvaForApprove(string group_code,
            string[] agroup_code,
            string[] emp_approve,
            string[] emp_ic_approve,
            string user_no,
            string name,
            bool isAdmin = false)//,bool isAdmin
        {

            List<int> nStatus = new List<int>();

            var sQuery = Query().Where(w => w.active_status == "Y" && w.submit_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y");

            if (!isAdmin)
            {
                //if ((agroup_code != null && agroup_code.Length > 0))
                //{
                //    sQuery = sQuery.Where(w => agroup_code.Contains(w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code));
                //}
                //else
                //{
                //    sQuery = sQuery.Take(0);
                //}

                sQuery = sQuery.Where(w => user_no == w.req_mgr_Approve_user || user_no == w.req_incharge_Approve_user);
            }
            if (!string.IsNullOrEmpty(group_code))
            {
                //sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((name + "").Trim().ToLower() ?? ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            }
            if (emp_approve != null)
            {
                sQuery = sQuery.Where(w => emp_approve.Contains(((w.req_mgr_Approve_user + "").Trim()).ToLower()));
            }
            if (emp_ic_approve != null)
            {
                sQuery = sQuery.Where(w => emp_ic_approve.Contains(((w.req_incharge_Approve_user + "").Trim()).ToLower()));
            }
            return sQuery.ToList();
        }
        public IEnumerable<TM_Trainee_Eva> GetEvaForAcknow(string group_code,
            string[] agroup_code,
            string[] emp_approve,
            string[] emp_ic_approve,
            string user_no,
            string name,
            DateTime? date_start, DateTime? start_to, DateTime? date_end, DateTime? end_to,
            bool isAdmin = false)//,bool isAdmin
        {
            List<int> nStatus = new List<int>();
            int nStatusID = (int)StatusTraineeEvaluation.HR_Approve;
            var sQuery = Query().Where(w => w.active_status == "Y" && w.submit_status == "Y" && w.TM_TraineeEva_Status.Id == nStatusID && w.hr_acknowledge != "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y");
            if (!isAdmin)
            {
                sQuery = sQuery.Take(0);
            }
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            }
            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((name + "").Trim().ToLower() ?? ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            }
            if (emp_approve != null)
            {
                sQuery = sQuery.Where(w => emp_approve.Contains(((w.req_mgr_Approve_user + "").Trim()).ToLower()));
            }
            if (emp_ic_approve != null)
            {
                sQuery = sQuery.Where(w => emp_ic_approve.Contains(((w.req_incharge_Approve_user + "").Trim()).ToLower()));
            }
            if (date_start != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.TM_PR_Candidate_Mapping.trainee_start.Value) >= DbFunctions.TruncateTime(date_start.Value));
            }
            if (start_to != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.TM_PR_Candidate_Mapping.trainee_start.Value) <= DbFunctions.TruncateTime(start_to.Value));
            }
            if (date_end != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.TM_PR_Candidate_Mapping.trainee_end.Value) >= DbFunctions.TruncateTime(date_end.Value));
            }
            if (end_to != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.TM_PR_Candidate_Mapping.trainee_end.Value) <= DbFunctions.TruncateTime(end_to.Value));
            }
            return sQuery.ToList();
        }

        public IEnumerable<TM_Trainee_Eva> GetEvaForReport(string group_code,
            string[] agroup_code,
            string[] emp_approve,
            string[] emp_ic_approve,
            string user_no,
            string name,
            int EvaStatus,
            DateTime? date_start, DateTime? start_to, DateTime? date_end, DateTime? end_to,
            bool isAdmin = false)//,bool isAdmin
        {

            var sQuery = Query().Where(w => w.active_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y");
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
            if (emp_approve != null)
            {
                sQuery = sQuery.Where(w => emp_approve.Contains(((w.req_mgr_Approve_user + "").Trim()).ToLower()));
            }
            if (emp_ic_approve != null)
            {
                sQuery = sQuery.Where(w => emp_ic_approve.Contains(((w.req_incharge_Approve_user + "").Trim()).ToLower()));
            }
            if (!string.IsNullOrEmpty(group_code))
            {
                sQuery = sQuery.Where(w => w.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            }
            if (EvaStatus != 0)
            {
                sQuery = sQuery.Where(w => w.TM_TraineeEva_Status.Id == EvaStatus);
            }
            if (!string.IsNullOrEmpty(name))
            {
                sQuery = sQuery.Where(w => ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower())).Contains((name + "").Trim().ToLower() ?? ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower() + ((w.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "").Trim().ToLower() + " " + (w.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "").Trim().ToLower()))));
            }
            if (date_start != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.TM_PR_Candidate_Mapping.trainee_start.Value) >= DbFunctions.TruncateTime(date_start.Value));
            }
            if (start_to != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.TM_PR_Candidate_Mapping.trainee_start.Value) <= DbFunctions.TruncateTime(start_to.Value));
            }
            if (date_end != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.TM_PR_Candidate_Mapping.trainee_end.Value) >= DbFunctions.TruncateTime(date_end.Value));
            }
            if (end_to != null)
            {
                sQuery = sQuery.Where(w => DbFunctions.TruncateTime(w.TM_PR_Candidate_Mapping.trainee_end.Value) <= DbFunctions.TruncateTime(end_to.Value));
            }

            return sQuery.ToList();
        }
        //public bool CanSave(TM_Trainee_Eva TM_Trainee_Eva)
        //{
        //    bool sCan = false;

        //    if (TM_Rank.Id == 0)
        //    {
        //        var sCheck = Query().FirstOrDefault(w => (w.rank_name_en + "").Trim() == (TM_Rank.rank_name_en + "").Trim());
        //        if (sCheck == null)
        //        {
        //            sCan = true;
        //        }
        //    }
        //    else
        //    {
        //        var sCheck = Query().FirstOrDefault(w => (w.rank_name_en + "").Trim() == (TM_Rank.rank_name_en + "").Trim() && w.Id != TM_Rank.Id);
        //        if (sCheck == null)
        //        {
        //            sCan = true;
        //        }
        //    }
        //    return sCan;
        //}

        #region interneva
        public bool checkFailed(List<int> answerList)
        {
            if (answerList.Contains(15))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public string InternEva(List<int> answerList)
        {
            bool status = true;
            int core2 = 3;
            int core3 = 3;
            string stringStatus = "i";
            status = checkFailed(answerList);
            // slice answes in cores
            List<int> core1_answer = answerList.Skip(0).Take(3).ToList();
            List<int> core2_answer = answerList.Skip(3).Take(3).ToList();
            List<int> core3_answer = answerList.Skip(6).Take(3).ToList();
            List<int> core4_answer = answerList.Skip(9).Take(1).ToList();
            List<int> core5_answer = answerList.Skip(10).Take(4).ToList();
            List<int> core6_answer = answerList.Skip(14).Take(1).ToList();

            //check core1
            if (core1_answer.Contains(14))
            {
                status = false;
            }

            //check core2&3
            core2 = checkCore(core2_answer);
            core3 = checkCore(core3_answer);

            List<int> subCoreAnswerList = new List<int>();
            subCoreAnswerList.Add(core2);
            subCoreAnswerList.Add(core3);

            if (status)
            {
                stringStatus = checkSubCore(subCoreAnswerList);
                // check acknowledge and future core
                if (stringStatus == "i")
                {
                    //merge uncompetency answer list
                    core4_answer.AddRange(core5_answer);
                    core4_answer.AddRange(core6_answer);
                    bool isDownGrade = checkUncompentecy(core4_answer);
                    if (isDownGrade)
                    {
                        return "n";
                    }

                }
                return stringStatus;

            }
            return "f";
        }

        public bool checkUncompentecy(List<int> intList)
        {
            if (intList.Contains(14))
            {
                return true;
            }
            return false;
        }

        public string checkSubCore(List<int> answerList)
        {
            int counted2 = answerList.Where(i => i == 2).ToList().Count;
            int counted3 = answerList.Where(i => i == 3).ToList().Count;
            if (counted2 == 2)
            {
                return "l";
            }
            else if(counted2 == 1 && counted3 == 0)
            {
                return "n";
            }
            if (answerList.Contains(3))
            {
                return "f";
            }
            else return "i";
        }
        public int checkCore(List<int> answerList)
        // 1 = all pass, 2 = 4 in answer, 3 = failed
        {
            int count = 0;
            foreach (int i in answerList)
            {
                if (i >= 11 && i <= 13)
                {
                    count += 1;
                }
            }
            if (count == 3)
            {
                return 1;
            }
            else if (count == 2)
            {
                return 2;
            }
            else return 3;
        }
        #endregion
        #region Save Edit Delect 
        public int CreateNew(ref TM_Trainee_Eva s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Trainee_Eva s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int CreateNewOrUpdate(TM_Trainee_Eva s)
        {
            var sResult = 0;
            var _getDataTIF = Query().Where(w => w.TM_PR_Candidate_Mapping.Id == s.TM_PR_Candidate_Mapping.Id).FirstOrDefault();
            if (_getDataTIF == null)
            {
                Add(s);
            }
            else
            {
                _getDataTIF.trainee_learned = s.trainee_learned;
                _getDataTIF.trainee_done_well = s.trainee_done_well;
                _getDataTIF.trainee_developmental = s.trainee_developmental;
                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                _getDataTIF.submit_status = s.submit_status;
            }
            sResult = SaveChanges();
            return sResult;
        }
        public int UpdateEva(ref TM_Trainee_Eva s)
        {
            var sResult = 0;
            int nID = s.Id;
            var _getDataTIF = Query().Where(w => w.Id == nID).FirstOrDefault();
            if (_getDataTIF == null)
            {

            }
            else
            {
                _getDataTIF.trainee_learned = s.trainee_learned;
                _getDataTIF.trainee_done_well = s.trainee_done_well;
                _getDataTIF.trainee_developmental = s.trainee_developmental;
                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                _getDataTIF.submit_status = s.submit_status;
                _getDataTIF.TM_TraineeEva_Status = s.TM_TraineeEva_Status;
                _getDataTIF.req_mgr_Approve_user = s.req_mgr_Approve_user;
                _getDataTIF.req_incharge_Approve_user = s.req_incharge_Approve_user;

            }
            sResult = SaveChanges();
            return sResult;
        }
        public int UpdateEvaApprove(ref TM_Trainee_Eva s)
        {
            var sResult = 0;
            int nID = s.Id;
            var _getDataTIF = Query().Where(w => w.Id == nID).FirstOrDefault();
            if (_getDataTIF == null)
            {

            }
            else
            {
                _getDataTIF.incharge_comments = s.incharge_comments;
                _getDataTIF.hiring_status = s.hiring_status;
                _getDataTIF.confidentiality_agreement = s.confidentiality_agreement;
                _getDataTIF.TM_TraineeEva_Status = s.TM_TraineeEva_Status;
                _getDataTIF.TM_Trainee_HiringRating_Id = s.TM_Trainee_HiringRating_Id;
                if (_getDataTIF.approve_type == "M")
                {
                    _getDataTIF.mgr_Approve_user = s.mgr_Approve_user;
                    _getDataTIF.mgr_Approve_date = s.mgr_Approve_date;
                    _getDataTIF.mgr_Approve_status = s.mgr_Approve_status;
                }
                else
                {
                    _getDataTIF.incharge_Approve_user = s.incharge_Approve_user;
                    _getDataTIF.incharge_Approve_date = s.incharge_Approve_date;
                    _getDataTIF.incharge_Approve_status = s.incharge_Approve_status;
                }

            }
            sResult = SaveChanges();
            return sResult;
        }

        public int AcKnowledgeEva(ref TM_Trainee_Eva s)
        {
            var sResult = 0;
            int nID = s.Id;
            var _getDataTIF = Query().Where(w => w.Id == nID).FirstOrDefault();
            if (_getDataTIF == null)
            {

            }
            else
            {
                _getDataTIF.hr_acknowledge = s.hr_acknowledge;
                _getDataTIF.acknowledge_date = s.acknowledge_date;
                _getDataTIF.acknowledge_user = s.acknowledge_user;
                _getDataTIF.TM_TraineeEva_Status = s.TM_TraineeEva_Status;
            }
            sResult = SaveChanges();
            return sResult;
        }
        //public int ApproveTIFForm(TM_Trainee_Eva s)
        //{
        //    var sResult = 0;
        //    var _getDataTIF = Query().Where(w => w.TM_PR_Candidate_Mapping.Id == s.TM_PR_Candidate_Mapping.Id).FirstOrDefault();
        //    if (_getDataTIF == null)
        //    {
        //        Add(s);
        //    }
        //    else
        //    {
        //        _getDataTIF.comments = s.comments;
        //        _getDataTIF.update_user = s.update_user;
        //        _getDataTIF.update_date = s.update_date;
        //        _getDataTIF.submit_status = s.submit_status;
        //        _getDataTIF.TM_TIF_Status = s.TM_TIF_Status;
        //    }
        //    sResult = SaveChanges();
        //    return sResult;
        //}

        public int RollBackStatus(int nTIFID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;

            if (nTIFID != 0)
            {
                var _getData = Query().Where(w => w.Id == nTIFID && w.active_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y").FirstOrDefault();
                if (_getData != null)
                {
                    _getData.submit_status = "N";
                }
            }
            sResult = SaveChanges();
            return sResult;
        }
        public int RollBackToBu(TM_Trainee_Eva s)
        {
            var sResult = 0;
            if (s.Id != 0)
            {
                var _getData = Query().Where(w => w.Id == s.Id && w.active_status == "Y" && w.TM_PR_Candidate_Mapping.active_status == "Y").FirstOrDefault();
                if (_getData != null)
                {
                    _getData.incharge_Approve_date = null;
                    _getData.incharge_Approve_status = "";
                    _getData.incharge_Approve_user = "";
                    _getData.mgr_Approve_date = null;
                    _getData.mgr_Approve_status = "";
                    _getData.mgr_Approve_user = "";
                    _getData.TM_TraineeEva_Status = s.TM_TraineeEva_Status;
                }
            }
            sResult = SaveChanges();
            return sResult;
        }
        #endregion
    }
}
