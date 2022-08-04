using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using HumanCapitalManagement.Service.PartnerEvaluation;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.NMNVM;
using HumanCapitalManagement.ViewModel.PESVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.NominationSystem
{
    public class NominationReportController : BaseController
    {
        private PES_Nomination_YearService _PES_Nomination_YearService;
        private TM_PES_NMN_StatusService _TM_PES_NMN_StatusService;
        private TM_PES_NMN_SignatureStepService _TM_PES_NMN_SignatureStepService;
        private TM_KPIs_BaseService _TM_KPIs_BaseService;
        private PES_NominationService _PES_NominationService;
        private TM_PES_NMN_CompetenciesService _TM_PES_NMN_CompetenciesService;
        private PES_Nomination_CompetenciesService _PES_Nomination_CompetenciesService;
        private PES_Final_Rating_YearService _PES_Final_Rating_YearService;
        private PES_Nomination_KPIsService _PES_Nomination_KPIsService;
        private PES_Nomination_AnswerService _PES_Nomination_AnswerService;
        private PES_Final_RatingService _PES_Final_RatingService;
        private PES_Nomination_FilesService _PES_Nomination_FilesService;
        private PES_Nomination_SignaturesService _PES_Nomination_SignaturesService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public NominationReportController(PES_Nomination_YearService PES_Nomination_YearService
            , TM_PES_NMN_StatusService TM_PES_NMN_StatusService
            , TM_PES_NMN_SignatureStepService TM_PES_NMN_SignatureStepService
            , TM_KPIs_BaseService TM_KPIs_BaseService
            , PES_NominationService PES_NominationService, TM_PES_NMN_CompetenciesService TM_PES_NMN_CompetenciesService
            , PES_Nomination_CompetenciesService PES_Nomination_CompetenciesService
            , PES_Final_Rating_YearService PES_Final_Rating_YearService
            , PES_Nomination_KPIsService PES_Nomination_KPIsService
            , PES_Nomination_AnswerService PES_Nomination_AnswerService
             , PES_Final_RatingService PES_Final_RatingService
            , PES_Nomination_FilesService PES_Nomination_FilesService
            , PES_Nomination_SignaturesService PES_Nomination_SignaturesService
            )
        {
            _PES_Nomination_YearService = PES_Nomination_YearService;
            _TM_PES_NMN_StatusService = TM_PES_NMN_StatusService;
            _TM_PES_NMN_SignatureStepService = TM_PES_NMN_SignatureStepService;
            _TM_KPIs_BaseService = TM_KPIs_BaseService;
            _PES_NominationService = PES_NominationService;
            _TM_PES_NMN_CompetenciesService = TM_PES_NMN_CompetenciesService;
            _PES_Nomination_CompetenciesService = PES_Nomination_CompetenciesService;
            _PES_Final_Rating_YearService = PES_Final_Rating_YearService;
            _PES_Nomination_KPIsService = PES_Nomination_KPIsService;
            _PES_Nomination_AnswerService = PES_Nomination_AnswerService;
            _PES_Final_RatingService = PES_Final_RatingService;
            _PES_Nomination_FilesService = PES_Nomination_FilesService;
            _PES_Nomination_SignaturesService = PES_Nomination_SignaturesService;
        }
        // GET: NominationReport
        public ActionResult NominationReportList(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vNominationForm result = new vNominationForm();
            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpNmN" + unixTimestamps;
            result.session = sSession;
            rpvPTREvaluation_Session objSession = new rpvPTREvaluation_Session();
            Session[sSession] = new rpvPTREvaluation_Session();//new List<ListAdvanceTransaction>();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchNMNForm SearchItem = (CSearchNMNForm)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchNMNForm)));
                int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
                int nStatus = SystemFunction.GetIntNullToZero(SearchItem.status_id);
                List<int> lStatus = new List<int>();
                lStatus.Add((int)PESClass.Nomination_Status.Waiting_Approval);
                lStatus.Add((int)PESClass.Nomination_Status.Form_Completed);
                lStatus.Add((int)PESClass.Nomination_Status.Draft_Form);
                lStatus.Add((int)PESClass.Nomination_Status.Revised_Form);


                var lstData = _PES_NominationService.GetPESReport(nYear, nStatus, CGlobal.UserInfo.EmployeeNo, lStatus, CGlobal.UserIsAdminPES());

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.fy_year = SearchItem.fy_year;

                    #region 
                    List<vPTRApproval_Sumary> lstBUHead = new List<vPTRApproval_Sumary>();
                    List<vPTRApproval_Sumary> lstPracticeHead = new List<vPTRApproval_Sumary>();
                    List<vPTRApproval_Sumary> lstCeoHead = new List<vPTRApproval_Sumary>();
                    List<vPTRApproval_Sumary> lstCeoElect = new List<vPTRApproval_Sumary>();


                    var lstBU = lstData.Where(a => a.active_status == "Y" && a.PES_Nomination_Signatures.Any(a2 => a2.active_status == "Y" && a2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head)).SelectMany(s => s.PES_Nomination_Signatures.Where(w2 => w2.active_status == "Y" && w2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head)).ToList();
                    if (lstBU.Any())
                    {
                        string[] _aApproveNO = lstBU.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstBUHead = (from item in lstBU.Where(w => w.active_status == "Y")
                                     from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                     select new vPTRApproval_Sumary
                                     {
                                         eva_id = item.PES_Nomination.Id,
                                         approva_date = item.Approve_date,
                                         emp_name = lstEmpReq.EmpFullName,
                                         emp_no = item.Req_Approve_user,
                                         // eva_status_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                         rating_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                         rating_name = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.rating_name_en : "Waiting",
                                     }).ToList();
                    }
                    var lstPH = lstData.Where(a => a.active_status == "Y" && a.PES_Nomination_Signatures.Any(a2 => a2.active_status == "Y" && a2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP)).SelectMany(s => s.PES_Nomination_Signatures.Where(w2 => w2.active_status == "Y" && w2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP)).ToList();
                    if (lstPH.Any())
                    {
                        string[] _aApproveNO = lstPH.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstPracticeHead = (from item in lstPH.Where(w => w.active_status == "Y")
                                           from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                           select new vPTRApproval_Sumary
                                           {
                                               eva_id = item.PES_Nomination.Id,
                                               approva_date = item.Approve_date,
                                               emp_name = lstEmpReq.EmpFullName,
                                               emp_no = item.Req_Approve_user,
                                               // eva_status_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                               rating_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                               rating_name = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.rating_name_en : "Waiting",
                                           }).ToList();
                    }
                    var lstCeo = lstData.Where(a => a.active_status == "Y" && a.PES_Nomination_Signatures.Any(a2 => a2.active_status == "Y" && a2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo)).SelectMany(s => s.PES_Nomination_Signatures.Where(w2 => w2.active_status == "Y" && w2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo)).ToList();
                    if (lstCeo.Any())
                    {
                        string[] _aApproveNO = lstCeo.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstCeoHead = (from item in lstCeo.Where(w => w.active_status == "Y")
                                      from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      select new vPTRApproval_Sumary
                                      {
                                          eva_id = item.PES_Nomination.Id,
                                          approva_date = item.Approve_date,
                                          emp_name = lstEmpReq.EmpFullName,
                                          emp_no = item.Req_Approve_user,
                                          // eva_status_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                          rating_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                          rating_name = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.rating_name_en : "Waiting",
                                      }).ToList();
                    }

                    var lstCeoE = lstData.Where(a => a.active_status == "Y" && a.PES_Nomination_Signatures.Any(a2 => a2.active_status == "Y" && a2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo)).SelectMany(s => s.PES_Nomination_Signatures.Where(w2 => w2.active_status == "Y" && w2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo)).ToList();
                    if (lstCeoE.Any())
                    {
                        string[] _aApproveNO = lstCeoE.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstCeoElect = (from item in lstCeoE.Where(w => w.active_status == "Y")
                                       from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                       select new vPTRApproval_Sumary
                                       {
                                           eva_id = item.PES_Nomination.Id,
                                           approva_date = item.Approve_date,
                                           emp_name = lstEmpReq.EmpFullName,
                                           emp_no = item.Req_Approve_user,
                                           // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                           rating_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                           rating_name = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.rating_name_en : "Waiting",
                                       }).ToList();
                    }

                    #endregion




                    string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                    if (!string.IsNullOrEmpty(SearchItem.group_id))
                    {
                        _getPartnerUser = _getPartnerUser.Where(w => w.UnitGroupID + "" == SearchItem.group_id).ToList();
                    }
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    result.lstData = (from lstAD in lstData
                                          // from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      join lstEmpReq in _getPartnerUser on lstAD.user_no equals lstEmpReq.EmpNo
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      from glst in lstBUHead.Where(w => lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                      from plst in lstPracticeHead.Where(w => lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                      from clst in lstCeoHead.Where(w => lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())

                                      from ceoelst in lstCeoElect.Where(w => lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())


                                      select new vNominationForm_obj
                                      {

                                          name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                          name = lstEmpReq.EmpFullName,
                                          emp_no = lstAD.user_no,
                                          fy_year = lstAD.PES_Nomination_Year.evaluation_year.HasValue ? lstAD.PES_Nomination_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                          eva_status = lstAD.TM_PES_NMN_Status != null ? lstAD.TM_PES_NMN_Status.status_name_en : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sgroup = lstEmpReq.UnitGroupName + "",
                                          srank = lstEmpReq.Rank + "",
                                          sceo = (clst.emp_no + "" != "") ? (clst.emp_name + "<br/>" + "(" + (clst.approva_date.HasValue ? clst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                          spractice = (plst.emp_no + "" != "") ? (plst.emp_name + "<br/>" + "(" + (plst.approva_date.HasValue ? plst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                          sbu = (glst.emp_no + "" != "") ? (glst.emp_name + "<br/>" + "(" + (glst.approva_date.HasValue ? glst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                          ceoe = (ceoelst.emp_no + "" != "") ? (ceoelst.emp_name + "<br/>" + "(" + (ceoelst.approva_date.HasValue ? ceoelst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",

                                          self_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Self) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Self).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Self).TM_Annual_Rating.rating_name_en : "") : "",
                                          final_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo).TM_Annual_Rating.rating_name_en : "") : "",
                                          bu_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head).TM_Annual_Rating.rating_name_en : "") : "",
                                          practice_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP).TM_Annual_Rating.rating_name_en : "") : "",
                                          ceoe_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo).TM_Annual_Rating.rating_name_en : "") : "",
                                          Id = lstAD.Id,
                                      }).ToList();

                    if (result.lstData.Any())
                    {
                        result.lstData.Where(w => w.emp_no == CGlobal.UserInfo.EmployeeNo).ToList().ForEach(ed =>
                        {
                            //  ed.bu_eva = "-";
                            //  ed.practice_eva = "-";
                            //  ed.ceoe_eva = "-";

                        });
                        objSession.lstData = (from item in result.lstData
                                              from lstEva in lstData.AsEnumerable().Where(w => w.Id == item.Id).DefaultIfEmpty(new PES_Nomination())
                                              select new vPTREvaluation_report
                                              {
                                                  name = item.name,
                                                  sbu = item.sbu.Replace("<br/>", " "),
                                                  sceo = item.sceo.Replace("<br/>", " "),
                                                  self_eva = item.self_eva,
                                                  sgroup = item.sgroup,
                                                  spractice = item.spractice.Replace("<br/>", " "),
                                                  emp_no = item.emp_no,
                                                  eva_status = item.eva_status,
                                                  bu_eva = item.bu_eva,
                                                  final_eva = item.final_eva,
                                                  fy_year = item.fy_year,
                                                  practice_eva = item.practice_eva,
                                                  srank = item.srank,
                                                  bu_comment = lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head) != null ? (lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head).responses + "") : "",
                                                  practice_comment = lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP) != null ? (lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP).responses + "") : "",
                                                  final_comment = lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo) != null ? (lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo).responses + "") : "",

                                                  ceoe = item.ceoe.Replace("<br/>", " "),
                                                  ceoe_comment = lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo) != null ? (lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo).responses + "") : "",
                                                  ceoe_eva = item.ceoe_eva,

                                              }).ToList();

                        objSession.lstData.ForEach(ed =>
                        {
                            ed.bu_comment = HCMFunc.DataDecryptPES(ed.bu_comment + "");
                            ed.practice_comment = HCMFunc.DataDecryptPES(ed.practice_comment + "");
                            ed.final_comment = HCMFunc.DataDecryptPES(ed.final_comment + "");
                            ed.ceoe_comment = HCMFunc.DataDecryptPES(ed.ceoe_comment + "");
                        });

                        Session[sSession] = objSession.lstData;
                    }


                }
            }
            else
            {
                var _getYear = _PES_Nomination_YearService.FindNowYear();
                if (_getYear != null)
                {
                    result.fy_year = _getYear.Id + "";
                }
            }
            #endregion
            return View(result);
        }
        public ActionResult NominationReportEdit(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vNominationForm_obj_save result = new vNominationForm_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {

                    var _getData = _PES_NominationService.Find(nId);
                    ViewBag.formyear = _getData.PES_Nomination_Year.evaluation_year.Value.Year;
                    bool checkAuthor = false;


                    if (_getData != null
                        && _getData.PES_Nomination_Signatures != null
                        && (_getData.PES_Nomination_Signatures.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && a.active_status == "Y")
                        || CGlobal.UserIsAdminPES())
                        || checkAuthor
                        )
                    {

                        var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                        if (_checkActiv != null)
                        {
                            if (_getData.PES_Nomination_Signatures.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo && a.active_status == "Y")
                                || CGlobal.UserIsAdminPES())
                            {
                                result.is_ceo = "Y";
                            }

                            result.IdEncrypt = qryStr;
                            result.code = _getData.user_no;
                            result.sname = _checkActiv.EmpFullName;
                            result.sgroup = _checkActiv.UnitGroupName;
                            result.srank = _checkActiv.Rank;
                            result.status_name = _getData.TM_PES_NMN_Status != null ? _getData.TM_PES_NMN_Status.status_name_en + "" : "";
                            result.status_id = _getData.TM_PES_NMN_Status_Id + "";
                            result.yearcurrent = _getData.PES_Nomination_Year.evaluation_year.Value.DateTimebyCulture("yyyy");
                            result.other_role = _getData.other_roles;
                            result.type_id = _getData.TM_PES_NMN_Type_Id + "";
                            result.type_name = _getData.TM_PES_NMN_Type != null ? _getData.TM_PES_NMN_Type.type_name_en : "";
                            result.Year_joined_KPMG = _getData.Year_joined_KPMG.HasValue ? _getData.Year_joined_KPMG.Value.DateTimebyCulture() : "";
                            result.working_with_KPMG = _getData.working_with_KPMG + "";
                            result.working_outside_KPMG = _getData.working_outside_KPMG + "";
                            result.being_ADDirector = _getData.being_ADDirector + "";
                            result.being_AD_Director = _getData.being_AD_Director.HasValue ? _getData.being_AD_Director.Value.DateTimebyCulture() : "";
                            result.Professional_Qualifications = _getData.Professional_Qualifications + "";
                            result.DEVELOPMENT_AREAS = HCMFunc.DataDecryptPES(_getData.DEVELOPMENT_AREAS);
                            if (_getData.PES_Nomination_Files != null && _getData.PES_Nomination_Files.Any(a => a.active_status == "Y"))
                            {
                                int nFile = 1;
                                result.lstFile = _getData.PES_Nomination_Files.Where(a => a.active_status == "Y").Select(s => new vNominationForm_lst_File
                                {
                                    file_name = s.sfile_oldname,
                                    description = s.description,
                                    Edit = (_getData.TM_PES_NMN_Status.Id != (int)PESClass.Nomination_Status.Draft_Form && _getData.TM_PES_NMN_Status.Id != (int)PESClass.Nomination_Status.Revised_Form) ? "" + nFile++ : @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                    View = @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-success""><i class=""glyphicon glyphicon-download-alt""></i></button>",
                                }).ToList();
                            }
                            #region Eva Data 
                            result.eva_mode = "Eva";


                            if (_getData.PES_Nomination_Year.evaluation_year.HasValue)
                            {

                                int nCurrent = _getData.PES_Nomination_Year.evaluation_year.Value.Year;
                                result.yearone = (_getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-1)).DateTimebyCulture("yyyy");
                                result.yeartwo = (_getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-2)).DateTimebyCulture("yyyy");
                                result.yearthree = (_getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-3)).DateTimebyCulture("yyyy");
                            }
                            if (_getData.PES_Nomination_Signatures != null && _getData.PES_Nomination_Signatures.Any(a => a.active_status == "Y"))
                            {

                                result.lstApprove = PESFunc.GetNominationApproveList(_getData.PES_Nomination_Signatures.Where(w => w.active_status == "Y").ToList(), _getData);
                            }


                            var _getCom = _TM_PES_NMN_CompetenciesService.GetDataForSelect();
                            if (_getCom.Any())
                            {
                                List<vNominationForm_RATING_SUMMARY> lstRating_Summary = new List<vNominationForm_RATING_SUMMARY>();
                                var GetRatescore = _PES_Nomination_CompetenciesService.GetScored(_getData.Id, true).ToList();

                                lstRating_Summary.AddRange(
                                               (from lstQ in _getCom
                                                select new vNominationForm_RATING_SUMMARY
                                                {

                                                    nstep = lstQ.seq,
                                                    id = lstQ.Id + "",
                                                    question = lstQ.question,
                                                    header = lstQ.header,
                                                    nSeq = lstQ.seq,
                                                    sgroup = lstQ.header,
                                                    sExcellent = "N",
                                                    sHigh = "N",
                                                    sNI = "N",
                                                    sLow = "N",
                                                    nscore = lstQ.nscore + "",
                                                    isCurrent = "N",
                                                    nrate = GetRatescore.Where(w => w.active_status == "Y" && w.TM_PES_NMN_Competencies_Id == lstQ.Id).Select(s => s.TM_PES_NMN_Competencies_Rating_Id).FirstOrDefault(),
                                                }).ToList()
                                               );

                                lstRating_Summary.ForEach(ed =>
                                {
                                    if (ed.nrate == (int)PESClass.Incidents_Score.Excellent)
                                    {
                                        ed.sExcellent = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.High)
                                    {
                                        ed.sHigh = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.Low)
                                    {
                                        ed.sLow = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.NI)
                                    {
                                        ed.sNI = "Y";
                                    }
                                });

                                result.lstRating_Summary = lstRating_Summary.ToList();
                            }

                            var _getKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                            if (_getKPIs.Any())
                            {
                                var _getDataYCur = _PES_Nomination_KPIsService.FindFor3YearKPI(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.Year);
                                var _getDataYOne = _PES_Nomination_KPIsService.FindFor3YearKPI(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-1).Year);
                                var _getDataYTwo = _PES_Nomination_KPIsService.FindFor3YearKPI(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-2).Year);
                                var _getDataYThree = _PES_Nomination_KPIsService.FindFor3YearKPI(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-3).Year);

                                //canEdit.Add((int)PESClass.Nomination_Status.Draft_Form);
                                //canEdit.Add((int)PESClass.Nomination_Status.Revised_Form);
                                result.lstKPIs = (from lst in _getKPIs
                                                  from lstKPICur in _getDataYCur.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                  from lstKPIOne in _getDataYOne.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                  from lstKPITwo in _getDataYTwo.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                  from lstKPIThree in _getDataYThree.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                  select new vNominationForm_KPIs
                                                  {
                                                      sname = lst.kpi_name_en,
                                                      target_data = HCMFunc.DataDecryptPES(lstKPICur.target),// lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lstKPICur.target) + "-" + HCMFunc.DataDecryptPES(lstKPICur.target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPICur.target_max))) : HCMFunc.DataDecryptPES(lstKPICur.target_max)),
                                                      yearoneTar = HCMFunc.DataDecryptPES(lstKPIOne.target), //lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lstKPIOne.target)+ "-" + HCMFunc.DataDecryptPES(lstKPIOne.target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.target_max))) : HCMFunc.DataDecryptPES(lstKPIOne.target_max)),
                                                      yeartwoTar = HCMFunc.DataDecryptPES(lstKPITwo.target), // lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lstKPITwo.target)+ "-" + HCMFunc.DataDecryptPES(lstKPITwo.target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.target_max))) : HCMFunc.DataDecryptPES(lstKPITwo.target_max)),
                                                      yearthreeTar = HCMFunc.DataDecryptPES(lstKPIThree.target), // lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lstKPIThree.target)+ "-" + HCMFunc.DataDecryptPES(lstKPIThree.target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.target_max))) : HCMFunc.DataDecryptPES(lstKPIThree.target_max)),

                                                      //remark = HCMFunc.DataDecryptPES(lst.final_remark + ""),
                                                      actual = HCMFunc.DataDecryptPES(lstKPICur.actual),
                                                      yearone = HCMFunc.DataDecryptPES(lstKPIOne.actual),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                                      yeartwo = HCMFunc.DataDecryptPES(lstKPITwo.actual),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                                      yearthree = HCMFunc.DataDecryptPES(lstKPIThree.actual),//HCMFunc.DataDecryptPES(lstKPIThree.actual),
                                                      sdisable = (_getData.TM_PES_NMN_Status.Id == (int)PESClass.Nomination_Status.Draft_Form || _getData.TM_PES_NMN_Status.Id == (int)PESClass.Nomination_Status.Revised_Form) ? "N" : "Y",
                                                      IdEncrypt = lst.Id + "",
                                                      stype = lst.type_of_kpi,
                                                      //target_group = lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lst.group_target) + "-" + HCMFunc.DataDecryptPES(lst.group_target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max))) : HCMFunc.DataDecryptPES(lst.group_target_max)),
                                                      //group_actual = lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual))) : HCMFunc.DataDecryptPES(lst.group_actual),


                                                  }).ToList();
                            }
                            var _getRateYCur = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.Year);
                            var _getRateYOne = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-1).Year);
                            var _getRateYTwo = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-2).Year);
                            var _getRateYThree = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-3).Year);
                            result.yearcurrent_rate = "";
                            result.yearcurrent_selfrate = "";
                            result.yearone_rate = "";
                            result.yeartwo_rate = "";
                            result.yearthree_rate = "";

                            if (_getRateYCur != null)
                            {
                                result.yearcurrent_rate = _getRateYCur.Final_TM_Annual_Rating_Id + "";
                                result.yearcurrent_selfrate = _getRateYCur.Self_TM_Annual_Rating_Id + "";
                            }
                            if (_getRateYOne != null)
                            {
                                result.yearone_rate = _getRateYOne.Final_TM_Annual_Rating_Id + "";
                            }
                            if (_getRateYTwo != null)
                            {
                                result.yeartwo_rate = _getRateYTwo.Final_TM_Annual_Rating_Id + "";
                            }
                            if (_getRateYThree != null)
                            {
                                result.yearthree_rate = _getRateYThree.Final_TM_Annual_Rating_Id + "";
                            }
                            #region answer

                            if (_getData.PES_Nomination_Answer != null && _getData.PES_Nomination_Answer.Any(a => a.active_status == "Y"))
                            {
                                var _getAnswer = _getData.PES_Nomination_Answer.Where(a => a.active_status == "Y").ToList();
                                result.Client_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Client_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Client_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Client_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Client_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Client_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Client_Q4 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Client_Q4).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Business_Skills_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Business_Skills_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Business_Skills_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Business_Skills_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Management_Leadership_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Management_Leadership_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Management_Leadership_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Management_Leadership_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Management_Leadership_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Management_Leadership_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Social_Skills_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Social_Skills_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Social_Skills_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Social_Skills_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Thinking_Skills_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Thinking_Skills_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Thinking_Skills_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Thinking_Skills_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Thinking_Skills_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Thinking_Skills_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Technical_Competence_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Technical_Competence_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.RISK_MANAGEMENT_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.RISK_MANAGEMENT_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.RISK_MANAGEMENT_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.RISK_MANAGEMENT_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Business_Skills_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Business_Skills_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Technical_Competence_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Technical_Competence_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                result.Technical_Competence_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Technical_Competence_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                            }
                            #endregion

                            #endregion


                            if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2018)
                            {
                                return View(result);
                            }
                            else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2019)
                            {
                                return View(result);
                            }
                            else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == DateTime.Now.Year)
                            {
                                return View("NominationReportEdit"+DateTime.Now.ToString("yy"), result);
                            }
                            else
                            {
                                return View(result);
                            }
                          
                        }
                        else
                        {
                            return RedirectToAction("PTRReportsList", "PTRReports");
                        }

                    }
                    else
                    {
                        return RedirectToAction("NominationReportList", "NominationReport");
                    }
                }
                else
                {
                    return RedirectToAction("NominationReportList", "NominationReport");
                }

            }
            else
            {
                return RedirectToAction("NominationReportList", "NominationReport");
            }
            #endregion
        }

        #region Ajax Function
        [HttpPost]
        public ActionResult LoadNominationReportList(CSearchPTREvaluation SearchItem)
        {
            vNominationForm_Return result = new vNominationForm_Return();
            List<vNominationForm_obj> lstData_resutl = new List<vNominationForm_obj>();
            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            int nStatus = SystemFunction.GetIntNullToZero(SearchItem.status_id);
            List<int> lStatus = new List<int>();
            lStatus.Add((int)PESClass.Nomination_Status.Waiting_Approval);
            lStatus.Add((int)PESClass.Nomination_Status.Form_Completed);
            lStatus.Add((int)PESClass.Nomination_Status.Draft_Form);
            lStatus.Add((int)PESClass.Nomination_Status.Revised_Form);
            var lstData = _PES_NominationService.GetPESReport(nYear, nStatus, CGlobal.UserInfo.EmployeeNo, lStatus, CGlobal.UserIsAdminPES());
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            rpvPTREvaluation_Session objSession = new rpvPTREvaluation_Session();
            if (lstData.Any())
            {

                #region 
                List<vPTRApproval_Sumary> lstBUHead = new List<vPTRApproval_Sumary>();
                List<vPTRApproval_Sumary> lstPracticeHead = new List<vPTRApproval_Sumary>();
                List<vPTRApproval_Sumary> lstCeoHead = new List<vPTRApproval_Sumary>();
                List<vPTRApproval_Sumary> lstCeoElect = new List<vPTRApproval_Sumary>();

                var lstBU = lstData.Where(a => a.active_status == "Y" && a.PES_Nomination_Signatures.Any(a2 => a2.active_status == "Y" && a2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head)).SelectMany(s => s.PES_Nomination_Signatures.Where(w2 => w2.active_status == "Y" && w2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head)).ToList();
                if (lstBU.Any())
                {
                    string[] _aApproveNO = lstBU.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                    var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                    lstBUHead = (from item in lstBU.Where(w => w.active_status == "Y")
                                 from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                 select new vPTRApproval_Sumary
                                 {
                                     eva_id = item.PES_Nomination.Id,
                                     approva_date = item.Approve_date,
                                     emp_name = lstEmpReq.EmpFullName,
                                     emp_no = item.Req_Approve_user,
                                     // eva_status_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                     rating_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                     rating_name = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.rating_name_en : "Waiting",
                                 }).ToList();
                }
                var lstPH = lstData.Where(a => a.active_status == "Y" && a.PES_Nomination_Signatures.Any(a2 => a2.active_status == "Y" && a2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP)).SelectMany(s => s.PES_Nomination_Signatures.Where(w2 => w2.active_status == "Y" && w2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP)).ToList();
                if (lstPH.Any())
                {
                    string[] _aApproveNO = lstPH.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                    var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                    lstPracticeHead = (from item in lstPH.Where(w => w.active_status == "Y")
                                       from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                       select new vPTRApproval_Sumary
                                       {
                                           eva_id = item.PES_Nomination.Id,
                                           approva_date = item.Approve_date,
                                           emp_name = lstEmpReq.EmpFullName,
                                           emp_no = item.Req_Approve_user,
                                           // eva_status_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                           rating_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                           rating_name = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.rating_name_en : "Waiting",
                                       }).ToList();
                }
                var lstCeo = lstData.Where(a => a.active_status == "Y" && a.PES_Nomination_Signatures.Any(a2 => a2.active_status == "Y" && a2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo)).SelectMany(s => s.PES_Nomination_Signatures.Where(w2 => w2.active_status == "Y" && w2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo)).ToList();
                if (lstCeo.Any())
                {
                    string[] _aApproveNO = lstCeo.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                    var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                    lstCeoHead = (from item in lstCeo.Where(w => w.active_status == "Y")
                                  from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                  select new vPTRApproval_Sumary
                                  {
                                      eva_id = item.PES_Nomination.Id,
                                      approva_date = item.Approve_date,
                                      emp_name = lstEmpReq.EmpFullName,
                                      emp_no = item.Req_Approve_user,
                                      // eva_status_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                      rating_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                      rating_name = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.rating_name_en : "Waiting",
                                  }).ToList();
                }

                var lstCeoE = lstData.Where(a => a.active_status == "Y" && a.PES_Nomination_Signatures.Any(a2 => a2.active_status == "Y" && a2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo)).SelectMany(s => s.PES_Nomination_Signatures.Where(w2 => w2.active_status == "Y" && w2.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo)).ToList();
                if (lstCeoE.Any())
                {
                    string[] _aApproveNO = lstCeoE.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                    var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                    lstCeoElect = (from item in lstCeoE.Where(w => w.active_status == "Y")
                                   from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                   select new vPTRApproval_Sumary
                                   {
                                       eva_id = item.PES_Nomination.Id,
                                       approva_date = item.Approve_date,
                                       emp_name = lstEmpReq.EmpFullName,
                                       emp_no = item.Req_Approve_user,
                                       // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                       rating_id = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.Id : 0,
                                       rating_name = item.TM_Annual_Rating != null ? item.TM_Annual_Rating.rating_name_en : "Waiting",
                                   }).ToList();
                }


                #endregion




                string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                if (!string.IsNullOrEmpty(SearchItem.group_id))
                {
                    _getPartnerUser = _getPartnerUser.Where(w => w.UnitGroupID + "" == SearchItem.group_id).ToList();
                }
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                result.lstData = (from lstAD in lstData
                                      // from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                  join lstEmpReq in _getPartnerUser on lstAD.user_no equals lstEmpReq.EmpNo
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  from glst in lstBUHead.Where(w => lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head) != null
                                  && w.eva_id == lstAD.Id
                                  && w.emp_no == lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                  from plst in lstPracticeHead.Where(w => lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP) != null
                                  && w.eva_id == lstAD.Id
                                  && w.emp_no == lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                  from clst in lstCeoHead.Where(w => lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo) != null
                                  && w.eva_id == lstAD.Id
                                  && w.emp_no == lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                  from ceoelst in lstCeoElect.Where(w => lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo) != null
                                  && w.eva_id == lstAD.Id
                                  && w.emp_no == lstAD.PES_Nomination_Signatures.FirstOrDefault(a => a.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())



                                  select new vNominationForm_obj
                                  {

                                      name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                      name = lstEmpReq.EmpFullName,
                                      emp_no = lstAD.user_no,
                                      fy_year = lstAD.PES_Nomination_Year.evaluation_year.HasValue ? lstAD.PES_Nomination_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                      eva_status = lstAD.TM_PES_NMN_Status != null ? lstAD.TM_PES_NMN_Status.status_name_en : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      sgroup = lstEmpReq.UnitGroupName + "",
                                      srank = lstEmpReq.Rank + "",
                                      sceo = (clst.emp_no + "" != "") ? (clst.emp_name + "<br/>" + "(" + (clst.approva_date.HasValue ? clst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                      spractice = (plst.emp_no + "" != "") ? (plst.emp_name + "<br/>" + "(" + (plst.approva_date.HasValue ? plst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                      sbu = (glst.emp_no + "" != "") ? (glst.emp_name + "<br/>" + "(" + (glst.approva_date.HasValue ? glst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                      ceoe = (ceoelst.emp_no + "" != "") ? (ceoelst.emp_name + "<br/>" + "(" + (ceoelst.approva_date.HasValue ? ceoelst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",

                                      self_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Self) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Self).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Self).TM_Annual_Rating.rating_name_en : "") : "",
                                      final_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo).TM_Annual_Rating.rating_name_en : "") : "",
                                      bu_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head).TM_Annual_Rating.rating_name_en : "") : "",
                                      practice_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP).TM_Annual_Rating.rating_name_en : "") : "",
                                      ceoe_eva = lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo) != null ? (lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo).TM_Annual_Rating != null ? lstAD.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo).TM_Annual_Rating.rating_name_en : "") : "",

                                      Id = lstAD.Id,
                                  }).ToList();

                if (result.lstData.Any())
                {
                    objSession.lstData = (from item in result.lstData
                                          from lstEva in lstData.AsEnumerable().Where(w => w.Id == item.Id).DefaultIfEmpty(new PES_Nomination())
                                          select new vPTREvaluation_report
                                          {
                                              name = item.name,
                                              emp_no = item.emp_no,
                                              eva_status = item.eva_status,
                                              bu_eva = item.bu_eva,
                                              final_eva = item.final_eva,
                                              fy_year = item.fy_year,
                                              practice_eva = item.practice_eva,
                                              sbu = item.sbu.Replace("<br/>", " "),
                                              sceo = item.sceo.Replace("<br/>", " "),
                                              self_eva = item.self_eva,
                                              sgroup = item.sgroup,
                                              spractice = item.spractice.Replace("<br/>", " "),
                                              srank = item.srank,
                                              bu_comment = lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head) != null ? (lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Group_Head).responses + "") : "",
                                              practice_comment = lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP) != null ? (lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.HOP).responses + "") : "",
                                              final_comment = lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo) != null ? (lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo).responses + "") : "",
                                              ceoe = item.ceoe.Replace("<br/>", " "),
                                              ceoe_comment = lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo) != null ? (lstEva.PES_Nomination_Signatures.FirstOrDefault(f => f.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Deputy_Ceo).responses + "") : "",
                                              ceoe_eva = item.ceoe_eva,
                                          }).ToList();
                    objSession.lstData.ForEach(ed =>
                    {
                        ed.bu_comment = HCMFunc.DataDecryptPES(ed.bu_comment + "");
                        ed.practice_comment = HCMFunc.DataDecryptPES(ed.practice_comment + "");
                        ed.final_comment = HCMFunc.DataDecryptPES(ed.final_comment + "");
                        ed.ceoe_comment = HCMFunc.DataDecryptPES(ed.ceoe_comment + "");
                    });
                }

            }
            Session[SearchItem.session] = objSession;
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        #endregion
    }
}