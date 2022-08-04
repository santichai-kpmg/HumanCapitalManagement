using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using HumanCapitalManagement.Service.CommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HumanCapitalManagement.Service.CommonClass.PESServiceClass;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PTR_Evaluation_ApproveService : ServiceBase<PTR_Evaluation_Approve>
    {
        public PTR_Evaluation_ApproveService(IRepository<PTR_Evaluation_Approve> repo) : base(repo)
        {
            //
        }
        public PTR_Evaluation_Approve Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<PTR_Evaluation_Approve> GetEvaForApprove2(string user_no, int nYear, bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.TM_PTR_Eva_ApproveStep.type_of_step == "G" && w.active_status == "Y");

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PTR_Evaluation.PTR_Evaluation_Year.Id == nYear);
            }
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);

            List<int> nApproveStatus = new List<int>();
            nApproveStatus.Add((int)StepApprovePlan.Self);
            nApproveStatus.Add((int)StepApproveEvaluate.Self);

            var sQuery2 = Query();
            if (nYear != 0)
            {
                sQuery2 = sQuery2.Where(w2 => w2.PTR_Evaluation.PTR_Evaluation_Year.Id == nYear);
            }
            var GetMax = (from sQ in sQuery2.Where(w2 => w2.TM_PTR_Eva_ApproveStep.type_of_step == "G" && w2.Approve_status != "Y"
                          //  && (nYear != 0 ? w2.PTR_Evaluation.PTR_Evaluation_Year.Id == nYear  : true)
                          && w2.active_status == "Y"
                          && nStatus.Contains(w2.PTR_Evaluation.TM_PTR_Eva_Status.Id) && !nApproveStatus.Contains(w2.TM_PTR_Eva_ApproveStep.Id))
                          group new { sQ } by new { sQ.PTR_Evaluation.Id } into grp
                          select new
                          {
                              obj = grp.Where(w => w.sQ.TM_PTR_Eva_ApproveStep.seq == grp.Min(p => p.sQ.TM_PTR_Eva_ApproveStep.seq)
                              && w.sQ.PTR_Evaluation.Id == grp.Key.Id).Select(s => s.sQ.Id).FirstOrDefault(),

                          }
                        ).ToList();
            if (GetMax != null && GetMax.Any())
            {
                List<int> nID = GetMax.Select(s => s.obj).ToList();
                sQuery = sQuery.Where(w => w.active_status == "Y" && w.PTR_Evaluation.active_status == "Y" && w.Approve_status != "Y"
                 && nStatus.Contains(w.PTR_Evaluation.TM_PTR_Eva_Status.Id)
                 && nID.Contains(w.Id));

                if (!isAdmin)
                {
                    sQuery = sQuery.Where(w => user_no == w.Req_Approve_user);
                }
            }
            else
            {
                sQuery = sQuery.Take(0);
            }



            //if (!string.IsNullOrEmpty(group_code))
            //{
            //    sQuery = sQuery.Where(w => w.PTR_Evaluation.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            //}

            return sQuery.ToList();
        }

        public IEnumerable<PTR_Evaluation_Approve> GetEvaForApprove(string user_no, int nYear, bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.TM_PTR_Eva_ApproveStep.type_of_step == "G" && w.active_status == "Y");

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PTR_Evaluation.PTR_Evaluation_Year.Id == nYear);
            }
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);

            List<int> nApproveStatus = new List<int>();
            nApproveStatus.Add((int)StepApprovePlan.Self);
            nApproveStatus.Add((int)StepApproveEvaluate.Self);

            var sQuery2 = Query();
            if (nYear != 0)
            {
                sQuery2 = sQuery2.Where(w2 => w2.PTR_Evaluation.PTR_Evaluation_Year.Id == nYear);
            }


            sQuery = sQuery.Where(w => w.active_status == "Y" && w.PTR_Evaluation.active_status == "Y" /*&& w.Approve_status != "Y"*/
             && nStatus.Contains(w.PTR_Evaluation.TM_PTR_Eva_Status.Id) && !nApproveStatus.Contains(w.TM_PTR_Eva_ApproveStep.Id));

            if (!isAdmin)
            {
                sQuery = sQuery.Where(w => user_no == w.Req_Approve_user);
            }

            return sQuery.ToList();
        }

        public IEnumerable<PTR_Evaluation_Approve> GetPlanForApprove(string user_no, int nYear, bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.TM_PTR_Eva_ApproveStep.type_of_step == "P" && w.active_status == "Y");

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PTR_Evaluation.PTR_Evaluation_Year.Id == nYear);
            }
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);

            List<int> nApproveStatus = new List<int>();
            nApproveStatus.Add((int)StepApprovePlan.Self);
            nApproveStatus.Add((int)StepApproveEvaluate.Self);

            var sQuery2 = Query();
            if (nYear != 0)
            {
                sQuery2 = sQuery2.Where(w2 => w2.PTR_Evaluation.PTR_Evaluation_Year.Id == nYear);
            }
            var GetMax = (from sQ in sQuery2.Where(w2 => w2.TM_PTR_Eva_ApproveStep.type_of_step == "P" && w2.Approve_status != "Y"
                          //  && (nYear != 0 ? w2.PTR_Evaluation.PTR_Evaluation_Year.Id == nYear  : true)
                          && w2.active_status == "Y"
                          && nStatus.Contains(w2.PTR_Evaluation.TM_PTR_Eva_Status.Id) && !nApproveStatus.Contains(w2.TM_PTR_Eva_ApproveStep.Id))
                          group new { sQ } by new { sQ.PTR_Evaluation.Id } into grp
                          select new
                          {
                              obj = grp.Where(w => w.sQ.TM_PTR_Eva_ApproveStep.seq == grp.Min(p => p.sQ.TM_PTR_Eva_ApproveStep.seq)
                              && w.sQ.PTR_Evaluation.Id == grp.Key.Id).Select(s => s.sQ.Id).FirstOrDefault(),

                          }
                        ).ToList();
            if (GetMax != null && GetMax.Any())
            {
                List<int> nID = GetMax.Select(s => s.obj).ToList();
                sQuery = sQuery.Where(w => w.active_status == "Y" && w.PTR_Evaluation.active_status == "Y" && w.Approve_status != "Y"
                 && nStatus.Contains(w.PTR_Evaluation.TM_PTR_Eva_Status.Id)
                 && nID.Contains(w.Id));

                if (!isAdmin)
                {
                    sQuery = sQuery.Where(w => user_no == w.Req_Approve_user);
                }
            }
            else
            {
                sQuery = sQuery.Take(0);
            }



            //if (!string.IsNullOrEmpty(group_code))
            //{
            //    sQuery = sQuery.Where(w => w.PTR_Evaluation.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            //}

            return sQuery.ToList();
        }
        public IEnumerable<PTR_Evaluation_Approve> GetEvaForMail(int nEva_id)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.TM_PTR_Eva_ApproveStep.type_of_step == "G" && w.PTR_Evaluation.Id == nEva_id && w.active_status == "Y");
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);

            List<int> nApproveStatus = new List<int>();
            nApproveStatus.Add((int)StepApprovePlan.Self);
            nApproveStatus.Add((int)StepApproveEvaluate.Self);

            var GetMax = (from sQ in Query().Where(w2 => w2.TM_PTR_Eva_ApproveStep.type_of_step == "G"
                          && w2.Approve_status != "Y"
                          && w2.active_status == "Y"
                          && nStatus.Contains(w2.PTR_Evaluation.TM_PTR_Eva_Status.Id)
                          && !nApproveStatus.Contains(w2.TM_PTR_Eva_ApproveStep.Id)
                          && w2.PTR_Evaluation.Id == nEva_id
                          )
                          group new { sQ } by new { sQ.PTR_Evaluation.Id } into grp
                          select new
                          {
                              obj = grp.Where(w => w.sQ.TM_PTR_Eva_ApproveStep.seq == grp.Min(p => p.sQ.TM_PTR_Eva_ApproveStep.seq)
                              && w.sQ.PTR_Evaluation.Id == grp.Key.Id).Select(s => s.sQ.Id).FirstOrDefault(),

                          }
                        ).ToList();
            if (GetMax != null && GetMax.Any())
            {
                List<int> nID = GetMax.Select(s => s.obj).ToList();
                sQuery = sQuery.Where(w => w.active_status == "Y" && w.PTR_Evaluation.active_status == "Y" && w.Approve_status != "Y"
                 && nStatus.Contains(w.PTR_Evaluation.TM_PTR_Eva_Status.Id)
                 && nID.Contains(w.Id));

            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public IEnumerable<PTR_Evaluation_Approve> GetPlanForMail(int nEva_id)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.TM_PTR_Eva_ApproveStep.type_of_step == "P" && w.PTR_Evaluation.Id == nEva_id);
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);

            List<int> nApproveStatus = new List<int>();
            nApproveStatus.Add((int)StepApprovePlan.Self);
            nApproveStatus.Add((int)StepApproveEvaluate.Self);

            var GetMax = (from sQ in Query().Where(w2 => w2.TM_PTR_Eva_ApproveStep.type_of_step == "P"
                          && w2.Approve_status != "Y"
                          && w2.active_status == "Y"
                          && nStatus.Contains(w2.PTR_Evaluation.TM_PTR_Eva_Status.Id)
                          && !nApproveStatus.Contains(w2.TM_PTR_Eva_ApproveStep.Id)
                          && w2.PTR_Evaluation.Id == nEva_id
                          )
                          group new { sQ } by new { sQ.PTR_Evaluation.Id } into grp
                          select new
                          {
                              obj = grp.Where(w => w.sQ.TM_PTR_Eva_ApproveStep.seq == grp.Min(p => p.sQ.TM_PTR_Eva_ApproveStep.seq)
                              && w.sQ.PTR_Evaluation.Id == grp.Key.Id).Select(s => s.sQ.Id).FirstOrDefault(),

                          }
                        ).ToList();
            if (GetMax != null && GetMax.Any())
            {
                List<int> nID = GetMax.Select(s => s.obj).ToList();
                sQuery = sQuery.Where(w => w.active_status == "Y" && w.PTR_Evaluation.active_status == "Y" && w.Approve_status != "Y"
                 && nStatus.Contains(w.PTR_Evaluation.TM_PTR_Eva_Status.Id)
                 && nID.Contains(w.Id));

            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public IEnumerable<PTR_Evaluation_Approve> GetApproveByEva(int Eva_id)//,bool isAdmin
        {
            var sQuery = Query();
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);
            sQuery = sQuery.Where(w => w.active_status == "Y" && w.PTR_Evaluation.active_status == "Y" && w.PTR_Evaluation.Id == Eva_id && w.Approve_status == "Y"
               && nStatus.Contains(w.PTR_Evaluation.TM_PTR_Eva_Status.Id) && w.TM_PTR_Eva_ApproveStep.type_of_step == "G");

            return sQuery.ToList();
        }

        public IEnumerable<PTR_Evaluation_Approve> GetApproveByEvaReturn(int Eva_id)//,bool isAdmin
        {
            var sQuery = Query();
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Revised_Evaluate);
            nStatus.Add((int)Eva_Status.Waiting_for_Revised_Plan);
            sQuery = sQuery.Where(w => w.active_status == "Y" && w.PTR_Evaluation.active_status == "Y" && w.PTR_Evaluation.Id == Eva_id && w.Approve_status == "Y"
               && nStatus.Contains(w.PTR_Evaluation.TM_PTR_Eva_Status.Id) && w.TM_PTR_Eva_ApproveStep.type_of_step == "G");

            return sQuery.ToList();
        }

        public IEnumerable<PTR_Evaluation_Approve> GetApproveByEvaRevise(int Eva_id)//,bool isAdmin
        {
            var sQuery = Query();
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Revised_Evaluate);
            nStatus.Add((int)Eva_Status.Waiting_for_Revised_Plan);
            sQuery = sQuery.Where(w => w.active_status == "Y" && w.PTR_Evaluation.active_status == "Y" && w.PTR_Evaluation.Id == Eva_id && w.Approve_status == "Y"
               && nStatus.Contains(w.PTR_Evaluation.TM_PTR_Eva_Status.Id) && w.TM_PTR_Eva_ApproveStep.type_of_step == "G");

            return sQuery.ToList();
        }


        public IEnumerable<PTR_Evaluation_Approve> GetApproveByPlan(int Eva_id)//,bool isAdmin
        {
            var sQuery = Query();
            List<int> nStatus = new List<int>();
            nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);
            sQuery = sQuery.Where(w => w.active_status == "Y" && w.PTR_Evaluation.active_status == "Y" && w.PTR_Evaluation.Id == Eva_id
               && nStatus.Contains(w.PTR_Evaluation.TM_PTR_Eva_Status.Id) && w.TM_PTR_Eva_ApproveStep.type_of_step == "P");



            return sQuery.ToList();
        }

        public bool CanCompleted(int PES_nID, int nID)
        {
            bool sCan = false;
            var sCheck = Query().Any(w => w.PTR_Evaluation.Id == PES_nID && w.Id != nID && w.Approve_status != "Y" && w.active_status == "Y");
            if (!sCheck)
            {
                sCan = true;
            }

            return sCan;
        }
        public int CreateNewByList(List<PTR_Evaluation_Approve> s)
        {
            var sResult = 0;
            if (s != null && s.Any())
            {
                var _updateUser = s.Select(f => f.update_user).FirstOrDefault();
                List<PTR_Evaluation_Approve> lstEdit = new List<PTR_Evaluation_Approve>();
                foreach (var item in s.Where(w => w.PTR_Evaluation != null && w.TM_PTR_Eva_ApproveStep != null))
                {
                    var _checkDuplicate = Query().Where(w => w.Req_Approve_user == item.Req_Approve_user
                    && w.PTR_Evaluation.Id == item.PTR_Evaluation.Id
                    && w.TM_PTR_Eva_ApproveStep.Id == item.TM_PTR_Eva_ApproveStep.Id
                    ).FirstOrDefault();
                    if (_checkDuplicate == null)
                    {
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
        public int Update(PTR_Evaluation_Approve s)
        {
            var sResult = 0;
            var _getDataSave = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataSave != null)
            {
                _getDataSave.PTR_Evaluation = s.PTR_Evaluation;
                _getDataSave.update_user = s.update_user;
                _getDataSave.update_date = s.update_date;
                _getDataSave.Annual_Rating = s.Annual_Rating;
                _getDataSave.Approve_status = s.Approve_status;
                _getDataSave.Approve_date = s.Approve_date;
                _getDataSave.Approve_user = s.Approve_user;
                _getDataSave.responses = s.responses;
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int RollBackStatus(int nTIFID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;

            if (nTIFID != 0)
            {
                var _getData = Query().Where(w => w.PTR_Evaluation.Id == nTIFID && w.active_status == "Y").ToList();
                if (_getData.Any())
                {
                    _getData.ForEach(ed =>
                    {
                        ed.Approve_date = null;
                        ed.Approve_status = "";
                        ed.Approve_user = "";

                    });
                }
            }
            sResult = SaveChanges();
            return sResult;
        }


    }
}
