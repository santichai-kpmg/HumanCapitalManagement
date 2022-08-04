using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Controllers.MainController;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.IO;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.MainModels;

namespace HumanCapitalManagement.Controllers.MainController
{

    public class FYPlanController : BaseController
    {

        private TM_FY_PlanService _TM_FY_PlanService;
        private TM_FY_DetailService _TM_FY_DetailService;
        private DivisionService _DivisionService;
        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();


        public FYPlanController(TM_FY_PlanService TM_FY_PlanService, TM_FY_DetailService TM_FY_DetailService, DivisionService DivisionService)
        {
            _TM_FY_PlanService = TM_FY_PlanService;
            _TM_FY_DetailService = TM_FY_DetailService;
            _DivisionService = DivisionService;
        }

        [HttpPost]
        public ActionResult CreateFYPlan(vFY_Plan_obj_save ItemData)
        {
            vFYPlan_Return result = new vFYPlan_Return();

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
                if (!string.IsNullOrEmpty(ItemData.fy_year))
                {
                    DateTime? FYPlanYear = null;

                    if (!string.IsNullOrEmpty(ItemData.fy_year))
                    {
                        FYPlanYear = SystemFunction.ConvertStringToDateTime("01-Jan-" + (ItemData.fy_year).Trim(), "");
                    }


                    TM_FY_Plan objSave = new TM_FY_Plan()
                    {

                        fy_year = FYPlanYear,
                        active_status = "Y",
                        create_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        update_user = CGlobal.UserInfo.UserId,

                    };



                    if (_TM_FY_PlanService.CanSave(objSave))
                    {
                        var sComplect = _TM_FY_PlanService.CreateNew(ref objSave);
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


                }

            }


            return Json(new
            {
                result
            });

            //return View(result);
        }

        /*
                 [HttpPost]
        public ActionResult CreatePlan(vPTRMailBox_obj_save ItemData)
        {
            vPTRMailBox_Return result = new vPTRMailBox_Return();

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
                    PTR_Evaluation_Year objSave = new PTR_Evaluation_Year()
                    {
                        update_user = CGlobal.UserInfo.UserId,
                        update_date = dNow,
                        create_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        active_status = "Y",
                        evaluation_year = dAction,

                    };
                    if (_PTR_Evaluation_YearService.CanSave(objSave))
                    {
                        var sComplect = _PTR_Evaluation_YearService.CreateNew(ref objSave);
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
                        result.Msg = "Duplicate Type name.";
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
             */

        [HttpPost]
        public ActionResult CreateFYPlanDetail(vFY_Plan_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vFYPlan_Return result = new vFYPlan_Return();


            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }

            if (ItemData != null && !string.IsNullOrEmpty(ItemData.IdEncrypt))
            {

                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));

                if (nId != 0)
                {
                    var _GetData = _TM_FY_PlanService.Find(nId);
                    if (_GetData != null)
                    {
                        _GetData.update_user = CGlobal.UserInfo.UserId;
                        _GetData.update_date = dNow;

                        List<TM_FY_Detail> lstAns = new List<TM_FY_Detail>();
                        if (ItemData.lstData != null)
                        {
                            string[] aUnit = ItemData.lstData.Select(s => s.group_code).ToArray();
                            var _GetDivision = _DivisionService.GetDivisionForSave(aUnit);
                            lstAns = (from lstA in ItemData.lstData
                                      from lstG in _GetDivision.Where(w => w.division_code == lstA.group_code).DefaultIfEmpty(new Models.Common.TM_Divisions())
                                      select new TM_FY_Detail
                                      {
                                          update_user = CGlobal.UserInfo.UserId,
                                          update_date = dNow,
                                          create_user = CGlobal.UserInfo.UserId,
                                          create_date = dNow,
                                          active_status = "Y",
                                          TM_Divisions = lstG != null ? lstG : null,
                                          TM_FY_Plan = _GetData,
                                          para = SystemFunction.GetDecimalNull(lstA.para),
                                          aa = SystemFunction.GetDecimalNull(lstA.aa),
                                          ad = SystemFunction.GetDecimalNull(lstA.ad),
                                          am = SystemFunction.GetDecimalNull(lstA.am),
                                          dir = SystemFunction.GetDecimalNull(lstA.dir),
                                          mgr = SystemFunction.GetDecimalNull(lstA.mgr),
                                          ptr = SystemFunction.GetDecimalNull(lstA.ptr),
                                          sr = SystemFunction.GetDecimalNull(lstA.sr),
                                      }).ToList();
                        }



                        if (_TM_FY_PlanService.CanSave(_GetData))
                        {
                            var sComplete = _TM_FY_PlanService.Update(_GetData);
                            if (sComplete > 0)
                            {
                                var sCompleteAns = _TM_FY_DetailService.UpdateAnswer(lstAns, _GetData.Id, CGlobal.UserInfo.UserId, dNow);
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
                            result.Msg = "Duplicate FY Plan.";
                        }

                    }
                }



            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Approval.";
            }

            return Json(new
            {
                result
            });
        }




        // GET: FYPlan
        public ActionResult FYPlan(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            vFYPlan result = new vFYPlan();
            #region main code
            //if (!string.IsNullOrEmpty(qryStr))
            //{
            //    CSearchFYPlan SearchItem = (CSearchFYPlan)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchFYPlan)));
            //    var lstData = _TM_FY_PlanService.GetDataForSelect();

            //    string BackUrl = Uri.EscapeDataString(qryStr);
            //    if (lstData.Any())
            //    {

            //        result.lstData = (from lstAD in lstData
            //                          select new vFYPlan_obj
            //                          {
            //                              fy_year = lstAD.fy_year.HasValue ? lstAD.fy_year.Value.DateTimebyCulture("yyyy") : "",
            //                              active_status = lstAD.active_status != null ? lstAD.active_status : "",
            //                              create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
            //                              create_user = lstAD.create_user + "",
            //                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
            //                              update_user = lstAD.update_user + "",
            //                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
            //                          }).ToList();

            //    }
            //}

            #endregion
            return View(result);


        }

        public ActionResult FYPlanList(CSearchFYPlan SearchItem)
        {

            vFYPlan_Return result = new vFYPlan_Return();
            List<vFYPlan_obj> lstData_result = new List<vFYPlan_obj>();

            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            var lstData = _TM_FY_PlanService.GetFYPlanList(nYear);

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

                lstData_result = (from lstAD in lstData
                                  select new vFYPlan_obj
                                  {
                                      fy_year = lstAD.fy_year.HasValue ? lstAD.fy_year.Value.DateTimebyCulture("yyyy") : "",
                                      active_status = lstAD.active_status != null ? lstAD.active_status : "",
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      create_user = lstAD.create_user + "",
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstAD.update_user + "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();

                result.lstData = lstData_result.ToList();

            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }


        public ActionResult CreateddlFYPlanActiveYear(vSelect item)
        {
            vSelect lstData = new vSelect();

            var lst = _TM_FY_PlanService.GetDataForSelect().ToList();

            if (lst != null && lst.Any())
            {
                var dNow = DateTime.Now;
                var _getDefault = lst.Where(w => w.fy_year.Value.Year == dNow.Year).FirstOrDefault();
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";

                lstData.value = _getDefault != null ? _getDefault.Id + "" : item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.Where(w => w.fy_year.HasValue).OrderByDescending(o => o.fy_year).Select(s => new lstDataSelect { value = s.Id + "", text = s.fy_year.Value.Year + "" }).ToList();
            }

            return PartialView("_select", lstData);
        }

        public ActionResult FYPlanEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vFY_Plan_obj_save result = new vFY_Plan_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _TM_FY_PlanService.Find(nId);
                    if (_getData != null)
                    {
                        result.IdEncrypt = qryStr;
                        var _getGroup = _DivisionService.GetAll();
                        if (_getGroup.Any())
                        {
                            result.lstData = _getGroup.Where(w => w.TM_Pool.TM_Company.Company_code == "4100").Select(s => new vFY_Plan_detail
                            {
                                Id = s.Id + "",
                                division = s.TM_Pool.Pool_name_en,
                                sgroup = s.division_name_en,
                                group_code = s.division_code

                            }).ToList();

                            if (_getData.TM_FY_Detail != null && _getData.TM_FY_Detail.Any())
                            {
                                result.lstData.ForEach(ed =>
                                {
                                    var GetGroup = _getData.TM_FY_Detail.Where(w => w.TM_Divisions.division_code == ed.group_code).FirstOrDefault();
                                    if (GetGroup != null)
                                    {
                                        ed.para = GetGroup.para.NodecimalFormat();
                                        ed.aa = GetGroup.aa.NodecimalFormat();
                                        ed.sr = GetGroup.sr.NodecimalFormat();
                                        ed.am = GetGroup.am.NodecimalFormat();
                                        ed.mgr = GetGroup.mgr.NodecimalFormat();
                                        ed.ad = GetGroup.ad.NodecimalFormat();
                                        ed.dir = GetGroup.dir.NodecimalFormat();
                                        ed.ptr = GetGroup.ptr.NodecimalFormat();
                                    }


                                });

                            }
                        }

                    }
                }
            }
            return View(result);

            #endregion
        }


        [HttpPost]
        public ActionResult LoadFYPlanList(CSearchFYPlan SearchItem)
        {

            vFYPlan_Return result = new vFYPlan_Return();
            List<vFYPlan_obj> lstData_result = new List<vFYPlan_obj>();

            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);


            var lstData = _TM_FY_PlanService.GetFYPlanList(nYear);




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


                lstData_result = (from lstAD in lstData
                                  select new vFYPlan_obj
                                  {

                                      fy_year = lstAD.fy_year.HasValue ? lstAD.fy_year.Value.DateTimebyCulture("yyyy") : "",
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      create_user = lstAD.create_user == null ? lstAD.create_user : "",
                                      active_status = lstAD.active_status == null ? lstAD.active_status : "",
                                      update_user = lstAD.update_user + "",
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",

                                  }).ToList();
                result.lstData = lstData_result.ToList();
            }


            result.Status = SystemFunction.process_Success;

            return Json(new { result });

        }

    }
}