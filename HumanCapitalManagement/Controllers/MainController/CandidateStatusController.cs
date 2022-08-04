using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class CandidateStatusController : BaseController
    {
        private TM_Candidate_StatusService _TM_Candidate_StatusService;
        private TM_Candidate_Status_NextService _TM_Candidate_Status_NextService;
        public CandidateStatusController(TM_Candidate_StatusService TM_Candidate_StatusService
            , TM_Candidate_Status_NextService TM_Candidate_Status_NextService)
        {
            _TM_Candidate_StatusService = TM_Candidate_StatusService;
            _TM_Candidate_Status_NextService = TM_Candidate_Status_NextService;
        }
        // GET: CandidateStatus
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CandidateStatusList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vCandidateStatus result = new vCandidateStatus();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchCandidateStatus SearchItem = (CSearchCandidateStatus)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchCandidateStatus)));
                var lstData = _TM_Candidate_StatusService.GetCandidateStatus(
               SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vCandidateStatus_obj
                                      {
                                          name_en = lstAD.candidate_status_name_en.StringRemark(500),
                                          //  short_name_en = lstAD.Company_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.candidate_status_description.StringRemark(),
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();

                }
            }

            #endregion
            return View(result);
        }

        public ActionResult CandidateStatusEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vCandidateStatus_obj_save result = new vCandidateStatus_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Candidate_StatusService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.Id = _getData.Id;
                        result.name_en = _getData.candidate_status_name_en + "";
                        result.active_name = _getData.active_status + "" == "Y" ? "Active" : "Inactive";
                        result.aStatus = _getData.TM_Candidate_Status_Next != null ? _getData.TM_Candidate_Status_Next.Where(w => w.active_status == "Y").Select(s => s.next_status_id + "").ToArray() : new string[] { };

                    }
                }
            }
            return View(result);

            #endregion

        }
        #region Ajax Function 
        [HttpPost]
        public ActionResult LoadCandidateStatusList(CSearchPool SearchItem)
        {
            vCandidateStatus_Return result = new vCandidateStatus_Return();
            List<vCandidateStatus_obj> lstData_resutl = new List<vCandidateStatus_obj>();
            var lstData = _TM_Candidate_StatusService.GetCandidateStatus(
            SearchItem.name,
            SearchItem.active_status);
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
                                  select new vCandidateStatus_obj
                                  {
                                      name_en = lstAD.candidate_status_name_en.StringRemark(500),
                                      //  short_name_en = lstAD.Company_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.candidate_status_description.StringRemark(),
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult EditCandidateStatus(vCandidateStatus_obj_save ItemData)
        {
            vCandidateStatus_Return result = new vCandidateStatus_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Candidate_StatusService.Find(nId);
                    if (_getData != null)
                    {
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        List<TM_Candidate_Status_Next> lstStatus = new List<TM_Candidate_Status_Next>();
                        #region List RequestType Permission
                        if (ItemData.aStatus != null && ItemData.aStatus.Length > 0)
                        {
                            int[] aID = ItemData.aStatus.Select(s => SystemFunction.GetIntNullToZero(s)).ToArray();
                            if (aID != null && aID.Length > 0)
                            {
                                lstStatus = aID.Select(s => new TM_Candidate_Status_Next
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    active_status = "Y",
                                    TM_Candidate_Status = _getData,
                                    next_status_id = s
                                }).ToList();
                            }
                         
                        }
                        var sCom = _TM_Candidate_StatusService.Update(_getData);
                        if (lstStatus.Any() && sCom > 0)
                        {
                            var sComplect = _TM_Candidate_Status_NextService.UpdateNext_Status(lstStatus, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }
                        }
                        else if (sCom > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }
                        #endregion



                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Group Permission Not Found.";
                    }
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