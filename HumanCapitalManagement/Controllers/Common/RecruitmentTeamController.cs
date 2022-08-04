using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class RecruitmentTeamController : BaseController
    {
        private TM_Recruitment_TeamService _TM_Recruitment_TeamService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public RecruitmentTeamController(TM_Recruitment_TeamService TM_Recruitment_TeamService)
        {
            _TM_Recruitment_TeamService = TM_Recruitment_TeamService;
        }
        // GET: RecruitmentTeam

        public ActionResult RecruitmentTeamList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vRecruitmentTeam result = new vRecruitmentTeam();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchRecruitmentTeam SearchItem = (CSearchRecruitmentTeam)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchRecruitmentTeam)));
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
                string[] UserNo = null;
                if (!string.IsNullOrEmpty(SearchItem.name))
                {
                    UserNo = sQuery.Where(w =>
                           (
                           (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                           ).Contains((SearchItem.name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                            ).Select(s => s.UserID).ToArray();
                }

                var lstData = _TM_Recruitment_TeamService.GetRecruitmentList(
           UserNo,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    string[] UserID = lstData.Select(s => s.user_no).ToArray();
                    var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                    result.lstData = (from lstAD in lstData
                                      from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      select new vRecruitmentTeam_obj
                                      {
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          name_en = lstAD.user_no + " : " + lstUnit.EmpFullName,
                                          group_name = lstUnit.UnitGroup,
                                          rank = lstUnit.Rank,
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                          update_user = lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();

                }
            }

            #endregion
            return View(result);
        }
        public ActionResult RecruitmentTeamCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vRecruitmentTeam_obj_save result = new vRecruitmentTeam_obj_save();
            result.Id = 0;
            result.active_status = "Y";
            return View(result);
        }
        public ActionResult RecruitmentTeamEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vRecruitmentTeam_obj_save result = new vRecruitmentTeam_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Recruitment_TeamService.Find(nId);
                    if (_getData != null)
                    {
                        var _Getuser = dbHr.Employee.Where(w => w.Employeeno == _getData.user_no).FirstOrDefault();
                        var _getUnit = dbHr.JobInfo.Where(w => w.Employeeno == _getData.user_no).FirstOrDefault();
                        result = new vRecruitmentTeam_obj_save
                        {
                            user_no = _getData.user_no,
                            active_status = _getData.active_status,
                            IdEncrypt = HCMFunc.Encrypt(_getData.Id + ""),
                            user_name = _Getuser != null ? _Getuser.Employeename + " " + _Getuser.Employeesurname : "",
                            unit_name = _getUnit != null ? _getUnit.UnitGroup : "",
                            description = _getData.User_description,
                            user_id = _getData.user_id,

                        };
                    }
                }
            }
            return View(result);

            #endregion

        }

        #region Ajax Function
        [HttpPost]
        public ActionResult LoadRecruitmentTeamList(CSearchRecruitmentTeam SearchItem)
        {
            vRecruitmentTeam_Return result = new vRecruitmentTeam_Return();
            List<vRecruitmentTeam_obj> lstData_resutl = new List<vRecruitmentTeam_obj>();
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3"));
            string[] UserNo = null;
            if (!string.IsNullOrEmpty(SearchItem.name))
            {
                UserNo = sQuery.Where(w =>
                       (
                       (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                       ).Contains((SearchItem.name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                        ).Select(s => s.UserID).ToArray();
            }

            var lstData = _TM_Recruitment_TeamService.GetRecruitmentList(
       UserNo,
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
                string[] UserID = lstData.Select(s => s.user_no).ToArray();
                var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                lstData_resutl = (from lstAD in lstData
                                  from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                  select new vRecruitmentTeam_obj
                                  {
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      name_en = lstAD.user_no + " : " + lstUnit.EmpFullName,
                                      group_name = lstUnit.UnitGroup,
                                      rank = lstUnit.Rank,
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                      update_user = lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult CreateRecruitmentTeam(vRecruitmentTeam_obj_save ItemData)
        {
            vRecruitmentTeam_Return result = new vRecruitmentTeam_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.user_id))
                {
                    TM_Recruitment_Team objSave = new TM_Recruitment_Team()
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        User_description = ItemData.description,
                        user_id = ItemData.user_id,
                        user_no = ItemData.user_no,
                    };
                    if (_TM_Recruitment_TeamService.CanSave(objSave))
                    {
                        var sComplect = _TM_Recruitment_TeamService.CreateNew(objSave);
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
                    else
                    {
                        result.Status = SystemFunction.process_Duplicate;
                        result.Msg = "Staff already exist, please try again.";
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
        public ActionResult EditRecruitmentTeam(vRecruitmentTeam_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vRecruitmentTeam_Return result = new vRecruitmentTeam_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Recruitment_TeamService.Find(nId);
                    if (_getData != null)
                    {
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = ItemData.active_status;
                        _getData.User_description = ItemData.description;
                        if (_TM_Recruitment_TeamService.CanSave(_getData))
                        {
                            var sComplect = _TM_Recruitment_TeamService.Update(_getData);
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
                        else
                        {
                            result.Status = SystemFunction.process_Duplicate;
                            result.Msg = "Duplicate Type name.";
                        }
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
        public JsonResult RecruitmentAutoComplete(string SearchItem)
        {
            List<C_USERS_RETURN> result = new List<C_USERS_RETURN>();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            string[] aAllUser = _TM_Recruitment_TeamService.GetAllRecruitments().Select(s => s.user_id).ToArray();
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && !aAllUser.Contains(w.UserID));

            var _getData = sQuery.Where(w =>
            (
            (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
            ).Contains((SearchItem + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
            (w.Employeename + "").Trim().ToLower() +
            (w.Employeesurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
            ).Take(15).ToList();
            if (_getData.Any())
            {
                string[] aNo = _getData.Select(s => s.Employeeno).ToArray();
                var _getUnit = dbHr.JobInfo.Where(w => aNo.Contains(w.Employeeno)).ToList();
                result = (from lstGt in _getData
                          from lstUnit in _getUnit.Where(w => w.Employeeno == lstGt.Employeeno).DefaultIfEmpty(new JobInfo())
                          select new C_USERS_RETURN
                          {
                              id = lstGt.Employeeno,
                              user_id = lstGt.UserID,
                              user_name = lstGt.Employeename + " " + lstGt.Employeesurname,
                              unit_name = lstUnit.UnitGroup
                          }
                    ).ToList();
                //_getData.Select(s => new C_USERS_RETURN
                //    {
                //        id = s.Employeeno,
                //        user_id = s.UserID,
                //        user_name = s.Employeename,
                //        user_last_name = s.Employeesurname
                //    }).ToList();
            }
            #endregion
            //return result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}