using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Service.AddressService;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.VisaExpiry;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HumanCapitalManagement.Models.VisaExpiry;
using HumanCapitalManagement.Models.VisExpiry;

namespace HumanCapitalManagement.Controllers.Visa_Expiry
{
    public class VisaExpiryController : BaseController
    {
        // GET: VisaExpiry

        private TM_EmployeeForeign_VisaService _TM_EmployeeForeign_VisaService;
        private TM_Document_EmployeeService _TM_Document_EmployeeService;
        private TM_CountryService _TM_CountryService;
        private TM_PrefixService _TM_PrefixService;
        private TM_Remark_VisaService _TM_Remark_VisaService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        
       
        public VisaExpiryController(TM_EmployeeForeign_VisaService TM_EmployeeForeign_VisaService,
            TM_Document_EmployeeService TM_Document_EmployeeService,
            TM_CountryService TM_CountryService,
            TM_PrefixService TM_PrefixService,
            TM_Remark_VisaService TM_Remark_VisaService
            )
        {
            _TM_EmployeeForeign_VisaService = TM_EmployeeForeign_VisaService;
            _TM_Document_EmployeeService = TM_Document_EmployeeService;
            _TM_CountryService = TM_CountryService;
            _TM_PrefixService = TM_PrefixService;
            _TM_Remark_VisaService = TM_Remark_VisaService;
        }
        public ActionResult Report()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            return View();
        }
        public ActionResult EmployeeList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vEmployee result = new vEmployee();
            //result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
               
                CSearchEmployee SearchItem = (CSearchEmployee)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchEmployee)));
                var lstData = _TM_EmployeeForeign_VisaService.GetDataAllActive();
                string BackUrl = Uri.EscapeDataString(qryStr);

                if (lstData.Any())
                {
                    //var getStaff = dbHr.AllInfo_WS.Where(w => w.EmpNo == lstData).Select(s => s.EmpFullName).FirstOrDefault();
                    result.lstDataEmployee = (from lstAD in lstData
                                              select new vEmployee_obj
                                              {
                                                  IdEncrypt = lstAD.Id + "",
                                                  code = lstAD.EmployeeNo+"",
                                                  TM_Prefix_Id = lstAD.TM_Prefix_Visa.PrefixNameEN +"",
                                                  full_name = lstAD.Employeename_ENG + " " + lstAD.Employeesurname_ENG + "",
                                                  Company_name = lstAD.TM_Company_Visa.company_name + "",
                                                  mail = lstAD.Employee_email + "",
                                                  //TSIC_Id = lstAD.TM_TSIC.TSIC_ENG + "",
                                                 // Staff_id = dbHr.AllInfo_WS.Where(w => w.EmpNo == lstAD.staff_id).Select(s => s.EmpFullName).FirstOrDefault(),
                                                  staff = dbHr.AllInfo_WS.Where(w => w.EmpNo == lstAD.staff_No).Select(s => s.EmpFullName).FirstOrDefault(),
                                                  create_user = lstAD.create_user+"",
                                                  create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture("dd-MMM-yyyy") : "",
                                                  update_user = lstAD.update_user+"",
                                                  active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                  update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture("dd-MMM-yyyy") : "",
                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              }).ToList();
                }
            }
            else
            {
                return View(result);
            }
            #endregion
            return View(result);
        }
        public ActionResult EmployeeView(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vEmployee_obj_save result = new vEmployee_obj_save();
            List<vDocumentEmployee_obj> lstData_resutl = new List<vDocumentEmployee_obj>();
            List<vMember_obj> lstData_resutl_Member = new List<vMember_obj>();
            string BackUrl = Uri.EscapeDataString(qryStr);
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    #region Detail Customer
                    var _getData = _TM_EmployeeForeign_VisaService.Find(nId);
                    var linkback = HCMFunc.Encrypt(_getData.Id + "");
                    if (_getData != null)
                    {
                        //result.IdEncrypt = _getData.TM_CompanyDetail.Id + "";
                        result.Id = _getData.Id;
                        result.IdEncrypt = HCMFunc.Encrypt(_getData.Id + "");
                        //result.BackPage = HCMFunc.Encrypt(_getData.TM_CompanyDetail.Id + "");
                        result.Company_name = _getData.TM_Company_Visa.company_name + "";
                        result.BackPage_Head = HCMFunc.Encrypt(_getData.Id + "");
                        result.TM_Prefix_Id = _getData.TM_Prefix_Visa.Id+"";
                        result.code = _getData.EmployeeNo + "";
                        result.name_eng = _getData.Employeename_ENG + "";
                        result.name_th = _getData.Employeename_TH + "";
                        result.lastname_eng = _getData.Employeesurname_ENG + "";
                        result.lastname_th = _getData.Employeesurname_TH + "";
                        result.middle = _getData.Employee_Middle_ENG + "";
                        result.mail = _getData.Employee_email + "";
                        result.telephone = _getData.Employee_telephone + "";
                        result.active_status = _getData.active_status + "" == "Y" ? "Active" : "Inactive";
                        result.create_date = _getData.create_date.HasValue ? _getData.create_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "";
                        result.update_date = _getData.update_date.HasValue ? _getData.update_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "";
                        result.Member = _getData.family_group + "";
                        result.staff = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.staff_No).Select(s => s.EmpFullName).FirstOrDefault();
                        result.remark = _getData.TM_Remark_Visa.Id + "";
                    }
                    #endregion
                    //List<TM_Employee> lstData = new List<TM_Employee>();
                    #region Document Employee List
                    var lstDataEmployee = _TM_Document_EmployeeService.GetDataByEmp(nId);
                    result.lstDocEmployee = new List<vDocumentEmployee_obj>();
                    if (lstDataEmployee != null)
                    {
                        lstData_resutl = (from lstAD in lstDataEmployee
                                          select new vDocumentEmployee_obj
                                          {
                                              Id = lstAD.Id,
                                              IdEncrypt = lstAD.Id + "",
                                              document_number = lstAD.doc_number + "",
                                              date_of_issued = (lstAD.date_of_issued.HasValue ? lstAD.date_of_issued.Value.DateTimebyCulture("dd-MMM-yyyy") : ""),
                                              Country_id = lstAD.TM_Country.country_name_en + "",
                                              Type_doc_id = lstAD.TM_Type_Document.type_docname_eng + "",
                                              valid_date = (lstAD.valid_date.HasValue ? lstAD.valid_date.Value.DateTimebyCulture("dd-MMM-yyyy") : ""),
                                              create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                              active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          }).ToList();
                        result.lstDocEmployee = lstData_resutl.ToList();
                    }
                    #endregion

                    #region Family group List
                    var CodeEmployee = _getData.Id + "";
                    var lstDataMember = _TM_EmployeeForeign_VisaService.GetDataMemberbyCode(CodeEmployee);

                    result.lstMember = new List<vMember_obj>();
                    if (lstDataMember != null)
                    {

                        lstData_resutl_Member = (from lstAD in lstDataMember
                                                 select new vMember_obj
                                                 {
                                                     Id = lstAD.Id,
                                                     EditMember = @"<button id=""btnEditMember""  type=""button"" onclick=""EditMember('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                     //IdEncrypt = lstAD.TM_CompanyDetail.Id + "",
                                                     TM_Prefix_Id = lstAD.TM_Prefix_Visa.PrefixNameEN + "",
                                                     code = lstAD.EmployeeNo + "",
                                                     name_eng = lstAD.Employeename_ENG + "",
                                                     name_th = lstAD.Employeename_TH + "",
                                                     full_name = lstAD.Employeename_ENG + " " + lstAD.Employeesurname_ENG + "",
                                                     lastname_eng = lstAD.Employeesurname_ENG + "",
                                                     lastname_th = lstAD.Employeename_TH + "",
                                                     middle = lstAD.Employee_Middle_ENG + "",
                                                     staff = dbHr.AllInfo_WS.Where(w => w.EmpNo == lstAD.staff_No).Select(s => s.EmpFullName).FirstOrDefault(),
                                                     create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                                     update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                                     active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                     mail = lstAD.Employee_email + "",
                                                     Backpage_Customer = linkback,

                                                 }).ToList();
                        result.lstMember = lstData_resutl_Member.ToList();

                    }
                    #endregion
                }
            }
            return View(result);
        }
        public ActionResult EmployeeCreate(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vEmployee_obj_save result = new vEmployee_obj_save();
            List<vDocumentEmployee_obj> lstData_resutl = new List<vDocumentEmployee_obj>();
            #region Detail Customer
            var lastrowid = _TM_EmployeeForeign_VisaService.FindLastRowId();
            //var _getData = _TM_CompanyDetailService.Findcompbycode(qryStr);
            //var _getData = _TM_EmployeeForeign_VisaService.Find();
            if (lastrowid != null)
            {
                //int idCode = lastrowid.Id + 1;
                //result.IdEncrypt = _getData.TM_CompanyDetail.Id + "";
                //result.BackPage = HCMFunc.Encrypt(_getData.TM_CompanyDetail.Id + "");
                //result.IdEncrypt = HCMFunc.Encrypt(_getData.Id + "");
                //result.code = "EMP" + string.Format("{0:D5}", idCode);
                //result.Company_name = _getData.company_name_eng + "";
                //result.TM_CompanyDetail_Id = _getData.Id + "";
            }
            else
            {
                //result.code = "EMP00001";
            }
            #endregion
            return View(result);
        }
        public ActionResult EmployeeEdit(string qryStr)
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}
            vEmployee_obj_save result = new vEmployee_obj_save();
            List<vDocumentEmployee_obj> lstData_resutl = new List<vDocumentEmployee_obj>();
            #region Detail Customer
           
            string nId = (HCMFunc.Decrypt(qryStr + ""));
            var _getData = _TM_EmployeeForeign_VisaService.Find(Convert.ToInt32(nId));
            if (_getData != null)
            {
                //result.IdEncrypt = _getData.TM_CompanyDetail.Id + "";
                result.Id =  _getData.Id;
                result.IdEMP = HCMFunc.Encrypt(qryStr + "");
                //result.BackPage = HCMFunc.Encrypt(_getData.TM_CompanyDetail.Id + "");
                result.membercode = _getData.family_group + "";
                result.Company_name = _getData.TM_Company_Visa.Id + "";
                result.TM_Prefix_Id = _getData.TM_Prefix_Visa.Id + "";
                result.code = _getData.EmployeeNo + "";
                result.name_eng = _getData.Employeename_ENG + "";
                result.name_th = _getData.Employeename_TH + "";
                result.lastname_eng = _getData.Employeesurname_ENG + "";
                result.lastname_th = _getData.Employeesurname_TH + "";
                result.middle = _getData.Employee_Middle_ENG + "";
                result.mail = _getData.Employee_email + "";
                result.staff = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.staff_No).Select(s => s.EmpFullName).FirstOrDefault();
                result.staff_no = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.staff_No).Select(s => s.EmpNo).FirstOrDefault();
                result.staff_name = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.staff_No).Select(s => s.EmpFullName).FirstOrDefault();
                result.staff_group = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.staff_No).Select(s => s.UnitGroupName).FirstOrDefault();
                result.staff_rank = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.staff_No).Select(s => s.Rank).FirstOrDefault();
                result.telephone = _getData.Employee_telephone + "";
                result.middle = _getData.Employee_Middle_ENG + "";
                result.active_status = _getData.active_status + "" == "Y" ? "Active" : "Inactive";
                result.create_date = _getData.create_date.HasValue ? _getData.create_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "";
                result.update_date = _getData.update_date.HasValue ? _getData.update_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "";
                result.remark = _getData.TM_Remark_Visa.Id + "";
            }
            #endregion
            return View(result);
        }
        //Create no by company EmployeeAdd
        public ActionResult EmployeeAdd()
        {

            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}
            vEmployee_obj_save result = new vEmployee_obj_save();
            List<vDocumentEmployee_obj> lstData_resutl = new List<vDocumentEmployee_obj>();
         
            return View(result);
        }
        public ActionResult AddDocument()
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}
            vDocumentEmployee_obj result = new vDocumentEmployee_obj();

            return PartialView("_PopupVisa", result);
        }
        public ActionResult MemberCreate(string qryStr)
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}
            string nId = (HCMFunc.Decrypt(qryStr + ""));
            vEmployee_obj_save result = new vEmployee_obj_save();
            List<vDocumentEmployee_obj> lstData_resutl = new List<vDocumentEmployee_obj>();
            result.membercode = nId;
            result.BackPage = HCMFunc.Encrypt(nId + ""); ;
            var _getData = _TM_EmployeeForeign_VisaService.Find(Convert.ToInt32(nId));
            if (_getData != null)
            {
                result.Company_name = _getData.TM_Company_Visa.Id + "";
            }
            return View(result);
        }
        public ActionResult EditDocument(vDocumentEmployee_obj_search ItemData)
        {
            //var sCheck = acCheckLoginAndPermission();
            //if (sCheck != null)
            //{
            //    return sCheck;
            //}
            vDocumentEmployee_obj result = new vDocumentEmployee_obj();
            //string BackUrl = Uri.EscapeDataString(ItemData.IdEncrypt);
            if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Document_EmployeeService.Find(nId);
                    if (_getData != null)
                    {
                        result.Id_Doc = _getData.Id;
                        result.document_number = _getData.doc_number + "";
                        result.date_of_issued = _getData.date_of_issued.HasValue ? _getData.date_of_issued.Value.DateTimebyCulture("dd-MMM-yyyy") : "";
                        result.Country_id = _getData.TM_Country.Id + "";
                        result.Type_doc_id = _getData.TM_Type_Document.Id + "";
                        result.valid_date = _getData.valid_date.HasValue ? _getData.valid_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "";
                        result.create_date = _getData.create_date.HasValue ? _getData.create_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "";
                        result.update_date = _getData.update_date.HasValue ? _getData.update_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "";
                        result.date_of_issued_edit = _getData.date_of_issued;
                        result.valid_date_edit = _getData.valid_date;
                    }
                    else
                    {
                        result.IdEncrypt = "";
                    }
                }
                else
                {
                    result.IdEncrypt = "";
                }

            }
            else
            {
                result.IdEncrypt = "";
            }
            return PartialView("_PopupVisaEdit", result);
        }



        #region AJAX 
        [HttpPost]
        public ActionResult EditEMP(vEmployee_obj_save ItemData)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            DateTime dNow = DateTime.Now;
               if (ItemData != null)
               {

                    TM_EmployeeForeign_Visa objSave = new TM_EmployeeForeign_Visa
                    {
                            Id = Convert.ToInt32(ItemData.Id),
                            create_user = CGlobal.UserInfo.UserId,
                            update_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            update_date = dNow,
                            active_status =ItemData.active_status+"",
                            EmployeeNo = ItemData.code + "",
                            TM_Company_Id = Convert.ToInt32(ItemData.Company_name),
                            TM_Prefix_Id = Convert.ToInt32(ItemData.TM_Prefix_Id),
                            Employeename_ENG = ItemData.name_eng + "",
                            Employeename_TH = ItemData.name_th + "",
                            Employee_Middle_ENG = ItemData.middle + "",
                            Employeesurname_ENG = ItemData.lastname_eng + "",
                            Employeesurname_TH = ItemData.lastname_th + "",
                            Employee_email = ItemData.mail + "",
                            Employee_telephone = ItemData.telephone + "",
                            staff_No = ItemData.staff+"",
                            TM_Remark_Id = Convert.ToInt32(ItemData.remark)


                    };

                    var sCom = _TM_EmployeeForeign_VisaService.CreateNewOrUpdate(ref objSave);
                    if (sCom > 0)
                    {
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't get any data";
                    }
               }
               else
               {
                   result.Status = SystemFunction.process_Failed;
                   result.Msg = "Error, Can't get any data";
               }
                


            
            
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult AddEMP(vEmployee_obj_save ItemData)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            if (!ModelState.IsValid)
            {
                var stc = ModelState.ToList().Where(w => w.Value.Errors.Count() > 0).Select(s => s);
                var valueerror = "";
                foreach (var x in stc)
                {
                    valueerror += "<br> - " + x.Value.Errors[0].ErrorMessage.Split('\'')[0].Replace("'^[a-zA-Z0-9ก-๙]*$'", "");
                }
                result.Msg = "<p class='text-danger' style='font-weight:900;text-align:center;'>Invalid character</p>";
                result.Msg += valueerror;

                result.Status = SystemFunction.process_Failed;

                return Json(new { result });
            }
            DateTime dNow = DateTime.Now;
            if (ItemData != null)
            {

                TM_EmployeeForeign_Visa objSave = new TM_EmployeeForeign_Visa
                {
                    create_user = CGlobal.UserInfo.UserId,
                    update_user = CGlobal.UserInfo.UserId,
                    create_date = dNow,
                    update_date = dNow,
                    active_status = "Y",
                    EmployeeNo = ItemData.code + "",
                    TM_Company_Id = Convert.ToInt32(ItemData.Company_name),
                    TM_Prefix_Id = Convert.ToInt32(ItemData.TM_Prefix_Id),
                    Employeename_ENG = ItemData.name_eng + "",
                    Employeename_TH = ItemData.name_th + "",
                    Employee_Middle_ENG = ItemData.middle + "",
                    Employeesurname_ENG = ItemData.lastname_eng + "",
                    Employeesurname_TH = ItemData.lastname_th + "",
                    Employee_email = ItemData.mail + "",
                    Employee_telephone = ItemData.telephone + "",
                    staff_No = ItemData.staff + "",
                    TM_Remark_Id = Convert.ToInt32(ItemData.remark)
              
                };
                var sCom = _TM_EmployeeForeign_VisaService.CreateNewOrUpdate(ref objSave);
                if (sCom > 0)
                {
                    result.Status = SystemFunction.process_Success;
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Can't get any data";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Can't get any data";
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult AddMember(vEmployee_obj_save ItemData)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                //var chkdub = _TM_CompanyDetailService.FindCode(ItemData.code);

                var checkfullname = _TM_EmployeeForeign_VisaService.FindByname(ItemData.name_eng, ItemData.lastname_eng);
                string status = "";
                #region check active status
                if (ItemData.active_status != null)
                {
                    if (ItemData.active_status == "Y")
                    {
                        status = "Y";
                    }
                    else
                    {
                        status = "N";
                    }
                }
                else
                {
                    status = "Y";
                }
                #endregion
                //First create
                if (checkfullname != null)
                {
                    if (checkfullname.Employeename_ENG == ItemData.name_eng && checkfullname.Employeesurname_ENG == ItemData.lastname_eng && checkfullname.Id + "" != ItemData.IdEMP)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, This Employee Name and Lastname already have , Please input other name and last name.";
                        return Json(new
                        {
                            result
                        });
                    }
                    //Not have name and lastname use in database
                    else
                    {
                        //Check have id Employee 
                        if (ItemData.IdEMP != null)
                        {
                            TM_EmployeeForeign_Visa objSave = new TM_EmployeeForeign_Visa
                            {
                                Id = Convert.ToInt32(ItemData.IdEMP),
                                create_user = CGlobal.UserInfo.UserId,
                                update_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                update_date = dNow,
                                active_status = status,
                                EmployeeNo = ItemData.code + "",
                                TM_Prefix_Id = Convert.ToInt32(ItemData.TM_Prefix_Id),
                                TM_Company_Id = Convert.ToInt32(ItemData.Company_name),
                                Employeename_ENG = ItemData.name_eng + "",
                                Employeename_TH = ItemData.name_th + "",
                                Employee_Middle_ENG = ItemData.middle + "",
                                Employeesurname_ENG = ItemData.lastname_eng + "",
                                Employeesurname_TH = ItemData.lastname_th + "",
                                Employee_email = ItemData.mail + "",
                                Employee_telephone = ItemData.telephone + "",
                                family_group = ItemData.membercode + "",

                            };

                            var sCom = _TM_EmployeeForeign_VisaService.CreateNewOrUpdate(ref objSave);
                            if (sCom > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Can't get any data";
                            }
                        }
                        //Not have name and last name
                        else
                        {
                            //check id old or new
                            //Old and update
                            if (ItemData.IdEMP != "0")
                            {
                                TM_EmployeeForeign_Visa objSave = new TM_EmployeeForeign_Visa
                                {
                                    Id = Convert.ToInt32(ItemData.IdEMP),
                                    create_user = CGlobal.UserInfo.UserId,
                                    update_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    update_date = dNow,
                                    active_status = status,
                                    EmployeeNo = ItemData.code + "",
                                    TM_Prefix_Id = Convert.ToInt32(ItemData.TM_Prefix_Id),
                                    TM_Company_Id = Convert.ToInt32(ItemData.Company_name),
                                    Employeename_ENG = ItemData.name_eng + "",
                                    Employeename_TH = ItemData.name_th + "",
                                    Employee_Middle_ENG = ItemData.middle + "",
                                    Employeesurname_ENG = ItemData.lastname_eng + "",
                                    Employeesurname_TH = ItemData.lastname_th + "",
                                    Employee_email = ItemData.mail + "",
                                    Employee_telephone = ItemData.telephone + "",
                                    family_group = ItemData.membercode + "",
                                    staff_No = ItemData.staff + "",
                                    TM_Remark_Id = Convert.ToInt32(ItemData.remark)
                                };

                                var sCom = _TM_EmployeeForeign_VisaService.CreateNewOrUpdate(ref objSave);
                                if (sCom > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Can't get any data";
                                }
                            }
                        }
                    }
                }
                else
                {
                        TM_EmployeeForeign_Visa objSave = new TM_EmployeeForeign_Visa
                        {
                             create_user = CGlobal.UserInfo.UserId,
                             update_user = CGlobal.UserInfo.UserId,
                             create_date = dNow,
                             update_date = dNow,
                             active_status = "Y",
                             EmployeeNo = ItemData.code + "",
                             TM_Prefix_Id = Convert.ToInt32(ItemData.TM_Prefix_Id),
                             Employeename_ENG = ItemData.name_eng + "",
                             TM_Company_Id = Convert.ToInt32(ItemData.Company_name),
                             Employeename_TH = ItemData.name_th + "",
                             Employee_Middle_ENG = ItemData.middle + "",
                             Employeesurname_ENG = ItemData.lastname_eng + "",
                             Employeesurname_TH = ItemData.lastname_th + "",
                             Employee_email = ItemData.mail + "",
                             Employee_telephone = ItemData.telephone + "",
                             family_group = ItemData.membercode + "",
                             staff_No = ItemData.staff + "",
                             TM_Remark_Id = Convert.ToInt32(ItemData.remark)
                        };

                             var sCom = _TM_EmployeeForeign_VisaService.CreateNewOrUpdate(ref objSave);
                             if (sCom > 0)
                             {
                                 result.Status = SystemFunction.process_Success;
                             }
                             else
                             {
                                 result.Status = SystemFunction.process_Failed;
                                 result.Msg = "Error, Can't get any data";
                             }
                }



            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                //result.Msg = "Error, Can't get any data";
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult DeleteEmployee(vEmployee_obj_save ItemData)
        {
            CResutlWebMethod result = new CResutlWebMethod();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                string status = "D";
                //Delete
                if (ItemData != null)
                {
                    var _getdataCustomer = _TM_EmployeeForeign_VisaService.FindCode(ItemData.code);
                    var _getdataMember = _TM_EmployeeForeign_VisaService.FindMembyCodeactive(ItemData.Id);
                    if (ItemData.remark_delete == null || ItemData.remark_delete == "")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Input data remark.";
                        return Json(new
                        {
                            result
                        });
                    }
                    if (_getdataCustomer.active_status == "Y" && _getdataCustomer.active_status != "")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Customer is not inactive.";
                        return Json(new
                        {
                            result
                        });
                    }
                    if (_getdataMember != null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Member family is not inactive.";
                        return Json(new
                        {
                            result
                        });
                    }
                    TM_EmployeeForeign_Visa objSave = new TM_EmployeeForeign_Visa
                    {
                        Id = Convert.ToInt32(ItemData.Id),
                        create_user = CGlobal.UserInfo.UserId,
                        update_user = CGlobal.UserInfo.UserId,
                        create_date = dNow,
                        update_date = dNow,
                        active_status = status,
                        //remark = ItemData.remark_delete + "",
                        EmployeeNo = ItemData.code + "",
                    };

                    var sCom = _TM_EmployeeForeign_VisaService.UpdateDelete(ref objSave);
                    if (sCom > 0)
                    {
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't delete any data";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                //result.Msg = "Error, Can't get any data";
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult AddDoc_EMP(vDocumentEmployee_obj ItemData)
        {
            CResutlWebMethod result = new CResutlWebMethod();
        if (ItemData != null)
        {
            DateTime dNow = DateTime.Now;
            var chkdub = _TM_Document_EmployeeService.FindDocumentNumber(ItemData.document_number);
            //Edit
            if (ItemData.Alert_Type_Create == "Y")
            {
                    var _getEMPId = _TM_EmployeeForeign_VisaService.FindCode(ItemData.Employee_Id);
                    if (_getEMPId != null)
                    {
                        TM_Document_Employee objSave = new TM_Document_Employee
                        {
                            create_user = CGlobal.UserInfo.UserId,
                            update_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            update_date = dNow,
                            active_status = ItemData.active_status_edit+ "",
                            doc_number = ItemData.document_number + "",
                            date_of_issued = Convert.ToDateTime(ItemData.date_of_issued),
                            TM_Country_Id = Convert.ToInt32(ItemData.Country_id),
                            valid_date = Convert.ToDateTime(ItemData.valid_date),
                            TM_EmployeeForeign_Visa_Id = _getEMPId.Id,
                            TM_Type_Document_Id = Convert.ToInt32(ItemData.Type_doc_id),
                            Id = Convert.ToInt32(ItemData.Id_Doc)
                        };

                        var sCom = _TM_Document_EmployeeService.CreateNewOrUpdate(ref objSave);
                        if (sCom > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Can't get any data";
                        }
                    }
                }
            //Create
            else
            {
                if (chkdub != null)
                {

                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, This Document Number already have , Please use edit mode insted.";
                    return Json(new
                    {
                        result
                    });
                }
                else
                {
                    if (ItemData.Employee_Id != null)
                    {
                        var _getEMPId = _TM_EmployeeForeign_VisaService.FindCode(ItemData.Employee_Id);
                        if (_getEMPId != null)
                        {
                            TM_Document_Employee objSave = new TM_Document_Employee
                            {
                                create_user = CGlobal.UserInfo.UserId,
                                update_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                update_date = dNow,
                                active_status = "Y",
                                doc_number = ItemData.document_number + "",
                                date_of_issued = Convert.ToDateTime(ItemData.date_of_issued),
                                TM_Country_Id = Convert.ToInt32(ItemData.Country_id),
                                valid_date = Convert.ToDateTime(ItemData.valid_date),
                                TM_EmployeeForeign_Visa_Id = _getEMPId.Id,
                                TM_Type_Document_Id = Convert.ToInt32(ItemData.Type_doc_id),
                            };

                            var sCom = _TM_Document_EmployeeService.CreateNewOrUpdate(ref objSave);
                            if (sCom > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Can't get any data";
                            }
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't get any data";
                    }
                }
            }

            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                //result.Msg = "Error, Can't get any data";
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult LoadEmployee(CSearchEmployee SearchItem)
        {
            vCompany_Return result = new vCompany_Return();
            List<vEmployee_obj> lstData_resutl = new List<vEmployee_obj>();
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);

            if (!string.IsNullOrEmpty(SearchItem.Company_Id) || !string.IsNullOrEmpty(SearchItem.Customer_code) || !string.IsNullOrEmpty(SearchItem.Prefix_Id) || !string.IsNullOrEmpty(SearchItem.active_status) || !string.IsNullOrEmpty(SearchItem.Employee_name) || !string.IsNullOrEmpty(SearchItem.Employee_lastname) || !string.IsNullOrEmpty(SearchItem.Staff_Id))
            {
                //Filter Search
                var lstData = _TM_EmployeeForeign_VisaService.GetDataforSearch(SearchItem.Company_Id,SearchItem.Customer_code, SearchItem.Prefix_Id, SearchItem.active_status, SearchItem.Employee_name, SearchItem.Employee_lastname, SearchItem.Staff_Id);
                var lstData_status = _TM_EmployeeForeign_VisaService.GetDataforSearchactive(SearchItem.active_status);
                if (lstData.Any())
                {
                    New_HRISEntities dbHr = new New_HRISEntities();
                    lstData_resutl = (from lstAD in lstData
                                      select new vEmployee_obj
                                      {
                                          IdEncrypt = lstAD.Id + "",
                                          code = lstAD.EmployeeNo + "",
                                          TM_Prefix_Id = lstAD.TM_Prefix_Visa.PrefixNameEN + "",
                                          full_name = lstAD.Employeename_ENG + " " + lstAD.Employeesurname_ENG + "",
                                          Company_name = lstAD.TM_Company_Visa.company_name + "",
                                          mail = lstAD.Employee_email + "",
                                          //TSIC_Id = lstAD.TM_TSIC.TSIC_ENG + "",
                                          // Staff_id = dbHr.AllInfo_WS.Where(w => w.EmpNo == lstAD.staff_id).Select(s => s.EmpFullName).FirstOrDefault(),
                                          staff = dbHr.AllInfo_WS.Where(w => w.EmpNo == lstAD.staff_No).Select(s => s.EmpFullName).FirstOrDefault(),
                                          create_user = lstAD.create_user + "",
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture("dd-MMM-yyyy") : "",
                                          update_user = lstAD.update_user + "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture("dd-MMM-yyyy") : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();
                    result.lstDataEmployee = lstData_resutl.ToList();
                }
 
                
            }

            else
            {
                var lstData = _TM_EmployeeForeign_VisaService.GetDataAllActive();
                if (lstData.Any())
                {
                    New_HRISEntities dbHr = new New_HRISEntities();
                    //var st_id = dbHr.AllInfo_WS.Find();
                    //var staff_detail = dbHr.AllInfo_WS.Where(w => w.EmpNo == staff_id).Select(s => s.EmpFullName + "").FirstOrDefault();
                    lstData_resutl = (from lstAD in lstData
                                      select new vEmployee_obj
                                      {
                                          IdEncrypt = lstAD.Id + "",
                                          code = lstAD.EmployeeNo + "",
                                          TM_Prefix_Id = lstAD.TM_Prefix_Visa.PrefixNameEN + "",
                                          full_name = lstAD.Employeename_ENG + " " + lstAD.Employeesurname_ENG + "",
                                          Company_name = lstAD.TM_Company_Visa.company_name + "",
                                          mail = lstAD.Employee_email + "",
                                          staff = dbHr.AllInfo_WS.Where(w => w.EmpNo == lstAD.staff_No).Select(s => s.EmpFullName).FirstOrDefault(),
                                          create_user = lstAD.create_user + "",
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture("dd-MMM-yyyy") : "",
                                          update_user = lstAD.update_user + "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture("dd-MMM-yyyy") : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();
                    result.lstDataEmployee = lstData_resutl.ToList();
                }
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult LoadDocument(CSearchEmployee SearchItem)
        {
            vCompany_Return result = new vCompany_Return();
            vEmployee_obj_save resultDoc = new vEmployee_obj_save();
            List<vDocumentEmployee_obj> lstData_resutl = new List<vDocumentEmployee_obj>();
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);

            if (!string.IsNullOrEmpty(SearchItem.active_status))
            {
                //Filter Search
                var lstData = _TM_Document_EmployeeService.GetDataforSearchactive(SearchItem.active_status);
                if (lstData.Any())
                {
                    New_HRISEntities dbHr = new New_HRISEntities();
                    lstData_resutl = (from lstAD in lstData.Where(w => w.TM_EmployeeForeign_Visa_Id+"" == SearchItem.Customer_code)
                                      select new vDocumentEmployee_obj
                                      {
                                          Id = lstAD.Id,
                                          IdEncrypt = lstAD.Id + "",
                                          document_number = lstAD.doc_number + "",
                                          date_of_issued = (lstAD.date_of_issued.HasValue ? lstAD.date_of_issued.Value.DateTimebyCulture("dd-MMM-yyyy") : ""),
                                          Country_id = lstAD.TM_Country.country_name_en + "",
                                          Type_doc_id = lstAD.TM_Type_Document.type_docname_eng + "",
                                          valid_date = (lstAD.valid_date.HasValue ? lstAD.valid_date.Value.DateTimebyCulture("dd-MMM-yyyy") : ""),
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();
                    result.lstDocEmployee = lstData_resutl.ToList();
                    resultDoc.lstDocEmployee = result.lstDocEmployee;
                }
            }

            else
            {
                var lstData = _TM_Document_EmployeeService.GetDataAllActive();
                if (lstData.Any())
                {
                    New_HRISEntities dbHr = new New_HRISEntities();
                    //var st_id = dbHr.AllInfo_WS.Find();
                    //var staff_detail = dbHr.AllInfo_WS.Where(w => w.EmpNo == staff_id).Select(s => s.EmpFullName + "").FirstOrDefault();
                    lstData_resutl = (from lstAD in lstData
                                      select new vDocumentEmployee_obj
                                      {
                                          Id = lstAD.Id,
                                          IdEncrypt = lstAD.Id + "",
                                          document_number = lstAD.doc_number + "",
                                          date_of_issued = (lstAD.date_of_issued.HasValue ? lstAD.date_of_issued.Value.DateTimebyCulture("dd-MMM-yyyy") : ""),
                                          Country_id = lstAD.TM_Country.country_name_en + "",
                                          Type_doc_id = lstAD.TM_Type_Document.type_docname_eng + "",
                                          valid_date = (lstAD.valid_date.HasValue ? lstAD.valid_date.Value.DateTimebyCulture("dd-MMM-yyyy") : ""),
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();
                    result.lstDocEmployee = lstData_resutl.ToList();
                }
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult LoadMember(CSearchEmployee SearchItem)
        {
            vCompany_Return result = new vCompany_Return();
            List<vMember_obj> lstData_resutl = new List<vMember_obj>();
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);

            if (!string.IsNullOrEmpty(SearchItem.active_status))
            {
                //Filter Search
                var lstData = _TM_Document_EmployeeService.GetDataforSearchactive(SearchItem.active_status);
                var lstDataMember = _TM_EmployeeForeign_VisaService.GetDataMemberforSearchactive(SearchItem.active_status);
                if (lstData.Any())
                {
                    New_HRISEntities dbHr = new New_HRISEntities();
                    lstData_resutl = (from lstAD in lstDataMember
                                      select new vMember_obj
                                      {
                                          Id = lstAD.Id,
                                          EditMember = @"<button id=""btnEditMember""  type=""button"" onclick=""EditMember('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          //IdEncrypt = lstAD.TM_CompanyDetail.Id + "",
                                          TM_Prefix_Id = lstAD.TM_Prefix_Visa.PrefixNameEN + "",
                                          code = lstAD.EmployeeNo + "",
                                          name_eng = lstAD.Employeename_ENG + "",
                                          name_th = lstAD.Employeename_TH + "",
                                          full_name = lstAD.Employeename_ENG + " " + lstAD.Employeesurname_ENG + "",
                                          lastname_eng = lstAD.Employeesurname_ENG + "",
                                          lastname_th = lstAD.Employeesurname_TH + "",
                                          middle = lstAD.Employee_Middle_ENG + "",
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          mail = lstAD.Employee_email + "",
                                      }).ToList();
                    result.lstDataMember = lstData_resutl.ToList();
                }
            }

            else
            {
                var lstData = _TM_Document_EmployeeService.GetDataAllActive();
                var lstDataMember = _TM_EmployeeForeign_VisaService.GetDataAllMemberActive();
                if (lstData.Any())
                {
                    New_HRISEntities dbHr = new New_HRISEntities();
                    //var st_id = dbHr.AllInfo_WS.Find();
                    //var staff_detail = dbHr.AllInfo_WS.Where(w => w.EmpNo == staff_id).Select(s => s.EmpFullName + "").FirstOrDefault();
                    lstData_resutl = (from lstAD in lstDataMember
                                      select new vMember_obj
                                      {
                                          Id = lstAD.Id,
                                          EditMember = @"<button id=""btnEditMember""  type=""button"" onclick=""EditMember('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          //IdEncrypt = lstAD.TM_CompanyDetail.Id + "",
                                          TM_Prefix_Id = lstAD.TM_Prefix_Visa.PrefixNameEN + "",
                                          code = lstAD.EmployeeNo + "",
                                          name_eng = lstAD.Employeename_ENG + "",
                                          name_th = lstAD.Employeename_TH + "",
                                          full_name = lstAD.Employeename_ENG + " " + lstAD.Employeesurname_ENG + "",
                                          lastname_eng = lstAD.Employeesurname_ENG + "",
                                          lastname_th = lstAD.Employeesurname_TH + "",
                                          middle = lstAD.Employee_Middle_ENG + "",
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture("dd-MMM-yyyy") : "",
                                          active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          mail = lstAD.Employee_email + "",
                                      }).ToList();
                    result.lstDataMember = lstData_resutl.ToList();
                }
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        #endregion
    }
}