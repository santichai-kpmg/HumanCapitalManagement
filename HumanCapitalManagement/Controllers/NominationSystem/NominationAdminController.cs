using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using HumanCapitalManagement.Service.PartnerEvaluation;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using HumanCapitalManagement.ViewModel.NMNVM;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.NominationSystem
{
    public class NominationAdminController : BaseController
    {
        private PES_Nomination_YearService _PES_Nomination_YearService;
        private TM_PES_NMN_StatusService _TM_PES_NMN_StatusService;
        private TM_PES_NMN_SignatureStepService _TM_PES_NMN_SignatureStepService;
        private TM_KPIs_BaseService _TM_KPIs_BaseService;
        private PES_NominationService _PES_NominationService;
        private PES_Nomination_SignaturesService _PES_Nomination_SignaturesService;
        private TM_PES_NMN_CompetenciesService _TM_PES_NMN_CompetenciesService;
        private PES_Nomination_Default_CommitteeService _PES_Nomination_Default_CommitteeService;
        private PES_Final_Rating_YearService _PES_Final_Rating_YearService;
        private PES_Final_RatingService _PES_Final_RatingService;
        private PES_Nomination_KPIsService _PES_Nomination_KPIsService;

        New_HRISEntities dbHr = new New_HRISEntities();
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        public NominationAdminController(PES_Nomination_YearService PES_Nomination_YearService
            , TM_PES_NMN_StatusService TM_PES_NMN_StatusService
            , TM_PES_NMN_SignatureStepService TM_PES_NMN_SignatureStepService
            , TM_KPIs_BaseService TM_KPIs_BaseService
            , PES_NominationService PES_NominationService
            , PES_Nomination_SignaturesService PES_Nomination_SignaturesService
            , TM_PES_NMN_CompetenciesService TM_PES_NMN_CompetenciesService
            , PES_Nomination_Default_CommitteeService PES_Nomination_Default_CommitteeService
            , PES_Final_Rating_YearService PES_Final_Rating_YearService
            , PES_Final_RatingService PES_Final_RatingService
            , PES_Nomination_KPIsService PES_Nomination_KPIsService
            )
        {
            _PES_Nomination_YearService = PES_Nomination_YearService;
            _TM_PES_NMN_StatusService = TM_PES_NMN_StatusService;
            _TM_PES_NMN_SignatureStepService = TM_PES_NMN_SignatureStepService;
            _TM_KPIs_BaseService = TM_KPIs_BaseService;
            _PES_NominationService = PES_NominationService;
            _PES_Nomination_SignaturesService = PES_Nomination_SignaturesService;
            _TM_PES_NMN_CompetenciesService = TM_PES_NMN_CompetenciesService;
            _PES_Nomination_Default_CommitteeService = PES_Nomination_Default_CommitteeService;
            _PES_Final_Rating_YearService = PES_Final_Rating_YearService;
            _PES_Final_RatingService = PES_Final_RatingService;
            _PES_Nomination_KPIsService = PES_Nomination_KPIsService;
        }

        // GET: NominationAdmin
        public ActionResult NMNAdminList(string qryStr)
        {
            var get = HCMFunc.DataEncryptPES("10.31");
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vNMNAdmin result = new vNMNAdmin();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchNMNAdmin SearchItem = (CSearchNMNAdmin)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchNMNAdmin)));
                var lstData = _PES_Nomination_YearService.GetCRank(
               SearchItem.pr_status,
           SearchItem.pr_status);
                result.pr_status = SearchItem.pr_status;

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vNMNAdmin_obj
                                      {
                                          name_en = lstAD.evaluation_year.HasValue ? lstAD.evaluation_year.Value.DateTimeWithTimebyCulture("yyyy") : "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();

                }
            }

            #endregion
            return View(result);
        }
        public ActionResult NMNAdminEdit(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vNMNAdmin_obj_save result = new vNMNAdmin_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PES_Nomination_YearService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = qryStr;
                        result.file_name = _getData.sfile_oldname + "";
                        result.actual_file_name = _getData.actual_sfile_oldname + "";
                        int[] aStatus = new int[] { 1, 3 };
                        string[] aRank = new string[] { "12", "20", "32" };
                        string[] empNO = new string[] { };

                        if (_getData.PES_Nomination != null && _getData.PES_Nomination.Any(a => a.active_status == "Y"))
                        {
                            empNO = _getData.PES_Nomination.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                            var _getSponser = dbHr.AllInfo_WS.Where(w => w.RankID == "12" && aStatus.Contains((int)w.Status)).ToList();
                            if (_getSponser.Any())
                            {
                                result.lstsponsering = _getSponser.OrderBy(o => o.EmpFullName).Select(s => new lstDataSelect
                                {
                                    value = s.EmpNo + "",
                                    text = s.EmpFullName + " (" + s.EmpNo + ")",
                                }).ToList();
                            }
                            var _getYearRate = _PES_Final_Rating_YearService.GetDataForSelect();
                            result.lstsponsering.Insert(0, new lstDataSelect { value = PESClass.KhunSithakarn + "", text = "Sithakarn Anuntasilpa (" + PESClass.KhunSithakarn + ")" });

                            result.lstNewData = (from lstAD in _getData.PES_Nomination.Where(a => a.active_status == "Y")
                                                 from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                 select new vPartnerKPIs_obj
                                                 {
                                                     codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                     id = lstAD.Id + "",
                                                     emp_name = lstEmpReq.EmpFullName,
                                                     emp_comp = lstEmpReq.CompanyCode,
                                                     emp_group = lstEmpReq.UnitGroupName,
                                                     emp_code = lstEmpReq.EmpNo,
                                                     emp_rank = lstEmpReq.RankCode,
                                                     lstApproval = PESFunc.GetSignatureName(lstAD.PES_Nomination_Signatures.Where(w => w.active_status == "Y").ToList()),
                                                     sponsering_id = lstAD.PES_Nomination_Signatures.Where(w => w.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Sponsoring_Partner && w.active_status == "Y").Select(s => s.Req_Approve_user).FirstOrDefault(),
                                                     Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditRating('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                     lstRating = PESFunc.GetRatingYear(_getYearRate.ToList(), lstAD.user_no),
                                                     emp_dec = lstAD.TM_PES_NMN_Type != null ? lstAD.TM_PES_NMN_Type.type_name_en : "",
                                                 }).ToList();

                            result.lstActualData = (from lstAD in _getData.PES_Nomination.Where(a => a.active_status == "Y")
                                                    from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vPartnerKPIs_obj
                                                    {
                                                        codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                        Edit = "",
                                                        emp_name = lstEmpReq.EmpFullName,
                                                        emp_comp = lstEmpReq.CompanyCode,
                                                        emp_group = lstEmpReq.UnitGroupName,
                                                        emp_code = lstEmpReq.EmpNo,
                                                        emp_rank = lstEmpReq.RankCode,




                                                    }).ToList();
                        }
                        result.action_date = _getData.evaluation_year.HasValue ? _getData.evaluation_year.Value.DateTimeWithTimebyCulture() : "";
                        string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        string sSession = "ImportFileKPI" + unixTimestamps;
                        result.Session = sSession;



                        result.lstOldData = new List<vPartner_obj>();
                        var _getPartner = dbHr.AllInfo_WS.Where(w => aRank.Contains(w.RankID) && aStatus.Contains((int)w.Status) && !empNO.Contains(w.EmpNo)).ToList();
                        if (_getPartner.Any())
                        {
                            result.lstOldData = _getPartner.Select(s => new vPartner_obj
                            {
                                codeEncrypt = HCMFunc.EncryptPES(s.EmpNo + ""),
                                Edit = "",
                                emp_name = s.EmpFullName,
                                emp_comp = s.CompanyCode,
                                emp_group = s.UnitGroupName,
                                emp_code = s.EmpNo,
                                emp_rank = s.RankCode,
                            }).ToList();

                            //result.lstOldData = (from lstAD in _getPartner
                            //                     select new vPartner_obj
                            //                     {
                            //                         //codeEncrypt = HCMFunc.EncryptPES(lstAD.EmpNo + ""),
                            //                         Edit = "",
                            //                         emp_name = lstAD.EmpFullName,
                            //                         emp_comp = lstAD.CompanyCode,
                            //                         emp_group = lstAD.UnitGroupName,
                            //                         emp_code = lstAD.EmpNo,
                            //                         emp_rank = lstAD.RankCode,
                            //                     }).ToList();
                        }
                    }
                }
            }
            return View(result);

            #endregion
        }

        public ActionResult NMNAdminImportKPI(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vNMNAdmin result = new vNMNAdmin();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchNMNAdmin SearchItem = (CSearchNMNAdmin)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchNMNAdmin)));
                var lstData = _PES_Nomination_YearService.GetCRank(
               SearchItem.pr_status,
           SearchItem.pr_status);
                result.pr_status = SearchItem.pr_status;

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vNMNAdmin_obj
                                      {
                                          name_en = lstAD.evaluation_year.HasValue ? lstAD.evaluation_year.Value.DateTimeWithTimebyCulture("yyyy") : "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();

                }
            }

            #endregion
            return View(result);
        }

        #region Ajax Function
        [HttpPost]
        public ActionResult UpdateCommittee(vNominationForm_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vNominationForm_Return result = new vNominationForm_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PES_Nomination_YearService.Find(nId);
                    if (_getData != null && _getData.PES_Nomination != null)
                    {
                        var _getCommitteeShareholder = _PES_Nomination_Default_CommitteeService.GetCommitteeForShareholder(nId);
                        var _getCommittee = _PES_Nomination_Default_CommitteeService.GetCommitteeForAD_PTR(nId);
                        var _getNomination = _getData.PES_Nomination.Where(w => w.active_status == "Y").ToList();
                        foreach (var item in _getNomination)
                        {
                            if (item.TM_PES_NMN_Type_Id == (int)PESClass.Nomination_Type.Shareholder)
                            {
                                if (_getCommitteeShareholder != null && _getCommitteeShareholder.Any())
                                {
                                    List<PES_Nomination_Signatures> lstApprove = new List<PES_Nomination_Signatures>();

                                    lstApprove.AddRange(_getCommitteeShareholder.Select(s => new PES_Nomination_Signatures
                                    {
                                        active_status = "Y",
                                        Req_Approve_user = s.user_no + "",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.Nominating,
                                        PES_Nomination = item,

                                    }));
                                    if (lstApprove.Any()) { var sComplect = _PES_Nomination_SignaturesService.UpdateCommittee(lstApprove, item.Id, CGlobal.UserInfo.UserId, (int)PESClass.SignaturesStep.Nominating, dNow); }

                                }
                            }
                            else
                            {
                                if (_getCommittee != null && _getCommittee.Any())
                                {
                                    List<PES_Nomination_Signatures> lstApprove = new List<PES_Nomination_Signatures>();

                                    lstApprove.AddRange(_getCommittee.Select(s => new PES_Nomination_Signatures
                                    {
                                        active_status = "Y",
                                        Req_Approve_user = s.user_no + "",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.Nominating,
                                        PES_Nomination = item,

                                    }));
                                    if (lstApprove.Any())
                                    { var sComplect = _PES_Nomination_SignaturesService.UpdateCommittee(lstApprove, item.Id, CGlobal.UserInfo.UserId, (int)PESClass.SignaturesStep.Nominating, dNow); }
                                }

                            }
                        }
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                    var _getEditList = _PES_Nomination_YearService.Find(nId);
                    if (_getEditList != null)
                    {
                        var _getYearRate = _PES_Final_Rating_YearService.GetDataForSelect();
                        var empNO = _getData.PES_Nomination.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                        result.lstNewData = (from lstAD in _getData.PES_Nomination.Where(a => a.active_status == "Y")
                                             from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                             select new vPartnerKPIs_obj
                                             {
                                                 codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                                 id = lstAD.Id + "",
                                                 emp_name = lstEmpReq.EmpFullName,
                                                 emp_comp = lstEmpReq.CompanyCode,
                                                 emp_group = lstEmpReq.UnitGroupName,
                                                 emp_code = lstEmpReq.EmpNo,
                                                 emp_rank = lstEmpReq.RankCode,
                                                 lstApproval = PESFunc.GetSignatureName(lstAD.PES_Nomination_Signatures.Where(w => w.active_status == "Y").ToList()),
                                                 sponsering_id = lstAD.PES_Nomination_Signatures.Where(w => w.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Sponsoring_Partner && w.active_status == "Y").Select(s => s.Req_Approve_user).FirstOrDefault(),
                                                 Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditRating('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                 lstRating = PESFunc.GetRatingYear(_getYearRate.ToList(), lstAD.user_no),
                                                 emp_dec = lstAD.TM_PES_NMN_Type != null ? lstAD.TM_PES_NMN_Type.type_name_en : "",
                                             }).ToList();

                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult EditMNMMailBox(vNMNAdmin_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vNominationForm_Return result = new vNominationForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PES_Nomination_YearService.Find(nId);
                    if (_getData != null)
                    {


                        if (ItemData.lstNewData != null && ItemData.lstNewData.Any())
                        {
                            foreach (var item in ItemData.lstNewData)
                            {
                                int PtrID = SystemFunction.GetIntNullToZero(item.id);
                                var _getSignature = _PES_Nomination_SignaturesService.FindForEditSponsoring(PtrID, (int)PESClass.SignaturesStep.Sponsoring_Partner);
                                if (_getSignature != null)
                                {
                                    _getSignature.update_user = CGlobal.UserInfo.UserId;
                                    _getSignature.update_date = dNow;
                                    _getSignature.Req_Approve_user = item.sponsering_id + "";
                                    _getSignature.responses = "";
                                    _getSignature.Approve_status = "";
                                    _getSignature.Approve_user = "";

                                    _PES_Nomination_SignaturesService.Update(_getSignature);

                                }
                            }
                        }
                        var setform = (List<PES_Nomination_KPIs>)Session["ssKPI"];
                        if (setform != null)
                        {
                            var groupuser = setform.GroupBy(g => g.user_id).ToList();
                            foreach (var u in groupuser)
                            {
                                var getbyuser = setform.Where(w => w.user_id == u.Key).ToList();
                                var groupyear = getbyuser.GroupBy(g => g.PES_Final_Rating_Year).ToList();
                                foreach (var y in groupyear)
                                {

                                    //var _getRateYCur = _PES_Final_RatingService.FindForYearRate(getbyuser.FirstOrDefault().user_id , y.Key.evaluation_year.Value.Year);
                                    ////int nSelfRate = SystemFunction.GetIntNullToZero(ItemData.yearcurrent_selfrate + "");
                                    //if (_getRateYCur != null)
                                    //{
                                    //    //_getRateYCur.Self_TM_Annual_Rating_Id = nSelfRate;
                                    //    _getRateYCur.update_user = CGlobal.UserInfo.UserId;
                                    //    _getRateYCur.update_date = dNow;
                                    //    _PES_Final_RatingService.Update(_getRateYCur);

                                    //}


                                    var get_rating = _PES_Final_Rating_YearService.FindByYear(y.Key.evaluation_year.Value.Year);
                                    var getbyyear = getbyuser.Where(w => w.PES_Final_Rating_Year == y.Key).ToList();
                                    var update = _PES_Nomination_KPIsService.ImportKPIs(getbyyear, y.Key.Id, u.Key, CGlobal.UserInfo.UserId, dNow, get_rating);

                                }

                            }
                        }
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Request Type Not Found.";
                    }
                }
            }

            return Json(new
            {
                result
            });
        }


        [HttpPost]
        public ActionResult LoadNMNAdminList(CSearchPTRMailBox SearchItem)
        {
            vNMNAdmin_Return result = new vNMNAdmin_Return();
            List<vNMNAdmin_obj> lstData_resutl = new List<vNMNAdmin_obj>();
            var lstData = _PES_Nomination_YearService.GetCRank(
            "",
            "");
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            if (lstData.Any())
            {

                lstData_resutl = (from lstAD in lstData
                                  select new vNMNAdmin_obj
                                  {
                                      name_en = lstAD.evaluation_year.HasValue ? lstAD.evaluation_year.Value.DateTimeWithTimebyCulture("yyyy") : "",
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();

                string[] UserAdmin = WebConfigurationManager.AppSettings["PESSubAdmin"].Split(';');
                if (UserAdmin.Length > 0 && UserAdmin.Contains(CGlobal.UserInfo.UserId))
                { lstData_resutl = lstData_resutl.Where(w => w.name_en == DateTime.Now.Year.ToString()).ToList(); }

                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }

        [HttpPost]
        public ActionResult CreateNominationYear(vNMNAdmin_obj_save ItemData)
        {
            vNMNAdmin_Return result = new vNMNAdmin_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }

            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.action_date))
                {
                    DateTime? dAction = null;

                    if (!string.IsNullOrEmpty(ItemData.action_date))
                    {
                        dAction = SystemFunction.ConvertStringToDateTime("01-Jan-" + (ItemData.action_date).Trim(), "");
                    }
                    PES_Nomination_Year objSave = new PES_Nomination_Year()
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        evaluation_year = dAction,

                    };
                    if (_PES_Nomination_YearService.CanSave(objSave))
                    {
                        var sComplect = _PES_Nomination_YearService.CreateNew(ref objSave);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            result.IdEncrypt = HCMFunc.EncryptPES(objSave.Id + "");

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Duplicate;
                        result.Msg = "Duplicate Year.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please enter name";
                }
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult AddNominationForm(vNMNAdmin_Return ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vAddEvaluationPlan_Return result = new vAddEvaluationPlan_Return();
            List<vPartner_obj> lstNewData = new List<vPartner_obj>();
            if (ItemData != null && ItemData.lstData != null && ItemData.lstData.Any())
            {
                int[] aStatus = new int[] { 1, 3 };
                string[] aRank = new string[] { "12", "20", "32" };
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                DateTime dNow = DateTime.Now;



                if (nId != 0)
                {
                    var _GetStatus = _TM_PES_NMN_StatusService.Find((int)PESClass.Nomination_Status.Draft_Form);
                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }
                    var _GetFY = _PES_Nomination_YearService.Find(nId);
                    var _GetApproveStep = _TM_PES_NMN_SignatureStepService.GetDataForSelect();
                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                    if (_GetFY != null && _GetApproveStep.Any())
                    {

                        List<int> lstSignature = new List<int>();
                        //lstPlan.Add((int)StepApprovePlan.Self);
                        //lstSignature.Add((int)PESClass.SignaturesStep.Sponsoring_Partner);
                        lstSignature.Add((int)PESClass.SignaturesStep.Group_Head);
                        lstSignature.Add((int)PESClass.SignaturesStep.HOP);
                        lstSignature.Add((int)PESClass.SignaturesStep.Risk_Management);
                        lstSignature.Add((int)PESClass.SignaturesStep.COO);
                        if (_GetFY.evaluation_year.Value.Year != 2020 && _GetFY.evaluation_year.Value.Year != 2021)
                        {
                            lstSignature.Add((int)PESClass.SignaturesStep.Deputy_Ceo);
                        }
                        lstSignature.Add((int)PESClass.SignaturesStep.Ceo);
                        //_GetApproveStep = _GetApproveStep.Where(w => lstPlan.Contains(w.Id)).ToList();

                        foreach (var item in ItemData.lstData)
                        {
                            string user_no = HCMFunc.DecryptPES(item.IdEncrypt + "");
                            var _checkActiv = dbHr.AllInfo_WS.Where(w => aRank.Contains(w.RankID) && aStatus.Contains((int)w.Status) && w.EmpNo == user_no).FirstOrDefault();
                            if (_checkActiv != null)
                            {

                                List<PES_Nomination_Signatures> lstApprove = new List<PES_Nomination_Signatures>();
                                lstApprove.Add(new PES_Nomination_Signatures
                                {
                                    active_status = "Y",
                                    Req_Approve_user = _checkActiv.EmpNo + "",
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.Self,

                                });
                                //add พี่ปุยเป็น Sponsoring_Partner ก่อน
                                lstApprove.Add(new PES_Nomination_Signatures
                                {
                                    active_status = "Y",
                                    Req_Approve_user = PESClass.KhunSithakarn + "",
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.Sponsoring_Partner,

                                });

                                #region Add Approve Step 
                                var CheckGroupH = dbHr.tbMaster_UnitGroupHead.Where(w => w.EmployeeNo == _checkActiv.EmpNo && w.RankID == 1).FirstOrDefault();
                                var CheckPool = dbHr.tbMaster_PoolHead.Where(w => w.EmployeeNo == _checkActiv.EmpNo).FirstOrDefault();
                                var CheckCEO = dbHr.tbMaster_CompanyHead.Where(w => w.EmployeeNo == _checkActiv.EmpNo).FirstOrDefault();
                                foreach (var app in _GetApproveStep.Where(w => lstSignature.Contains(w.Id)).OrderBy(o => o.seq))
                                {
                                    if (app.Id == (int)PESClass.SignaturesStep.Group_Head)
                                    {
                                        if (CheckGroupH == null && CheckPool == null && CheckCEO == null)
                                        {
                                            var _getGroupHead = dbHr.tbMaster_UnitGroupHead.Where(w => w.UnitGroupID == _checkActiv.UnitGroupID && w.RankID == 1 && w.EndDate > DateTime.Now).FirstOrDefault();
                                            if (_getGroupHead != null)
                                            {
                                                lstApprove.Add(new PES_Nomination_Signatures
                                                {
                                                    active_status = "Y",
                                                    Req_Approve_user = _getGroupHead.EmployeeNo + "",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.Group_Head,
                                                });
                                            }

                                        }
                                    }
                                    else if (app.Id == (int)PESClass.SignaturesStep.HOP)
                                    {

                                        if (CheckPool == null && CheckCEO == null)
                                        {
                                            var _getPoolHead = dbHr.tbMaster_PoolHead.Where(w => w.PoolID == _checkActiv.PoolID && w.EndDate > DateTime.Now).FirstOrDefault();
                                            if (_getPoolHead != null)
                                            {
                                                lstApprove.Add(new PES_Nomination_Signatures
                                                {
                                                    active_status = "Y",
                                                    Req_Approve_user = _getPoolHead.EmployeeNo + "",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.HOP,
                                                });
                                            }

                                        }
                                    }
                                    else if (app.Id == (int)PESClass.SignaturesStep.Ceo)
                                    {

                                        if (CheckCEO == null)
                                        {
                                            var _getCEOHead = dbHr.tbMaster_CompanyHead.Where(w => w.tbMaster_Company.LocalCompCode == _checkActiv.CompanyCode && w.EndDate > DateTime.Now).FirstOrDefault();
                                            if (_getCEOHead != null)
                                            {
                                                lstApprove.Add(new PES_Nomination_Signatures
                                                {
                                                    active_status = "Y",
                                                    Req_Approve_user = _getCEOHead.EmployeeNo + "",
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.Ceo,
                                                });
                                            }
                                        }
                                    }
                                    else if (app.Id == (int)PESClass.SignaturesStep.COO)
                                    {

                                        //add COO
                                        lstApprove.Add(new PES_Nomination_Signatures
                                        {
                                            active_status = "Y",
                                            Req_Approve_user = PESClass.COO + "",
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.COO,

                                        });

                                    }
                                    else if (app.Id == (int)PESClass.SignaturesStep.Risk_Management)
                                    {
                                        //add Risk_Management khun 
                                        lstApprove.Add(new PES_Nomination_Signatures
                                        {
                                            active_status = "Y",
                                            Req_Approve_user = PESClass.Risk_Management + "",
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.Risk_Management,
                                        });
                                    }
                                    else if (app.Id == (int)PESClass.SignaturesStep.Deputy_Ceo)
                                    {
                                        //add Deputy_Ceo
                                        lstApprove.Add(new PES_Nomination_Signatures
                                        {
                                            active_status = "Y",
                                            Req_Approve_user = PESClass.Deputy_Ceo + "",
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            TM_PES_NMN_SignatureStep_Id = (int)PESClass.SignaturesStep.Deputy_Ceo,
                                        });
                                    }
                                }
                                #endregion

                                PES_Nomination objSave = new PES_Nomination()
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    active_status = "Y",
                                    PES_Nomination_Year = _GetFY,
                                    user_id = _checkActiv.UserID,
                                    user_no = _checkActiv.EmpNo,
                                    PES_Nomination_Signatures = lstApprove.ToList(),
                                    TM_PES_NMN_Type_Id = _checkActiv.RankID == "20" ? (int)PESClass.Nomination_Type.Partner : (_checkActiv.RankID == "12" ? (int)PESClass.Nomination_Type.Shareholder : (int)PESClass.Nomination_Type.Director),
                                    TM_PES_NMN_Status = _GetStatus
                                };
                                if (_PES_NominationService.CanSave(objSave))
                                {
                                    var sComplect = _PES_NominationService.CreateNewOrUpdate(objSave);
                                    if (sComplect > 0)
                                    {
                                        //var _getUpdateMail = _PES_Nomination_YearService.Find(nId);
                                        //string[] empNO = new string[] { };

                                        //if (_getUpdateMail != null && _getUpdateMail.PES_Nomination.Any(a => a.active_status == "Y"))
                                        //{
                                        //    empNO = _getUpdateMail.PES_Nomination.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                                        //    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();

                                        //    var _getYearRate = _PES_Final_Rating_YearService.GetDataForSelect();
                                        //    var _getSponser = dbHr.AllInfo_WS.Where(w => w.RankID == "12" && aStatus.Contains((int)w.Status)).ToList();
                                        //    if (_getSponser.Any())
                                        //    {
                                        //        result.lstsponsering = _getSponser.OrderBy(o => o.EmpFullName).Select(s => new lstDataSelect
                                        //        {
                                        //            value = s.EmpNo + "",
                                        //            text = s.EmpFullName + " (" + s.EmpNo + ")",
                                        //        }).ToList();
                                        //    }
                                        //    result.lstsponsering.Insert(0, new lstDataSelect { value = PESClass.KhunSithakarn + "", text = "Sithakarn Anuntasilpa (" + PESClass.KhunSithakarn + ")" });


                                        //    result.lstNewData = (from lstAD in _getUpdateMail.PES_Nomination.Where(a => a.active_status == "Y")
                                        //                         from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                        //                         select new vPartnerKPIs_obj
                                        //                         {
                                        //                             id = lstAD.Id + "",
                                        //                             codeEncrypt = HCMFunc.EncryptPES(lstAD.user_no + ""),
                                        //                             emp_name = lstEmpReq.EmpFullName,
                                        //                             emp_comp = lstEmpReq.CompanyCode,
                                        //                             emp_group = lstEmpReq.UnitGroupName,
                                        //                             emp_code = lstEmpReq.EmpNo,
                                        //                             emp_rank = lstEmpReq.RankCode,
                                        //                             lstApproval = PESFunc.GetSignatureName(lstAD.PES_Nomination_Signatures.Where(w => w.active_status == "Y").ToList()),
                                        //                             sponsering_id = lstAD.PES_Nomination_Signatures.Where(w => w.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Sponsoring_Partner && w.active_status == "Y").Select(s => s.Req_Approve_user).FirstOrDefault(),
                                        //                             Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditRating('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                        //                             lstRating = PESFunc.GetRatingYear(_getYearRate.ToList(), lstAD.user_no),
                                        //                             emp_dec = lstAD.TM_PES_NMN_Type != null ? lstAD.TM_PES_NMN_Type.type_name_en : "",

                                        //                         }).ToList();

                                        //    result.lstOldData = new List<vPartner_obj>();
                                        //    var _getPartner = dbHr.AllInfo_WS.Where(w => aRank.Contains(w.RankID) && aStatus.Contains((int)w.Status) && !empNO.Contains(w.EmpNo)).ToList();
                                        //    if (_getPartner.Any())
                                        //    {
                                        //        result.lstOldData = _getPartner.Select(s => new vPartner_obj
                                        //        {
                                        //            codeEncrypt = HCMFunc.EncryptPES(s.EmpNo + ""),
                                        //            Edit = "",
                                        //            emp_name = s.EmpFullName,
                                        //            emp_comp = s.CompanyCode,
                                        //            emp_group = s.UnitGroupName,
                                        //            emp_code = s.EmpNo,
                                        //            emp_rank = s.RankCode,
                                        //        }).ToList();
                                        //    }
                                        //}

                                    }
                                    else
                                    {
                                        //result.Status = SystemFunction.process_Failed;
                                        //result.Msg = "Error, please try again.";
                                    }

                                }
                                else
                                {
                                    //result.Status = SystemFunction.process_Duplicate;
                                    //result.Msg = "Duplicate Type name.";
                                }
                                result.Status = SystemFunction.process_Success;
                                result.IdEncrypt = HCMFunc.EncryptPES(objSave.Id + "");


                            }
                        }
                    }
                }

                result.Status = SystemFunction.process_Success;
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select employee.";
            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult CreateUserNomination(vNominationForm_lst_Authorized ItemData)
        {
            objNominationForm_lst_Authorized_Return result = new objNominationForm_lst_Authorized_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vNominationForm_lst_Authorized>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));

                    if (nId != 0)
                    {
                        var _getData = _PES_Nomination_YearService.Find(nId);
                        if (_getData != null)
                        {
                            string[] aRank = new string[] { "12" };
                            var checkActive = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && aRank.Contains(w.RankID) && w.EmpNo == ItemData.authorized_user).FirstOrDefault();
                            if (checkActive != null)
                            {
                                PES_Nomination_Default_Committee objSave = new PES_Nomination_Default_Committee()
                                {
                                    //Id = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.id + "")),
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    PES_Nomination_Year = _getData,
                                    active_status = "Y",
                                    user_no = ItemData.authorized_user,
                                    committee_type = "N"

                                };
                                if (_PES_Nomination_Default_CommitteeService.CanSave(objSave))
                                {
                                    var sComplect = _PES_Nomination_Default_CommitteeService.CreateNewOrUpdate(objSave);
                                    if (sComplect > 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        var _getEditList = _PES_Nomination_YearService.Find(nId);
                                        if (_getEditList != null)
                                        {
                                            if (_getEditList.PES_Nomination_Default_Committee != null && _getEditList.PES_Nomination_Default_Committee.Any(a => a.active_status == "Y" && a.committee_type == "N"))
                                            {
                                                string[] empNO = _getEditList.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                                                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                                result.lstData = (from lst in _getEditList.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y" && a.committee_type == "N")
                                                                  from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                  select new vNominationForm_lst_Authorized
                                                                  {
                                                                      authorized_name = lstEmpReq.EmpFullName,
                                                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                      authorized_rank = lstEmpReq.Rank + "",
                                                                      authorized_user = lst.user_no + "",
                                                                  }).ToList();
                                            }
                                        }
                                        else
                                        {
                                            result.lstData = new List<vNominationForm_lst_Authorized>();
                                        }

                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, please try again.";
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Duplicate;
                                    result.Msg = "Duplicate Employee name.";
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Employee Not Found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Rating Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation not found";
                    }

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please select rating.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult DeleteUserNomination(vNominationForm_lst_Authorized ItemData)
        {
            objNominationForm_lst_Authorized_Return result = new objNominationForm_lst_Authorized_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vNominationForm_lst_Authorized>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));

                    if (nId != 0)
                    {
                        var _getData = _PES_Nomination_Default_CommitteeService.Find(nId);
                        if (_getData != null)
                        {
                            int EvaID = _getData.PES_Nomination_Year.Id;

                            _getData.active_status = "N";
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            var sComplect = _PES_Nomination_Default_CommitteeService.CreateNewOrUpdate(_getData);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _PES_Nomination_YearService.Find(EvaID);
                                if (_getEditList != null)
                                {
                                    if (_getEditList.PES_Nomination_Default_Committee != null && _getEditList.PES_Nomination_Default_Committee.Any(a => a.active_status == "Y"))
                                    {
                                        string[] empNO = _getEditList.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                        result.lstData = (from lst in _getEditList.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y")
                                                          from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                          select new vNominationForm_lst_Authorized
                                                          {
                                                              authorized_name = lstEmpReq.EmpFullName,
                                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                              authorized_rank = lstEmpReq.Rank + "",
                                                              authorized_user = lst.user_no + "",
                                                          }).ToList();
                                    }
                                }
                                else
                                {
                                    result.lstData = new List<vNominationForm_lst_Authorized>();
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }



                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Rating Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation not found";
                    }

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please select rating.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult CreateUserShareholder(vNominationForm_lst_Authorized ItemData)
        {
            objNominationForm_lst_Authorized_Return result = new objNominationForm_lst_Authorized_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vNominationForm_lst_Authorized>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));

                    if (nId != 0)
                    {
                        var _getData = _PES_Nomination_YearService.Find(nId);
                        if (_getData != null)
                        {
                            string[] aRank = new string[] { "12" };
                            var checkActive = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && aRank.Contains(w.RankID) && w.EmpNo == ItemData.authorized_user).FirstOrDefault();
                            if (checkActive != null)
                            {
                                PES_Nomination_Default_Committee objSave = new PES_Nomination_Default_Committee()
                                {
                                    //Id = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.id + "")),
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    PES_Nomination_Year = _getData,
                                    active_status = "Y",
                                    user_no = ItemData.authorized_user,
                                    committee_type = "S"

                                };
                                if (_PES_Nomination_Default_CommitteeService.CanSave(objSave))
                                {
                                    var sComplect = _PES_Nomination_Default_CommitteeService.CreateNewOrUpdate(objSave);
                                    if (sComplect > 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        var _getEditList = _PES_Nomination_YearService.Find(nId);
                                        if (_getEditList != null)
                                        {
                                            if (_getEditList.PES_Nomination_Default_Committee != null && _getEditList.PES_Nomination_Default_Committee.Any(a => a.active_status == "Y" && a.committee_type == "S"))
                                            {
                                                string[] empNO = _getEditList.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                                                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                                result.lstData = (from lst in _getEditList.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y" && a.committee_type == "S")
                                                                  from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                  select new vNominationForm_lst_Authorized
                                                                  {
                                                                      authorized_name = lstEmpReq.EmpFullName,
                                                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                      authorized_rank = lstEmpReq.Rank + "",
                                                                      authorized_user = lst.user_no + "",
                                                                  }).ToList();
                                            }
                                        }
                                        else
                                        {
                                            result.lstData = new List<vNominationForm_lst_Authorized>();
                                        }

                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, please try again.";
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Duplicate;
                                    result.Msg = "Duplicate Employee name.";
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Employee Not Found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Rating Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation not found";
                    }

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please select rating.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult CreateNominationYearRating(vNMNAdmin_obj_save ItemData)
        {
            vNMNAdmin_Return result = new vNMNAdmin_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }

            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.action_date))
                {
                    DateTime? dAction = null;

                    if (!string.IsNullOrEmpty(ItemData.action_date))
                    {
                        dAction = SystemFunction.ConvertStringToDateTime("01-Jan-" + (ItemData.action_date).Trim(), "");
                    }
                    PES_Final_Rating_Year objSave = new PES_Final_Rating_Year()
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        evaluation_year = dAction,

                    };
                    if (_PES_Final_Rating_YearService.CanSave(objSave))
                    {
                        var sComplect = _PES_Final_Rating_YearService.CreateNew(ref objSave);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            result.IdEncrypt = HCMFunc.EncryptPES(objSave.Id + "");

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Duplicate;
                        result.Msg = "Duplicate Year.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please enter name";
                }
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult EditRatingNC(vNominationForm_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vAddEvaluationPlan_Return result = new vAddEvaluationPlan_Return();
            List<vPartner_obj> lstNewData = new List<vPartner_obj>();
            if (ItemData != null)
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                DateTime dNow = DateTime.Now;
                if (nId != 0)
                {
                    var _getData = _PES_NominationService.Find(nId);
                    if (_getData != null && ItemData.lstAnnul_Rate != null)
                    {
                        foreach (var item in ItemData.lstAnnul_Rate)
                        {
                            int nRate = SystemFunction.GetIntNullToZero(item.rate_id);
                            var _getYear = _PES_Final_Rating_YearService.Find(SystemFunction.GetIntNullToZero(item.id + ""));
                            if (_getYear != null)
                            {
                                if (nRate > 0)
                                {
                                    PES_Final_Rating objSave = new PES_Final_Rating()
                                    {
                                        active_status = "Y",
                                        user_no = _getData.user_no + "",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        Final_TM_Annual_Rating_Id = nRate,
                                        PES_Final_Rating_Year = _getYear

                                    };
                                    var xx = _PES_Final_RatingService.CreateNewOrUpdate(objSave);

                                }
                            }


                        }
                    }

                }

                result.Status = SystemFunction.process_Success;
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select employee.";
            }

            return Json(new
            {
                result
            });
        }


        [HttpPost]
        public ActionResult UploadFileBypass()
        {

            vImport_NominationForm_KPIs result = new vImport_NominationForm_KPIs();
            File_Upload_Can objFile = new File_Upload_Can();

            var dNow = DateTime.Now;

            var count_can_map = 0;
            List<vNominationForm_obj> new_lst_timesheet_form = new List<vNominationForm_obj>();


            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }

            string IdEncrypt = Request.Form["IdEncrypt"];
            int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(IdEncrypt + ""));
            if (nId != 0)
            {
                var _getData = _PES_Nomination_YearService.Find(nId);


                if (Request != null)
                {
                    string sCDMap = Request.Form["sCDMap"];
                    string ssKPI = Request.Form["ssKPI"];

                    objFile = Session[sCDMap] as File_Upload_Can;
                    if (objFile == null)
                    {
                        objFile = new File_Upload_Can();
                    }

                    if (Request.Files.Count > 0)
                    {

                        foreach (string file in Request.Files)
                        {
                            var fileContent = Request.Files[file];
                            if (fileContent != null && fileContent.ContentLength > 0)
                            {
                                // get a stream
                                var stream = fileContent.InputStream;
                                // and optionally write the file to disk
                                var fileName = Path.GetFileName(file);
                                var gererror = "";
                                try
                                {
                                    using (var package = new ExcelPackage(stream))
                                    {
                                        //var ws = package.Workbook.Worksheets.First();
                                        var ws = package.Workbook.Worksheets.First();
                                        // objFile.sfile64 = package.GetAsByteArray();
                                        objFile.sfile_name = fileName;
                                        objFile.sfileType = Path.GetExtension(fileName).ToLower() + "";

                                        DataTable tbl = new DataTable();
                                        // List<string> lstHead = new List<string>();
                                        var testget = ws.Cells[1, 1, 1, ws.Dimension.End.Column].Value;
                                        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                        {
                                            tbl.Columns.Add(true ? (firstRowCell.Text + "").Trim().ToLower() : string.Format("Column {0}", firstRowCell.Start.Column));
                                            //lstHead.Add((firstRowCell.Text + "").Trim().ToLower());
                                        }
                                        var startRow = true ? 2 : 1;
                                        for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                        {
                                            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];


                                            DataRow row = tbl.Rows.Add();
                                            foreach (var cell in wsRow)
                                            {

                                                if (row[cell.Start.Column - 1] == null)
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    row[cell.Start.Column - 1] = cell.Text;
                                                }
                                            }
                                        }
                                        if (tbl != null)
                                        {
                                            if (tbl.Columns.Contains("Emp")

                                                )
                                            {

                                                //var getcan = _TM_CandidatesService.GetDataForSelect().Where(w => w.candidate_TraineeNumber == tbl.Columns.Contains("Trainee Code") + "").ToList();
                                                var getrate = _TM_KPIs_BaseService.GetDataForSelect();
                                                List<PES_Nomination_KPIs> lstkpi = new List<PES_Nomination_KPIs>();
                                                foreach (var row in tbl.AsEnumerable())
                                                {


                                                    var nowyear = _getData.evaluation_year.Value;
                                                    for (var lstyear = DateTime.Now.AddYears(-3).Year; lstyear <= nowyear.Year; nowyear = nowyear.AddYears(-1))
                                                    {



                                                        foreach (var rate in getrate)
                                                        {
                                                            PES_Nomination_KPIs kpi = new PES_Nomination_KPIs();
                                                            var Emp = tbl.Columns.Contains("Emp") ? Convert.ToInt64(row.Field<string>("Emp")).ToString("00000000") : "";
                                                            var Budget = tbl.Columns.Contains(nowyear.ToString("yy") + " Budget") ? row.Field<string>(String.Format("{0:yy}", nowyear) + " Budget") : "";
                                                            var Total_Fee_Managed = tbl.Columns.Contains(String.Format("{0:yy}", nowyear) + " Total Fee Managed") ? row.Field<string>(String.Format("{0:yy}", nowyear) + " Total Fee Managed") : "";
                                                            var Contribution_Margin = tbl.Columns.Contains(String.Format("{0:yy}", nowyear) + " Contribution margin") ? row.Field<string>(String.Format("{0:yy}", nowyear) + " Contribution margin") : "";
                                                            var Recovery = tbl.Columns.Contains(String.Format("{0:yy}", nowyear) + " Recovery") ? row.Field<string>(String.Format("{0:yy}", nowyear) + " Recovery") : "";
                                                            var Lockup_Days = tbl.Columns.Contains(String.Format("{0:yy}", nowyear) + " Lockup Days") ? row.Field<string>(String.Format("{0:yy}", nowyear) + " Lockup Days") : "";
                                                            var Chg = tbl.Columns.Contains(String.Format("{0:yy}", nowyear) + " Chg") ? row.Field<string>(String.Format("{0:yy}", nowyear) + " Chg") : "";
                                                            var Billing = tbl.Columns.Contains(String.Format("{0:yy}", nowyear) + " Billing") ? row.Field<string>(String.Format("{0:yy}", nowyear) + " Billing") : "";
                                                            var Collection = tbl.Columns.Contains(String.Format("{0:yy}", nowyear) + " Collection") ? row.Field<string>(String.Format("{0:yy}", nowyear) + " Collection") : "";

                                                            gererror = Emp + "<" + nowyear.Year + "<" + rate.kpi_name_en;

                                                            var get_emp = wsHRis.getEmployeeInfoByEmpNo(Emp);


                                                            kpi.user_id = Emp;
                                                            if (rate.Id == 1)
                                                            {
                                                                kpi.target = HCMFunc.DataEncryptPES(Budget);
                                                                kpi.actual = HCMFunc.DataEncryptPES(Math.Round(Convert.ToDecimal(Total_Fee_Managed) / 1000000, 2).ToString());
                                                            }
                                                            if (rate.Id == 2)
                                                            {
                                                                if (nowyear.Year == DateTime.Now.Year)
                                                                {
                                                                    kpi.target = HCMFunc.DataEncryptPES(rate.base_min.ToString() + "-" + rate.base_max.ToString() + "%");
                                                                }
                                                                else
                                                                {
                                                                    kpi.target = HCMFunc.DataEncryptPES("55-70%");
                                                                }

                                                                kpi.actual = HCMFunc.DataEncryptPES(Contribution_Margin);
                                                            }
                                                            if (rate.Id == 3)
                                                            {
                                                                if (nowyear.Year == DateTime.Now.Year)
                                                                {
                                                                    kpi.target = HCMFunc.DataEncryptPES(rate.base_min.ToString() + "-" + rate.base_max.ToString() + "%");
                                                                }
                                                                else
                                                                {
                                                                    kpi.target = HCMFunc.DataEncryptPES("43-60%");
                                                                }
                                                                kpi.actual = HCMFunc.DataEncryptPES(Recovery);
                                                            }
                                                            if (rate.Id == 4)
                                                            {
                                                                if (nowyear.Year == DateTime.Now.Year)
                                                                {
                                                                    kpi.target = HCMFunc.DataEncryptPES(rate.base_min.ToString() + "-" + rate.base_max.ToString());
                                                                }
                                                                else
                                                                {
                                                                    kpi.target = HCMFunc.DataEncryptPES("75");
                                                                }
                                                                kpi.actual = HCMFunc.DataEncryptPES(Lockup_Days);
                                                            }
                                                            if (rate.Id == 5)
                                                            {
                                                                //if (nowyear.Year == DateTime.Now.Year)
                                                                //{
                                                                //    kpi.target = HCMFunc.DataEncryptPES(rate.base_min.ToString() + "-" + rate.base_max.ToString() + "%");
                                                                //}
                                                                //else
                                                                //{
                                                                if (Emp == "00004991")
                                                                {
                                                                    var getaaa = "";
                                                                }
                                                                    var get_rank = get_emp.AsEnumerable().FirstOrDefault().Field<string>("RankID");
                                                                    var get_pool = get_emp.AsEnumerable().FirstOrDefault().Field<int>("PoolID");
                                                                    var get_unitgroup = get_emp.AsEnumerable().FirstOrDefault().Field<int>("UnitGroupID");

                                                                    var get_value = PESFunc.charge_Map(rate.Id.ToString(), get_rank, get_pool,get_unitgroup);

                                                                    kpi.target = HCMFunc.DataEncryptPES(get_value);
                                                                //}
                                                                kpi.actual = HCMFunc.DataEncryptPES(Chg);
                                                            }
                                                            if (rate.Id == 6)
                                                            {
                                                                kpi.target = HCMFunc.DataEncryptPES("0");

                                                                kpi.actual = HCMFunc.DataEncryptPES(Math.Round(Convert.ToDecimal(Billing) / 1000000, 2).ToString());
                                                            }
                                                            if (rate.Id == 7)
                                                            {
                                                                kpi.target = HCMFunc.DataEncryptPES("0");

                                                                kpi.actual = HCMFunc.DataEncryptPES(Math.Round(Convert.ToDecimal(Collection) / 1000000, 2).ToString());
                                                            }
                                                            //kpi.TM_KPIs_Base = _TM_KPIs_BaseService.Find(rate.Id);
                                                            kpi.TM_KPIs_Base_Id = _TM_KPIs_BaseService.Find(rate.Id).Id;


                                                            //var CurrentYear = _getData.evaluation_year;
                                                            //var _getTheeYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.AddYears(-3).Year);
                                                            //var _getTwoYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.AddYears(-2).Year);
                                                            //var _getOneYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.AddYears(-1).Year);
                                                            //var _getCurYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.Year);

                                                            kpi.PES_Final_Rating_Year = _PES_Final_Rating_YearService.FindByYear(nowyear.Year);
                                                            //kpi.PES_Final_Rating_Year = _PES_Final_Rating_YearService.FindByYear(nowyear.Year).Id;



                                                            kpi.active_status = "Y";
                                                            kpi.update_user = CGlobal.UserInfo.UserId;
                                                            kpi.update_date = dNow;
                                                            kpi.create_user = CGlobal.UserInfo.UserId;
                                                            kpi.create_date = dNow;

                                                            lstkpi.Add(kpi);


                                                        }
                                                    }
                                                }



                                                Session["ssKPI"] = null;
                                                Session["ssKPI"] = lstkpi.ToList();
                                                result.Status = SystemFunction.process_Success;

                                            }
                                            else
                                            {
                                                result.Status = SystemFunction.process_Failed;
                                                result.Msg = "Incorrect template, please try again.";
                                            }
                                            //end of test import file

                                        }
                                    }



                                }
                                catch (Exception e)
                                {
                                    result.Msg = gererror + e.Message + "";
                                    result.Status = SystemFunction.process_Failed;
                                }
                            }
                        }



                    }
                    else
                    {

                    }
                }
            }
            return Json(new { result });

        }

    }



    #endregion

}