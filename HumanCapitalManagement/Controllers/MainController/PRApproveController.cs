using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class PRApproveController : BaseController
    {
        private TM_Step_ApproveService _TM_Step_ApproveService;
        private TM_PR_StatusService _TM_PR_StatusService;
        private PersonnelRequestService _PersonnelRequestService;
        private MailContentService _MailContentService;
        private E_Mail_HistoryService _E_Mail_HistoryService;
        private TM_FY_PlanService _TM_FY_PlanService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public PRApproveController(
           TM_Step_ApproveService TM_Step_ApproveService
           , TM_PR_StatusService TM_PR_StatusService
           , PersonnelRequestService PersonnelRequestService,
           MailContentService MailContentService,
           E_Mail_HistoryService E_Mail_HistoryService
            , TM_FY_PlanService TM_FY_PlanService)
        {
            _TM_Step_ApproveService = TM_Step_ApproveService;
            _TM_PR_StatusService = TM_PR_StatusService;
            _PersonnelRequestService = PersonnelRequestService;
            _MailContentService = MailContentService;
            _E_Mail_HistoryService = E_Mail_HistoryService;
            _TM_FY_PlanService = TM_FY_PlanService;
        }
        // GET: PRApprove
        public ActionResult PRApproveList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPersonnelAP result = new vPersonnelAP();
            result.active_status = "Y";
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPersonnelRequest SearchItem = (CSearchPersonnelRequest)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPersonnelRequest)));
                bool isAdmin = CGlobal.UserIsAdmin();
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                var lstData = _PersonnelRequestService.GetPRForApprove(
                SearchItem.group_code, aDivisionPermission,
                CGlobal.UserInfo.EmployeeNo
                , SearchItem.ref_no
                , isAdmin);
                string BackUrl = Uri.EscapeDataString(qryStr);
                result.group_code = SearchItem.group_code;
                if (lstData.Any())
                {
                    string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    // var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    List<string> aApproveUser = new List<string>();
                    aApproveUser.AddRange(lstData.Select(s => s.Req_BUApprove_user).ToList());
                    aApproveUser.AddRange(lstData.Select(s => s.Req_HeadApprove_user).ToList());
                    aApproveUser.AddRange(lstData.Select(s => s.Req_CeoApprove_user).ToList());
                    var _getApprove = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();

                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                          // from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vPersonnelAP_obj
                                      {

                                          refno = lstAD.RefNo + "",
                                          sgroup = lstAD.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Position.position_name_en + "",
                                          request_type = lstAD.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "",
                                          rank = lstAD.TM_Pool_Rank.TM_Rank.rank_name_en + "",
                                          pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                          request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                          hc = lstAD.no_of_headcount + "",
                                          request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                          update_user = lstAD.BUApprove_status != "Y" ? _getApprove.FirstOrDefault(w => w.EmpNo == lstAD.Req_BUApprove_user).EmpFullName + ""
                                          : (lstAD.HeadApprove_status != "Y" ? _getApprove.FirstOrDefault(w => w.EmpNo == lstAD.Req_HeadApprove_user).EmpFullName + "" : _getApprove.FirstOrDefault(w => w.EmpNo == lstAD.Req_CeoApprove_user).EmpFullName + ""),
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                      }).ToList();
                }
            }
            return View(result);
        }
        public ActionResult PRApproveEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            vPersonnelAP_obj_save result = new vPersonnelAP_obj_save();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            result.lstApprove = new List<vPersonnelAp_obj>();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PersonnelRequestService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.BUApprove_status + "" != "Y" || _getData.HeadApprove_status + "" != "Y" || (_getData.CeoApprove_status + "" != "Y" && _getData.need_ceo_approve + "" == "Y")
                            && aDivisionPermission.Contains(_getData.TM_Divisions.division_code)
                            && _getData.TM_PR_Status.Id == 2

                            )

                        {
                            result.IdEncrypt = qryStr;
                            List<string> lstUserApp = new List<string>();
                            lstUserApp.Add(_getData.request_user);
                            lstUserApp.Add(_getData.Req_BUApprove_user);
                            lstUserApp.Add(_getData.Req_HeadApprove_user);
                            lstUserApp.Add(_getData.Req_CeoApprove_user);
                            if (!string.IsNullOrEmpty(_getData.BUApprove_user))
                            {
                                lstUserApp.Add(_getData.BUApprove_user);
                            }
                            if (!string.IsNullOrEmpty(_getData.HeadApprove_user))
                            {
                                lstUserApp.Add(_getData.HeadApprove_user);
                            }
                            if (!string.IsNullOrEmpty(_getData.CeoApprove_user))
                            {
                                lstUserApp.Add(_getData.CeoApprove_user);
                            }

                            result.FY_plan = "0 person(s)";
                            if (_getData.request_date.Value.Month >= 10)
                            {
                                dNow = dNow.AddYears(1);
                            }
                            var _getPlan = _TM_FY_PlanService.FindbyYear(dNow.Year);
                            if (_getPlan != null && _getPlan.TM_FY_Detail != null)
                            {
                                var _getFYPlan = _getPlan.TM_FY_Detail.Where(w => w.TM_Divisions.division_code == _getData.TM_Divisions.division_code).FirstOrDefault();
                                if (_getFYPlan != null)
                                {
                                    decimal? nSum = ((_getFYPlan.para.HasValue ? _getFYPlan.para.Value : 0) +
                            (_getFYPlan.aa.HasValue ? _getFYPlan.aa.Value : 0) +
                             (_getFYPlan.sr.HasValue ? _getFYPlan.sr.Value : 0) +
                              (_getFYPlan.am.HasValue ? _getFYPlan.am.Value : 0) +
                             (_getFYPlan.mgr.HasValue ? _getFYPlan.mgr.Value : 0) +
                             (_getFYPlan.ad.HasValue ? _getFYPlan.ad.Value : 0) +
                              (_getFYPlan.dir.HasValue ? _getFYPlan.dir.Value : 0) +
                             (_getFYPlan.ptr.HasValue ? _getFYPlan.ptr.Value : 0));
                                    result.FY_plan = nSum.NodecimalFormat() + " person(s)";
                                }
                            }
                            result.FY_plan_title = "HC Plan FY " + dNow.ToString("yy") + " :";


                            int UnitID = SystemFunction.GetIntNullToZero(_getData.TM_Divisions.division_code);
                            result.cur_headcount = dbHr.AllInfo_WS.Where(w => w.UnitGroupID == UnitID && (w.Status == 1 || w.Status == 3)).Count() + " person(s)";


                            result.group_id = _getData.TM_Divisions.division_name_en;
                            var _getApproveStep = _TM_Step_ApproveService.GetDataForSelect();
                            if (_getApproveStep != null && _getApproveStep.Any())
                            {
                                string[] aUser = lstUserApp.ToArray();
                                var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                result.lstApprove = _getApproveStep.Select(s => new vPersonnelAp_obj
                                {
                                    nStep = s.step,
                                    step_name = (s.step_name_en + "").Replace("(", "<br/>("),
                                }).ToList();
                                if (_getEmp.Any())
                                {
                                    var _getUserReq = _getApproveStep.Where(w => w.step == 1).FirstOrDefault();
                                    if (_getUserReq != null)
                                    {
                                        vPersonnelAp_obj objApp = new vPersonnelAp_obj();
                                    }
                                    //Requested by
                                    result.lstApprove.Where(w => w.nStep == 1).ToList().ForEach(ed =>
                                    {
                                        ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.request_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                        ed.approve_date = _getData.request_date.HasValue ? _getData.request_date.Value.DateTimebyCulture() : "";
                                    });
                                    //Requested by Group Head
                                    result.lstApprove.Where(w => w.nStep == 2).ToList().ForEach(ed =>
                                    {
                                        if (_getData.BUApprove_date.HasValue)
                                        {

                                            ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_BUApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                            +
                                            ""
                                            +
                                            ((!string.IsNullOrEmpty(_getData.BUApprove_user) && _getData.BUApprove_user != _getData.Req_BUApprove_user) ?
                                            "<br/>(by "
                                            + _getEmp.Where(w => w.EmpNo == _getData.BUApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                            + ")"
                                            : "");
                                            ed.approve_date = _getData.BUApprove_date.Value.DateTimebyCulture();
                                            ed.description = _getData.BUApprove_remark + "";
                                        }
                                        else
                                        {
                                            ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_BUApprove_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                            ed.approve_date = "";
                                        }
                                    });
                                    //Approve by Functional HOP
                                    result.lstApprove.Where(w => w.nStep == 3).ToList().ForEach(ed =>
                                    {
                                        if (_getData.HeadApprove_date.HasValue)
                                        {

                                            ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_HeadApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                            +
                                            ""
                                            +
                                            ((!string.IsNullOrEmpty(_getData.HeadApprove_user) && _getData.HeadApprove_user != _getData.Req_HeadApprove_user) ?
                                            "<br/>(by "
                                            + _getEmp.Where(w => w.EmpNo == _getData.HeadApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                            + ")"
                                            : "");
                                            ed.approve_date = _getData.HeadApprove_date.Value.DateTimebyCulture();
                                            ed.description = _getData.HeadApprove_remark + "";
                                        }
                                        else
                                        {
                                            ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_HeadApprove_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                            ed.approve_date = "";
                                        }
                                    });
                                    //ceo approve
                                    if (_getData.need_ceo_approve + "" == "Y")
                                    {
                                        result.lstApprove.Where(w => w.nStep == 4).ToList().ForEach(ed =>
                                        {
                                            if (_getData.CeoApprove_date.HasValue)
                                            {

                                                ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_CeoApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                                +
                                                ""
                                                +
                                                ((!string.IsNullOrEmpty(_getData.CeoApprove_user) && _getData.CeoApprove_user != _getData.Req_CeoApprove_user) ?
                                                "<br/>(by "
                                                + _getEmp.Where(w => w.EmpNo == _getData.CeoApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                                + ")"
                                                : "");
                                                ed.approve_date = _getData.CeoApprove_date.Value.DateTimebyCulture();
                                                ed.description = _getData.CeoApprove_remark + "";
                                            }
                                            else
                                            {
                                                ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_CeoApprove_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                                ed.approve_date = "";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        result.lstApprove.RemoveAll(w => w.nStep == 4);
                                    }



                                    if (_getData.BUApprove_status + "" != "Y")
                                    {
                                        result.app_step_name = _getApproveStep.Where(w => w.step == 2).Select(s => s.step_name_en).FirstOrDefault() + "";
                                        result.app_user_name = result.lstApprove.Where(w => w.nStep == 2).Select(s => s.app_name).FirstOrDefault() + "";
                                        result.app_mode = "2";
                                    }
                                    else if (_getData.HeadApprove_status + "" != "Y")
                                    {
                                        result.app_step_name = _getApproveStep.Where(w => w.step == 3).Select(s => s.step_name_en).FirstOrDefault() + "";
                                        result.app_user_name = result.lstApprove.Where(w => w.nStep == 3).Select(s => s.app_name).FirstOrDefault() + "";
                                        result.app_mode = "3";
                                    }
                                    else if (_getData.CeoApprove_status + "" != "Y" && _getData.need_ceo_approve + "" == "Y")
                                    {
                                        result.app_step_name = _getApproveStep.Where(w => w.step == 4).Select(s => s.step_name_en).FirstOrDefault() + "";
                                        result.app_user_name = result.lstApprove.Where(w => w.nStep == 4).Select(s => s.app_name).FirstOrDefault() + "";
                                        result.app_mode = "4";
                                    }
                                }
                            }
                            if (_getData.TM_SubGroup != null)
                            {
                                var GetHeadSG = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.TM_SubGroup.head_user_no).FirstOrDefault();
                                result.sub_group_id = _getData.TM_SubGroup.sub_group_name_en + "";
                                result.sub_group = "Y";
                                result.sub_group_head = GetHeadSG != null ? GetHeadSG.EmpFullName + "" : "-";
                            }
                            else
                            {
                                result.sub_group_id = "-";
                                result.sub_group = "N";
                                result.sub_group_head = "";
                            }
                            result.rank_id = _getData.TM_Pool_Rank != null ? _getData.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                            // result.sub_group_id = _getData.TM_SubGroup != null ? _getData.TM_SubGroup.sub_group_name_en + "" : "-";
                            result.position_id = _getData.TM_Position != null ? _getData.TM_Position.position_name_en + "" : "-";
                            result.employment_type_id = _getData.TM_Employment_Request.TM_Employment_Type != null ? _getData.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                            result.request_type_id = _getData.TM_Employment_Request.TM_Request_Type != null ? _getData.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";
                            result.target_start = _getData.target_period.HasValue ? _getData.target_period.Value.DateTimebyCulture() : "";
                            result.target_end = _getData.target_period_to.HasValue ? _getData.target_period_to.Value.DateTimebyCulture() : "";
                            result.no_of_head = _getData.no_of_headcount + "";
                            result.job_descriptions = _getData.job_descriptions;
                            result.qualification_experience = _getData.qualification_experience;
                            result.description = _getData.remark;
                            result.code = _getData.RefNo + "";
                            if (_getData.replaced_status + "" == "Y" && !string.IsNullOrEmpty(_getData.user_replaced))
                            {
                                var _getReplaceuser = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_replaced).FirstOrDefault();
                                if (_getReplaceuser != null)
                                {
                                    result.user_no = _getData.user_replaced;
                                    result.user_name = _getReplaceuser.EmpFullName;
                                    result.unit_name = _getReplaceuser.UnitGroup;
                                    result.user_position = _getReplaceuser.Rank;
                                    result.replaced_user = _getData.replaced_status;
                                }

                            }

                            if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any(w => w.active_status == "Y"))
                            {
                                string[] User = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                                var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                                result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y"
                                                        //w.TM_Candidate_Status_Cycle.OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview ||
                                                        //w.TM_Candidate_Status_Cycle.OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Accepted
                                                        )
                                                        from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                        select new vPRCandidates_lstData
                                                        {
                                                            candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                            candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                            owner_name = lstEmp.EmpFullName,
                                                            rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                            remark = lstAD.description + "",
                                                            action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                        }).ToList();
                            }
                        }
                        else
                        {
                            return RedirectToAction("PRApproveList", "PRApprove");
                        }


                    }

                }
            }
            else
            {
                return RedirectToAction("PRApproveList", "PRApprove");
            }

            return View(result);
        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPRApproveList(CSearchPersonnelRequest SearchItem)
        {
            vPersonnelAP_Return result = new vPersonnelAP_Return();
            List<vPersonnelAP_obj> lstData_resutl = new List<vPersonnelAP_obj>();
            bool isAdmin = CGlobal.UserIsAdmin();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            var lstData = _PersonnelRequestService.GetPRForApprove(
            SearchItem.group_code, aDivisionPermission,
            CGlobal.UserInfo.EmployeeNo
            , SearchItem.ref_no
            , isAdmin);
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
                string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                //var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); 
                List<string> aApproveUser = new List<string>();
                aApproveUser.AddRange(lstData.Select(s => s.Req_BUApprove_user).ToList());
                aApproveUser.AddRange(lstData.Select(s => s.Req_HeadApprove_user).ToList());
                aApproveUser.AddRange(lstData.Select(s => s.Req_CeoApprove_user).ToList());
                var _getApprove = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                lstData_resutl = (from lstAD in lstData
                                  from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                      //from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vPersonnelAP_obj
                                  {
                                      refno = lstAD.RefNo + "",
                                      sgroup = lstAD.TM_Divisions.division_name_en + "",
                                      position = lstAD.TM_Position.position_name_en + "",
                                      request_type = lstAD.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "",
                                      rank = lstAD.TM_Pool_Rank.TM_Rank.rank_name_en + "",
                                      pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                      request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                      hc = lstAD.no_of_headcount + "",
                                      request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                      update_user = lstAD.BUApprove_status != "Y" ? _getApprove.FirstOrDefault(w => w.EmpNo == lstAD.Req_BUApprove_user).EmpFullName + ""
                                          : (lstAD.HeadApprove_status != "Y" ? _getApprove.FirstOrDefault(w => w.EmpNo == lstAD.Req_HeadApprove_user).EmpFullName + "" : _getApprove.FirstOrDefault(w => w.EmpNo == lstAD.Req_CeoApprove_user).EmpFullName + ""),
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult ApprovePersonnelRequest(vPersonnelAP_obj_save ItemData, string sMode)
        {
            objPersonnelRequest_Return result = new objPersonnelRequest_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt) && !string.IsNullOrEmpty(sMode) && !string.IsNullOrEmpty(ItemData.app_mode))
                {
                    if (sMode == "N" && (ItemData.app_remark + "").Trim() == "")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please provide the reason why you are rejecting this Personnel Request.";
                        return Json(new
                        {
                            result
                        });
                    }
                    if (sMode != "Y" && sMode != "N")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't find Approve or Reject.";
                        return Json(new
                        {
                            result
                        });
                    }
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    var _getData = _PersonnelRequestService.FindForApprove(nId);
                    if (_getData != null)
                    {
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        //check admin if admin can approve every request
                        #region check ADMIN
                        bool isAdmin = CGlobal.UserIsAdmin();
                        #endregion
                        //reject status
                        var _rejectStatus = _TM_PR_StatusService.Find(5);

                        #region update request
                        if (ItemData.app_mode + "" == "2")
                        {
                            if (_getData.Req_BUApprove_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                            {
                                _getData.BUApprove_user = CGlobal.UserInfo.EmployeeNo;
                                _getData.BUApprove_date = dNow;
                                _getData.BUApprove_remark = (ItemData.app_remark + "").Trim();
                                _getData.BUApprove_status = sMode;


                                if (sMode + "" != "Y")
                                {
                                    _getData.TM_PR_Status = _rejectStatus;
                                    _getData.reject_reason = (ItemData.app_remark + "").Trim();
                                    _getData.reject_user = CGlobal.UserInfo.EmployeeNo;
                                }
                                else if (_getData.Req_HeadApprove_user == CGlobal.UserInfo.EmployeeNo)
                                {
                                    _getData.HeadApprove_user = CGlobal.UserInfo.EmployeeNo;
                                    _getData.HeadApprove_date = dNow;
                                    _getData.HeadApprove_remark = (ItemData.app_remark + "").Trim();
                                    _getData.HeadApprove_status = sMode;
                                    if (_getData.need_ceo_approve + "" != "Y")
                                    {
                                        var _getStatus = _TM_PR_StatusService.Find(3);
                                        _getData.TM_PR_Status = _getStatus;
                                    }
                                    else if (_getData.Req_CeoApprove_user == CGlobal.UserInfo.EmployeeNo)
                                    {
                                        var _getStatus = _TM_PR_StatusService.Find(3);
                                        _getData.TM_PR_Status = _getStatus;
                                        _getData.CeoApprove_user = CGlobal.UserInfo.EmployeeNo;
                                        _getData.CeoApprove_date = dNow;
                                        _getData.CeoApprove_remark = (ItemData.app_remark + "").Trim();
                                        _getData.CeoApprove_status = sMode;
                                    }
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Don't have permission to approve.";
                                return Json(new
                                {
                                    result
                                });
                            }
                        }
                        else if (ItemData.app_mode + "" == "3")
                        {
                            if ((_getData.Req_HeadApprove_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                                && _getData.BUApprove_status + "" == "Y")
                            {
                                _getData.HeadApprove_user = CGlobal.UserInfo.EmployeeNo;
                                _getData.HeadApprove_date = dNow;
                                _getData.HeadApprove_remark = (ItemData.app_remark + "").Trim();
                                _getData.HeadApprove_status = sMode;
                                if (sMode + "" != "Y")
                                {
                                    _getData.TM_PR_Status = _rejectStatus;
                                    _getData.reject_reason = (ItemData.app_remark + "").Trim();
                                    _getData.reject_user = CGlobal.UserInfo.EmployeeNo;
                                }
                                else
                                {
                                    if (_getData.need_ceo_approve + "" != "Y")
                                    {
                                        var _getStatus = _TM_PR_StatusService.Find(3);
                                        _getData.TM_PR_Status = _getStatus;
                                    }
                                    else if (_getData.Req_CeoApprove_user == CGlobal.UserInfo.EmployeeNo)
                                    {
                                        var _getStatus = _TM_PR_StatusService.Find(3);
                                        _getData.TM_PR_Status = _getStatus;
                                        _getData.CeoApprove_user = CGlobal.UserInfo.EmployeeNo;
                                        _getData.CeoApprove_date = dNow;
                                        _getData.CeoApprove_remark = (ItemData.app_remark + "").Trim();
                                        _getData.CeoApprove_status = sMode;
                                    }
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                if (_getData.BUApprove_status + "" != "Y")
                                {
                                    result.Msg = "Error, Request BU appreve.";
                                }
                                else
                                {
                                    result.Msg = "Error, Don't have permission to approve.";
                                }

                                return Json(new
                                {
                                    result
                                });
                            }
                        }
                        else if (ItemData.app_mode + "" == "4")
                        {
                            if ((_getData.Req_CeoApprove_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                                && _getData.BUApprove_status + "" == "Y"
                                && _getData.HeadApprove_status + "" == "Y")
                            {
                                var _getStatus = _TM_PR_StatusService.Find(3);
                                _getData.CeoApprove_user = CGlobal.UserInfo.EmployeeNo;
                                _getData.CeoApprove_date = dNow;
                                _getData.CeoApprove_remark = (ItemData.app_remark + "").Trim();
                                _getData.CeoApprove_status = sMode;
                                _getData.TM_PR_Status = sMode + "" == "Y" ? _getStatus : _rejectStatus;
                                _getData.reject_reason = sMode + "" != "Y" ? (ItemData.app_remark + "").Trim() : "";
                                _getData.reject_user = sMode + "" != "Y" ? CGlobal.UserInfo.EmployeeNo : "";
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                if (_getData.BUApprove_status + "" != "Y"
                                || _getData.HeadApprove_status + "" != "Y")
                                {
                                    result.Msg = "Error, Request BU or HOP  appreve.";
                                }
                                else
                                {
                                    result.Msg = "Error, Don't have permission to approve.";
                                }

                                return Json(new
                                {
                                    result
                                });
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Ref. No : " + _getData.RefNo + "(Place Call Admin)";
                            return Json(new
                            {
                                result
                            });
                        }
                        #endregion
                        var sComplect = _PersonnelRequestService.UpdateApprove(_getData);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            if (sMode + "" == "Y")
                            {
                                result.Msg = "Approval Completed";
                            }
                            else
                            {
                                result.Msg = "Reject Completed";
                            }
                            //send mail


                            string sError = "";
                            string sError2 = "";
                            string mail_to_log = "";
                            string mail_to_log2 = "";
                            if (_getData.TM_PR_Status.Id == 2)
                            {
                                var Mail1 = _MailContentService.Find(1);
                               // var Mail2 = _MailContentService.Find(2);
                                var bSuss = SendRequestMail(_getData, Mail1, ref sError, ref mail_to_log);
                               // var bSuss2 = SendApproveMail(_getData, Mail2, ref sError2, ref mail_to_log2);
                                if (!bSuss /*|| !bSuss2*/)
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, " + sError + sError2;
                                }
                                AddMailLog(_getData, Mail1, sError, mail_to_log, bSuss);
                                //AddMailLog(_getData, Mail2, sError2, mail_to_log2, bSuss2);
                            }
                            else if (_getData.TM_PR_Status.Id == 5)
                            {
                                var Mail4 = _MailContentService.Find(4);
                                var bSuss = SendRejectMail(_getData, Mail4, ref sError, ref mail_to_log);
                                if (!bSuss)
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, " + sError;
                                }
                                AddMailLog(_getData, Mail4, sError, mail_to_log, bSuss);
                            }
                            else if (_getData.TM_PR_Status.Id == 3)
                            {
                                var Mail5 = _MailContentService.Find(5);
                                var bSuss = SendSuccessfullyMail(_getData, Mail5, ref sError, ref mail_to_log);
                                if (!bSuss)
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, " + sError;
                                }
                                AddMailLog(_getData, Mail5, sError, mail_to_log, bSuss);
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
                        result.Msg = "Error, Personnel Request not found.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Personnel Request not found.";
                }
            }
            return Json(new
            {
                result
            });
        }
        private void AddMailLog(Models.MainModels.PersonnelRequest PersonnelRequest, Models.Common.MailContent MailContent, string descriptions, string mail_to_log, bool suss)
        {
            try
            {
                DateTime dNow = DateTime.Now;
                E_Mail_History objSave = new E_Mail_History();
                objSave.PersonnelRequest = PersonnelRequest;
                objSave.MailContent = MailContent;
                objSave.create_user = CGlobal.UserInfo.UserId;
                objSave.update_user = CGlobal.UserInfo.UserId;
                objSave.create_date = dNow;
                objSave.update_date = dNow;
                objSave.sent_status = suss ? "Y" : "N";
                objSave.descriptions = descriptions;
                objSave.mail_to = mail_to_log;
                _E_Mail_HistoryService.CreateNew(objSave);
            }
            catch (Exception e)
            {


            }

        }
        #endregion
    }
}