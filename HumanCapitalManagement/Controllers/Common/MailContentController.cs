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
    public class MailContentController : BaseController
    {
        private wsEmail2.EMail_NewSoapClient wsEmail2 = new wsEmail2.EMail_NewSoapClient();
        private MailContentService _MailContentService;
        private TM_MailContent_CcService _TM_MailContent_CcService;
        private TM_MailContent_Cc_bymailService _TM_MailContent_Cc_bymailService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public MailContentController(MailContentService MailContentService,
            TM_MailContent_CcService TM_MailContent_CcService,
            TM_MailContent_Cc_bymailService TM_MailContent_Cc_bymailService
            )
        {
            _MailContentService = MailContentService;
            _TM_MailContent_CcService = TM_MailContent_CcService;
            _TM_MailContent_Cc_bymailService = TM_MailContent_Cc_bymailService;
        }
        // GET: MailContent
        public ActionResult MailContentList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vMailContent result = new vMailContent();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchMailContent SearchItem = (CSearchMailContent)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchMailContent)));
                var lstData = _MailContentService.GetMailContent(
               SearchItem.name,
           SearchItem.active_status);
                result.active_status = SearchItem.active_status;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    result.lstData = (from lstAD in lstData
                                      select new vMailContent_obj
                                      {
                                          name_en = lstAD.mail_type.StringRemark(500),
                                          description = lstAD.mail_header,
                                          //  short_name_en = lstAD.Company_short_name_en,
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          // description = lstAD.sub_group_description.StringRemark(),
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
        public ActionResult MailContentEdit(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vMailContent_obj_save result = new vMailContent_obj_save();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _MailContentService.Find(nId);
                    if (_getData != null)
                    {
                        result.name_en = _getData.mail_type_name + "";
                        result.content = _getData.content + "";
                        result.mail_header = _getData.mail_header + "";
                        result.sender_name = _getData.sender_name + "";
                        result.IdEncrypt = id;

                        if (_getData.TM_MailContent_Cc != null && _getData.TM_MailContent_Cc.Any(a => a.active_status == "Y"))
                        {
                            string[] UserID = _getData.TM_MailContent_Cc.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                            var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                            result.lstData = (from lstAD in _getData.TM_MailContent_Cc.Where(w => w.active_status == "Y")
                                              from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                              select new vmail_cc
                                              {
                                                  emp_name = lstAD.user_no + " : " + lstUnit.EmpFullName,
                                                  emp_group = lstUnit.UnitGroup,
                                                  emp_position = lstUnit.Rank,
                                                  emp_dec = lstAD.description.StringRemark(45),
                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                              }).ToList();
                        }

                        if (_getData.TM_MailContent_Cc_bymail != null && _getData.TM_MailContent_Cc_bymail.Any(a => a.active_status == "Y"))
                        {
                            result.lstData_mail = (from lstAD in _getData.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y")
                                                   select new vmail_cc_bymail
                                                   {
                                                       mail_cc = lstAD.e_mail,
                                                       mail_dec = lstAD.description.StringRemark(45),
                                                       mail_update = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                                       mail_update_by = lstAD.update_user,
                                                       Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DelEmail('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                   }).ToList();
                        }
                        if (_getData.TM_MailContent_Param != null && _getData.TM_MailContent_Param.Any(a => a.active_status == "Y"))
                        {
                            result.lstData_param = (from lstAD in _getData.TM_MailContent_Param.Where(w => w.active_status == "Y")
                                                    select new vMailContent_Parameter
                                                    {
                                                        name = lstAD.param + "",
                                                        description = lstAD.description + "",
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
        public ActionResult LoadMailContentList(CSearchPool SearchItem)
        {
            vMailContent_Return result = new vMailContent_Return();
            List<vMailContent_obj> lstData_resutl = new List<vMailContent_obj>();
            var lstData = _MailContentService.GetMailContent(
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
                                  select new vMailContent_obj
                                  {
                                      name_en = lstAD.mail_type.StringRemark(500),
                                      description = lstAD.mail_header,
                                      ////  short_name_en = lstAD.Company_short_name_en,
                                      active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      //description = lstAD.sub_group_description.StringRemark(),
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
        public ActionResult EditMailContent(vMailContent_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vMailContent_Return result = new vMailContent_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _MailContentService.Find(nId);
                    if (_getData != null)
                    {
                        if (!string.IsNullOrEmpty(ItemData.mail_header))
                        {

                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.content = ItemData.content;
                            _getData.mail_header = (ItemData.mail_header + "").Trim();
                            _getData.sender_name = (ItemData.sender_name + "").Trim();

                            var sComplect = _MailContentService.Update(_getData);
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
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter mail subject.";
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
        public ActionResult PreviewMailContent(vMailContent_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vMailContent_Return result = new vMailContent_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.mail_to))
                {
                    var mail = wsEmail2.SendMail(CGlobal.UserInfo.EMail, ItemData.sender_name, ItemData.mail_to.Trim(), "", "", ItemData.mail_header + dNow.ToString("dd-MMM-yyyy"), ItemData.content);
                    if (mail)
                    {
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't Send";
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
        public JsonResult UserAutoComplete(string SearchItem, string sQueryID)
        {
            List<C_USERS_RETURN> result = new List<C_USERS_RETURN>();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            string[] aAllUser = new string[] { };
            int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(sQueryID + ""));
            var _getMail = _MailContentService.Find(nId);
            if (_getMail != null && _getMail.TM_MailContent_Cc != null && _getMail.TM_MailContent_Cc.Any(a => a.active_status == "Y"))
            {
                aAllUser = _getMail.TM_MailContent_Cc.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
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
        public ActionResult EditMailContentCC(vMailContentApprover_obj_save ItemData)
        {
            objCC_Return result = new objCC_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _CheckUser = dbHr.Employee.Where(w => w.Employeeno == ItemData.emp_no && (w.C_Status == "1" || w.C_Status == "3")).FirstOrDefault();
                    if (_CheckUser != null)
                    {
                        var _getData = _MailContentService.Find(nId);
                        if (_getData != null)
                        {

                            TM_MailContent_Cc objSave = new TM_MailContent_Cc()
                            {
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                active_status = "Y",
                                description = ItemData.description,
                                user_id = _CheckUser.UserID,
                                user_no = _CheckUser.Employeeno,
                                MailContent = _getData,
                            };

                            var sComplect = _TM_MailContent_CcService.AddAndUpdate(objSave);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _MailContentService.Find(nId);
                                if (_getEditList != null && _getEditList.TM_MailContent_Cc != null && _getEditList.TM_MailContent_Cc.Any(a => a.active_status == "Y"))
                                {
                                    string[] UserID = _getEditList.TM_MailContent_Cc.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                    var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                                    result.lstData = (from lstAD in _getEditList.TM_MailContent_Cc.Where(w => w.active_status == "Y")
                                                      from lstUnit in _getUnit.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                      select new vmail_cc
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
                                    result.lstData = new List<vmail_cc>();
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
                            result.Msg = "Error, Mail Content Not Found.";
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
        public ActionResult DelMailContentCC(string ItemData)
        {
            objPool_Return result = new objPool_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData + ""));
                if (nId != 0)
                {

                    var _Get_TM_MailContent_CcService = _TM_MailContent_CcService.Find(nId);
                    if (_Get_TM_MailContent_CcService != null)
                    {
                        _Get_TM_MailContent_CcService.active_status = "N";
                        _Get_TM_MailContent_CcService.update_user = CGlobal.UserInfo.UserId;
                        _Get_TM_MailContent_CcService.update_date = dNow;

                        var sComplect = _TM_MailContent_CcService.AddAndUpdate(_Get_TM_MailContent_CcService);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getEditList = _MailContentService.Find(_Get_TM_MailContent_CcService.MailContent.Id);
                            if (_getEditList != null && _getEditList.TM_MailContent_Cc != null && _getEditList.TM_MailContent_Cc.Any(a => a.active_status == "Y"))
                            {
                                string[] UserID = _getEditList.TM_MailContent_Cc.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                var _getUnit = dbHr.AllInfo_WS.Where(w => UserID.Contains(w.EmpNo)).ToList();
                                result.lstData = (from lstAD in _getEditList.TM_MailContent_Cc.Where(w => w.active_status == "Y")
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

        [HttpPost]
        public ActionResult EditMailContentCCBymail(vMailContentApprover_obj_save ItemData)
        {
            objCCmail_Return result = new objCCmail_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    if (IsValidEmail((ItemData.emp_no + "").Trim().ToLower()))
                    {
                        var _getData = _MailContentService.Find(nId);
                        if (_getData != null)
                        {

                            TM_MailContent_Cc_bymail objSave = new TM_MailContent_Cc_bymail()
                            {
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                active_status = "Y",
                                description = ItemData.description,
                                e_mail = (ItemData.emp_no + "").Trim().ToLower(),
                                MailContent = _getData,
                            };

                            var sComplect = _TM_MailContent_Cc_bymailService.AddAndUpdate(objSave);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _MailContentService.Find(nId);
                                if (_getEditList.TM_MailContent_Cc_bymail != null && _getEditList.TM_MailContent_Cc_bymail.Any(a => a.active_status == "Y"))
                                {
                                    result.lstData_mail = (from lstAD in _getEditList.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y")
                                                           select new vmail_cc_bymail
                                                           {
                                                               mail_cc = lstAD.e_mail,
                                                               mail_dec = lstAD.description.StringRemark(45),
                                                               mail_update = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                                               mail_update_by = lstAD.update_user,
                                                               Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DelEmail('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                           }).ToList();
                                }
                                else
                                {
                                    result.lstData_mail = new List<vmail_cc_bymail>();
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
                            result.Msg = "Error, Mail Content Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, E-Mail wrong format.";
                    }


                }

            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult DelMailContentCCBymail(string ItemData)
        {
            objCCmail_Return result = new objCCmail_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData + ""));
                if (nId != 0)
                {

                    var _Get_TM_MailContent_Cc_bymailService = _TM_MailContent_Cc_bymailService.Find(nId);
                    if (_Get_TM_MailContent_Cc_bymailService != null)
                    {
                        _Get_TM_MailContent_Cc_bymailService.active_status = "N";
                        _Get_TM_MailContent_Cc_bymailService.update_user = CGlobal.UserInfo.UserId;
                        _Get_TM_MailContent_Cc_bymailService.update_date = dNow;

                        var sComplect = _TM_MailContent_Cc_bymailService.AddAndUpdate(_Get_TM_MailContent_Cc_bymailService);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getEditList = _MailContentService.Find(_Get_TM_MailContent_Cc_bymailService.MailContent.Id);
                            if (_getEditList.TM_MailContent_Cc_bymail != null && _getEditList.TM_MailContent_Cc_bymail.Any(a => a.active_status == "Y"))
                            {
                                result.lstData_mail = (from lstAD in _getEditList.TM_MailContent_Cc_bymail.Where(w => w.active_status == "Y")
                                                       select new vmail_cc_bymail
                                                       {
                                                           mail_cc = lstAD.e_mail,
                                                           mail_dec = lstAD.description.StringRemark(45),
                                                           mail_update = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                                           mail_update_by = lstAD.update_user,
                                                           Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DelEmail('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                       }).ToList();
                            }
                            else
                            {
                                result.lstData_mail = new List<vmail_cc_bymail>();
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

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}