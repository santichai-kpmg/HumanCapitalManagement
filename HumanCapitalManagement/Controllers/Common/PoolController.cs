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
    public class PoolController : BaseController
    {

        private PoolService _PoolService;
        private TM_Pool_Approve_PermitService _TM_Pool_Approve_PermitService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public PoolController(PoolService PoolService, TM_Pool_Approve_PermitService TM_Pool_Approve_PermitService)
        {
            _PoolService = PoolService;
            _TM_Pool_Approve_PermitService = TM_Pool_Approve_PermitService;
        }
        // GET: Pool
        public ActionResult PoolList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPool result = new vPool();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPool SearchItem = (CSearchPool)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPool)));
                var lstData = _PoolService.GetTM_Pool(
           SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.lstData = (from lstAD in lstData
                                      select new vPool_obj
                                      {
                                          name_en = lstAD.Pool_name_en.StringRemark(500),
                                          short_name_en = lstAD.Pool_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.Pool_description.StringRemark(),
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
        public ActionResult PoolEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vPool_obj_save result = new vPool_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    New_HRISEntities dbHr = new New_HRISEntities();
                    var _getData = _PoolService.Find(nId);
                    if (_getData != null)
                    {
                        int CodeID = SystemFunction.GetIntNullToZero(_getData.Pool_code);
                        var lst = dbHr.tbMaster_Pool.Where(w => w.ID == CodeID).FirstOrDefault();
                        if (lst != null)
                        {
                            result.name_en_hris = lst.Name + "";
                            result.short_name_en_hris = lst.ShortName + "";
                        }
                        result.IdEncrypt = id;
                        result.code = _getData.Pool_code + "";
                        result.name_en = _getData.Pool_name_en + "";
                        result.short_name_en = _getData.Pool_short_name_en + "";
                        result.description = _getData.Pool_description + "";
                        result.active_status = _getData.active_status;

                        if (_getData.TM_Pool_Approve_Permit != null && _getData.TM_Pool_Approve_Permit.Any(a => a.active_status == "Y"))
                        {
                            string[] UserID = _getData.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                            result.lstData = (from lstAD in _getData.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y")
                                              from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                              select new vPool_Approve_Permit
                                              {
                                                  emp_name = lstAD.user_no + " : " + lstUnit.EmpFullName,
                                                  emp_group = lstUnit.UnitGroup,
                                                  emp_position = lstUnit.Rank,
                                                  emp_dec = lstAD.description.StringRemark(45),
                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                              }).ToList();
                        }
                    }

                }
            }
            return View(result);

            #endregion

        }
        #region Ajax Function
        [HttpPost]
        public ActionResult EditPool(vPool_obj_save ItemData)
        {
            objPool_Return result = new objPool_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PoolService.Find(nId);
                    if (_getData != null)
                    {
                        TM_Pool objSave = new TM_Pool()
                        {
                            Id = nId,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            Pool_description = ItemData.description,
                            Pool_name_en = ItemData.name_en,
                            Pool_short_name_en = ItemData.short_name_en,
                        };
                        if (_PoolService.CanSave(objSave))
                        {
                            var sComplect = _PoolService.Update(objSave);
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
                            result.Msg = "Duplicate Pool name.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Pool Not Found.";
                    }

                }

            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult LoadPoolList(CSearchPool SearchItem)
        {
            vPool_Return result = new vPool_Return();
            List<vPool_obj> lstData_resutl = new List<vPool_obj>();
            var lstData = _PoolService.GetTM_Pool(
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
                                  select new vPool_obj
                                  {
                                      name_en = lstAD.Pool_name_en.StringRemark(500),
                                      short_name_en = lstAD.Pool_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.Pool_description.StringRemark(),
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
        public JsonResult UserAutoComplete(string SearchItem, string sQueryID)
        {
            List<C_USERS_RETURN> result = new List<C_USERS_RETURN>();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            string[] aAllUser = new string[] { };
            int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(sQueryID + ""));
            var _getGroup = _PoolService.Find(nId);
            if (_getGroup != null && _getGroup.TM_Pool_Approve_Permit != null && _getGroup.TM_Pool_Approve_Permit.Any(a => a.active_status == "Y"))
            {
                aAllUser = _getGroup.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
            }
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && !aAllUser.Contains(w.Employeeno));

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

        [HttpPost]
        public ActionResult EditPoolApprover(vPoolApprover_obj_save ItemData)
        {
            objPool_Return result = new objPool_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _CheckUser = dbHr.Employee.Where(w => w.Employeeno == ItemData.emp_no && (w.C_Status == "1" || w.C_Status == "3")).FirstOrDefault();
                    if (_CheckUser != null)
                    {
                        var _getData = _PoolService.Find(nId);
                        if (_getData != null)
                        {

                            TM_Pool_Approve_Permit objSave = new TM_Pool_Approve_Permit()
                            {
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                active_status = "Y",
                                description = ItemData.description,
                                user_id = _CheckUser.UserID,
                                user_no = _CheckUser.Employeeno,
                                TM_Pool = _getData,
                            };

                            var sComplect = _TM_Pool_Approve_PermitService.AddAndUpdateApprover(objSave);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _PoolService.Find(nId);
                                if (_getEditList != null && _getEditList.TM_Pool_Approve_Permit != null && _getEditList.TM_Pool_Approve_Permit.Any(a => a.active_status == "Y"))
                                {
                                    string[] UserID = _getEditList.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                    var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                                    result.lstData = (from lstAD in _getEditList.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y")
                                                      from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                      select new vPool_Approve_Permit
                                                      {
                                                          emp_name = lstAD.user_no + " : " + lstUnit.EmpFullName,
                                                          emp_group = lstUnit.UnitGroup,
                                                          emp_position = lstUnit.Rank,
                                                          emp_dec = lstAD.description.StringRemark(45),
                                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                      }).ToList();
                                }
                                else
                                {
                                    result.lstData = new List<vPool_Approve_Permit>();
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
                            result.Msg = "Error, Group Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, User Not Found.";
                    }


                }

            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult DelPoolApprover(string ItemData)
        {
            objPool_Return result = new objPool_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData + ""));
                if (nId != 0)
                {

                    var _Get_TM_Pool_Approve_PermitService = _TM_Pool_Approve_PermitService.Find(nId);
                    if (_Get_TM_Pool_Approve_PermitService != null)
                    {
                        _Get_TM_Pool_Approve_PermitService.active_status = "N";
                        _Get_TM_Pool_Approve_PermitService.update_user = CGlobal.UserInfo.UserId;
                        _Get_TM_Pool_Approve_PermitService.update_date = dNow;

                        var sComplect = _TM_Pool_Approve_PermitService.AddAndUpdateApprover(_Get_TM_Pool_Approve_PermitService);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getEditList = _PoolService.Find(_Get_TM_Pool_Approve_PermitService.TM_Pool.Id);
                            if (_getEditList != null && _getEditList.TM_Pool_Approve_Permit != null && _getEditList.TM_Pool_Approve_Permit.Any(a => a.active_status == "Y"))
                            {
                                string[] UserID = _getEditList.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                                result.lstData = (from lstAD in _getEditList.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y")
                                                  from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                  select new vPool_Approve_Permit
                                                  {
                                                      emp_name = lstAD.user_no + " : " + lstUnit.EmpFullName,
                                                      emp_group = lstUnit.UnitGroup,
                                                      emp_position = lstUnit.Rank,
                                                      emp_dec = lstAD.description.StringRemark(45),
                                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                  }).ToList();
                            }
                            else
                            {
                                result.lstData = new List<vPool_Approve_Permit>();
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
                        result.Msg = "Error, User Not Found.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Group Not Found.";
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