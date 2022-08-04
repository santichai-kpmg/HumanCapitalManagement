using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.ConsentModel;
using HumanCapitalManagement.Service.ConsentService;
using HumanCapitalManagement.ViewModel.ConsentForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.ConsentController
{
    public class ConsentAdminProcessController : BaseController
    {
        private TM_Consent_FormService _TM_Consent_FormService;
        private TM_Consent_QuestionService _TM_Consent_QuestionService;

        public ConsentAdminProcessController(
            TM_Consent_FormService TM_Consent_FormService,
            TM_Consent_QuestionService TM_Consent_QuestionService
            )
        {
            _TM_Consent_FormService = TM_Consent_FormService;
            _TM_Consent_QuestionService = TM_Consent_QuestionService;
        }



        // GET: ConsentAdminProcess
        public ActionResult ConsentAdminProcess()
        {
            /*var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }*/

            vConsentForm_View result = new vConsentForm_View();
            var getData = _TM_Consent_FormService.GetDataForSelect().ToList();
            string BackUrl = "";
            result.lstData = getData.Select(s => new vConsentForm_View_obj()
            {
                Id = s.Id,
                View = @"<button id=""btnView""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(s.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-eye-open""></i></button>",
                Edit = s.Active_Status != "Y" ? @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(s.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>" : @"<button id=""btnEdit""  type=""button"" class=""btn btn-xs btn-primary"" disabled><i class=""glyphicon glyphicon-edit""></i></button>",
                Name = s.Name,
                Create_Date = s.Create_Date.Value.ToString("dd MMM yyyy"),
                Active_status = s.Active_Status == "Y" ? "Active" : s.Active_Status == "N" ? "Inactive" : "-",
            }).ToList();
            /*result.active_status = "Y";*/
            return View(result);
        }
        public ActionResult ConsentAdminProcess_View(string id)
        {
            vConsentForm_Question result = new vConsentForm_Question();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Consent_FormService.Find(nId);

                    if (_getData != null)
                    {
                        result.IdEncrypt = id;
                        result.Name = _getData.Name;
                        result.Description = _getData.Description;
                        result.Active_status = _getData.Active_Status == "Y" ? "Active" : _getData.Active_Status == "N" ? "Inactive" : "-";
                        result.Create_date = _getData.Create_Date.Value.ToString("dd MMM yyyy");
                        result.Create_user = _getData.Create_User;
                        result.Update_date = _getData.Update_Date.Value.ToString("dd MMM yyyy");
                        result.Update_user = _getData.Update_User;
                        if (_getData.TM_Consent_Question != null && _getData.TM_Consent_Question.Any())
                        {
                            result.lstData = _getData.TM_Consent_Question.Select(s => new vConsentForm_Question_obj
                            {
                                Id = s.Id,
                                Seq = s.Seq,
                                Type = s.Type.ToString() == "1" ? "Acknowledge Page" : s.Type.ToString() == "2" ? "Consent Page" : s.Type.ToString() == "3" ? "Submit Page" : "-",
                                Topic = s.Topic,
                                Content = s.Content,
                                Description = s.Description,
                                Active_status = s.Active_Status == "Y" ? "Active" : s.Active_Status == "N" ? "In Active" : "-",
                                Create_date = s.Create_Date.Value.ToString("dd MMM yyyy"),
                                Create_user = s.Create_User,
                                Update_date = s.Update_Date.Value.ToString("dd MMM yyyy"),
                                Update_user = s.Update_User,

                            }).ToList();
                        }
                    }
                }
            }
            return View(result);
        }
        public ActionResult ConsentAdminProcess_Create(string qryStr)
        {
            /*var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }*/

            vConsentForm_Question result = new vConsentForm_Question();

            /*result.Page_type.Add(new _Page_Type() { Id = 1, Name = "Acknowledge Page" });
            result.Page_type.Add(new _Page_Type() { Id = 2, Name = "Consent Page" });
            result.Page_type.Add(new _Page_Type() { Id = 3, Name = "Submit Page" });
            result.Page_type.Add(new _Page_Type() { Id = 4, Name = "Additional Page" });*/

            /*result.Active_status = "Active";*/
            result.Create_date = DateTime.Now.DateTimebyCulture().ToString();

            return View(result);
        }
        public ActionResult ConsentAdminProcess_Edit(string id)
        {
            /*var sCheck = acCheckLoginAndPermission();
           if (sCheck != null)
           {
               return sCheck;
           }*/

            vConsentForm_Question result = new vConsentForm_Question();
            result.editData = new vConsentForm_Question_obj();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Consent_FormService.Find(nId);

                    if (_getData != null)
                    {
                        result.Id = _getData.Id;
                        result.IdEncrypt = id;
                        result.Name = _getData.Name;
                        result.Description = _getData.Description;
                        result.Active_status = _getData.Active_Status == "Y" || _getData.Active_Status == "y" ? "Active" : _getData.Active_Status == "N" ? "Inactive" : "-";
                        result.Create_date = _getData.Create_Date.Value.ToString("dd MMM yyyy");
                        result.Create_user = _getData.Create_User;
                        result.Update_date = _getData.Update_Date.Value.ToString("dd MMM yyyy");
                        result.Update_user = _getData.Update_User;
                        if (_getData.TM_Consent_Question != null && _getData.TM_Consent_Question.Any())
                        {
                            result.lstData = _getData.TM_Consent_Question.Select(s => new vConsentForm_Question_obj
                            {
                                Id = s.Id,
                                Seq = s.Seq,
                                Type = s.Type.ToString() == "1" ? "Acknowledge Page" : s.Type.ToString() == "2" ? "Consent Page" : s.Type.ToString() == "3" ? "Submit Page" : "-",
                                Topic = s.Topic,
                                Content = s.Content,
                                Description = s.Description,
                                Active_status = s.Active_Status == "Y" ? "Active" : s.Active_Status == "N" ? "Inactive" : "-",
                                Create_date = s.Create_Date.Value.ToString("dd MMM yyyy"),
                                Create_user = s.Create_User,
                                Update_date = s.Update_Date.Value.ToString("dd MMM yyyy"),
                                Update_user = s.Update_User,

                            }).ToList();
                        }
                    }
                }
            }
            return View(result);
        }


        [HttpPost]
        public ActionResult CreateConsentFrom_Question(vConsentForm_Question ItemData)
        {

            /*vConsentForm_Question result = new vConsentForm_Question();*/
            vConsent_Group_Question_Return result = new vConsent_Group_Question_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData.lstData != null && ItemData.lstData.Any())
                {

                    var chkOrder = 1;
                    List<TM_Consent_Question> lstObjQuestion = new List<TM_Consent_Question>();
                    foreach (var s in ItemData.lstData)
                    {

                        TM_Consent_Question objChild = new TM_Consent_Question
                        {
                            Update_User = CGlobal.UserInfo.EmployeeNo + "",
                            Update_Date = dNow,
                            Create_User = CGlobal.UserInfo.EmployeeNo + "",
                            Create_Date = dNow,
                            Active_Status = s.Active_status,
                            Seq = SystemFunction.GetIntNullToZero(s.Id + "").ToString(),
                            Type = ((s.Type == "Acknowledge Page" ? 1 : s.Type == "Consent Page" ? 2 : s.Type == "Submit Page" ? 3 : 0) + "").Trim() + "",
                            Topic = (s.Topic + "").Trim() + "",
                            Content = (s.Content + "").Trim() + "",
                            Description = (s.Description + "").Trim(),
                        };
                        chkOrder++;
                        lstObjQuestion.Add(objChild);
                    }


                    TM_Consent_Form objSave = new TM_Consent_Form()
                    {
                        Id = 0,
                        Update_User = CGlobal.UserInfo.EmployeeNo + "",
                        Update_Date = dNow,
                        Create_User = CGlobal.UserInfo.EmployeeNo + "",
                        Create_Date = dNow,
                        Active_Status = ItemData.Active_status == "Active" ? "Y" : "N",
                        Name = ItemData.Name,
                        Description = ItemData.Description,
                    };
                    objSave.TM_Consent_Question = lstObjQuestion;

                    var sComplect = _TM_Consent_FormService.CreateNew(ref objSave);
                    if (sComplect > 0 && ItemData.lstData.Count() > 0)
                    {
                        result.Status = SystemFunction.process_Success;
                        result.Msg = "Create complete.";
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Can't create data due to something went wrong";
                    }
                }

            }
            return Json(new
            {
                result
            });
        }

        //[HttpPost]
        //public ActionResult EditConsentFrom_Question(vConsentForm_Question ItemData)
        //{
        //    vConsentForm_Question result = new vConsentForm_Question();
        //    if (ItemData != null)
        //    {
        //        DateTime dNow = DateTime.Now;
        //        int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
        //        if (nId != 0)
        //        {
        //            var _getData = _TM_Consent_FormService.Find(nId);
        //            if (_getData != null)
        //            {
        //                result.Name = _getData.Name;

        //            }
        //        }
        //    }

        //    return Json(new
        //    {
        //        result
        //    });
        //}


        [HttpPost]
        public ActionResult EditConsentFrom_Question(vConsentForm_Question_obj ItemData)
        {
            vConsent_Group_Question_Return result = new vConsent_Group_Question_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Consent_FormService.Find(nId);
                    if (_getData != null)
                    {
                        //result.Name = _getData.Name;

                        var objSave = _TM_Consent_QuestionService.Find(ItemData.Id);
                        if (objSave != null)
                        {
                            objSave.Type = ItemData.Type;
                            objSave.Topic = ItemData.Topic;
                            objSave.Content = ItemData.Content;
                            objSave.Description = ItemData.Description;
                        }

                        var sComplect = _TM_Consent_QuestionService.Update(objSave);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            result.Msg = "Update complete.";
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Can't update data due to something went wrong or data have not any update since last update.";
                        }

                    }
                }
            }

            return Json(new
            {
                result
            });

        }
        [HttpPost]
        public ActionResult EditStatusConsentFrom(vConsentForm_Question_obj ItemData)
        {
            vConsent_Group_Question_Return result = new vConsent_Group_Question_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Consent_FormService.Find(nId);
                    if (_getData != null)
                    {
                        //result.Name = _getData.Name;

                        var objSave = _TM_Consent_FormService.Find(ItemData.Id);

                        
                        if (objSave != null)
                        {
                            objSave.Update_User = CGlobal.UserInfo.UserId;
                            objSave.Update_Date = dNow;
                            objSave.Active_Status = "Y";

                            var sComplect = _TM_Consent_FormService.Update(objSave);
                            if (sComplect > 0)
                            {
                                _TM_Consent_FormService.UpdateInactive(_getData);
                                result.Status = SystemFunction.process_Success;
                                result.Msg = "Update complete.";
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Can't update data due to something went wrong or data have not any update since last update.";
                            }

                        }
                    }
                }
            }

            return Json(new
            {
                result
            });
        }
    }
}