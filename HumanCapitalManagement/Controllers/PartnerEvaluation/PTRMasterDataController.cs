using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.PartnerEvaluation;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
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

namespace HumanCapitalManagement.Controllers.PartnerEvaluation
{
    public class PTRMasterDataController : BaseController
    {
        private PTR_Evaluation_YearService _PTR_Evaluation_YearService;
        private PTR_EvaluationService _PTR_EvaluationService;
        private TM_PTR_Eva_ApproveStepService _TM_PTR_Eva_ApproveStepService;
        private TM_KPIs_BaseService _TM_KPIs_BaseService;
        private PTR_Evaluation_KPIsService _PTR_Evaluation_KPIsService;
        private PTR_Evaluation_FileService _PTR_Evaluation_FileService;
        private TM_PTR_Eva_StatusService _TM_PTR_Eva_StatusService;
        private TM_Partner_EvaluationService _TM_Partner_EvaluationService;
        private PTR_Evaluation_ApproveService _PTR_Evaluation_ApproveService;
        private TM_Annual_RatingService _TM_Annual_RatingService;
        private PTR_Evaluation_AnswerService _PTR_Evaluation_AnswerService;
        private TM_Feedback_RatingService _TM_Feedback_RatingService;
        private PTR_Feedback_EmpService _PTR_Feedback_EmpService;
        private PTR_Feedback_UnitGroupService _PTR_Feedback_UnitGroupService;
        private PTR_Evaluation_AuthorizedService _PTR_Evaluation_AuthorizedService;
        private PTR_Evaluation_AuthorizedEvaService _PTR_Evaluation_AuthorizedEvaService;

        //nomination
        private PES_NominationService _PES_NominationService;
        private PES_Nomination_Default_CommitteeService _PES_Nomination_Default_CommitteeService;
        private PES_Nomination_YearService _PES_Nomination_YearService;
        private PES_Final_Rating_YearService _PES_Final_Rating_YearService;

        New_HRISEntities dbHr = new New_HRISEntities();
        public PTRMasterDataController(PTR_Evaluation_YearService PTR_Evaluation_YearService
            , PTR_EvaluationService PTR_EvaluationService
            , TM_PTR_Eva_ApproveStepService TM_PTR_Eva_ApproveStepService
            , TM_KPIs_BaseService TM_KPIs_BaseService
            , PTR_Evaluation_KPIsService PTR_Evaluation_KPIsService
            , PTR_Evaluation_FileService PTR_Evaluation_FileService
            , TM_PTR_Eva_StatusService TM_PTR_Eva_StatusService
            , TM_Partner_EvaluationService TM_Partner_EvaluationService
            , PTR_Evaluation_ApproveService PTR_Evaluation_ApproveService
            , TM_Annual_RatingService TM_Annual_RatingService
            , PTR_Evaluation_AnswerService PTR_Evaluation_AnswerService
            , TM_Feedback_RatingService TM_Feedback_RatingService
            , PTR_Feedback_EmpService PTR_Feedback_EmpService
            , PTR_Feedback_UnitGroupService PTR_Feedback_UnitGroupService
            , PTR_Evaluation_AuthorizedService PTR_Evaluation_AuthorizedService
            , PTR_Evaluation_AuthorizedEvaService PTR_Evaluation_AuthorizedEvaService
            , PES_NominationService PES_NominationService
            , PES_Nomination_Default_CommitteeService PES_Nomination_Default_CommitteeService
            , PES_Nomination_YearService PES_Nomination_YearService
            , PES_Final_Rating_YearService PES_Final_Rating_YearService
            )
        {
            _PTR_Evaluation_YearService = PTR_Evaluation_YearService;
            _PTR_EvaluationService = PTR_EvaluationService;
            _TM_PTR_Eva_ApproveStepService = TM_PTR_Eva_ApproveStepService;
            _TM_KPIs_BaseService = TM_KPIs_BaseService;
            _PTR_Evaluation_KPIsService = PTR_Evaluation_KPIsService;
            _PTR_Evaluation_FileService = PTR_Evaluation_FileService;
            _TM_PTR_Eva_StatusService = TM_PTR_Eva_StatusService;
            _TM_Partner_EvaluationService = TM_Partner_EvaluationService;
            _PTR_Evaluation_ApproveService = PTR_Evaluation_ApproveService;
            _TM_Annual_RatingService = TM_Annual_RatingService;
            _PTR_Evaluation_AnswerService = PTR_Evaluation_AnswerService;
            _TM_Feedback_RatingService = TM_Feedback_RatingService;
            _PTR_Feedback_EmpService = PTR_Feedback_EmpService;
            _PTR_Feedback_UnitGroupService = PTR_Feedback_UnitGroupService;
            _PTR_Evaluation_AuthorizedService = PTR_Evaluation_AuthorizedService;
            _PTR_Evaluation_AuthorizedEvaService = PTR_Evaluation_AuthorizedEvaService;
            _PES_NominationService = PES_NominationService;
            _PES_Nomination_Default_CommitteeService = PES_Nomination_Default_CommitteeService;
            _PES_Nomination_YearService = PES_Nomination_YearService;
            _PES_Final_Rating_YearService = PES_Final_Rating_YearService;
        }
        // GET: PTRMasterData
        public ActionResult PTRMasterDataList(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPTRMasterData result = new vPTRMasterData();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPTRMasterData SearchItem = (CSearchPTRMasterData)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPTRMasterData)));
                var lstData = _TM_Partner_EvaluationService.GetTIF_Form(
               SearchItem.name,
          "");
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.lstData = (from lstAD in lstData
                                      select new vPTRMasterData_obj
                                      {
                                          name_en = lstAD.action_date.HasValue ? lstAD.action_date.Value.DateTimeWithTimebyCulture() : "",
                                          //  short_name_en = lstAD.Company_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();
                }
            }

            #endregion
            return View(result);
        }
        public ActionResult PTRMasterDataCreate()
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPTRMasterData_obj_save result = new vPTRMasterData_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            result.action_date = DateTime.Now.ToString("yyyy");
            return View(result);
        }
        public ActionResult PTRMasterDataEdit(string id)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            return View();
        }
        public ActionResult PTRAuthorizedPerson(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            vPTREvaluation_obj_save result = new vPTREvaluation_obj_save();
            if (qryStr != "")
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                            if (_checkActiv != null)
                            {
                                var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                                result.IdEncrypt = qryStr;
                                result.code = _getData.user_no;
                                result.sname = _checkActiv.EmpFullName;
                                result.sgroup = _checkActiv.UnitGroupName;
                                result.srank = _checkActiv.Rank;
                                result.status_name = _getData.TM_PTR_Eva_Status != null ? _getData.TM_PTR_Eva_Status.status_name_en + "" : "";
                                result.status_id = _getData.TM_PTR_Eva_Status != null ? _getData.TM_PTR_Eva_Status.Id + "" : "";
                                result.yearcurrent = _getData.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy");
                                result.other_role = _getData.other_roles;

                                #region if else Data

                                #region Plaining Data
                                result.eva_mode = "Plan";


                                if (_getData.PTR_Evaluation_Authorized != null && _getData.PTR_Evaluation_Authorized.Any(a => a.active_status == "Y"))
                                {
                                    string[] empNO = _getData.PTR_Evaluation_Authorized.Where(a => a.active_status == "Y").Select(s => s.authorized_user).ToArray();
                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                    result.lstAuthorized = (from lst in _getData.PTR_Evaluation_Authorized.Where(a => a.active_status == "Y")
                                                            from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.authorized_user).DefaultIfEmpty(new AllInfo_WS())
                                                            select new vPTREvaluation_lst_Authorized
                                                            {
                                                                authorized_name = lstEmpReq.EmpFullName,
                                                                Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                authorized_rank = lstEmpReq.Rank + "",
                                                                authorized_user = lst.authorized_user + "",
                                                            }).ToList();
                                }


                                #endregion



                                #endregion

                                return PartialView("_PTRAuthorizedPerson", result);
                            }
                            else
                            {
                                return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                            }
                        }
                        else
                        {
                            return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                        }

                    }
                    else
                    {
                        return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                    }
                }
                else
                {
                    return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                }
            }
            else
            {
                return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
            }


        }
        public ActionResult PTRAuthorizedPersonEva(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            vPTREvaluation_obj_save result = new vPTREvaluation_obj_save();
            if (qryStr != "")
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                            if (_checkActiv != null)
                            {
                                var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                                result.IdEncrypt = qryStr;
                                result.code = _getData.user_no;
                                result.sname = _checkActiv.EmpFullName;
                                result.sgroup = _checkActiv.UnitGroupName;
                                result.srank = _checkActiv.Rank;
                                result.status_name = _getData.TM_PTR_Eva_Status != null ? _getData.TM_PTR_Eva_Status.status_name_en + "" : "";
                                result.status_id = _getData.TM_PTR_Eva_Status != null ? _getData.TM_PTR_Eva_Status.Id + "" : "";
                                result.yearcurrent = _getData.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy");
                                result.other_role = _getData.other_roles;

                                #region if else Data

                                #region Plaining Data
                                result.eva_mode = "Plan";


                                if (_getData.PTR_Evaluation_AuthorizedEva != null && _getData.PTR_Evaluation_AuthorizedEva.Any(a => a.active_status == "Y"))
                                {
                                    string[] empNO = _getData.PTR_Evaluation_AuthorizedEva.Where(a => a.active_status == "Y").Select(s => s.authorized_user).ToArray();
                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                    result.lstAuthorized = (from lst in _getData.PTR_Evaluation_AuthorizedEva.Where(a => a.active_status == "Y")
                                                            from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.authorized_user).DefaultIfEmpty(new AllInfo_WS())
                                                            select new vPTREvaluation_lst_Authorized
                                                            {
                                                                authorized_name = lstEmpReq.EmpFullName,
                                                                Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                authorized_rank = lstEmpReq.Rank + "",
                                                                authorized_user = lst.authorized_user + "",
                                                            }).ToList();
                                }


                                #endregion



                                #endregion

                                return PartialView("_PTRAuthorizedPersonEva", result);
                            }
                            else
                            {
                                return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                            }
                        }
                        else
                        {
                            return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                        }

                    }
                    else
                    {
                        return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                    }
                }
                else
                {
                    return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                }
            }
            else
            {
                return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
            }


        }

        public ActionResult PESNimonationCommittee(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            vPTREvaluation_obj_save result = new vPTREvaluation_obj_save();
            if (qryStr != "")
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PES_Nomination_YearService.Find(nId);
                    if (_getData != null)
                    {
                        result.sname = _getData.evaluation_year.AsDateTimeWithTimebyCulture("yyyy");
                        result.IdEncrypt = qryStr;




                        if (_getData.PES_Nomination_Default_Committee != null && _getData.PES_Nomination_Default_Committee.Any(a => a.active_status == "Y" && a.committee_type == "N"))
                        {
                            string[] empNO = _getData.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                            result.lstAuthorized = (from lst in _getData.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y" && a.committee_type == "N")
                                                    from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vPTREvaluation_lst_Authorized
                                                    {
                                                        authorized_name = lstEmpReq.EmpFullName,
                                                        Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                        authorized_rank = lstEmpReq.Rank + "",
                                                        authorized_user = lst.user_no + "",
                                                    }).ToList();
                        }



                        return PartialView("_PESNimonationCommittee", result);
                    }
                    else
                    {
                        return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                    }
                }
                else
                {
                    return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                }
            }
            else
            {
                return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
            }


        }

        public ActionResult PESShareholder(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            vPTREvaluation_obj_save result = new vPTREvaluation_obj_save();
            if (qryStr != "")
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PES_Nomination_YearService.Find(nId);
                    if (_getData != null)
                    {
                        result.sname = _getData.evaluation_year.AsDateTimeWithTimebyCulture("yyyy");
                        result.IdEncrypt = qryStr;

                        if (_getData.PES_Nomination_Default_Committee != null && _getData.PES_Nomination_Default_Committee.Any(a => a.active_status == "Y" && a.committee_type == "S"))
                        {
                            string[] empNO = _getData.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                            result.lstAuthorized = (from lst in _getData.PES_Nomination_Default_Committee.Where(a => a.active_status == "Y" && a.committee_type == "S")
                                                    from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vPTREvaluation_lst_Authorized
                                                    {
                                                        authorized_name = lstEmpReq.EmpFullName,
                                                        Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                        authorized_rank = lstEmpReq.Rank + "",
                                                        authorized_user = lst.user_no + "",
                                                    }).ToList();
                        }
                        return PartialView("_PESShareholder", result);
                    }
                    else
                    {
                        return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                    }
                }
                else
                {
                    return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                }
            }
            else
            {
                return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
            }


        }

        public ActionResult PESNimonationRating(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            vNominationForm_obj_save result = new vNominationForm_obj_save();
            if (qryStr != "")
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PES_NominationService.Find(nId);
                    if (_getData != null)
                    {

                        var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                        if (_checkActiv != null)
                        {

                            result.IdEncrypt = qryStr;
                            result.code = _getData.user_no;
                            result.sname = _checkActiv.EmpFullName;
                            result.sgroup = _checkActiv.UnitGroupName;
                            result.srank = _checkActiv.Rank;
                            result.status_name = _getData.TM_PES_NMN_Status != null ? _getData.TM_PES_NMN_Status.status_name_en + "" : "";
                            result.status_id = _getData.TM_PES_NMN_Status != null ? _getData.TM_PES_NMN_Status.Id + "" : "";
                            result.yearcurrent = _getData.PES_Nomination_Year.evaluation_year.Value.DateTimebyCulture("yyyy");
                            result.other_role = _getData.other_roles;

                            #region if else Data

                            #region Plaining Data
                            result.eva_mode = "Plan";

                            var _getYearRate = _PES_Final_Rating_YearService.GetDataForSelect();
                            if (_getYearRate != null && _getYearRate.Any(a => a.active_status == "Y"))
                            {
                                result.lstRate = new List<lstDataSelect>();
                                result.lstRate.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                                result.lstAnnul_Rate = (from lst in _getYearRate

                                                        select new vNominationForm_RATING_Annual
                                                        {
                                                            id = lst.Id + "",
                                                            sYear = lst.evaluation_year.Value.DateTimebyCulture("yyyy"),
                                                            srate = lst.PES_Final_Rating != null ? lst.PES_Final_Rating.Where(w => w.active_status == "Y" && w.user_no == _getData.user_no).Select(s => s.Final_TM_Annual_Rating.rating_name_en).FirstOrDefault() + "" : "",
                                                            rate_id = lst.PES_Final_Rating != null ? lst.PES_Final_Rating.Where(w => w.active_status == "Y" && w.user_no == _getData.user_no).Select(s => s.Final_TM_Annual_Rating.Id).FirstOrDefault() + "" : "",
                                                        }).ToList();


                                var _geRate = _TM_Annual_RatingService.GetDataForSelect().ToList();
                                if (_geRate.Any())
                                {
                                    result.lstRate.AddRange(_geRate.OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.rating_name_en }).ToList());
                                }
                            }


                            #endregion



                            #endregion

                            return PartialView("_PESNimonationRating", result);
                        }
                        else
                        {
                            return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                    }
                }
                else
                {
                    return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
                }
            }
            else
            {
                return RedirectToAction("Error404Partial", "MasterPage", new vErrorObject { msg = "error session expired" });
            }


        }
        //Config
        public ActionResult PTRConfig(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            return View();
        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPTRMasterDataList(CSearchPool SearchItem)
        {
            vPTRMasterData_Return result = new vPTRMasterData_Return();
            List<vPTRMasterData_obj> lstData_resutl = new List<vPTRMasterData_obj>();
            var lstData = _TM_Partner_EvaluationService.GetTIF_Form(
            SearchItem.name,
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
                                  select new vPTRMasterData_obj
                                  {
                                      name_en = lstAD.action_date.HasValue ? lstAD.action_date.Value.DateTimeWithTimebyCulture() : "",
                                      //  short_name_en = lstAD.Company_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      View = @"<button id=""btnEdit""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult CreatePTRMasterData(vPTRMasterData_obj_save ItemData)
        {
            vPTRMasterData_Return result = new vPTRMasterData_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData.lstQstPlan != null && ItemData.lstQstPlan.Any() && ItemData.lstQstGoal != null && ItemData.lstQstGoal.Any())
                {
                    DateTime? dAction = null;

                    if (!string.IsNullOrEmpty(ItemData.action_date))
                    {
                        dAction = SystemFunction.ConvertStringToDateTime("01-Jan-" + (ItemData.action_date).Trim(), "");
                    }
                    List<TM_PTR_Eva_Questions> lstObjQuestion = new List<TM_PTR_Eva_Questions>();
                    List<TM_PTR_Eva_Questions> lstObjQuestionPlan = new List<TM_PTR_Eva_Questions>();
                    lstObjQuestionPlan = ItemData.lstQstPlan.Select(s => new TM_PTR_Eva_Questions
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        header = (s.header + "").Trim(),
                        question = (s.question + "").Trim(),
                        seq = SystemFunction.GetIntNullToZero(s.nID + ""),
                        questions_type = "P",
                        qgroup = (s.sgroup + "").Trim(),

                    }).ToList();

                    List<TM_PTR_Eva_Questions> lstObjQuestionGoal = new List<TM_PTR_Eva_Questions>();
                    lstObjQuestionGoal = ItemData.lstQstGoal.Select(s => new TM_PTR_Eva_Questions
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        header = (s.header + "").Trim(),
                        question = (s.question + "").Trim(),
                        seq = SystemFunction.GetIntNullToZero(s.nID + ""),

                        questions_type = "G",
                        qgroup = (s.sgroup + "").Trim(),
                    }).ToList();

                    if (lstObjQuestionPlan.Any())
                    {
                        lstObjQuestion.AddRange(lstObjQuestionPlan);
                    }
                    if (lstObjQuestionGoal.Any())
                    {
                        lstObjQuestion.AddRange(lstObjQuestionGoal);
                    }
                    TM_Partner_Evaluation objSave = new TM_Partner_Evaluation()
                    {
                        Id = 0,
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        TM_PTR_Eva_Questions = lstObjQuestion.ToList(),
                        description = (ItemData.description + "").Trim(),
                        action_date = dAction,
                    };

                    var sComplect = _TM_Partner_EvaluationService.CreateNew(ref objSave);
                    if (sComplect > 0)
                    {
                        _TM_Partner_EvaluationService.UpdateInactive(objSave);
                        result.Status = SystemFunction.process_Success;
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
                    result.Msg = "Error, please enter name";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditPTRMasterData(vPTRMasterData_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTRMasterData_Return result = new vPTRMasterData_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Partner_EvaluationService.Find(nId);
                    //if (_getData != null)
                    //{
                    //    if (!string.IsNullOrEmpty(ItemData.name_en))
                    //    {

                    //        _getData.update_user = CGlobal.UserInfo.UserId;
                    //        _getData.update_date = dNow;
                    //        _getData.active_status = ItemData.active_status;

                    //            var sComplect = _TM_TIF_FormService.Update(_getData);
                    //            if (sComplect > 0)
                    //            {
                    //                result.Status = SystemFunction.process_Success;
                    //            }
                    //            else
                    //            {
                    //                result.Status = SystemFunction.process_Failed;
                    //                result.Msg = "Error, please try again.";
                    //            }


                    //    }
                    //    else
                    //    {
                    //        result.Status = SystemFunction.process_Failed;
                    //        result.Msg = "Error, please enter name";
                    //    }
                    //}
                    //else
                    //{
                    //    result.Status = SystemFunction.process_Failed;
                    //    result.Msg = "Error, Request Type Not Found.";
                    //}
                }

            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult CreateUserAuthorized(vPTREvaluation_lst_Authorized ItemData)
        {
            objPTREvaluation_lst_Authorized_Return result = new objPTREvaluation_lst_Authorized_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vPTREvaluation_lst_Authorized>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));

                    if (nId != 0)
                    {
                        var _getData = _PTR_EvaluationService.Find(nId);
                        if (_getData != null)
                        {
                            if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                            {
                                string[] aRank = new string[] { "12", "20", "32" };
                                var checkActive = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && aRank.Contains(w.RankID) && w.EmpNo == ItemData.authorized_user).FirstOrDefault();
                                if (checkActive != null)
                                {
                                    PTR_Evaluation_Authorized objSave = new PTR_Evaluation_Authorized()
                                    {
                                        //Id = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.id + "")),
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        PTR_Evaluation = _getData,
                                        active_status = "Y",
                                        authorized_user = ItemData.authorized_user,

                                    };
                                    if (_PTR_Evaluation_AuthorizedService.CanSave(objSave))
                                    {
                                        var sComplect = _PTR_Evaluation_AuthorizedService.CreateNewOrUpdate(objSave);
                                        if (sComplect > 0)
                                        {
                                            result.Status = SystemFunction.process_Success;
                                            var _getEditList = _PTR_EvaluationService.Find(nId);
                                            if (_getEditList != null)
                                            {
                                                if (_getEditList.PTR_Evaluation_Authorized != null && _getEditList.PTR_Evaluation_Authorized.Any(a => a.active_status == "Y"))
                                                {
                                                    string[] empNO = _getEditList.PTR_Evaluation_Authorized.Where(a => a.active_status == "Y").Select(s => s.authorized_user).ToArray();
                                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                                    result.lstData = (from lst in _getEditList.PTR_Evaluation_Authorized.Where(a => a.active_status == "Y")
                                                                      from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.authorized_user).DefaultIfEmpty(new AllInfo_WS())
                                                                      select new vPTREvaluation_lst_Authorized
                                                                      {
                                                                          authorized_name = lstEmpReq.EmpFullName,
                                                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                          authorized_rank = lstEmpReq.Rank + "",
                                                                          authorized_user = lst.authorized_user + "",
                                                                      }).ToList();
                                                }
                                            }
                                            else
                                            {
                                                result.lstData = new List<vPTREvaluation_lst_Authorized>();
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
                                result.Msg = "Error, Don't have permission to save.";
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
        public ActionResult DeleteUserAuthorized(vPTREvaluation_lst_Authorized ItemData)
        {
            objPTREvaluation_lst_Authorized_Return result = new objPTREvaluation_lst_Authorized_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vPTREvaluation_lst_Authorized>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));

                    if (nId != 0)
                    {
                        var _getData = _PTR_Evaluation_AuthorizedService.Find(nId);
                        if (_getData != null)
                        {
                            int EvaID = _getData.PTR_Evaluation.Id;
                            if (_getData.PTR_Evaluation.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                            {
                                _getData.active_status = "N";
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                var sComplect = _PTR_Evaluation_AuthorizedService.Update(_getData);
                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    var _getEditList = _PTR_EvaluationService.Find(EvaID);
                                    if (_getEditList != null)
                                    {
                                        if (_getEditList.PTR_Evaluation_Authorized != null && _getEditList.PTR_Evaluation_Authorized.Any(a => a.active_status == "Y"))
                                        {
                                            string[] empNO = _getEditList.PTR_Evaluation_Authorized.Where(a => a.active_status == "Y").Select(s => s.authorized_user).ToArray();
                                            var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                            result.lstData = (from lst in _getEditList.PTR_Evaluation_Authorized.Where(a => a.active_status == "Y")
                                                              from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.authorized_user).DefaultIfEmpty(new AllInfo_WS())
                                                              select new vPTREvaluation_lst_Authorized
                                                              {
                                                                  authorized_name = lstEmpReq.EmpFullName,
                                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                  authorized_rank = lstEmpReq.Rank + "",
                                                                  authorized_user = lst.authorized_user + "",
                                                              }).ToList();
                                        }
                                    }
                                    else
                                    {
                                        result.lstData = new List<vPTREvaluation_lst_Authorized>();
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
                                result.Msg = "Error, Don't have permission to save.";
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
        public ActionResult CreateUserAuthorizedEva(vPTREvaluation_lst_Authorized ItemData)
        {
            objPTREvaluation_lst_Authorized_Return result = new objPTREvaluation_lst_Authorized_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vPTREvaluation_lst_Authorized>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));

                    if (nId != 0)
                    {
                        var _getData = _PTR_EvaluationService.Find(nId);
                        if (_getData != null)
                        {
                            if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                            {
                                string[] aRank = new string[] { "12", "20", "32" };
                                var checkActive = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && aRank.Contains(w.RankID) && w.EmpNo == ItemData.authorized_user).FirstOrDefault();
                                if (checkActive != null)
                                {
                                    PTR_Evaluation_AuthorizedEva objSave = new PTR_Evaluation_AuthorizedEva()
                                    {
                                        //Id = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.id + "")),
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        PTR_Evaluation = _getData,
                                        active_status = "Y",
                                        authorized_user = ItemData.authorized_user,

                                    };
                                    if (_PTR_Evaluation_AuthorizedEvaService.CanSave(objSave))
                                    {
                                        var sComplect = _PTR_Evaluation_AuthorizedEvaService.CreateNewOrUpdate(objSave);
                                        if (sComplect > 0)
                                        {
                                            result.Status = SystemFunction.process_Success;
                                            var _getEditList = _PTR_EvaluationService.Find(nId);
                                            if (_getEditList != null)
                                            {
                                                if (_getEditList.PTR_Evaluation_AuthorizedEva != null && _getEditList.PTR_Evaluation_AuthorizedEva.Any(a => a.active_status == "Y"))
                                                {
                                                    string[] empNO = _getEditList.PTR_Evaluation_AuthorizedEva.Where(a => a.active_status == "Y").Select(s => s.authorized_user).ToArray();
                                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                                    result.lstData = (from lst in _getEditList.PTR_Evaluation_AuthorizedEva.Where(a => a.active_status == "Y")
                                                                      from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.authorized_user).DefaultIfEmpty(new AllInfo_WS())
                                                                      select new vPTREvaluation_lst_Authorized
                                                                      {
                                                                          authorized_name = lstEmpReq.EmpFullName,
                                                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                          authorized_rank = lstEmpReq.Rank + "",
                                                                          authorized_user = lst.authorized_user + "",
                                                                      }).ToList();
                                                }
                                            }
                                            else
                                            {
                                                result.lstData = new List<vPTREvaluation_lst_Authorized>();
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
                                result.Msg = "Error, Don't have permission to save.";
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
        public ActionResult DeleteUserAuthorizedEva(vPTREvaluation_lst_Authorized ItemData)
        {
            objPTREvaluation_lst_Authorized_Return result = new objPTREvaluation_lst_Authorized_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vPTREvaluation_lst_Authorized>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));

                    if (nId != 0)
                    {
                        var _getData = _PTR_Evaluation_AuthorizedEvaService.Find(nId);
                        if (_getData != null)
                        {
                            int EvaID = _getData.PTR_Evaluation.Id;
                            if (_getData.PTR_Evaluation.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                            {
                                _getData.active_status = "N";
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                var sComplect = _PTR_Evaluation_AuthorizedEvaService.Update(_getData);
                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    var _getEditList = _PTR_EvaluationService.Find(EvaID);
                                    if (_getEditList != null)
                                    {
                                        if (_getEditList.PTR_Evaluation_AuthorizedEva != null && _getEditList.PTR_Evaluation_AuthorizedEva.Any(a => a.active_status == "Y"))
                                        {
                                            string[] empNO = _getEditList.PTR_Evaluation_AuthorizedEva.Where(a => a.active_status == "Y").Select(s => s.authorized_user).ToArray();
                                            var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                            result.lstData = (from lst in _getEditList.PTR_Evaluation_AuthorizedEva.Where(a => a.active_status == "Y")
                                                              from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.authorized_user).DefaultIfEmpty(new AllInfo_WS())
                                                              select new vPTREvaluation_lst_Authorized
                                                              {
                                                                  authorized_name = lstEmpReq.EmpFullName,
                                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteAuth('" + HCMFunc.EncryptPES(lst.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                  authorized_rank = lstEmpReq.Rank + "",
                                                                  authorized_user = lst.authorized_user + "",
                                                              }).ToList();
                                        }
                                    }
                                    else
                                    {
                                        result.lstData = new List<vPTREvaluation_lst_Authorized>();
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
                                result.Msg = "Error, Don't have permission to save.";
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
        #endregion
    }
}