using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.ManageTifAndPr
{

    public class ManageTifAndPrController : BaseController
    {
        private PersonnelRequestService _PersonnelRequestService;
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TM_CandidatesService _TM_CandidatesService;
        private TM_Candidate_TIF_ApprovService _TM_Candidate_TIF_ApprovService;
        private TM_Candidate_MassTIF_ApprovService _TM_Candidate_MassTIF_ApprovService;
        private TM_Candidate_TIFService _TM_Candidate_TIFService;
        private TM_TIF_FormService _TM_TIF_FormService;
        private TM_TIF_RatingService _TM_TIF_RatingService;

        private New_HRISEntities dbHr = new New_HRISEntities();

        public ManageTifAndPrController(
                PersonnelRequestService PersonnelRequestService
               , TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
                , TM_CandidatesService TM_CandidatesService
                , TM_Candidate_TIF_ApprovService TM_Candidate_TIF_ApprovService
                , TM_Candidate_MassTIF_ApprovService TM_Candidate_MassTIF_ApprovService
                , TM_Candidate_TIFService TM_Candidate_TIFService
                , TM_TIF_FormService TM_TIF_FormService
                , TM_TIF_RatingService TM_TIF_RatingService
                )
        {

            _PersonnelRequestService = PersonnelRequestService;
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_CandidatesService = TM_CandidatesService;
            _TM_Candidate_TIF_ApprovService = TM_Candidate_TIF_ApprovService;
            _TM_Candidate_MassTIF_ApprovService = TM_Candidate_MassTIF_ApprovService;
            _TM_Candidate_TIFService = TM_Candidate_TIFService;
            _TM_TIF_FormService = TM_TIF_FormService;
            _TM_TIF_RatingService = TM_TIF_RatingService;

        }
        // GET: ManageTifAndPr
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManageTifApproveList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = true;
            vInterview result = new vInterview();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                List<vInterview_obj> lstData_resutl = new List<vInterview_obj>();
                List<vInterview_obj> lstData_resutlMass = new List<vInterview_obj>();
                CSearchInterview SearchItem = (CSearchInterview)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchInterview)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                List<TM_Candidate_TIF_Approv> lstData = new List<TM_Candidate_TIF_Approv>();
                List<TM_Candidate_MassTIF_Approv> lstDataMass = new List<TM_Candidate_MassTIF_Approv>();

                if (SearchItem.tif_type + "" == "")
                {
                    lstData = _TM_Candidate_TIF_ApprovService.GetTIFFormForApprove(
                                                      SearchItem.group_code, aDivisionPermission,
                                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                    lstDataMass = _TM_Candidate_MassTIF_ApprovService.GetTIFFormForApprove(
                                                      SearchItem.group_code, aDivisionPermission,
                                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "N")
                {
                    lstData = _TM_Candidate_TIF_ApprovService.GetTIFFormForApprove(
                                                      SearchItem.group_code, aDivisionPermission,
                                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "M")
                {
                    lstDataMass = _TM_Candidate_MassTIF_ApprovService.GetTIFFormForApprove(
                                          SearchItem.group_code, aDivisionPermission,
                                         CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                }

                result.tif_type = SearchItem.tif_type;
                result.group_code = SearchItem.group_code;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    lstData_resutl = (from lstAD in lstData
                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vInterview_obj
                                      {
                                          refno = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                          name_en = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                          status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",

                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          approval = AEmp.EmpFullName,
                                          pr_type = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      }).ToList();
                }

                if (lstDataMass.Any())
                {

                    string[] aApproveUser = lstDataMass.Select(s => s.Req_Approve_user).ToArray();
                    string[] aUpdateUser = lstDataMass.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    lstData_resutlMass = (from lstAD in lstDataMass
                                          from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                          select new vInterview_obj
                                          {
                                              refno = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                              group_name = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                              position = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                              rank = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                              name_en = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                              status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",

                                              update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMass('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              approval = AEmp.EmpFullName,
                                              pr_type = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          }).ToList();
                }
                result.lstData = lstData_resutl.Concat(lstData_resutlMass).ToList();
            }

            #endregion
            return View(result);
        }

        public ActionResult ManageTifApproveEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vInterview_obj_save result = new vInterview_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Candidate_TIF_ApprovService.Find(nId);
                    if (_getData != null)
                    {
                        result.Id = _getData.Id;
                        result.approve_user = _getData.Req_Approve_user;
                    }
                }

            }
            return View(result);

            #endregion

        }

        [HttpPost]
        public ActionResult SaveApproveTIFFormList(vInterview_obj_save ItemData)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            if (ItemData.Id != null && !string.IsNullOrEmpty(ItemData.approve_user))
            {
                var get = _TM_Candidate_TIF_ApprovService.Find(ItemData.Id);
                if (string.IsNullOrEmpty(get.Approve_user) && get.Approve_date == null)
                {
                    get.Req_Approve_user = ItemData.approve_user;
                    get.update_date = DateTime.Now;
                    get.update_user = CGlobal.UserInfo.EmployeeNo;
                    var change = _TM_Candidate_TIF_ApprovService.Update(get);
                    if (change <= 0)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't find Approve or Reject.";
                        return Json(new
                        {
                            result
                        });
                    }
                    else {
                        result.Status = SystemFunction.process_Success;
                       
                        return Json(new
                        {
                            result
                        });
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Can't edit form after approved.";
                    return Json(new
                    {
                        result
                    });
                }
            }


            //if (!string.IsNullOrEmpty(id))
            //{
            //    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
            //    if (nId != 0)
            //    {
            //    }
            //}
            return Json(new { result });
        }


        public ActionResult ManagePRList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            bool isAdmin = true;
            vPersonnelRequest result = new vPersonnelRequest();
            result.active_status = "Y";
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPersonnelRequest SearchItem = (CSearchPersonnelRequest)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPersonnelRequest)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");
                int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
                int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
                var lstData = _PersonnelRequestService.GetPersonnelPRListData(
        SearchItem.group_code, aDivisionPermission,
        "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, CGlobal.UserInfo.EmployeeNo, isAdmin);
                string BackUrl = Uri.EscapeDataString(qryStr);

                result.group_code = SearchItem.group_code;
                result.pr_status = SearchItem.pr_status;
                result.ref_no = SearchItem.ref_no;


                if (lstData.Any())
                {

                    string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())

                                      select new vPersonnelRequest_obj
                                      {
                                          refno = lstAD.RefNo + "",
                                          sgroup = lstAD.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Position.position_name_en + "",
                                          request_type = lstAD.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "",
                                          rank = lstAD.TM_Pool_Rank.TM_Rank.rank_name_en + "",
                                          pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                          pr_status_id = lstAD.TM_PR_Status.Id + "",
                                          pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                          request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                          hc = lstAD.no_of_headcount + "",
                                          request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimeWithTimebyCulture() : "",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                      }).ToList();
                }
            }

            return View(result);
        }

        public ActionResult ManagePREdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            vPersonnelAP_obj_save result = new vPersonnelAP_obj_save();
            result.lstApprove = new List<vPersonnelAp_obj>();

            string BackUrl = Uri.EscapeDataString(qryStr);


            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PersonnelRequestService.FindPRAdmin(nId);
                    if (_getData != null)
                    {



                        if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any())
                        {
                            result.pr_ref_no = _getData.RefNo;
                            string[] User = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                            var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                            result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping
                                                    from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vPRCandidates_lstData
                                                    {
                                                        Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                        //Id = lstAD.Id.ToString(),
                                                        candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                        candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                        owner_name = lstEmp.EmpFullName,
                                                        rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                        remark = lstAD.description + "",
                                                        action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                        active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                        active_status_id = lstAD.active_status + "",
                                                        ref_no = lstAD.PersonnelRequest.RefNo.ToString()
                                                    }).ToList();
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("PRAdminList", "PRAdmin");
            }

            return View(result);
        }

        [HttpPost]
        public ActionResult SaveManagePREdit(string id, string pr_no)
        {
            vTIFForm_Return result = new vTIFForm_Return();
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pr_no))
            {

                var getid = HCMFunc.Decrypt(id);
                var get = _TM_PR_Candidate_MappingService.Find(SystemFunction.GetIntNullToZero(getid));
                var getpr = _PersonnelRequestService.FindByRefNo(pr_no);
                get.PersonnelRequest = getpr;
                get.update_date = DateTime.Now;
                get.update_user = CGlobal.UserInfo.EmployeeNo;
                var change = _TM_PR_Candidate_MappingService.Update(get);
                if (change <= 0)
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Can't Update.";
                    return Json(new
                    {
                        result
                    });


                }

                result.Status = SystemFunction.process_Success;
                //result.Msg = "Error, Can't Update.";
                return Json(new
                {
                    result
                });
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Can't edit Please check data.";
                return Json(new
                {
                    result
                });
            }

            //if (!string.IsNullOrEmpty(id))
            //{
            //    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
            //    if (nId != 0)
            //    {
            //    }
            //}
            return Json(new { result });
        }

    }
}