using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.ConsentService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.ConsentForm;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using lstDataSelect = HumanCapitalManagement.ViewModel.CommonVM.lstDataSelect;

namespace HumanCapitalManagement.Controllers.ConsentController
{
    public class ConsentFormReportController : Controller
    {
        private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        private Consent_Main_FormService _Consent_Main_FormService;
        private TM_Consent_FormService _TM_Consent_FormService;
        private TM_Consent_QuestionService _TM_Consent_QuestionService;
        private Consent_AsnwerService _Consent_AsnwerService;
        private MailContentService _MailContentService;

        public ConsentFormReportController(
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
        // GET: ConsentFormReport
        public ActionResult ConsentFormReport()
        {
            vConsentForm_View result = new vConsentForm_View();
            var getData = _Consent_Main_FormService.GetDataForSelect().ToList();
            return View(result);
        }

        public ActionResult ConsentFormName_List_dropdown(vSelect item)
        {
            vSelect lstData = new vSelect();
            var lst = _TM_Consent_FormService.GetDataForSelect_Report();

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

        [HttpPost]
        public ActionResult LoadDataSearch(string id)
        {
            vConsentForm_View result = new vConsentForm_View();
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    int nId = Int32.Parse(id);
                    string BackUrl = "";

                    /*var getData = _Consent_Main_FormService.GetDataForSelect().Where(w => w.Employee_no == CGlobal.UserInfo.EmployeeNo && w.TM_Consent_Form_Id == nId ).ToList();*/
                    var getData = _Consent_Main_FormService.GetDataForSelect().Where(w=>w.TM_Consent_Form_Id == nId && w.Consent_Asnwer.Where(s=>s.Asnwer =="N").Any()).OrderByDescending(o => o.Create_Date).FirstOrDefault();
                    //var getemp = wsHRis.getEmployeeInfoByEmpNo("00019318").AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");
                    result.objData = new vConsentForm_View_obj();
                    getData.Id = getData.Id;
                    result.objData.View = @"<button id=""btnView""  type=""button"" onclick=""View('" + HCMFunc.Encrypt(getData.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-eye-open""></i></button>";
                    result.objData.Name = getData.TM_Consent_Form_Id != null ? _TM_Consent_FormService.Find(getData.TM_Consent_Form_Id.Value).Name : "";
                    result.objData.Create_Date = getData.Create_Date.Value.ToString("dd MMM yyyy");
                    result.objData.Name_user = wsHRis.getEmployeeInfoByEmpNo(getData.Create_User).AsEnumerable().FirstOrDefault().Field<string>("EmpFullName");

                    result.objData.Active_status = getData.Active_Status == "Y" ? @"<p style='color: #009A44; font-size:large;'><i class='fa fa-fw fa-check'></i></p>" : getData.Active_Status == "N" ? @"<p style='color: #009A44; font-size:large;'><i class='fa fa-fw fa-close'></i></p>" : "-";

                }
                result.Status = SystemFunction.process_Success;
                result.Msg = "Save Sucess";
            }
            catch (Exception ex)
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = ex.Message;
            }
            return Json(new { result = result });

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
        // view in table 
        /* public ActionResult ConsentFromReport_View(string id)
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
                         result.Name = _getData.TM_Consent_Form_Id != null ? _TM_Consent_FormService.Find(_getData.TM_Consent_Form_Id ?? 0).Name : "";
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
                                 Content = s.TM_Consent_Question_Id.ToString(),
                                 Content = _TM_Consent_QuestionService.Find(s.TM_Consent_Question_Id ?? 0).Content,
                                 Answer = s.Asnwer == "Y" ? @"<p style='color: #009A44; font-size:large;'><i class='fa fa-fw fa-check'></i></p>" : s.Asnwer == "N" ? @"<p style='color: #BC204B; font-size:large;'><i class='fa fa-fw fa-close'></i></p>" : "-",
                                 Description = s.Description,
                                 Seq = s.TM_Consent_Question_Id ?? 0,
                             }).ToList();
                         }
                     }
                 }
             }
             return View(result);
         }*/


    }
}