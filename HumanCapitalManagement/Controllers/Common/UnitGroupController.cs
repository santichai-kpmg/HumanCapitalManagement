using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class UnitGroupController : BaseController
    {
        private DivisionService _DivisionService;
        private TM_UnitGroup_Approve_PermitService _TM_UnitGroup_Approve_PermitService;
        private PoolService _PoolService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public UnitGroupController(DivisionService DivisionService, PoolService PoolService, TM_UnitGroup_Approve_PermitService TM_UnitGroup_Approve_PermitService)
        {
            _DivisionService = DivisionService;
            _PoolService = PoolService;
            _TM_UnitGroup_Approve_PermitService = TM_UnitGroup_Approve_PermitService;
        }
        // GET: UnitGroup
        public ActionResult UnitGroupList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vUnitGroup result = new vUnitGroup();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchUnit_Group SearchItem = (CSearchUnit_Group)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchUnit_Group)));
                var lstData = _DivisionService.GetTM_Divisions(
           SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.lstData = (from lstAD in lstData
                                      select new vUnitGroup_obj
                                      {
                                          name_en = lstAD.division_name_en.StringRemark(20),
                                          short_name_en = lstAD.division_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          description = lstAD.division_description.StringRemark(),
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

        public ActionResult UnitGroupCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vUnitGroup_obj_save result = new vUnitGroup_obj_save();
            result.Id = 0;
            result.active_status = "Y";

            return View(result);
        }

        public ActionResult UnitGroupEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vUnitGroup_obj_save result = new vUnitGroup_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    New_HRISEntities dbHr = new New_HRISEntities();
                    var _getData = _DivisionService.Find(nId);
                    if (_getData != null)
                    {
                        int CodeID = SystemFunction.GetIntNullToZero(_getData.division_code);
                        var lst = dbHr.tbMaster_UnitGroup.Where(w => w.ID == CodeID).FirstOrDefault();
                        if (lst != null)
                        {
                            result.name_en_hris = lst.Name + "";
                            result.short_name_en_hris = lst.ShortName + "";
                            var _getPool = dbHr.tbMaster_Pool.Where(w => w.ID == lst.PoolID).FirstOrDefault();
                            if (_getPool != null)
                            {
                                result.pool_name = _getPool.Name + "";
                            }

                        }
                        result.IdEncrypt = id;
                        result.code = _getData.division_code + "";
                        result.name_en = _getData.division_name_en + "";
                        result.short_name_en = _getData.division_short_name_en + "";
                        result.description = _getData.division_description + "";
                        result.active_status = _getData.active_status;
                        result.lstgrouphead = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstpractice = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.lstceo = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.group_head_id = "";
                        result.practice_id = "";
                        result.ceo_id = "";
                        result.remark_group_approve = _getData.approve_description;



                        if (_getData.TM_UnitGroup_Approve_Permit != null && _getData.TM_UnitGroup_Approve_Permit.Any(a => a.active_status == "Y"))
                        {
                            string[] UserID = _getData.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                            result.lstData = (from lstAD in _getData.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y")
                                              from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                              select new vUnitGroup_Approve_Permit
                                              {
                                                  emp_name = lstAD.user_no + " : " + lstUnit.EmpFullName,
                                                  emp_group = lstUnit.UnitGroup,
                                                  emp_position = lstUnit.Rank,
                                                  emp_dec = lstAD.description.StringRemark(45),
                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                              }).ToList();

                            result.lstgrouphead.AddRange((from lstAD in _getData.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y")
                                                          from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                          select new vSelect_PR
                                                          {
                                                              id = lstAD.user_no,
                                                              name = lstUnit.EmpFullName,
                                                          }).OrderBy(o => o.name).ToList());
                            if (_getData.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray().Contains(_getData.default_grouphead + ""))
                            {
                                result.group_head_id = _getData.default_grouphead + "";
                            }

                        }
                        if (_getData.TM_Pool != null && _getData.TM_Pool.TM_Pool_Approve_Permit != null && _getData.TM_Pool.TM_Pool_Approve_Permit.Any(a => a.active_status == "Y"))
                        {
                            string[] UserID = _getData.TM_Pool.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                            result.lstpractice.AddRange((from lstAD in _getData.TM_Pool.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y")
                                                         from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                         select new vSelect_PR
                                                         {
                                                             id = lstAD.user_no,
                                                             name = lstUnit.EmpFullName,
                                                         }).OrderBy(o => o.name).ToList());
                            if (_getData.TM_Pool.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray().Contains(_getData.default_practice + ""))
                            {
                                result.practice_id = _getData.default_practice + "";
                            }
                        }
                        if (_getData.TM_Pool != null && _getData.TM_Pool.TM_Company != null && _getData.TM_Pool.TM_Company.TM_Company_Approve_Permit != null && _getData.TM_Pool.TM_Company.TM_Company_Approve_Permit.Any(a => a.active_status == "Y"))
                        {
                            string[] UserID = _getData.TM_Pool.TM_Company.TM_Company_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                            result.lstceo.AddRange((from lstAD in _getData.TM_Pool.TM_Company.TM_Company_Approve_Permit.Where(w => w.active_status == "Y")
                                                    from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vSelect_PR
                                                    {
                                                        id = lstAD.user_no,
                                                        name = lstUnit.EmpFullName,
                                                    }).OrderBy(o => o.name).ToList());
                            if (_getData.TM_Pool.TM_Company.TM_Company_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray().Contains(_getData.default_ceo + ""))
                            {
                                result.ceo_id = _getData.default_ceo + "";
                            }
                        }
                    }

                }
            }
            return View(result);

            #endregion

        }

        #region Ajax Function

        [HttpPost]
        public ActionResult CreateUnitGroup(vUnitGroup_obj_save ItemData)
        {
            objUnitGroup_Return result = new objUnitGroup_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;

                if (!string.IsNullOrEmpty(ItemData.code))
                {
                    int nId = SystemFunction.GetIntNullToZero(ItemData.code + "");
                    var _getUnitHRis = dbHr.tbMaster_Unit.Where(w => w.UnitGroupID == nId).FirstOrDefault();
                    if (_getUnitHRis == null)
                    {

                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,cannot find company.";
                        return Json(new
                        {
                            result
                        });

                    }
                    var _getPool = _PoolService.FindByCode(ItemData.pool, _getUnitHRis.tbMaster_Company.CountryCode + "");
                    if (_getPool != null)
                    {

                        TM_Divisions objSave = new TM_Divisions()
                        {
                            Id = 0,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            division_code = (ItemData.code + "").Trim(),
                            division_description = ItemData.description,
                            division_name_en = (ItemData.name_en + "").Trim(),
                            division_short_name_en = (ItemData.short_name_en + "").Trim(),
                            TM_Pool = _getPool,
                        };
                        if (_DivisionService.CanSave(objSave))
                        {
                            var sComplect = _DivisionService.CreateNew(objSave);
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
                            result.Msg = "Duplicate group name.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,plz select pool";
                    }

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error,plz select group";
                }
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult EditUnitGroup(vUnitGroup_obj_save ItemData)
        {
            objUnitGroup_Return result = new objUnitGroup_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _DivisionService.Find(nId);
                    if (_getData != null)
                    {
                        var _getUnitHRis = dbHr.tbMaster_Unit.Where(w => w.UnitGroupID + "" == _getData.division_code).FirstOrDefault();
                        if (_getUnitHRis == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error,cannot find company.";
                            return Json(new
                            {
                                result
                            });
                        }
                        try
                        {
                            var _getPool = _PoolService.FindByCode(_getData.TM_Pool.Pool_code, _getUnitHRis.tbMaster_Company.CountryCode + "");
                            if (_getPool != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.active_status = ItemData.active_status;
                                _getData.division_description = ItemData.description;
                                _getData.division_name_en = ItemData.name_en;
                                _getData.division_short_name_en = ItemData.short_name_en;
                                _getData.TM_Pool = _getPool;
                                _getData.default_grouphead = ItemData.group_head_id;
                                _getData.default_practice = ItemData.practice_id;
                                _getData.default_ceo = ItemData.ceo_id;
                                _getData.approve_description = ItemData.remark_group_approve;
                                if (_DivisionService.CanSave(_getData))
                                {
                                    var sComplect = _DivisionService.Update(_getData);
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
                                    result.Msg = "Duplicate group name.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error,plz select pool";
                            }


                        }
                        catch (Exception ex)
                        {
                            var asdf = ex.Message;
                        }
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
        [HttpPost]
        public ActionResult LoadUnitGroupList(CSearchUnit_Group SearchItem)
        {
            vUnitGroup_Return result = new vUnitGroup_Return();
            List<vUnitGroup_obj> lstData_resutl = new List<vUnitGroup_obj>();
            var lstData = _DivisionService.GetTM_Divisions(
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
                                  select new vUnitGroup_obj
                                  {
                                      name_en = lstAD.division_name_en.StringRemark(20),
                                      short_name_en = lstAD.division_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      description = lstAD.division_description.StringRemark(),
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
        public JsonResult GetUnitDetail(string SearchItem)
        {
            UnitGroup_Api result = new UnitGroup_Api();
            #region แก้ไขข้อมูลในการค้นหา

            SearchItem = (SearchItem + "").Trim().ToLower();
            New_HRISEntities dbHr = new New_HRISEntities();
            int nID = SystemFunction.GetIntNullToZero(SearchItem);
            var lst = dbHr.tbMaster_UnitGroup.Where(w => w.IsActive == true && w.ID == nID).FirstOrDefault();
            if (lst != null)
            {
                result.code = lst.ID + "";
                result.pool_id = lst.PoolID + "";
                result.group_name_hr = lst.Name + "";
                result.group_sh_name_hr = lst.ShortName + "";
                var _getPool = dbHr.tbMaster_Pool.Where(w => w.ID == lst.PoolID).FirstOrDefault();
                if (_getPool != null)
                {
                    result.pool_name = _getPool.Name + "";
                }

            }
            #endregion

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UserAutoComplete(string SearchItem, string sQueryID)
        {
            List<C_USERS_RETURN> result = new List<C_USERS_RETURN>();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            string[] aAllUser = new string[] { };
            int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(sQueryID + ""));
            var _getGroup = _DivisionService.Find(nId);
            if (_getGroup != null && _getGroup.TM_UnitGroup_Approve_Permit != null && _getGroup.TM_UnitGroup_Approve_Permit.Any(a => a.active_status == "Y"))
            {
                aAllUser = _getGroup.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
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
        public ActionResult EditUniApprover(vUnitApprover_obj_save ItemData)
        {
            objUnitGroup_Return result = new objUnitGroup_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _CheckUser = dbHr.Employee.Where(w => w.Employeeno == ItemData.emp_no && (w.C_Status == "1" || w.C_Status == "3")).FirstOrDefault();
                    if (_CheckUser != null)
                    {
                        var _getData = _DivisionService.Find(nId);
                        if (_getData != null)
                        {

                            TM_UnitGroup_Approve_Permit objSave = new TM_UnitGroup_Approve_Permit()
                            {
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                active_status = "Y",
                                description = ItemData.description,
                                user_id = _CheckUser.UserID,
                                user_no = _CheckUser.Employeeno,
                                TM_Divisions = _getData,
                            };

                            var sComplect = _TM_UnitGroup_Approve_PermitService.AddAndUpdateApprover(objSave);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _DivisionService.Find(nId);
                                if (_getEditList != null && _getEditList.TM_UnitGroup_Approve_Permit != null && _getEditList.TM_UnitGroup_Approve_Permit.Any(a => a.active_status == "Y"))
                                {
                                    string[] UserID = _getEditList.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                    var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                                    result.lstData = (from lstAD in _getEditList.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y")
                                                      from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                      select new vUnitGroup_Approve_Permit
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
                                    result.lstData = new List<vUnitGroup_Approve_Permit>();
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
        public ActionResult DelUniApprover(string ItemData)
        {
            objUnitGroup_Return result = new objUnitGroup_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData + ""));
                if (nId != 0)
                {

                    var _Get_TM_UnitGroup_Approve_PermitService = _TM_UnitGroup_Approve_PermitService.Find(nId);
                    if (_Get_TM_UnitGroup_Approve_PermitService != null)
                    {
                        _Get_TM_UnitGroup_Approve_PermitService.active_status = "N";
                        _Get_TM_UnitGroup_Approve_PermitService.update_user = CGlobal.UserInfo.UserId;
                        _Get_TM_UnitGroup_Approve_PermitService.update_date = dNow;

                        var sComplect = _TM_UnitGroup_Approve_PermitService.AddAndUpdateApprover(_Get_TM_UnitGroup_Approve_PermitService);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getEditList = _DivisionService.Find(_Get_TM_UnitGroup_Approve_PermitService.TM_Divisions.Id);
                            if (_getEditList != null && _getEditList.TM_UnitGroup_Approve_Permit != null && _getEditList.TM_UnitGroup_Approve_Permit.Any(a => a.active_status == "Y"))
                            {
                                string[] UserID = _getEditList.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                                result.lstData = (from lstAD in _getEditList.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y")
                                                  from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                  select new vUnitGroup_Approve_Permit
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
                                result.lstData = new List<vUnitGroup_Approve_Permit>();
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