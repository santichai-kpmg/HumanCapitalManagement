using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PES_Nomination_SignaturesService : ServiceBase<PES_Nomination_Signatures>
    {
        public PES_Nomination_SignaturesService(IRepository<PES_Nomination_Signatures> repo) : base(repo)
        {
            //
        }
        public PES_Nomination_Signatures Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public PES_Nomination_Signatures FindForEditSponsoring(int id, int nstep)
        {
            return Query().FirstOrDefault(s => s.PES_Nomination.Id == id && nstep == s.TM_PES_NMN_SignatureStep_Id && s.active_status == "Y");
        }
        public IEnumerable<PES_Nomination_Signatures> GetEvaForApprove(string user_no, int nYear, int approve_step, List<int> nStatus, List<int> nApproveStatus, List<int> nHeadID, List<int> nNomination, bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.active_status == "Y");

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PES_Nomination.PES_Nomination_Year.Id == nYear);
            }
            bool isHead = false;
            var sQuery2 = Query();
            if (nYear != 0)
            {
                sQuery2 = sQuery2.Where(w2 => w2.PES_Nomination.PES_Nomination_Year.Id == nYear);
            }
            var GetMax = (from sQ in sQuery2.Where(w2 => w2.Approve_status != "Y"
                          //  && (nYear != 0 ? w2.PES_Nomination.PES_Nomination_Year.Id == nYear  : true)
                          && w2.active_status == "Y"
                          && nStatus.Contains(w2.PES_Nomination.TM_PES_NMN_Status.Id) && !nApproveStatus.Contains(w2.TM_PES_NMN_SignatureStep.Id))
                          group new { sQ } by new { sQ.PES_Nomination.Id } into grp
                          select new
                          {
                              obj = grp.Where(w => w.sQ.TM_PES_NMN_SignatureStep.seq == grp.Min(p => p.sQ.TM_PES_NMN_SignatureStep.seq)
                              && w.sQ.PES_Nomination.Id == grp.Key.Id).Select(s => s.sQ.Id).FirstOrDefault(),
                          }
                        ).ToList();

            if (GetMax != null && GetMax.Any())
            {

                List<int> nID = GetMax.Select(s => s.obj).ToList();
                List<int> nPEsID = sQuery.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y" && w.Approve_status != "Y"
                && nStatus.Contains(w.PES_Nomination.TM_PES_NMN_Status.Id)
                && nID.Contains(w.Id) && nHeadID.Contains(w.TM_PES_NMN_SignatureStep.Id)).Select(s => s.PES_Nomination.Id).ToList();

                List<int> nCommittee = sQuery2.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y" && w.Approve_status != "Y"
             && nStatus.Contains(w.PES_Nomination.TM_PES_NMN_Status.Id)
             && nID.Contains(w.Id) && nNomination.Contains(w.TM_PES_NMN_SignatureStep.Id)).Select(s => s.PES_Nomination.Id).ToList();

                List<int> nComID = sQuery2.Where(w => w.active_status == "Y"
                && w.PES_Nomination.active_status == "Y"
                && w.Approve_status != "Y"
                && nCommittee.Contains(w.PES_Nomination.Id)
                && nNomination.Contains(w.TM_PES_NMN_SignatureStep.Id)
                ).Select(s => s.Id).ToList();
                nID = nID.Concat(nComID).ToList();


                sQuery = sQuery.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y" && w.Approve_status != "Y"
                 && nStatus.Contains(w.PES_Nomination.TM_PES_NMN_Status.Id)
                 && (nID.Contains(w.Id) || nPEsID.Contains(w.PES_Nomination.Id)));

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
            //    sQuery = sQuery.Where(w => w.PES_Nomination.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            //}

            return sQuery.ToList();
        }
        public IEnumerable<PES_Nomination_Signatures> GetEvaForApproveForEdit(string user_no, int nYear, int approve_step, List<int> nStatus, List<int> nApproveStatus, List<int> nHeadID, List<int> nNomination, bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.active_status == "Y");

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PES_Nomination.PES_Nomination_Year.Id == nYear);
            }
            bool isHead = false;
            var sQuery2 = Query();
            if (nYear != 0)
            {
                sQuery2 = sQuery2.Where(w2 => w2.PES_Nomination.PES_Nomination_Year.Id == nYear);
            }
            if (approve_step != 0)
            {
                sQuery = sQuery.Where(w => w.TM_PES_NMN_SignatureStep.Id == approve_step);
            }

            sQuery = sQuery.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y"/* && w.Approve_status != "Y"*/
               && nStatus.Contains(w.PES_Nomination.TM_PES_NMN_Status.Id)
               && !nApproveStatus.Contains(w.TM_PES_NMN_SignatureStep.Id)
                  //&& nNomination.Contains(w.TM_PES_NMN_SignatureStep.Id)
                  )
               ;

            if (!isAdmin)
            {
                sQuery = sQuery.Where(w => user_no == w.Req_Approve_user);
            }

            //if (!string.IsNullOrEmpty(group_code))
            //{
            //    sQuery = sQuery.Where(w => w.PES_Nomination.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            //}

            return sQuery.ToList();
        }
        public IEnumerable<PES_Nomination_Signatures> GetPlanForApprove(string user_no, int nYear, List<int> nStatus, List<int> nApproveStatus, bool isAdmin = false)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.active_status == "Y");

            if (nYear != 0)
            {
                sQuery = sQuery.Where(w => w.PES_Nomination.PES_Nomination_Year.Id == nYear);
            }
            //   List<int> nStatus = new List<int>();
            //    nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            //   nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);

            //  List<int> nApproveStatus = new List<int>();
            //  nApproveStatus.Add((int)StepApprovePlan.Self);
            //  nApproveStatus.Add((int)StepApproveEvaluate.Self);

            var sQuery2 = Query();
            if (nYear != 0)
            {
                sQuery2 = sQuery2.Where(w2 => w2.PES_Nomination.PES_Nomination_Year.Id == nYear);
            }
            var GetMax = (from sQ in sQuery2.Where(w2 => w2.Approve_status != "Y"
                          //  && (nYear != 0 ? w2.PES_Nomination.PES_Nomination_Year.Id == nYear  : true)
                          && w2.active_status == "Y"
                          && nStatus.Contains(w2.PES_Nomination.TM_PES_NMN_Status.Id) && !nApproveStatus.Contains(w2.TM_PES_NMN_SignatureStep.Id))
                          group new { sQ } by new { sQ.PES_Nomination.Id } into grp
                          select new
                          {
                              obj = grp.Where(w => w.sQ.TM_PES_NMN_SignatureStep.seq == grp.Min(p => p.sQ.TM_PES_NMN_SignatureStep.seq)
                              && w.sQ.PES_Nomination.Id == grp.Key.Id).Select(s => s.sQ.Id).FirstOrDefault(),

                          }
                        ).ToList();
            if (GetMax != null && GetMax.Any())
            {
                List<int> nID = GetMax.Select(s => s.obj).ToList();
                sQuery = sQuery.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y" && w.Approve_status != "Y"
                 && nStatus.Contains(w.PES_Nomination.TM_PES_NMN_Status.Id)
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
            //    sQuery = sQuery.Where(w => w.PES_Nomination.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_code == group_code);
            //}

            return sQuery.ToList();
        }
        public IEnumerable<PES_Nomination_Signatures> GetEvaForMail(int nEva_id, List<int> nStatus, List<int> nApproveStatus)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.PES_Nomination.Id == nEva_id);
            //  List<int> nStatus = new List<int>();
            //  nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            //   nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);

            //  List<int> nApproveStatus = new List<int>();
            //  nApproveStatus.Add((int)StepApprovePlan.Self);
            // nApproveStatus.Add((int)StepApproveEvaluate.Self);

            var GetMax = (from sQ in Query().Where(w2 => w2.Approve_status != "Y"
                          && w2.active_status == "Y"
                          && nStatus.Contains(w2.PES_Nomination.TM_PES_NMN_Status.Id)
                          && !nApproveStatus.Contains(w2.TM_PES_NMN_SignatureStep.Id)
                          && w2.PES_Nomination.Id == nEva_id
                          )
                          group new { sQ } by new { sQ.PES_Nomination.Id } into grp
                          select new
                          {
                              obj = grp.Where(w => w.sQ.TM_PES_NMN_SignatureStep.seq == grp.Min(p => p.sQ.TM_PES_NMN_SignatureStep.seq)
                              && w.sQ.PES_Nomination.Id == grp.Key.Id).Select(s => s.sQ.Id).FirstOrDefault(),

                          }
                        ).ToList();
            if (GetMax != null && GetMax.Any())
            {
                List<int> nID = GetMax.Select(s => s.obj).ToList();
                sQuery = sQuery.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y" && w.Approve_status != "Y"
                 && nStatus.Contains(w.PES_Nomination.TM_PES_NMN_Status.Id)
                 && nID.Contains(w.Id));

            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public IEnumerable<PES_Nomination_Signatures> GetPlanForMail(int nEva_id, List<int> nStatus, List<int> nApproveStatus)//,bool isAdmin
        {
            var sQuery = Query().Where(w => w.PES_Nomination.Id == nEva_id);
            // List<int> nStatus = new List<int>();
            //  nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            //  nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);

            // List<int> nApproveStatus = new List<int>();
            // nApproveStatus.Add((int)StepApprovePlan.Self);
            // nApproveStatus.Add((int)StepApproveEvaluate.Self);

            var GetMax = (from sQ in Query().Where(w2 => w2.Approve_status != "Y"
                          && w2.active_status == "Y"
                          && nStatus.Contains(w2.PES_Nomination.TM_PES_NMN_Status.Id)
                          && !nApproveStatus.Contains(w2.TM_PES_NMN_SignatureStep.Id)
                          && w2.PES_Nomination.Id == nEva_id
                          )
                          group new { sQ } by new { sQ.PES_Nomination.Id } into grp
                          select new
                          {
                              obj = grp.Where(w => w.sQ.TM_PES_NMN_SignatureStep.seq == grp.Min(p => p.sQ.TM_PES_NMN_SignatureStep.seq)
                              && w.sQ.PES_Nomination.Id == grp.Key.Id).Select(s => s.sQ.Id).FirstOrDefault(),

                          }
                        ).ToList();
            if (GetMax != null && GetMax.Any())
            {
                List<int> nID = GetMax.Select(s => s.obj).ToList();
                sQuery = sQuery.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y" && w.Approve_status != "Y"
                 && nStatus.Contains(w.PES_Nomination.TM_PES_NMN_Status.Id)
                 && nID.Contains(w.Id));

            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public IEnumerable<PES_Nomination_Signatures> GetApproveByEva(int Eva_id, List<int> nStatus, List<int> isnotStep)//,bool isAdmin
        {
            var sQuery = Query();
            //  List<int> nStatus = new List<int>();
            //  nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            // nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);
            sQuery = sQuery.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y" && w.PES_Nomination.Id == Eva_id && w.Approve_status == "Y"
               && nStatus.Contains(w.PES_Nomination.TM_PES_NMN_Status.Id) && !isnotStep.Contains((int)w.TM_PES_NMN_SignatureStep_Id));



            return sQuery.ToList();
        }
        public IEnumerable<PES_Nomination_Signatures> GetApproveByEvaAndStep(int Eva_id, List<int> Step)//,bool isAdmin
        {
            var sQuery = Query();
            //  List<int> nStatus = new List<int>();
            //  nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            // nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);
            sQuery = sQuery.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y" && w.PES_Nomination.Id == Eva_id && w.Approve_status != "Y"
                && Step.Contains((int)w.TM_PES_NMN_SignatureStep_Id));



            return sQuery.ToList();
        }
        public IEnumerable<PES_Nomination_Signatures> GetApproveByPlan(int Eva_id, List<int> nStatus, List<int> nApproveStatus)//,bool isAdmin
        {
            var sQuery = Query();
            // List<int> nStatus = new List<int>();
            // nStatus.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
            // nStatus.Add((int)Eva_Status.Waiting_for_Planning_Approval);
            sQuery = sQuery.Where(w => w.active_status == "Y" && w.PES_Nomination.active_status == "Y" && w.PES_Nomination.Id == Eva_id
               && nStatus.Contains(w.PES_Nomination.TM_PES_NMN_Status.Id));



            return sQuery.ToList();
        }
        public int CreateNewByList(List<PES_Nomination_Signatures> s)
        {
            var sResult = 0;
            if (s != null && s.Any())
            {
                var _updateUser = s.Select(f => f.update_user).FirstOrDefault();
                List<PES_Nomination_Signatures> lstEdit = new List<PES_Nomination_Signatures>();
                foreach (var item in s.Where(w => w.PES_Nomination != null && w.TM_PES_NMN_SignatureStep != null))
                {
                    var _checkDuplicate = Query().Where(w => w.Req_Approve_user == item.Req_Approve_user
                    && w.PES_Nomination.Id == item.PES_Nomination.Id
                    && w.TM_PES_NMN_SignatureStep.Id == item.TM_PES_NMN_SignatureStep.Id
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
        public int Update(PES_Nomination_Signatures s)
        {
            var sResult = 0;
            var _getDataSave = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataSave != null)
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
                var _getData = Query().Where(w => w.PES_Nomination.Id == nTIFID && w.active_status == "Y").ToList();
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

        public PES_Nomination_Signatures FindForDelete(int id, int nFileid)
        {
            return Query().SingleOrDefault(s => s.Id == nFileid && s.PES_Nomination.Id == id);
        }
        public bool CanSave(PES_Nomination_Signatures PES_Nomination_Signatures)
        {
            bool sCan = false;
            var sCheck = Query().FirstOrDefault(w => w.PES_Nomination.Id == PES_Nomination_Signatures.PES_Nomination.Id
                && w.Req_Approve_user == PES_Nomination_Signatures.Req_Approve_user
                && w.TM_PES_NMN_SignatureStep_Id == PES_Nomination_Signatures.TM_PES_NMN_SignatureStep_Id
                && w.active_status == "Y");
            if (sCheck == null)
            {
                sCan = true;
            }

            return sCan;
        }
        public bool CanCompleted(int PES_nID, int nID)
        {
            bool sCan = false;
            var sCheck = Query().Any(w => w.PES_Nomination.Id == PES_nID && w.Id != nID && w.Approve_status != "Y" && w.active_status == "Y");
            if (!sCheck)
            {
                sCan = true;
            }

            return sCan;
        }
        public int CreateNew(PES_Nomination_Signatures s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int DeleteFile(PES_Nomination_Signatures s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int CreateNewOrUpdate(PES_Nomination_Signatures s)
        {
            var sResult = 0;
            var objSave = s;
            var _getDataTIF = Query().Where(w => w.PES_Nomination.Id == objSave.PES_Nomination.Id
            && w.Req_Approve_user == objSave.Req_Approve_user
            && w.TM_PES_NMN_SignatureStep_Id == s.TM_PES_NMN_SignatureStep_Id
                            ).FirstOrDefault();
            if (_getDataTIF == null)
            {
                Add(s);
            }
            else
            {
                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                _getDataTIF.active_status = "Y";
                s = _getDataTIF;
            }
            sResult = SaveChanges();
            return sResult;
        }

        public int UpdateCommittee(List<PES_Nomination_Signatures> s, int nEmID, string UserUpdate, int nStep, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.PES_Nomination.Id == nEmID && w.TM_PES_NMN_SignatureStep_Id == nStep).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.Req_Approve_user).ToArray().Contains(w.Req_Approve_user) && w.active_status == "Y").ToList().ForEach(ed =>
           {
               ed.active_status = "N";
               ed.update_user = UserUpdate;
               ed.update_date = dNow;
           });
            //set old to active

            foreach (var item in s.Where(w => w.PES_Nomination != null))
            {
                var Addnew = _getAdv.Where(w => w.Req_Approve_user == item.Req_Approve_user).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.Req_Approve_user == item.Req_Approve_user).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.active_status = "Y";
                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }
    }
}
