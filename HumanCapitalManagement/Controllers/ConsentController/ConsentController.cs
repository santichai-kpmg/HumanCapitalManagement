using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.ConsentModel;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.ConsentService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.ConsentForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lstDataSelect = HumanCapitalManagement.ViewModel.CommonVM.lstDataSelect;

namespace HumanCapitalManagement.Controllers.ConsentController
{
    public class ConsentController : BaseController
    {
        private Consent_Main_FormService _Consent_Main_FormService;
        private TM_Consent_FormService _TM_Consent_FormService;
        private TM_Consent_QuestionService _TM_Consent_QuestionService;
        private Consent_AsnwerService _Consent_AsnwerService;
        private MailContentService _MailContentService;

        public ConsentController(
            Consent_Main_FormService Consent_Main_FormService,
            TM_Consent_FormService TM_Consent_FormService,
            TM_Consent_QuestionService TM_Consent_QuestionService,
            Consent_AsnwerService Consent_AsnwerService,
            MailContentService MailContentService
            )
        {
            _Consent_Main_FormService = Consent_Main_FormService;
            _TM_Consent_FormService = TM_Consent_FormService;
            _TM_Consent_QuestionService = TM_Consent_QuestionService;
            _Consent_AsnwerService = Consent_AsnwerService;
            _MailContentService = MailContentService;
        }
        public ActionResult ConsentForm()
        {

            vConsentForm_View result = new vConsentForm_View();
            var getData = _Consent_Main_FormService.GetDataForSelect().ToList();
            /*var y = _TM_Consent_FormService.Find(1023).Name;*/
            string BackUrl = "";
            result.lstData = getData.Where(w=>CGlobal.UserInfo.EmployeeNo == w.Create_User).Select(s => new vConsentForm_View_obj()
            {
                Id = s.Id,
                View = @"<button id=""btnView""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(s.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-eye-open""></i></button>",
                /* View = "",*/
                /*Name = s.TM_Consent_Form_Id.ToString(),*/
                Name = s.TM_Consent_Form_Id != null ? _TM_Consent_FormService.Find(s.TM_Consent_Form_Id.Value).Name : "",
                Create_Date = s.Create_Date.Value.ToString("dd MMM yyyy"),
                Active_status = s.Active_Status == "Y" ? @"<p style='color: #009A44; font-size:large;'><i class='fa fa-fw fa-check'></i></p>" : s.Active_Status == "N" ? @"<p style='color: #009A44; font-size:large;'><i class='fa fa-fw fa-close'></i></p>" : "-",
            }).ToList();
            /*result.active_status = "Y";*/
            return View(result);
        }

        public ActionResult ConsentFrom_View(string id)
        {
            vConsentForm_Question result = new vConsentForm_Question();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(id + ""));
                if (nId != 0)
                {
                    var _getData = _Consent_Main_FormService.Find(nId);

                    if (_getData != null)
                    {
                        result.Id = nId;
                        result.Name = _getData.TM_Consent_Form_Id != null ? _TM_Consent_FormService.Find(_getData.TM_Consent_Form_Id.Value).Name : "";
                        result.Active_status = _getData.Active_Status == "Y" ? "Active" : _getData.Active_Status == "N" ? "Inactive" : "-";
                        result.Create_date = _getData.Create_Date.Value.ToString("dd MMM yyyy");
                        result.Create_user = _getData.Create_User;
                        result.Update_date = _getData.Update_Date.Value.ToString("dd MMM yyyy");
                        result.Update_user = _getData.Update_User;
                        result.TM_comsent_form_Id = _getData.TM_Consent_Form_Id;

                        if (result.Active_status != "N")
                        {
                            result.lstAnswer = _getData.Consent_Asnwer.Select(s => new vConsentForm_Answer
                            {
                                Id = s.Id,
                                /*Content = s.TM_Consent_Question_Id.ToString(),*/
                                Content = _TM_Consent_QuestionService.Find(s.TM_Consent_Question_Id.Value).Content,
                                Answer = s.Asnwer == "Y" ? @"<p style='color: #009A44; font-size:large;'><i class='fa fa-fw fa-check'></i></p>" : s.Asnwer == "N" ? @"<p style='color: #BC204B; font-size:large;'><i class='fa fa-fw fa-close'></i></p>" : "-",
                                Description = s.Description,
                                Seq = s.TM_Consent_Question_Id.Value,
                            }).ToList();
                        }
                    }
                }
            }
            return View(result);
        }
        public ActionResult ConsentFormName_List_dropdown(vSelect item)
        {
            vSelect lstData = new vSelect();

            var lst = _TM_Consent_FormService.GetDataForSelect_user();

            if (lst != null && lst.Any())
            {
                lstData.databind = item.databind + "";
                lstData.id = item.id + "";
                lstData.value = item.value + "";
                lstData.disable = item.disable;
                lstData.lstData = lst.OrderByDescending(o => o.Id).Select(s => new lstDataSelect { value = s.Id + "", text = s.Name + "" }).ToList();
            }
            return PartialView("~/Views/Consent/_select.cshtml", lstData);
        }
        public ActionResult ConsentForm_MainForm(string id)
        {
            vConsentForm_Question result = new vConsentForm_Question();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = Int32.Parse(id);
                var _getData = _TM_Consent_FormService.Find(nId);

                if (_getData != null)
                {
                    DateTime dNow = DateTime.Now;
                    result.UserName = CGlobal.UserInfo.FullName + "";
                    result.EmployeeNo = CGlobal.UserInfo.EmployeeNo + "";
                    result.IdEncrypt = id;
                    result.Name = _getData.Name;
                    result.Consent_date = dNow.ToString("dd MMM yyyy");
                    result.objGet = new vConsentForm_user();
                    result.objGet.lstAnswer = new List<vConsentAnswer_user>();
                    result.objGet.lstData = new List<ViewModel.ConsentForm.lstDataSelect>();

                    if (_getData.TM_Consent_Question != null && _getData.TM_Consent_Question.Any())
                    {
                        result.lstData_display = _getData.TM_Consent_Question.GroupBy(g => g.Topic).Select(s => new vConsentForm_Question_main
                        {
                            Topic = s.First().Topic,
                            lstSub = s.Select(sub => new vConsentForm_Question_sub()
                            {
                                Id = sub.Id,
                                Seq = sub.Seq,
                                Type = sub.Type,
                                Content = sub.Type.ToString() == "2" ? @"<input class='pl form-check-input messageCheckbox' type='checkbox' name='flexCheck' id='" + sub.Id + "' value='" + sub.Content + "' style='margin-right: 10px;'><span class='form-check-label' for ='flexCheck'>" + sub.Content + "</span></input>" : sub.Content,
                            }).OrderBy(o => o.Seq).ToList(),
                        }).ToList();
                    }
                }
            }
            return View(result);
        }

        [HttpPost]
        public ActionResult ConsentFrom_Save(vConsentForm_Question ItemData)
        {
            vConsentForm_user result = new vConsentForm_user();
            try {
                if (ItemData != null)
                {
                    DateTime dnow = DateTime.Now;

                    if (ItemData.lstData_display != null)
                    {
                        List<Consent_Asnwer> lstObjAnswer = new List<Consent_Asnwer>();

                        foreach (var s in ItemData.objGet.lstAnswer)
                        {
                            Consent_Asnwer objAnswer = new Consent_Asnwer
                            {
                                Asnwer = s.Answer == "-" ? "Y" : "N",
                                Active_Status = "Y",
                                Description = s.Description,
                                Create_Date = dnow,
                                Update_Date = dnow,
                                Update_User = CGlobal.UserInfo.EmployeeNo + "",
                                Create_User = CGlobal.UserInfo.EmployeeNo + "",
                                TM_Consent_Question_Id = s.Id,
                            };
                            lstObjAnswer.Add(objAnswer);
                        }

                        Consent_Main_Form obj = new Consent_Main_Form
                        {
                            Id = 0,
                            Employee_no = ItemData.EmployeeNo,
                            Description = ItemData.objGet.Description,
                            Active_Status = "Y",
                            Create_Date = dnow,
                            Update_Date = dnow,
                            Update_User = CGlobal.UserInfo.EmployeeNo + "",
                            Create_User = CGlobal.UserInfo.EmployeeNo + "",
                            TM_Consent_Form_Id = Int16.Parse(ItemData.IdEncrypt),
                            /*Consent_Asnwer = ItemData.lstAnswer,*/
                        };
                        obj.Consent_Asnwer = lstObjAnswer;

                        //Have No
                        var sComplect = _Consent_Main_FormService.CreateNew(ref obj);
                        var y = obj.Consent_Asnwer.Select(s => s.Asnwer).ToList();

                        var check = 0;
                        foreach (var checkNo in y)
                        {
                            if (checkNo == "N")
                            {
                                check += 1;
                                check++;
                            }
                        }
                        if (check > 0)
                        {
                            //add mail step
                            var MailRequest = _MailContentService.GetMailContent("Consent Notification", "Y").FirstOrDefault();
                            //string genlink = HCMFunc.GetUrl("submit", itemdata.maindata.Id.ToString(), pathUrl, CGlobal.UserInfo.EmployeeNo);

                            string sContent = MailRequest.content;
                            string from = MailRequest.sender_name;
                            string subject = MailRequest.mail_header;


                            sContent = (sContent + "").Replace("$reciepver", "HR Consent Manament");
                            var meargetext = "";

                            foreach (var x in ItemData.objGet.lstAnswer.ToList())
                            {
                                if (x.Answer != "-")
                                {
                                    meargetext += "<b>- " + x.Answer + "</b><br>" + "Description: <p class='text-danger'>" + x.Description + "</p><br>";
                                }
                            }

                            sContent = (sContent + "").Replace("$Content$", meargetext);
                            string getemailcc = "";


                        var objMail = new vObjectMail_Send();
                        objMail.mail_from = "th-fm-hrconsent@kpmg.co.th";
                        objMail.title_mail_from = from;//GetContent.sender_name;
                        objMail.mail_to = "th-fm-hrconsent@kpmg.co.th";
                        objMail.mail_cc = !String.IsNullOrEmpty(getemailcc) ? CGlobal.UserInfo.EMail + "," + getemailcc : CGlobal.UserInfo.EMail;
                        objMail.mail_subject = subject;
                        objMail.mail_content = sContent;

                            var msg = "";
                            var sSendMail = HCMFunc.SendMail(objMail, ref msg);
                        }

                        //email to every one do form
                        var MailRequest_All = _MailContentService.GetMailContent("Consent Form Accept", "Y").FirstOrDefault();
                        //string genlink = HCMFunc.GetUrl("submit", itemdata.maindata.Id.ToString(), pathUrl, CGlobal.UserInfo.EmployeeNo);

                        string sContent_All = MailRequest_All.content;
                        string from_All = MailRequest_All.sender_name;
                        string subject_All = MailRequest_All.mail_header;


                        sContent_All = (sContent_All + "").Replace("$reciepver$", CGlobal.UserInfo.FullName);
                        var meargetext_All = "Completed form";

                        sContent_All = (sContent_All + "").Replace("$Link$", Url.Action("ConsentForm", "Consent", null, Request.Url.Scheme));
                        string getemailcc_All = "";


                        var objMail_All = new vObjectMail_Send();
                        objMail_All.mail_from = "th-fm-hrconsent@kpmg.co.th";
                        objMail_All.title_mail_from = from_All;
                        objMail_All.mail_to = CGlobal.UserInfo.EMail;
                        objMail_All.mail_cc = !String.IsNullOrEmpty(getemailcc_All) ? CGlobal.UserInfo.EMail + "," + getemailcc_All : CGlobal.UserInfo.EMail;
                        objMail_All.mail_subject = subject_All;
                        objMail_All.mail_content = sContent_All;

                        var msg_All = "";
                        var sSendMail_All = HCMFunc.SendMail(objMail_All, ref msg_All);



                    }
                }
                result.Status = SystemFunction.process_Success;
                result.Msg = "Save Completed";
            }
            catch(Exception ex) 
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;
            }
            return Json(new
            {
                result
            });
        }
    }
}




