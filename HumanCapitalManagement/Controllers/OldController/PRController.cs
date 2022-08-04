using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.OldTable;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HumanCapitalManagement.Controllers.OldController
{
    public class PRController : BaseController
    {
        private StoreDb db = new StoreDb();
        private New_HRISEntities dbHr = new New_HRISEntities();
        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();

        // GET: PR
        public ActionResult FormList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPRForm result = new vPRForm();
            #region main code
            CSearchPRForm SearchItem = new CSearchPRForm();
            var _Udivision = CGlobal.GetDivision().Select(s => s.sName).ToArray();

            IQueryable<PRForm> sPRQuery = db.PRForms.Where(w => _Udivision.Contains(w.Division) && w.Status.Id == 6);

            string BackUrl = "";
            if (!string.IsNullOrEmpty(qryStr))
            {
                BackUrl = Uri.EscapeDataString(qryStr);
                SearchItem = (CSearchPRForm)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPRForm)));
                if (SearchItem.division != "")
                {
                    sPRQuery = sPRQuery.Where(w => w.Division == SearchItem.division);
                }
            }

            var lstData = sPRQuery.ToList();

            if (lstData.Any())
            {
                string[] UserNo = lstData.Select(s => s.RequestBy).ToArray();
                var sEmp = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && UserNo.Contains(w.Employeeno)).ToList();

                result.lstData = (from lstAD in lstData
                                  from lstEmp in sEmp.Where(w => w.Employeeno == lstAD.RequestBy).DefaultIfEmpty(new Employee())
                                  select new vPRFormData
                                  {
                                      request_type = lstAD.RequestType.RequestTypeDesc,
                                      rank = lstAD.Positions.Select(s => s.Rank).FirstOrDefault(),
                                      pr_status = lstAD.Status.StatusDesc,
                                      hc = lstAD.Positions.Select(s => s.Headcount + "").FirstOrDefault(),
                                      position = lstAD.Positions.Select(s => s.PositionTitle).FirstOrDefault(),
                                      remark = lstAD.Remark.StringRemark(35),
                                      request_by = lstEmp.UserID,
                                      request_date = lstAD.RequestDate.DateTimeWithTimebyCulture(),
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + lstAD.Id + "" + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                  }).ToList();

            }
            #endregion



            return View(result);
        }
        public ActionResult Form(string id)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            //Allow only manager up to access
            //List<string> allowRank = new List<string>();
            //allowRank.Add("ADV");
            //allowRank.Add("PTR");
            //allowRank.Add("EXEC DI");
            //allowRank.Add("ASSOC D");
            //allowRank.Add("MGR");
            //if (!allowRank.Contains(User.Rank) && User.roles.Count() == 0)
            //{
            //    return RedirectToAction("NoPermission", "Home");
            //}

            DataTable dt;



            ViewBag.currFY = genFY();

            //Request
            ViewBag.RequestID = id;
            List<SelectListItem> divLst = getDivisionByBU();
            if (CGlobal.UserInfo.roles.Contains("Administrator") || CGlobal.UserInfo.roles.Contains("Recruiter"))
            {
                divLst.Add(new SelectListItem { Text = "Audit (Mass)", Value = "Audit (Mass)" });
                divLst.Add(new SelectListItem { Text = "T&L (Mass)", Value = "T&L (Mass)" });
            }
            else if (CGlobal.UserInfo.Pool == "Audit")
            {
                divLst.Add(new SelectListItem { Text = "Audit (Mass)", Value = "Audit (Mass)" });
            }
            else if (CGlobal.UserInfo.Pool == "Tax")
            {
                divLst.Add(new SelectListItem { Text = "T&L (Mass)", Value = "T&L (Mass)" });
            }
            ViewBag.DivLst = divLst;
            //---
            ViewBag.ReqCompany = "";
            ViewBag.ReqDiv = "";
            ViewBag.OperatePlanHC = 0; //getOperatingPlanHC(User.UnitGroup);
            ViewBag.HRISHC = 0; //getCurrentHC(User.UnitGroup);
            ViewBag.ReqTypeLst = getRequestType();

            //Request position detail
            ViewBag.RankLst = getRankList(CGlobal.UserInfo.Company);
            ViewBag.TypeReq = getEmpType();
            ViewBag.HClst = getHCNumber();

            clearSession();

            PRForm thisPR = new PRForm();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    ViewBag.EmployeeNo = CGlobal.UserInfo.EmployeeNo;
                    ViewBag.roles = new string[] { };
                    if (CGlobal.UserInfo.roles != null && CGlobal.UserInfo.roles.Length > 0)
                    {
                        ViewBag.roles = CGlobal.UserInfo.roles;
                    }
                    long prid = long.Parse(id);

                    var pr = from p in db.PRForms.Include("RequestType").Include("Positions").Include("Candidates").Include("Status").Include("AttachFiles").Include("Candidates.InterviewStatus").Include("Candidates.HiringProcess").Include("Positions.EmpType")
                             where p.Id == prid
                             select p;

                    thisPR = pr.ToList<PRForm>()[0];
                    ViewBag.refNo = thisPR.RefNo;
                    //ViewBag.ReqCompany = thisPR.CompanyCode;
                    //ViewBag.ReqDiv = thisPR.Division;
                    ViewBag.HRISHC = getCurrentHC(thisPR.Division);
                    ViewBag.OperatePlanHC = getOperatingPlanHC(thisPR.Division);

                    //Approval
                    dt = wsHRIS.getEmployeeInfoByEmpNo(thisPR.RequestBy);
                    ViewBag.RequestBy = dt.Rows[0]["EmpFullName"].ToString();
                    ViewBag.RequestDate = thisPR.RequestDate.ToString("dd-MMM-yyyy");
                    Session["RequestBy"] = thisPR.RequestBy;
                    dt = wsHRIS.getEmployeeInfoByEmpNo(thisPR.HeadApproveBy);
                    ViewBag.HeadReqBy = dt.Rows[0]["EmpFullName"].ToString();
                    if (thisPR.HeadApproveDate != null)
                    {
                        ViewBag.HeadReqDate = ((DateTime)thisPR.HeadApproveDate).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        ViewBag.HeadReqDate = "";
                    }
                    Session["HeadReqBy"] = thisPR.HeadApproveBy;

                    if (string.IsNullOrEmpty(thisPR.BUApproveBy))
                    {
                        ViewBag.AprovedBy = "";
                        ViewBag.AprovedDate = "";
                        Session["AprovedBy"] = "";
                    }
                    else
                    {
                        dt = wsHRIS.getEmployeeInfoByEmpNo(thisPR.BUApproveBy);
                        ViewBag.AprovedBy = dt.Rows[0]["EmpFullName"].ToString();
                        if (thisPR.BUApproveDate != null)
                        {
                            ViewBag.AprovedDate = ((DateTime)thisPR.BUApproveDate).ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            ViewBag.AprovedDate = "";
                        }
                        Session["AprovedBy"] = thisPR.BUApproveBy;
                    }


                    if (string.IsNullOrEmpty(thisPR.CeoApproveBy))
                    {
                        ViewBag.CEOAprovedBy = "";
                        ViewBag.CEOAprovedDate = "";
                        Session["CEOAprovedBy"] = "";
                    }
                    else
                    {
                        dt = wsHRIS.getEmployeeInfoByEmpNo(thisPR.CeoApproveBy);
                        ViewBag.CEOAprovedBy = dt.Rows[0]["EmpFullName"].ToString();
                        if (thisPR.CeoApproveDate != null)
                        {
                            ViewBag.CEOAprovedDate = ((DateTime)thisPR.CeoApproveDate).ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            ViewBag.CEOAprovedDate = "";
                        }
                        Session["CEOAprovedBy"] = thisPR.CeoApproveBy;
                    }

                    if (CGlobal.UserInfo.roles.Contains("Administrator") || CGlobal.UserInfo.roles.Contains("Recruiter")) //
                    {
                        ViewBag.IsAdmin = true;
                    }
                    else
                    {
                        ViewBag.IsAdmin = false;
                    }


                }
                catch (Exception e)
                {

                }
            }
            else
            {
                //Approval default
                ViewBag.RequestBy = CGlobal.UserInfo.FullName;
                ViewBag.RequestDate = "";
                Session["RequestBy"] = CGlobal.UserInfo.EmployeeNo;
                ViewBag.IsAdmin = false;

                getHeadcount(CGlobal.UserInfo.UnitGroup);
                //List<SpecialRole> divHead = db.SpecialRoles.Where(x => x.Division == User.UnitGroup).ToList<SpecialRole>();
                //if (divHead.Count() > 0)
                //{
                //    dt = wsHRIS.getEmployeeInfoByEmpNo(divHead[0].EmpNo);
                //    ViewBag.HeadReqBy = dt.Rows[0]["EmpFullName"].ToString();
                //    ViewBag.HeadReqDate = "";
                //    Session["HeadReqBy"] = dt.Rows[0]["EmpNo"].ToString();
                //}

                //List<BUPlanner> divBU = db.BUPlanners.Where(x => x.CostCenter == User.Pool).ToList<BUPlanner>();
                //if (divBU.Count() > 0)
                //{
                //    dt = wsHRIS.getEmployeeInfoByEmpNo(divBU[0].Planner);
                //    ViewBag.AprovedBy = dt.Rows[0]["EmpFullName"].ToString();
                //    ViewBag.AprovedDate = "";
                //    Session["AprovedBy"] = dt.Rows[0]["EmpNo"].ToString();
                //}

                //For MGR up only
                ViewBag.CEOAprovedBy = "Winid Silamongkol";
                ViewBag.CEOAprovedDate = "";
                Session["CEOAprovedBy"] = "00001620";

            }
            return View("Form", thisPR);
        }

        [HttpPost]
        public ActionResult submit(FormCollection collection)
        {
            string division = ((string[])collection.GetValues("ddlDivision"))[0];//Session["selDivision"].ToString();
            string company = ((string[])collection.GetValues("txtCompany"))[0];

            //txtRemark
            string remark = ((string[])collection.GetValues("txtRemark"))[0];

            //Request Type
            string requestType = ((string[])collection.GetValues("hdSelReqType"))[0];

            //Position
            string title = ((string[])collection.GetValues("txtReqPosTitle"))[0];
            string ddlRank = ((string[])collection.GetValues("ddlRank"))[0];
            string txtRank = ((string[])collection.GetValues("txtRank"))[0];
            string dpPeriod = ((string[])collection.GetValues("dpPeriod"))[0];
            string ddlHC = ((string[])collection.GetValues("ddlHC"))[0];
            string ddlType = ((string[])collection.GetValues("ddlType"))[0];
            string txtType = ((string[])collection.GetValues("txtType"))[0];
            string txtSpecific = ((string[])collection.GetValues("txtSpecific"))[0];
            string ddlReqType = ((string[])collection.GetValues("ddlReqType"))[0];
            string dpPeriodTo = ((string[])collection.GetValues("dpPeriodTo"))[0];

            Position position;
            if (ddlReqType == "Permanent Staff")
            {
                //data: { title: title, rank: rankTxt, period: period, headcount: headcount, type: type, specify: specify },
                position = addNewPosition(title, txtRank, dpPeriod, ddlHC, ddlType, txtSpecific);
            }
            else if (ddlReqType == "Temporary Staff")
            {
                //data: { title: title, rank: rankTxt, period: period, headcount: headcount, type: fixType, specify: specify },
                position = addNewPosition(title, txtRank, dpPeriod, ddlHC, txtType, txtSpecific);
            }
            else if (ddlReqType == "Trainee")
            {
                //data: { title: title, rank: "trainee", period: period, headcount: headcount, type: fixType, specify: periodTo },
                position = addNewPosition(title, "Trainee", dpPeriod, ddlHC, txtType, dpPeriodTo);
            }
            else
            {
                position = new Position();
            }
            //List<Position> positions = (List<Position>)Session["positionList"];

            //Attachment
            List<AttachFile> attachLst = (List<AttachFile>)Session["newAttachFile"];

            //Approver
            getHeadcount(division);
            string requestBy = Session["RequestBy"].ToString();
            string headBy = Session["HeadReqBy"].ToString();
            string BUBy = "00016421";//Session["AprovedBy"].ToString();
            string CEOBy;
            List<string> mngUp = new List<string>();
            mngUp.Add("Partner");
            mngUp.Add("Executive Director");
            mngUp.Add("Associate Director");
            mngUp.Add("Manager");

            if (position != null)
            {
                if (mngUp.Contains(position.Rank) || division.Contains("Mass"))
                {
                    CEOBy = Session["CEOAprovedBy"].ToString();
                }
                else
                {
                    CEOBy = "";
                }
            }
            else if (division.Contains("Mass"))
            {
                CEOBy = Session["CEOAprovedBy"].ToString();
            }
            else
            {
                CEOBy = "";
            }
            CEOBy = "00012358";
            bool isAdmin = false;
            if (User.IsInRole("CEO") || User.IsInRole("Administrator") || User.IsInRole("Recruiter"))
            {
                isAdmin = true;
            }

            //Description
            string jobDesc = ((string[])collection.GetValues("txtJobDesc"))[0];
            string education = ((string[])collection.GetValues("txtEduBack"))[0];
            //string language = ((string[])collection.GetValues("txtLanguage"))[0];
            long id = 0;

            try
            {
                PRForm form = new PRForm();
                if (requestType == "Permanent Staff")
                {
                    form = new PRForm();
                    form.RefNo = generateRefNo(company);
                    form.CompanyCode = company;
                    form.Division = division;
                    form.Remark = remark;
                    form.RequestType = db.RequestTypes.Single(x => x.RequestTypeDesc == "Permanent Staff");
                    form.Status = db.Status.Single(x => x.Sort == 2);
                    form.RequestDate = DateTime.Now;

                    form.RequestBy = requestBy;
                    form.HeadApproveBy = headBy;
                    form.BUApproveBy = BUBy;
                    form.CeoApproveBy = CEOBy;

                    //Auto approve for admin & recruiter
                    if (isAdmin)
                    {
                        form.HeadApproveDate = DateTime.Now;
                        form.BUApproveDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(CEOBy))
                        {
                            form.CeoApproveDate = DateTime.Now;
                        }
                        form.Status = db.Status.Single(x => x.Sort == 5);
                    }

                    form.JobDescription = jobDesc;
                    form.EducationDesc = education;
                    //form.Language = language;

                    db.PRForms.Add(form);
                    db.SaveChanges();

                    PRForm currentPR = db.PRForms.Single(x => x.Id == form.Id);
                    //Position newPos;
                    //foreach (Position pos in positions)
                    //{
                    //    newPos = new Position();
                    //    newPos.EmpType = db.EmployeeTypes.Single(x => x.Id == pos.EmpType.Id);
                    //    newPos.PositionTitle = pos.PositionTitle;
                    //    newPos.PRForm = currentPR;
                    //    newPos.Rank = pos.Rank;
                    //    newPos.TargetPeriod = pos.TargetPeriod;
                    //    newPos.Headcount = pos.Headcount;
                    //    db.Positions.Add(newPos);                        
                    //}
                    position.PRForm = currentPR;
                    db.Positions.Add(position);
                    db.SaveChanges();

                    //attachLst
                    AttachFile newAtt;
                    foreach (AttachFile att in attachLst)
                    {
                        newAtt = new AttachFile();
                        newAtt.FileName = att.FileName;
                        newAtt.FilePath = att.FilePath;
                        newAtt.PRForm = currentPR;
                        db.AttachFiles.Add(newAtt);
                    }
                    db.SaveChanges();

                    clearSession();
                    ViewBag.response = "Submition completed";
                }
                else if (requestType == "Temporary Staff")
                {
                    form = new PRForm();
                    form.RefNo = generateRefNo(company);
                    form.CompanyCode = company;
                    form.Division = division;
                    form.Remark = remark;
                    form.RequestType = db.RequestTypes.Single(x => x.RequestTypeDesc == "Temporary Staff");
                    form.Status = db.Status.Single(x => x.Sort == 2);
                    form.RequestDate = DateTime.Now;

                    form.RequestBy = requestBy;
                    form.HeadApproveBy = headBy;
                    form.BUApproveBy = BUBy;
                    form.CeoApproveBy = CEOBy;

                    //Auto approve for admin & recruiter
                    if (isAdmin)
                    {
                        form.HeadApproveDate = DateTime.Now;
                        form.BUApproveDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(CEOBy))
                        {
                            form.CeoApproveDate = DateTime.Now;
                        }
                        form.Status = db.Status.Single(x => x.Sort == 5);
                    }

                    form.JobDescription = jobDesc;
                    form.EducationDesc = education;
                    //form.Language = language;

                    db.PRForms.Add(form);
                    db.SaveChanges();

                    PRForm currentPR = db.PRForms.Single(x => x.Id == form.Id);
                    //Position newPos;
                    //foreach (Position pos in positions)
                    //{
                    //    newPos = new Position();
                    //    newPos.EmpType = db.EmployeeTypes.Single(x => x.Id == pos.EmpType.Id);
                    //    newPos.PositionTitle = pos.PositionTitle;
                    //    newPos.PRForm = currentPR;
                    //    newPos.Rank = pos.Rank;
                    //    newPos.TargetPeriod = pos.TargetPeriod;
                    //    newPos.Headcount = pos.Headcount;
                    //    db.Positions.Add(newPos);
                    //}
                    position.PRForm = currentPR;
                    db.Positions.Add(position);
                    db.SaveChanges();
                    clearSession();
                    ViewBag.response = "Submition completed";
                }
                else if (requestType == "Trainee")
                {
                    form = new PRForm();
                    form.RefNo = generateRefNo(company);
                    form.CompanyCode = company;
                    form.Division = division;
                    form.Remark = remark;
                    form.RequestType = db.RequestTypes.Single(x => x.RequestTypeDesc == "Trainee");
                    form.Status = db.Status.Single(x => x.Sort == 2);
                    form.RequestDate = DateTime.Now;

                    form.RequestBy = requestBy;
                    form.HeadApproveBy = headBy;
                    form.BUApproveBy = BUBy;
                    form.CeoApproveBy = CEOBy;

                    //Auto approve for admin & recruiter
                    if (isAdmin)
                    {
                        form.HeadApproveDate = DateTime.Now;
                        form.BUApproveDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(CEOBy))
                        {
                            form.CeoApproveDate = DateTime.Now;
                        }
                        form.Status = db.Status.Single(x => x.Sort == 5);
                    }

                    form.JobDescription = jobDesc;
                    form.EducationDesc = education;
                    //form.Language = language;

                    db.PRForms.Add(form);
                    db.SaveChanges();

                    PRForm currentPR = db.PRForms.Single(x => x.Id == form.Id);
                    //Position newPos;
                    //foreach (Position pos in positions)
                    //{
                    //    newPos = new Position();
                    //    newPos.EmpType = db.EmployeeTypes.Single(x => x.Id == pos.EmpType.Id);
                    //    newPos.PositionTitle = pos.PositionTitle;
                    //    newPos.PRForm = currentPR;
                    //    newPos.Rank = pos.Rank;
                    //    newPos.TargetPeriod = pos.TargetPeriod;
                    //    newPos.TargetPeriodTo = pos.TargetPeriodTo;
                    //    newPos.Headcount = pos.Headcount;
                    //    db.Positions.Add(newPos);
                    //}
                    position.PRForm = currentPR;
                    db.Positions.Add(position);
                    db.SaveChanges();
                    clearSession();
                    ViewBag.response = "Submition completed";
                }
                else if (requestType == "Mass Recruitment : Audit Staff")
                {
                    //Mass Input
                    string txtAuditHCTotal = ((string[])collection.GetValues("txtAuditHCTotal"))[0];
                    string txtMassAuditG1 = ((string[])collection.GetValues("txtMassAuditG1"))[0];
                    string txtMassAuditG2 = ((string[])collection.GetValues("txtMassAuditG2"))[0];
                    string txtMassAuditG3 = ((string[])collection.GetValues("txtMassAuditG3"))[0];
                    string txtMassAuditG4 = ((string[])collection.GetValues("txtMassAuditG4"))[0];
                    string txtMassAuditG5 = ((string[])collection.GetValues("txtMassAuditG5"))[0];

                    form = new PRForm();
                    form.RefNo = generateRefNo(company);
                    form.CompanyCode = company;
                    form.Division = division;
                    form.Remark = remark;
                    form.RequestType = db.RequestTypes.Single(x => x.RequestTypeDesc == "Mass Recruitment : Audit Staff");
                    form.Status = db.Status.Single(x => x.Sort == 2);
                    form.RequestDate = DateTime.Now;

                    form.RequestBy = requestBy;
                    form.HeadApproveBy = headBy;
                    form.BUApproveBy = BUBy;

                    //Auto approve for admin & recruiter
                    if (isAdmin)
                    {
                        form.HeadApproveDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(headBy))
                        {
                            form.BUApproveDate = DateTime.Now;
                        }
                        form.Status = db.Status.Single(x => x.Sort == 5);
                    }

                    db.PRForms.Add(form);
                    db.SaveChanges();

                    Position pos = new Position();
                    pos.PositionTitle = "Audit Mass (Staff)";
                    pos.EmpType = db.EmployeeTypes.Single(x => x.EmployeeTypeDesc == "Mass (Staff)");
                    pos.PRForm = db.PRForms.Single(x => x.Id == form.Id); ;
                    pos.Headcount = int.Parse(txtAuditHCTotal);
                    pos.Headcount1 = int.Parse(txtMassAuditG1);
                    pos.Headcount2 = int.Parse(txtMassAuditG2);
                    pos.Headcount3 = int.Parse(txtMassAuditG3);
                    pos.Headcount4 = int.Parse(txtMassAuditG4);
                    pos.Headcount5 = int.Parse(txtMassAuditG5);
                    db.Positions.Add(pos);
                    db.SaveChanges();
                    clearSession();
                    ViewBag.response = "Submition completed";
                }
                else if (requestType == "Mass Recruitment : Tax & Legal Staff")
                {
                    //Mass Input
                    string txtTaxHCTotal = ((string[])collection.GetValues("txtTaxHCTotal"))[0];
                    string txtMassTax1 = ((string[])collection.GetValues("txtMassTax1"))[0];
                    string txtMassTax2 = ((string[])collection.GetValues("txtMassTax2"))[0];
                    string txtMassTax3 = ((string[])collection.GetValues("txtMassTax3"))[0];
                    string txtMassTax4 = ((string[])collection.GetValues("txtMassTax4"))[0];
                    string txtMassTax5 = ((string[])collection.GetValues("txtMassTax5"))[0];
                    string txtMassTax6 = ((string[])collection.GetValues("txtMassTax6"))[0];
                    string txtMassTax7 = ((string[])collection.GetValues("txtMassTax7"))[0];

                    form = new PRForm();
                    form.RefNo = generateRefNo(company);
                    form.CompanyCode = company;
                    form.Division = division;
                    form.Remark = remark;
                    form.RequestType = db.RequestTypes.Single(x => x.RequestTypeDesc == "Mass Recruitment : Tax & Legal Staff");
                    form.Status = db.Status.Single(x => x.Sort == 5);
                    form.RequestDate = DateTime.Now;

                    form.RequestBy = requestBy;
                    form.HeadApproveBy = headBy;
                    form.BUApproveBy = BUBy;

                    //Auto approve for admin & recruiter
                    if (isAdmin)
                    {
                        form.HeadApproveDate = DateTime.Now;
                        form.BUApproveDate = DateTime.Now;
                        form.Status = db.Status.Single(x => x.Sort == 5);
                    }

                    db.PRForms.Add(form);
                    db.SaveChanges();

                    Position pos = new Position();
                    pos.PositionTitle = "Tax Mass (Staff)";
                    pos.EmpType = db.EmployeeTypes.Single(x => x.EmployeeTypeDesc == "Mass (Staff)");
                    pos.PRForm = db.PRForms.Single(x => x.Id == form.Id); ;
                    pos.Headcount = int.Parse(txtTaxHCTotal);
                    pos.Headcount1 = int.Parse(txtMassTax1);
                    pos.Headcount2 = int.Parse(txtMassTax2);
                    pos.Headcount3 = int.Parse(txtMassTax3);
                    pos.Headcount4 = int.Parse(txtMassTax4);
                    pos.Headcount5 = int.Parse(txtMassTax5);
                    pos.Headcount6 = int.Parse(txtMassTax6);
                    pos.Headcount7 = int.Parse(txtMassTax7);
                    db.Positions.Add(pos);
                    db.SaveChanges();
                    clearSession();
                    ViewBag.response = "Submition completed";
                }
                else if (requestType == "Mass Recruitment : Audit Trainee")
                {
                    //Mass Input
                    string txtAuditHCTotal = ((string[])collection.GetValues("txtAuditTraineeHCTotal"))[0];
                    string txtMassAuditG1 = ((string[])collection.GetValues("txtMassAuditTraineeG1"))[0];
                    string txtMassAuditG2 = ((string[])collection.GetValues("txtMassAuditTraineeG2"))[0];
                    string txtMassAuditG3 = ((string[])collection.GetValues("txtMassAuditTraineeG3"))[0];
                    string txtMassAuditG4 = ((string[])collection.GetValues("txtMassAuditTraineeG4"))[0];
                    string txtMassAuditG5 = ((string[])collection.GetValues("txtMassAuditTraineeG5"))[0];

                    form = new PRForm();
                    form.RefNo = generateRefNo(company);
                    form.CompanyCode = company;
                    form.Division = division;
                    form.Remark = remark;
                    form.RequestType = db.RequestTypes.Single(x => x.RequestTypeDesc == "Mass Recruitment : Audit Trainee");
                    form.Status = db.Status.Single(x => x.Sort == 2);
                    form.RequestDate = DateTime.Now;

                    form.RequestBy = requestBy;
                    form.HeadApproveBy = headBy;
                    form.BUApproveBy = BUBy;

                    //Auto approve for admin & recruiter
                    if (isAdmin)
                    {
                        form.HeadApproveDate = DateTime.Now;
                        form.BUApproveDate = DateTime.Now;
                        form.Status = db.Status.Single(x => x.Sort == 5);
                    }

                    db.PRForms.Add(form);
                    db.SaveChanges();

                    Position pos = new Position();
                    pos.PositionTitle = "Audit Mass (Trainee)";
                    pos.EmpType = db.EmployeeTypes.Single(x => x.EmployeeTypeDesc == "Mass (Trainee)");
                    pos.PRForm = db.PRForms.Single(x => x.Id == form.Id); ;
                    pos.Headcount = int.Parse(txtAuditHCTotal);
                    pos.Headcount1 = int.Parse(txtMassAuditG1);
                    pos.Headcount2 = int.Parse(txtMassAuditG2);
                    pos.Headcount3 = int.Parse(txtMassAuditG3);
                    pos.Headcount4 = int.Parse(txtMassAuditG4);
                    pos.Headcount5 = int.Parse(txtMassAuditG5);
                    db.Positions.Add(pos);
                    db.SaveChanges();
                    clearSession();
                    ViewBag.response = "Submition completed";
                }
                else if (requestType == "Mass Recruitment : Tax & Legal Trainee")
                {
                    //Mass Input
                    string txtTaxTraineeHCTotal = ((string[])collection.GetValues("txtTaxTraineeHCTotal"))[0];
                    string txtMassTaxTrainee1 = ((string[])collection.GetValues("txtMassTaxTrainee1"))[0];
                    string txtMassTaxTrainee2 = ((string[])collection.GetValues("txtMassTaxTrainee2"))[0];
                    string txtMassTaxTrainee3 = ((string[])collection.GetValues("txtMassTaxTrainee3"))[0];
                    string txtMassTaxTrainee4 = ((string[])collection.GetValues("txtMassTaxTrainee4"))[0];
                    string txtMassTaxTrainee5 = ((string[])collection.GetValues("txtMassTaxTrainee5"))[0];
                    string txtMassTaxTrainee6 = ((string[])collection.GetValues("txtMassTaxTrainee6"))[0];
                    string txtMassTaxTrainee7 = ((string[])collection.GetValues("txtMassTaxTrainee7"))[0];

                    form = new PRForm();
                    form.RefNo = generateRefNo(company);
                    form.CompanyCode = company;
                    form.Division = division;
                    form.Remark = remark;
                    form.RequestType = db.RequestTypes.Single(x => x.RequestTypeDesc == "Mass Recruitment : Tax & Legal Trainee");
                    form.Status = db.Status.Single(x => x.Sort == 2);
                    form.RequestDate = DateTime.Now;

                    form.RequestBy = requestBy;
                    form.HeadApproveBy = headBy;
                    form.BUApproveBy = BUBy;

                    //Auto approve for admin & recruiter
                    if (isAdmin)
                    {
                        form.HeadApproveDate = DateTime.Now;
                        form.BUApproveDate = DateTime.Now;
                        form.Status = db.Status.Single(x => x.Sort == 5);
                    }

                    db.PRForms.Add(form);
                    db.SaveChanges();

                    Position pos = new Position();
                    pos.PositionTitle = "Tax Mass (Trainee)";
                    pos.EmpType = db.EmployeeTypes.Single(x => x.EmployeeTypeDesc == "Mass (Trainee)");
                    pos.PRForm = db.PRForms.Single(x => x.Id == form.Id); ;
                    pos.Headcount = int.Parse(txtTaxTraineeHCTotal);
                    pos.Headcount1 = int.Parse(txtMassTaxTrainee1);
                    pos.Headcount2 = int.Parse(txtMassTaxTrainee2);
                    pos.Headcount3 = int.Parse(txtMassTaxTrainee3);
                    pos.Headcount4 = int.Parse(txtMassTaxTrainee4);
                    pos.Headcount5 = int.Parse(txtMassTaxTrainee5);
                    pos.Headcount6 = int.Parse(txtMassTaxTrainee6);
                    pos.Headcount7 = int.Parse(txtMassTaxTrainee7);
                    db.Positions.Add(pos);
                    db.SaveChanges();
                    clearSession();
                    ViewBag.response = "Submition completed";
                }


                //id = form.Id;
                string to = "";
                string from = CGlobal.UserInfo.EMail;
                string subject = "";
                string message = "";
                if (form.Id != 0)
                {
                    //เอา if not admin ออก หลังจากเอา auto approve ออก
                    to = wsHRIS.getEmployeeEmail("k", form.HeadApproveBy);
                    if (!string.IsNullOrEmpty(to))
                    {
                        subject = "HCM: Requesting for Headcount";
                        message = genApproveMailBody(form);
                        if (sendMail(from, to, "", subject, message))
                        {
                            ViewBag.test = true;
                        }
                        else
                        {
                            ViewBag.test = false;
                        }
                    }

                    //return View("Form", form);
                    return RedirectToAction("Form", new RouteValueDictionary(new { controller = "PR", action = "Form", Id = form.Id }));

                    //Redirect(Url.Action("Form", "PR", new { id = form.Id }));
                    //return RedirectToAction("Form", "PR", new { id = form.Id.ToString() });
                }

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            //return Form(id.ToString());
            return Redirect(Url.Action("Form", "PR"));
        }

        public ActionResult Candidate(string id, string pr)
        {
            Candidate can = null;
            ViewBag.prid = pr;

            //Interview list
            var interview = from i in db.Interviews
                            orderby i.Sort
                            select new SelectListItem { Text = i.InterviewDesc, Value = i.Id.ToString() };
            List<SelectListItem> selInterview = interview.ToList<SelectListItem>();
            selInterview.Insert(0, new SelectListItem { Text = "== Select Interview ==", Value = "0", Selected = true });

            //Hiring Process list
            var hp = from i in db.HiringProcess
                     orderby i.Sort
                     select new SelectListItem { Text = i.HiringProcessDesc, Value = i.Id.ToString() };
            List<SelectListItem> selHP = hp.ToList<SelectListItem>();
            selHP.Insert(0, new SelectListItem { Text = "== Select Process ==", Value = "0" });

            //Candidate Status list
            //var cs = from i in db.CandidateStatus
            //         orderby i.Sort
            //         select new SelectListItem { Text = i.CandidateStatusDesc, Value = i.Id.ToString() };
            //List<SelectListItem> selCS = cs.ToList<SelectListItem>();
            //selCS.Insert(0, new SelectListItem { Text = "== Select Candidate Status ==", Value = "0" });

            //Onboard list
            //var onboard = from i in db.Onboards
            //                orderby i.Sort
            //              select new SelectListItem { Text = i.OnboardDesc, Value = i.Id.ToString() };
            //List<SelectListItem> selOnboard = onboard.ToList<SelectListItem>();
            //selOnboard.Insert(0, new SelectListItem { Text = "== Select Onboard ==", Value = "0"});

            //Get HR list
            //Repositories pm = new Repositories();

            var hr = from ur in db.UsersRoles.Where(w => w.Role.Name == "Recruiter").ToList()

                     select new SelectListItem { Text = ur.Username, Value = ur.Username };
            List<SelectListItem> selHR = hr.ToList<SelectListItem>();
            selHR.Insert(0, new SelectListItem { Text = "== Select HR ==", Value = "0" });

            //Set value
            if (!string.IsNullOrEmpty(id))
            {
                long lID = long.Parse(id);
                can = db.Candidates.Single(x => x.Id == lID);

                if (can.InterviewStatus != null)
                {
                    selInterview.SingleOrDefault(x => x.Value == can.InterviewStatus.Id.ToString()).Selected = true;
                }
                if (can.HiringProcess != null)
                {
                    selHP.SingleOrDefault(x => x.Value == can.HiringProcess.Id.ToString()).Selected = true;
                }
                //if (can.CandidateStatus != null)
                //{
                //    selCS.SingleOrDefault(x => x.Value == can.CandidateStatus.Id.ToString()).Selected = true;
                //}
                //if (can.OnboardStatus != null)
                //{
                //    selOnboard.SingleOrDefault(x => x.Value == can.OnboardStatus.Id.ToString()).Selected = true;
                //}
                if (!string.IsNullOrEmpty(can.HROwner))
                {
                    selHR.SingleOrDefault(x => x.Value == can.HROwner).Selected = true;
                }
            }

            ViewBag.interviewLst = selInterview;
            ViewBag.HiringProcess = selHP;
            //ViewBag.CandidateStatus = selCS;
            //ViewBag.onboardLst = selOnboard;
            ViewBag.HROwner = selHR;

            return PartialView(can);
        }

        public ActionResult addCandidate(FormCollection collection)
        {
            Candidate c = null;
            string prid = ((string[])collection.GetValues("hdPRadd"))[0];
            string name = ((string[])collection.GetValues("txtCanName"))[0];
            string rank = ((string[])collection.GetValues("txtCanRnk"))[0];
            string fullRank = ((string[])collection.GetValues("txtCanFullRnk"))[0];
            string contactNo = ((string[])collection.GetValues("txtCanContactNo"))[0];
            string interviewId = ((string[])collection.GetValues("ddlInterview"))[0];
            string HiringProcessid = ((string[])collection.GetValues("ddlHiringProcess"))[0];
            //string CandidateStatus = ((string[])collection.GetValues("ddlCandidateStatus"))[0];
            string AcceptDate = ((string[])collection.GetValues("dpAcceptDate"))[0];
            string OfferDate = ((string[])collection.GetValues("dpCanOfferDate"))[0];
            string BUDate = ((string[])collection.GetValues("dpCanBUDate"))[0];
            string startDate = ((string[])collection.GetValues("dpCanStartDate"))[0];
            string rejectDate = ((string[])collection.GetValues("dpCanRejectedDate"))[0];
            //string endDate = ((string[])collection.GetValues("dpCanEndDate"))[0];
            //string onboardId = ((string[])collection.GetValues("ddlOnboard"))[0];
            string remark = ((string[])collection.GetValues("txtCanRemark"))[0];
            string owner = ((string[])collection.GetValues("ddlOwner"))[0];
            //string joinDate = ((string[])collection.GetValues("dpCanJoinDate"))[0];

            c = new Candidate();
            c.PRForm = db.PRForms.Single(x => x.Id.ToString() == prid);
            c.CandidateName = name;
            c.rank = rank;
            c.fullRank = fullRank;
            c.ContactNo = contactNo;
            if (interviewId != "0")
            {
                c.InterviewStatus = db.Interviews.Single(x => x.Id.ToString() == interviewId);
            }
            if (HiringProcessid != "0")
            {
                c.HiringProcess = db.HiringProcess.Single(x => x.Id.ToString() == HiringProcessid);
            }
            //if (CandidateStatus != "0")
            //{
            //    c.CandidateStatus = db.CandidateStatus.Single(x => x.Id.ToString() == CandidateStatus);
            //}
            if (!string.IsNullOrEmpty(AcceptDate))
            {
                try
                {
                    c.AcceptDate = DateTime.Parse(AcceptDate);
                }
                catch (Exception e)
                {
                }
            }
            if (!string.IsNullOrEmpty(OfferDate))
            {
                try
                {
                    c.OfferingDate = DateTime.Parse(OfferDate);
                }
                catch (Exception e)
                {
                }
            }
            if (!string.IsNullOrEmpty(BUDate))
            {
                try
                {
                    c.DateByBU = DateTime.Parse(BUDate);
                }
                catch (Exception e)
                {
                }
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                try
                {
                    c.StartDate = DateTime.Parse(startDate);
                }
                catch (Exception e)
                {
                }
            }
            if (!string.IsNullOrEmpty(rejectDate))
            {
                try
                {
                    c.RejectedDate = DateTime.Parse(rejectDate);
                }
                catch (Exception e)
                {
                }
            }
            //if (!string.IsNullOrEmpty(endDate))
            //{
            //    try
            //    {
            //        c.EndDate = DateTime.Parse(endDate);
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //}
            //if (onboardId != "0")
            //{
            //    c.OnboardStatus = db.Onboards.Single(x => x.Id.ToString() == onboardId);
            //}
            c.Remark = remark;
            c.HROwner = owner;
            //if (!string.IsNullOrEmpty(joinDate))
            //{
            //    try
            //    {
            //        c.JoinDate = DateTime.Parse(joinDate);
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //}

            db.Candidates.Add(c);
            db.SaveChanges();
            ViewBag.response = "Add Candidate complete";

            ////Set Default
            ////Interview list
            //var interview = from i in db.Interviews
            //                orderby i.Sort
            //                select new SelectListItem { Text = i.InterviewDesc, Value = i.Id.ToString() };
            //List<SelectListItem> selInterview = interview.ToList<SelectListItem>();
            //selInterview.Insert(0, new SelectListItem { Text = "== Select Interview ==", Value = "0", Selected = true });

            ////Hiring Process list
            //var HiringProcess = from i in db.HiringProcess
            //            orderby i.Sort
            //            select new SelectListItem { Text = i.HiringProcessDesc, Value = i.Id.ToString() };
            //List<SelectListItem> HiringProcesss = HiringProcess.ToList<SelectListItem>();
            //HiringProcesss.Insert(0, new SelectListItem { Text = "== Select Process ==", Value = "0" });

            ////Get HR list
            //Repositories pm = new Repositories();
            //IEnumerable<UserRoleView> urv = pm.UserRole();
            //var hr = from ur in urv
            //         where ur.RoleName == "Recruiter"
            //         select new SelectListItem { Text = ur.FullName, Value = ur.Username };
            //List<SelectListItem> selHR = hr.ToList<SelectListItem>();
            //selHR.Insert(0, new SelectListItem { Text = "== Select HR ==", Value = "0" });       

            //if (c.InterviewStatus != null)
            //{
            //    selInterview.SingleOrDefault(x => x.Value == c.InterviewStatus.Id.ToString()).Selected = true;
            //}
            //if (c.HiringProcess != null)
            //{
            //    HiringProcesss.SingleOrDefault(x => x.Value == c.HiringProcess.Id.ToString()).Selected = true;
            //}
            //if (!string.IsNullOrEmpty(c.HROwner))
            //{
            //    selHR.SingleOrDefault(x => x.Value == c.HROwner).Selected = true;
            //}   

            //ViewBag.interviewLst = selInterview;
            //ViewBag.HiringProcess = HiringProcesss;
            ////ViewBag.CandidateStatus = CandidateStatuses;
            ////ViewBag.onboardLst = selOnboard;
            //ViewBag.HROwner = selHR;

            return Redirect(Url.Action("Form", "PR", new { id = c.PRForm.Id })); //RedirectToAction("Form", c.PRForm);
        }

        public ActionResult editCandidate(FormCollection collection)
        {
            Candidate c = null;
            string canid = ((string[])collection.GetValues("hdCanId"))[0];
            string prid = ((string[])collection.GetValues("hdPRedit"))[0];
            string name = ((string[])collection.GetValues("txtCanName"))[0];
            string rank = ((string[])collection.GetValues("txtCanRnk"))[0];
            string fullRank = ((string[])collection.GetValues("txtCanFullRnk"))[0];
            string contactNo = ((string[])collection.GetValues("txtCanContactNo"))[0];
            string interviewId = ((string[])collection.GetValues("ddlInterview"))[0];
            //string offerId = ((string[])collection.GetValues("ddlOffer"))[0];
            string HiringProcessid = ((string[])collection.GetValues("ddlHiringProcess"))[0];
            //string CandidateStatus = ((string[])collection.GetValues("ddlCandidateStatus"))[0];
            string AcceptDate = ((string[])collection.GetValues("dpAcceptDate"))[0];
            string OfferDate = ((string[])collection.GetValues("dpCanOfferDate"))[0];
            string BUDate = ((string[])collection.GetValues("dpCanBUDate"))[0];
            string startDate = ((string[])collection.GetValues("dpCanStartDate"))[0];
            string rejectDate = ((string[])collection.GetValues("dpCanRejectedDate"))[0];
            //string endDate = ((string[])collection.GetValues("dpCanEndDate"))[0];
            //string onboardId = ((string[])collection.GetValues("ddlOnboard"))[0];
            string remark = ((string[])collection.GetValues("txtCanRemark"))[0];
            string owner = ((string[])collection.GetValues("ddlOwner"))[0];
            //string joinDate = ((string[])collection.GetValues("dpCanJoinDate"))[0];

            c = db.Candidates.Single(x => x.Id.ToString() == canid);
            if (c.PRForm == null)
            {
                c.PRForm = db.PRForms.Single(x => x.Id.ToString() == prid);
            }
            if (c.CandidateName != name)
            {
                c.CandidateName = name;
            }
            if (c.rank != rank)
            {
                c.rank = rank;
            }
            if (c.fullRank != fullRank)
            {
                c.fullRank = fullRank;
            }
            if (c.ContactNo != contactNo)
            {
                c.ContactNo = contactNo;
            }
            if (interviewId != "0")
            {
                if (c.InterviewStatus == null || (c.InterviewStatus != null && c.InterviewStatus.Id.ToString() != interviewId))
                {
                    c.InterviewStatus = db.Interviews.Single(x => x.Id.ToString() == interviewId);
                }
            }
            if (HiringProcessid != "0")
            {
                if (c.HiringProcess == null || (c.HiringProcess != null && c.HiringProcess.Id.ToString() != HiringProcessid))
                {
                    c.HiringProcess = db.HiringProcess.Single(x => x.Id.ToString() == HiringProcessid);
                }
            }
            //if (CandidateStatus != "0")
            //{
            //    if (c.CandidateStatus == null || (c.CandidateStatus != null && c.CandidateStatus.Id.ToString() != CandidateStatus))
            //    {
            //        c.CandidateStatus = db.CandidateStatus.Single(x => x.Id.ToString() == CandidateStatus);
            //    }
            //}
            if (!string.IsNullOrEmpty(AcceptDate))
            {
                try
                {
                    c.AcceptDate = DateTime.Parse(AcceptDate);
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                c.AcceptDate = null;
            }



            if (!string.IsNullOrEmpty(OfferDate))
            {
                try
                {
                    c.OfferingDate = DateTime.Parse(OfferDate);
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                c.OfferingDate = null;
            }



            if (!string.IsNullOrEmpty(BUDate))
            {
                try
                {
                    c.DateByBU = DateTime.Parse(BUDate);
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                c.DateByBU = null;
            }



            if (!string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(rejectDate))
            {
                try
                {
                    c.StartDate = DateTime.Parse(startDate);
                }
                catch (Exception e)
                {
                }
            }
            else if (string.IsNullOrEmpty(startDate))
            {
                c.StartDate = null;
            }

            if (!string.IsNullOrEmpty(rejectDate))
            {
                try
                {
                    c.RejectedDate = DateTime.Parse(rejectDate);
                    c.StartDate = null;
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                c.RejectedDate = null;
            }

            //if (!string.IsNullOrEmpty(endDate))
            //{
            //    try
            //    {
            //        c.EndDate = DateTime.Parse(endDate);
            //    }
            //    catch (Exception e)20
            //    {
            //    }
            //}
            //if (onboardId != "0")
            //{
            //    if (c.OnboardStatus == null || (c.OnboardStatus != null && c.OnboardStatus.Id.ToString() != onboardId))
            //    {
            //        c.OnboardStatus = db.Onboards.Single(x => x.Id.ToString() == onboardId);
            //    }                
            //}
            c.Remark = remark;
            c.HROwner = owner;
            //if (!string.IsNullOrEmpty(joinDate))
            //{
            //    try
            //    {
            //        c.JoinDate = DateTime.Parse(joinDate);
            //    }
            //    catch (Exception e)
            //    {
            //    }
            //}

            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.response = "Edit Candidate complete";

            return Redirect(Url.Action("Form", "PR", new { id = c.PRForm.Id })); //RedirectToAction("Form", c.PRForm);  
        }

        [HttpPost]
        public ActionResult Complete(FormCollection collection)
        {
            string prid = ((string[])collection.GetValues("hdCompletePRid"))[0];
            long lprid = long.Parse(prid);

            var pr = from p in db.PRForms.Include("RequestType").Include("Positions").Include("Candidates").Include("Status").Include("AttachFiles").Include("Candidates.InterviewStatus").Include("Candidates.HiringProcess")
                     where p.Id == lprid
                     select p;

            PRForm thisPR = pr.ToList<PRForm>()[0];

            List<Candidate> temp = thisPR.Candidates.Where(x => x.StartDate != null).ToList();
            int count = 0;
            foreach (Position p in thisPR.Positions)
            {
                count += p.Headcount;
            }

            if (temp.ToList().Count >= count)
            {
                thisPR.Status = db.Status.Single(x => x.StatusDesc == "Complete");
                db.Entry(thisPR).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.response = "This PR has been completed. Thank you very much for your effort.";
            }
            else
            {
                ViewBag.response = "Candidate joining does not meet headcount requested";
            }
            return Form(prid); //Redirect(Url.Action("Form", "PR", new { id = prid })); ;
        }

        public ActionResult printPR(string id)
        {
            DataTable dt;

            ViewBag.currFY = genFY();

            //Request
            ViewBag.RequestID = id;
            //List<SelectListItem> divLst = getDivisionByBU();
            //if (User.roles.Contains("Administrator") || User.roles.Contains("Recruiter"))
            //{
            //    divLst.Add(new SelectListItem { Text = "Audit (Mass)", Value = "Audit (Mass)" });
            //    divLst.Add(new SelectListItem { Text = "T&L (Mass)", Value = "T&L (Mass)" });
            //}
            //else if (User.Pool == "Audit")
            //{
            //    divLst.Add(new SelectListItem { Text = "Audit (Mass)", Value = "Audit (Mass)" });
            //}
            //else if (User.Pool == "Tax")
            //{
            //    divLst.Add(new SelectListItem { Text = "T&L (Mass)", Value = "T&L (Mass)" });
            //}
            //ViewBag.DivLst = divLst;

            //ViewBag.ReqCompany = "";
            //ViewBag.ReqDiv = "";
            //ViewBag.OperatePlanHC = 0; //getOperatingPlanHC(User.UnitGroup);
            //ViewBag.HRISHC = 0; //getCurrentHC(User.UnitGroup);
            //ViewBag.ReqTypeLst = getRequestType();

            //Request position detail
            //ViewBag.RankLst = getRankList(User.Company);
            //ViewBag.TypeReq = getEmpType();
            //ViewBag.HClst = getHCNumber();

            PRForm thisPR = new PRForm();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    long prid = long.Parse(id);

                    var pr = from p in db.PRForms.Include("RequestType").Include("Positions").Include("Candidates").Include("Status").Include("AttachFiles").Include("Candidates.InterviewStatus").Include("Candidates.HiringProcess").Include("Positions.EmpType")
                             where p.Id == prid
                             select p;

                    thisPR = pr.ToList<PRForm>()[0];
                    ViewBag.refNo = thisPR.RefNo;
                    //ViewBag.ReqCompany = thisPR.CompanyCode;
                    //ViewBag.ReqDiv = thisPR.Division;
                    ViewBag.HRISHC = getCurrentHC(thisPR.Division);
                    ViewBag.OperatePlanHC = getOperatingPlanHC(thisPR.Division);

                    //Approval
                    dt = wsHRIS.getEmployeeInfoByEmpNo(thisPR.RequestBy);
                    ViewBag.RequestBy = dt.Rows[0]["EmpFullName"].ToString();
                    ViewBag.RequestDate = thisPR.RequestDate.ToString("dd-MMM-yyyy");
                    Session["RequestBy"] = thisPR.RequestBy;
                    dt = wsHRIS.getEmployeeInfoByEmpNo(thisPR.HeadApproveBy);
                    ViewBag.HeadReqBy = dt.Rows[0]["EmpFullName"].ToString();
                    if (thisPR.HeadApproveDate != null)
                    {
                        ViewBag.HeadReqDate = ((DateTime)thisPR.HeadApproveDate).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        ViewBag.HeadReqDate = "";
                    }
                    Session["HeadReqBy"] = thisPR.HeadApproveBy;

                    if (string.IsNullOrEmpty(thisPR.BUApproveBy))
                    {
                        ViewBag.AprovedBy = "";
                        ViewBag.AprovedDate = "";
                        Session["AprovedBy"] = "";
                    }
                    else
                    {
                        dt = wsHRIS.getEmployeeInfoByEmpNo(thisPR.BUApproveBy);
                        ViewBag.AprovedBy = dt.Rows[0]["EmpFullName"].ToString();
                        if (thisPR.BUApproveDate != null)
                        {
                            ViewBag.AprovedDate = ((DateTime)thisPR.BUApproveDate).ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            ViewBag.AprovedDate = "";
                        }
                        Session["AprovedBy"] = thisPR.BUApproveBy;
                    }


                    if (string.IsNullOrEmpty(thisPR.CeoApproveBy))
                    {
                        ViewBag.CEOAprovedBy = "";
                        ViewBag.CEOAprovedDate = "";
                        Session["CEOAprovedBy"] = "";
                    }
                    else
                    {
                        dt = wsHRIS.getEmployeeInfoByEmpNo(thisPR.CeoApproveBy);
                        ViewBag.CEOAprovedBy = dt.Rows[0]["EmpFullName"].ToString();
                        if (thisPR.CeoApproveDate != null)
                        {
                            ViewBag.CEOAprovedDate = ((DateTime)thisPR.CeoApproveDate).ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            ViewBag.CEOAprovedDate = "";
                        }
                        Session["CEOAprovedBy"] = thisPR.CeoApproveBy;
                    }

                    if (CGlobal.UserInfo.roles.Contains("Administrator") || CGlobal.UserInfo.roles.Contains("Recruiter"))
                    {
                        ViewBag.IsAdmin = true;
                    }
                    else
                    {
                        ViewBag.IsAdmin = false;
                    }
                }
                catch (Exception e)
                {
                    ViewBag.RequestID = "";
                }
            }
            else
            {
                ViewBag.RequestID = "";

                ////Approval default
                //ViewBag.RequestBy = User.FullName;
                //ViewBag.RequestDate = "";
                //Session["RequestBy"] = User.EmployeeNo;
                //ViewBag.IsAdmin = false;

                //List<SpecialRole> divHead = db.SpecialRoles.Where(x => x.Division == User.UnitGroup).ToList<SpecialRole>();
                //if (divHead.Count() > 0)
                //{
                //    dt = wsHRIS.getEmployeeInfoByEmpNo(divHead[0].EmpNo);
                //    ViewBag.HeadReqBy = dt.Rows[0]["EmpFullName"].ToString();
                //    ViewBag.HeadReqDate = "";
                //    Session["HeadReqBy"] = dt.Rows[0]["EmpNo"].ToString();
                //}

                //List<BUPlanner> divBU = db.BUPlanners.Where(x => x.CostCenter == User.Pool).ToList<BUPlanner>();
                //if (divBU.Count() > 0)
                //{
                //    dt = wsHRIS.getEmployeeInfoByEmpNo(divBU[0].Planner);
                //    ViewBag.AprovedBy = dt.Rows[0]["EmpFullName"].ToString();
                //    ViewBag.AprovedDate = "";
                //    Session["AprovedBy"] = dt.Rows[0]["EmpNo"].ToString();
                //}

                ////For MGR up only
                //ViewBag.CEOAprovedBy = "Winid Silamongkol";
                //ViewBag.CEOAprovedDate = "";
                //Session["CEOAprovedBy"] = "00001620";

            }
            return View("printPR", thisPR);
        }

        public ActionResult Close()
        {
            return View();
        }

        private void clearSession()
        {
            Session["selDivision"] = null;
            Session["RequestBy"] = null;
            Session["HeadReqBy"] = null;
            Session["AprovedBy"] = null;
            Session["CEOAprovedBy"] = null;
            Session["newAttachFile"] = new List<AttachFile>();
        }

        private string generateRefNo(string comp)
        {
            bool FYisThisYear = false;
            if (DateTime.Now.Month < 10)
            {
                FYisThisYear = true;
            }
            List<PRForm> pr;
            if (FYisThisYear)
            {
                pr = db.PRForms.Where(x => x.CompanyCode == comp && ((x.RequestDate.Year == DateTime.Now.Year - 1 && x.RequestDate.Month >= 10) || (x.RequestDate.Year == DateTime.Now.Year && x.RequestDate.Month < 10))).ToList();
            }
            else
            {
                pr = db.PRForms.Where(x => x.CompanyCode == comp && (x.RequestDate.Year == DateTime.Now.Year && x.RequestDate.Month >= 10)).ToList();
            }

            int yrCompNo = pr.Count + 1;
            int year = DateTime.Now.Year - 2000;
            switch (comp)
            {
                case "Audit": return year + "A" + yrCompNo.ToString().PadLeft(4, '0');
                case "Tax": return year + "T" + yrCompNo.ToString().PadLeft(4, '0');
                case "Advisory": return year + "B" + yrCompNo.ToString().PadLeft(4, '0');
                case "Shared Services": return year + "S" + yrCompNo.ToString().PadLeft(4, '0');
                default: return "";
            }
        }

        public Position addNewPosition(string title, string rank, string period, string headcount, string type, string specify)
        {
            //List<Position> positions;
            //if (Session["positionList"] != null)
            //{
            //    positions = (List<Position>)Session["positionList"];
            //}
            //else
            //{
            //    positions = new List<Position>();
            //}

            //string rows = "";
            //rows += "<table class='table table-bordered table-striped'>";
            //rows += "	<thead>";
            //rows += "		<tr style='background-color:lightblue;'>";
            //rows += "			<th style='text-align:center; vertical-align:middle;'>Title</th>";
            //rows += "			<th style='text-align:center; vertical-align:middle;'>Rank</th>";
            //rows += "			<th style='text-align:center; vertical-align:middle;'>Target Period</th>";
            //rows += "			<th style='text-align:center; vertical-align:middle;'>Headcount</th>";
            //rows += "			<th style='text-align:center; vertical-align:middle;'>Type of request</th>";
            //rows += "			<th style='text-align:center; vertical-align:middle;'>Name of person<br />being replace</th>";
            //rows += "		</tr>";
            //rows += "	</thead>";
            //rows += "	<tbody>";
            //if (positions.Count() > 0)
            //{
            //    foreach (Position pos in positions)
            //    {
            //        rows += "<tr>";
            //        rows += "    <td style='text-align:center;'>" + pos.PositionTitle + "</td>";
            //        rows += "    <td style='text-align:center;'>" + pos.Rank + "</td>";
            //        rows += "    <td style='text-align:center;'>" + pos.TargetPeriod.ToString("dd-MMM-yyyy") + "</td>";
            //        rows += "    <td style='text-align:center;'>" + pos.Headcount + "</td>";
            //        rows += "    <td style='text-align:center;'>" + pos.EmpType.EmployeeTypeDesc + "</td>";
            //        rows += "    <td style='text-align:center;'>" + pos.Specify + "</td>";
            //        rows += "</tr>";
            //    }
            //}
            Position newPos = null;
            try
            {
                if (type == "Trainee")
                {
                    newPos = new Position { PositionTitle = title, Rank = rank, TargetPeriod = DateTime.Parse(period), TargetPeriodTo = DateTime.Parse(specify), Headcount = int.Parse(headcount), EmpType = db.EmployeeTypes.Single(x => x.EmployeeTypeDesc == type) };
                }
                else if (type == "Temporary Staff")
                {
                    newPos = new Position { PositionTitle = title, Rank = rank, TargetPeriod = DateTime.Parse(period), Headcount = int.Parse(headcount), EmpType = db.EmployeeTypes.Single(x => x.EmployeeTypeDesc == type), Specify = specify };
                }
                else
                {
                    newPos = new Position { PositionTitle = title, Rank = rank, TargetPeriod = DateTime.Parse(period), Headcount = int.Parse(headcount), EmpType = db.EmployeeTypes.Single(x => x.Id.ToString() == type), Specify = specify };
                }

                //if (newPos != null)
                //{
                //    positions.Add(newPos);
                //    rows += "<tr>";
                //    rows += "    <td style='text-align:center;'>" + newPos.PositionTitle + "</td>";
                //    rows += "    <td style='text-align:center;'>" + newPos.Rank + "</td>";
                //    if (rank == "trainee")
                //    {
                //        rows += "    <td style='text-align:center;'>" + newPos.TargetPeriod.ToString("dd-MMM-yyyy") + " to " + newPos.TargetPeriodTo.ToString("dd-MMM-yyyy") + "</td>";
                //    }
                //    else
                //    {
                //        rows += "    <td style='text-align:center;'>" + newPos.TargetPeriod.ToString("dd-MMM-yyyy") + "</td>";
                //    }

                //    rows += "    <td style='text-align:center;'>" + newPos.Headcount + "</td>";
                //    rows += "    <td style='text-align:center;'>" + newPos.EmpType.EmployeeTypeDesc + "</td>";
                //    rows += "    <td style='text-align:center;'>" + newPos.Specify + "</td>";
                //    rows += "</tr>";
                //}

                //if (positions.Count() < 7)
                //{
                //    for (int i = 0; i < 7 - positions.Count(); i++)
                //    {
                //        rows += "<tr>";
                //        rows += "    <td>&nbsp;</td>";
                //        rows += "    <td>&nbsp;</td>";
                //        rows += "    <td>&nbsp;</td>";
                //        rows += "    <td>&nbsp;</td>";
                //        rows += "    <td>&nbsp;</td>";
                //        rows += "    <td>&nbsp;</td>";
                //        rows += "</tr>";
                //    }
                //}
            }
            catch (Exception ex)
            {

            }

            //rows += "</tbody>";
            //rows += "</table>";

            //Session["positionList"] = positions;
            return newPos;//Json(new { div = rows }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult addNewAttach()
        {
            List<AttachFile> attachLst;
            string alert = "";
            string tb = "";

            try
            {
                if (Session["newAttachFile"] != null)
                {
                    attachLst = (List<AttachFile>)Session["newAttachFile"];
                }
                else
                {
                    attachLst = new List<AttachFile>();
                }
                string fileName = Request.Headers["X-File-Name"];
                string fileType = Request.Headers["X-File-Type"];
                int fileSize = Convert.ToInt32(Request.Headers["X-File-Size"]);
                //File's content is available in Request.InputStream property
                System.IO.Stream fileContent = Request.InputStream;
                //Creating a FileStream to save file's content
                System.IO.FileStream fileStream = System.IO.File.Create(Server.MapPath("~/Attachment/") + fileName);
                fileContent.Seek(0, System.IO.SeekOrigin.Begin);
                //Copying file's content to FileStream
                fileContent.CopyTo(fileStream);
                fileStream.Dispose();
                attachLst.Add(new AttachFile { FileName = fileName, FilePath = Url.Content("~/Attachment/") + fileName });

                Session["newAttachFile"] = attachLst;
                string currentURL = Url.Action("addNewAttach", "PR");
                currentURL.Replace("addNewAttach", "Attachment/");

                tb += "<table class='table table-bordered table-striped table-condensed'>";
                tb += "	<thead> ";
                tb += "		<tr style='background-color:lightblue;'> ";
                tb += "			<th style='text-align:center; vertical-align:middle;width:80px;'>No.</th> ";
                tb += "			<th style='text-align:center; vertical-align:middle;'>File Name</th> ";
                //tb += "			<th style='text-align:center; vertical-align:middle;width:150px;'>Download</th> ";
                tb += "		</tr> ";
                tb += "	</thead> ";
                tb += "	<tbody> ";
                int count = 1;
                foreach (AttachFile af in attachLst)
                {
                    tb += "			<tr> ";
                    tb += "				<td style='text-align:center; vertical-align:middle;'>" + count + "</td> ";
                    tb += "				<td><a target='_blank' href='" + af.FilePath + "'>" + af.FileName + "</td> ";
                    //tb += "				<td style='text-align:center; vertical-align:middle;'><a href='" + af.FilePath + "'>Download</a></td> ";
                    tb += "			</tr> ";
                    count++;
                }
                tb += "	</tbody> ";
                tb += "</table> ";


                alert = "File uploaded successfully";
            }
            catch (Exception e)
            {
                alert = e.ToString();//"Fail to upload attact file";
            }

            return Json(new { alert = alert, tb = tb });
        }

        [HttpGet]
        public ActionResult getHeadcount(string division)
        {
            Session["selDivision"] = division;
            long planHC = getOperatingPlanHC(division);
            long currHC = getCurrentHC(division);
            string lob = string.Empty;

            List<Division> divs = db.Divisions.Where(x => x.DivisionName == division).ToList();
            if (divs.Count() > 0)
            {
                lob = divs[0].BU;
            }
            else if (division.Contains("Audit"))
            {
                lob = "Audit";
            }
            else if (division.Contains("ADV"))
            {
                lob = "Advisory";
            }
            else if (division.Contains("T&L"))
            {
                lob = "Tax";
            }
            else if (division.Contains("SSVC"))
            {
                lob = "Shared Services";
            }

            DataTable dt;
            DataView dv;
            string temp;
            string headApprove;
            string BUApprove;
            if (!division.Contains("Mass"))
            {
                //Group Head Approver
                dt = wsHRIS.getAllUnitGroupHead("", "", "");
                dv = dt.DefaultView;
                dv.RowFilter = "UnitGroupName = '" + division + "'";
                dt = dv.ToTable();
                if (dt.Rows.Count > 0)
                {
                    temp = dt.Rows[0]["HeadNo"].ToString();
                }
                else
                {
                    List<SpecialRole> lstSpe = db.SpecialRoles.Where(x => x.Division == division).ToList<SpecialRole>();
                    if (lstSpe.Count() > 0)
                    {
                        //temp = lstSpe[0].EmpNo;
                        //steve
                        temp = "00012358";
                    }
                    else
                    {
                        temp = "00012358";
                    }
                }
                dt = wsHRIS.getEmployeeInfoByEmpNo("00012358");
                ViewBag.HeadReqBy = dt.Rows[0]["EmpFullName"].ToString();
                headApprove = dt.Rows[0]["EmpFullName"].ToString();
                Session["HeadReqBy"] = dt.Rows[0]["EmpNo"].ToString();

                //HOP(BU) Approver
                dt = wsHRIS.getAllPoolHead("");
                dv = dt.DefaultView;
                dv.RowFilter = "PoolName = '" + lob + "'";
                dt = dv.ToTable();
                if (dt.Rows.Count > 0)
                {
                    temp = dt.Rows[0]["HeadNo"].ToString();
                }
                else
                {
                    List<BUPlanner> lstBU = db.BUPlanners.Where(x => x.CostCenter == CGlobal.UserInfo.Pool).ToList<BUPlanner>();
                    if (lstBU.Count() > 0)
                    {
                        //temp = lstBU[0].Planner;
                        //steve
                        temp = "00012358";
                    }
                    else
                    {
                        temp = "00012358";
                    }
                }
                dt = wsHRIS.getEmployeeInfoByEmpNo("00012358");
                ViewBag.AprovedBy = dt.Rows[0]["EmpFullName"].ToString();
                BUApprove = dt.Rows[0]["EmpFullName"].ToString();
                Session["AprovedBy"] = dt.Rows[0]["EmpNo"].ToString();
            }
            else if (division == "Audit (Mass)")
            {
                //00001445
                dt = wsHRIS.getEmployeeInfoByEmpNo("00012358");
                ViewBag.HeadReqBy = dt.Rows[0]["EmpFullName"].ToString();
                headApprove = dt.Rows[0]["EmpFullName"].ToString();
                Session["HeadReqBy"] = dt.Rows[0]["EmpNo"].ToString();

                BUApprove = dt.Rows[0]["EmpFullName"].ToString(); ;
                ViewBag.AprovedBy = dt.Rows[0]["EmpFullName"].ToString();
                Session["AprovedBy"] = dt.Rows[0]["EmpNo"].ToString();
            }
            else if (division == "T&L (Mass)")
            {
                //00002021
                dt = wsHRIS.getEmployeeInfoByEmpNo("00012358");
                ViewBag.HeadReqBy = dt.Rows[0]["EmpFullName"].ToString();
                headApprove = dt.Rows[0]["EmpFullName"].ToString();
                Session["HeadReqBy"] = dt.Rows[0]["EmpNo"].ToString();
                DataTable dt2;
                //00001612
                dt2 = wsHRIS.getEmployeeInfoByEmpNo("00012358");
                BUApprove = dt2.Rows[0]["EmpFullName"].ToString();
                ViewBag.AprovedBy = dt.Rows[0]["EmpFullName"].ToString();
                Session["AprovedBy"] = dt2.Rows[0]["EmpNo"].ToString();
            }
            else
            {

                //00012358
                dt = wsHRIS.getEmployeeInfoByEmpNo("00012358");
                ViewBag.HeadReqBy = dt.Rows[0]["EmpFullName"].ToString();
                headApprove = dt.Rows[0]["EmpFullName"].ToString();
                Session["HeadReqBy"] = dt.Rows[0]["EmpNo"].ToString();

                BUApprove = "";
                ViewBag.AprovedBy = "";
                Session["AprovedBy"] = "";

                Session["CEOAprovedBy"] = "";
            }


            return Json(new { plan = planHC, curr = currHC, head = headApprove, bu = BUApprove, lob = lob }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PRApprove(string prid, string approver, string status)
        {
            string response = "";
            string to = "";
            string from = CGlobal.UserInfo.EMail;
            string subject = "";
            string message = "";
            try
            {
                if (approver == CGlobal.UserInfo.EmployeeNo)
                {
                    Status stat = db.Status.Single(x => x.StatusDesc == status);
                    stat = db.Status.Single(x => x.Sort == stat.Sort + 1);

                    long lprid = long.Parse(prid);
                    PRForm pr = db.PRForms.Include("Status").Include("RequestType").Single(x => x.Id == lprid);
                    if (stat.StatusDesc == "CEO Approve" && string.IsNullOrEmpty(pr.CeoApproveBy))
                    {
                        stat = db.Status.Single(x => x.Sort == stat.Sort + 1);
                    }
                    pr.Status = stat;

                    if (status == "Group Head Approve")
                    {
                        pr.HeadApproveDate = DateTime.Now;
                        if (string.IsNullOrEmpty(pr.BUApproveBy))
                        {
                            stat = db.Status.Single(x => x.StatusDesc == "Recruiting");
                            to = wsHRIS.getEmployeeEmail("k", pr.BUApproveBy);
                        }
                        else if (CGlobal.UserInfo.EmployeeNo == pr.BUApproveBy && pr.BUApproveBy != pr.CeoApproveBy && !string.IsNullOrEmpty(pr.CeoApproveBy))
                        {
                            stat = db.Status.Single(x => x.Sort == stat.Sort + 1);
                            pr.BUApproveDate = DateTime.Now;
                            to = wsHRIS.getEmployeeEmail("k", pr.CeoApproveBy);
                        }
                        else if (CGlobal.UserInfo.EmployeeNo == pr.BUApproveBy && pr.BUApproveBy != pr.CeoApproveBy && string.IsNullOrEmpty(pr.CeoApproveBy))
                        {
                            stat = db.Status.Single(x => x.StatusDesc == "Recruiting");
                            pr.BUApproveDate = DateTime.Now;
                        }
                        else if (CGlobal.UserInfo.EmployeeNo == pr.BUApproveBy && pr.BUApproveBy == pr.CeoApproveBy && !string.IsNullOrEmpty(pr.CeoApproveBy))
                        {
                            stat = db.Status.Single(x => x.StatusDesc == "Recruiting");
                            pr.BUApproveDate = DateTime.Now;
                            pr.CeoApproveDate = DateTime.Now;
                        }
                        pr.Status = stat;

                        subject = "HCM: Requesting for Headcount";
                        message = genApproveMailBody(pr);
                    }
                    else if (status == "Functional HOP Approve")
                    {
                        pr.BUApproveDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(pr.CeoApproveBy) && pr.CeoApproveBy != pr.BUApproveBy)
                        {
                            to = wsHRIS.getEmployeeEmail("k", pr.CeoApproveBy);
                        }
                        else if (pr.CeoApproveBy == pr.BUApproveBy)
                        {
                            pr.CeoApproveDate = DateTime.Now;
                            stat = db.Status.Single(x => x.Sort == 5);
                            to = "";
                            pr.Status = stat;
                        }
                        subject = "HCM: Requesting for Headcount";
                        message = genApproveMailBody(pr);
                    }
                    else if (status == "CEO Approve")
                    {
                        pr.CeoApproveDate = DateTime.Now;
                    }


                    db.Entry(pr).State = EntityState.Modified;
                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(to))
                    {
                        if (sendMail(from, to, "", subject, message))
                        {
                            response = "Complete";
                        }
                        else
                        {
                            response = "Send Mail fail";
                        }
                    }
                    else
                    {
                        response = "Complete";
                    }



                }
                else
                {
                    response = "Fail to approve";
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            return Json(new { res = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PRReject(string prid, string approver, string reason)
        {
            string response = "";
            try
            {
                if (approver == CGlobal.UserInfo.EmployeeNo)
                {
                    Status stat = db.Status.Single(x => x.StatusDesc == "Rejected");

                    long lprid = long.Parse(prid);
                    PRForm pr = db.PRForms.Include("Status").Include("RequestType").Single(x => x.Id == lprid);
                    pr.Status = stat;
                    pr.rejectReason = reason;
                    db.Entry(pr).State = EntityState.Modified;
                    db.SaveChanges();

                    sendMailCancelReject(pr, "reject");

                    response = "Complete";
                }
                else
                {
                    response = "Fail to approve";
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return Json(new { res = response }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PRCancel(string prid, string approver, string reason)
        {
            string response = "";
            try
            {
                if (approver == CGlobal.UserInfo.EmployeeNo)
                {
                    Status stat = db.Status.Single(x => x.StatusDesc == "Cancel");

                    long lprid = long.Parse(prid);
                    PRForm pr = db.PRForms.Include("Status").Include("RequestType").Single(x => x.Id == lprid);
                    pr.Status = stat;
                    pr.rejectReason = reason;
                    db.Entry(pr).State = EntityState.Modified;
                    db.SaveChanges();

                    sendMailCancelReject(pr, "cancel");

                    response = "Complete";
                }
                else
                {
                    response = "Fail to Cancel";
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            return Json(new { res = response }, JsonRequestBehavior.AllowGet);
        }

        private bool sendMailCancelReject(PRForm pr, string CancelReject)
        {
            string from = CGlobal.UserInfo.EMail;
            string subject;

            string temp = "Dear K./Requester/<br> ";
            temp += "<br> ";
            temp += "K./User/ have " + CancelReject + " the Personal Request form.<br> ";
            if (CancelReject == "cancel")
            {
                subject = "HCM: Cancel for Headcount";
                temp += "Cancel reason : " + pr.rejectReason + "<br> ";
            }
            else
            {
                subject = "HCM: Reject for Headcount";
                temp += "Reject reason : " + pr.rejectReason + "<br> ";
            }
            temp += "Please click <a target='_blank' href='/link/'>here</a> to view the request.<br> ";
            temp += "<br> ";
            temp += "Best Regards,<br> ";

            DataTable dt = wsHRIS.getEmployeeInfoByEmpNo(pr.RequestBy);
            DataTable dt2 = wsHRIS.getEmployeeInfoByEmpNo(pr.HeadApproveBy);
            DataTable dt3 = wsHRIS.getEmployeeInfoByEmpNo(pr.BUApproveBy);
            string to = dt.Rows[0]["Email"].ToString() + ";";
            string cc = dt2.Rows[0]["Email"].ToString() + ";" + dt3.Rows[0]["Email"].ToString() + ";";
            if (!string.IsNullOrEmpty(pr.CeoApproveBy))
            {
                DataTable dt4 = wsHRIS.getEmployeeInfoByEmpNo(pr.CeoApproveBy);
                cc += dt4.Rows[0]["Email"].ToString() + ";";
            }

            string url = Url.Action("Form", "PR", null, Request.Url.Scheme) + "/" + pr.Id;

            temp = temp.Replace("/User/", CGlobal.UserInfo.FullName);
            temp = temp.Replace("/Requester/", dt2.Rows[0]["EmpFullName"].ToString());
            temp = temp.Replace("/link/", url);

            return sendMail(from, to, cc, subject, temp);
        }

        private bool sendMail(string from, string to, string cc, string subject, string message)
        {
            try
            {
                //BCC
                string bcc = "";
                string mailTmp;
                List<UserRole> roleLst = db.UsersRoles.Where(x => x.RoleId == 4).ToList();
                foreach (UserRole ur in roleLst)
                {
                    mailTmp = wsHRIS.getEmployeeEmail("u", ur.Username);
                    if (mailTmp != "-1")
                    {
                        bcc += mailTmp + ";";
                    }
                }

                //For Testing
                //message = "Mail send to : " + to + "<br>cc : " + cc + "<br>bcc : " + bcc + "<br><br>" + message;
                //to = User.EMail;
                //cc = "";
                //bcc = "";

                //Send mail                
                if (!string.IsNullOrEmpty(message))
                {
                    //  return wsMail.SendMail(from, "teeraphon@kpmg.co.th,wachiraporn@kpmg.co.th", "teeraphon@kpmg.co.th,wachiraporn@kpmg.co.th,darunee@kpmg.co.th", "teeraphon@kpmg.co.th", subject, message);
                    //return wsMail.SendMail(from, "teeraphon@kpmg.co.th", "teeraphon@kpmg.co.th", "teeraphon@kpmg.co.th", subject, message);
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string genApproveMailBody(PRForm pr)
        {
            string temp = "";
            DataTable dt, dt2;
            string url = Url.Action("Form", "PR", null, Request.Url.Scheme) + "/" + pr.Id;
            if (pr != null)
            {
                temp = "Dear K./Approver/<br> ";
                temp += "<br> ";
                temp += "K./Requester/ have submitted the Personnel Request form and need your approval.<br> ";
                temp += "Please click <a target='_blank' href='/link/'>here</a> to view the request.<br> ";
                temp += "<br> ";
                temp += "Best Regards,<br> ";
                temp += "Recruitment Team<br> ";
                temp += "Human Capital Management Application (HCM)<br> ";

                if (pr.Status.StatusDesc == "Group Head Approve")
                {
                    dt = wsHRIS.getEmployeeInfoByEmpNo(pr.HeadApproveBy);
                    dt2 = wsHRIS.getEmployeeInfoByEmpNo(pr.RequestBy);
                    temp = temp.Replace("/Approver/", dt.Rows[0]["EmpFullName"].ToString());
                    temp = temp.Replace("/Requester/", dt2.Rows[0]["EmpFullName"].ToString());
                    temp = temp.Replace("/link/", url);

                    return temp;
                }
                else if (pr.Status.StatusDesc == "Functional HOP Approve")
                {
                    dt = wsHRIS.getEmployeeInfoByEmpNo(pr.BUApproveBy);
                    dt2 = wsHRIS.getEmployeeInfoByEmpNo(pr.RequestBy);
                    temp = temp.Replace("/Approver/", dt.Rows[0]["EmpFullName"].ToString());
                    temp = temp.Replace("/Requester/", dt2.Rows[0]["EmpFullName"].ToString());
                    temp = temp.Replace("/link/", url);

                    return temp;
                }
                else if (pr.Status.StatusDesc == "CEO Approve")
                {
                    dt = wsHRIS.getEmployeeInfoByEmpNo(pr.CeoApproveBy);
                    dt2 = wsHRIS.getEmployeeInfoByEmpNo(pr.RequestBy);
                    temp = temp.Replace("/Approver/", dt.Rows[0]["EmpFullName"].ToString());
                    temp = temp.Replace("/Requester/", dt2.Rows[0]["EmpFullName"].ToString());
                    temp = temp.Replace("/link/", url);

                    return temp;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }

        }

        private long getOperatingPlanHC(string division)
        {
            var fy = genFY();

            var plan = from p in db.FYPlans
                       where p.FY == fy && p.LOB == division
                       select p;

            List<FYPlan> plans = plan.ToList<FYPlan>();
            if (plans.Count() > 0 && (plans[0].NewHire != 0 || plans[0].Replace != 0))
            {
                //return plans[0].TotalOperatingPlan();
                return plans[0].TotalHC();
            }
            else
            {
                return 0;
            }
        }

        private long getCurrentHC(string division)
        {
            DataTable empList = wsHRIS.getEmployeeInfo("k", "", "3,1", "", "", "", "");

            var emp = empList.AsEnumerable().Select(e => new SelectListItem { Text = e["EmpNo"].ToString(), Value = e["UnitGroup"].ToString() });

            List<SelectListItem> rnkLst = emp.ToList<SelectListItem>().Where(x => x.Value == division).ToList();

            return rnkLst.Count();
        }

        private List<SelectListItem> getRankList(string bu)
        {
            DataTable empList;
            //if (User.IsInRole("CEO") || User.IsInRole("Administrator") || User.IsInRole("Recruiter"))
            //{
            //    empList = wsHRIS.getEmployeeInfo("k", "", "3", "", "", "", "");
            //}
            //else
            //{
            //    empList = wsHRIS.getEmployeeInfo("k", "", "3", "", "", bu, "");
            //}
            empList = wsHRIS.getEmployeeInfo("k", "", "3,1", "", "", "", "");
            //   var rnxxxk = empList.AsEnumerable().Where(w => string.IsNullOrEmpty(w.Field<string>("Rank")) || string.IsNullOrEmpty(w.Field<string>("RankCode"))).ToList();
            //     DataTable xxx = SystemFunction.LinqToDataTable(rnxxxk);

            var rnk = empList.AsEnumerable().OrderBy(x => x.Field<string>("Rank") + "").Select(e => new SelectListItem { Text = e.Field<string>("Rank") + "", Value = e.Field<string>("RankCode") + "" }).Distinct(new SelectListItemComparer());

            List<SelectListItem> rnkLst = rnk.ToList<SelectListItem>();
            //Rename Director
            SelectListItem rnkTmp = rnkLst.Single(x => x.Text.Contains("Executive Director"));
            rnkTmp.Text = "Director";

            rnkLst.Insert(0, new SelectListItem { Text = "== Select Rank ==", Value = "0", Selected = true });

            return rnkLst;
        }

        private List<SelectListItem> getDivisionByBU()
        {
            var divMst = from d in db.Divisions
                         orderby d.Sort
                         select new SelectListItem { Text = d.DivisionName, Value = d.DivisionName };

            List<string> divPermission = getDivisionPermission();
            if (!divPermission.Contains(CGlobal.UserInfo.Division))
            {
                divPermission.Add(CGlobal.UserInfo.Division);
            }
            List<SelectListItem> div = divMst.Where(x => divPermission.Contains(x.Value)).ToList();


            //var isCeo = from x in db.SpecialRoles
            //           where x.EmpNo == User.EmployeeNo 
            //           select x;

            //var isBu = from x in db.BUPlanners
            //            where x.Planner == User.EmployeeNo
            //            select x;

            //if (isCeo.ToList().Count > 0)
            //{
            //    DataTable empList = wsHRIS.getEmployeeInfo("k", "", "3", "", "", "", "");
            //    div = empList.AsEnumerable().Select(e => new SelectListItem { Text = e["UnitGroup"].ToString(), Value = e["UnitGroup"].ToString() }).Distinct(new SelectListItemComparer()).OrderBy(u => u.Text).ThenBy(u => u.Value).ToList();
            //}
            //else if (isBu.ToList().Count > 0)
            //{
            //    div = new List<SelectListItem>();
            //    foreach (BUPlanner planner in isBu)
            //    {
            //        DataTable empList = wsHRIS.getEmployeeInfo("k", "", "3", planner.CostCenter, "", "", "");
            //        div = empList.AsEnumerable().Select(e => new SelectListItem { Text = e["UnitGroup"].ToString(), Value = e["UnitGroup"].ToString() }).Distinct(new SelectListItemComparer()).OrderBy(u => u.Text).ThenBy(u => u.Value).ToList();
            //    }

            //}
            //else
            //{
            //    var temp = from x in db.SpecialRoles
            //               where x.EmpNo == User.EmployeeNo
            //               select x;

            //    div = temp.Select(e => new SelectListItem { Text = e.Division, Value = e.Division }).Distinct().OrderBy(u => u.Text).ThenBy(u => u.Value).ToList();
            //}

            div.Insert(0, new SelectListItem { Text = "== Select Division ==", Value = "0", Selected = true });
            return div;
        }

        private List<SelectListItem> getRequestType()
        {
            var rt = from r in db.RequestTypes
                     orderby r.Sort
                     select new SelectListItem { Text = r.RequestTypeDesc, Value = r.RequestTypeDesc };

            return rt.ToList<SelectListItem>();
        }

        private List<SelectListItem> getEmpType()
        {
            var empType = from et in db.EmployeeTypes
                          where et.Sort != 0
                          orderby et.Sort
                          select new SelectListItem { Text = et.EmployeeTypeDesc, Value = et.Id.ToString() };

            List<SelectListItem> type = empType.ToList();
            type.Insert(0, new SelectListItem { Text = "== Select Type ==", Value = "0", Selected = true });

            return type;
        }

        private List<SelectListItem> getHCNumber()
        {
            List<SelectListItem> hc = new List<SelectListItem>();
            for (int i = 1; i <= 10; i++)
            {
                hc.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return hc;
        }

        private string genFY()
        {
            DateTime date = DateTime.Today;
            string fyYear;
            if (date.Month >= 10)
            {
                fyYear = date.AddYears(1).ToString("yyyy");
                return date.AddYears(1).ToString("yy");
            }
            else
            {
                fyYear = date.ToString("yyyy");
                return date.ToString("yy");
            }

            //DateTime date = DateTime.Today;
            //if (date.Month >= 10)
            //{
            //    return date.AddYears(1).ToString("yy");
            //}
            //else
            //{
            //    return date.ToString("yy");
            //}
        }

        private List<string> getDivisionPermission()
        {
            List<string> division = new List<string>();

            if (User.IsInRole("CEO") || User.IsInRole("Administrator") || User.IsInRole("Recruiter"))
            {
                var divs = from d in db.Divisions
                           select d.DivisionName;
                division = divs.ToList<string>();
            }

            //Pool Head (HOP)
            DataTable dt = wsHRIS.getAllPoolHead("");
            DataTable dt2;
            DataView dv = dt.DefaultView;
            dv.RowFilter = "HeadNo = " + CGlobal.UserInfo.EmployeeNo;
            dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dt2 = wsHRIS.getAllUnitGroup(dr["PoolID"].ToString());
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        division.Add(dr2["Name"].ToString());
                    }
                }
            }

            //Unit Group head
            dt = wsHRIS.getAllUnitGroupHead("", "", "");
            dv = dt.DefaultView;
            dv.RowFilter = "HeadNo = " + CGlobal.UserInfo.EmployeeNo;
            dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    division.Add(dr["UnitGroupName"].ToString());
                }
            }

            //BU Planner
            var planner = from p in db.BUPlanners
                          where p.Planner == CGlobal.UserInfo.EmployeeNo
                          select p;
            List<BUPlanner> planners = planner.ToList<BUPlanner>();
            if (planners.Count() > 0)
            {
                List<Division> divLst;
                foreach (BUPlanner plan in planners)
                {
                    divLst = db.Divisions.Where(x => x.BU == plan.CostCenter).ToList();

                    foreach (Division div in divLst)
                    {
                        division.Add(div.DivisionName);
                    }
                }
            }

            //Current user unit group
            division.Add(CGlobal.UserInfo.UnitGroup);

            //Special Roles
            var role = from r in db.SpecialRoles
                       where r.EmpNo == CGlobal.UserInfo.EmployeeNo
                       select r;
            List<SpecialRole> roles = role.ToList<SpecialRole>();
            if (roles.Count() > 0)
            {
                foreach (SpecialRole sr in roles)
                {
                    division.Add(sr.Division);
                }
            }

            return division.Distinct().ToList();
        }

        public class SelectListItemComparer : IEqualityComparer<SelectListItem>
        {
            public bool Equals(SelectListItem x, SelectListItem y)
            {
                return x.Text == y.Text && x.Value == y.Value;
            }

            public int GetHashCode(SelectListItem item)
            {
                int hashText = item.Text == null ? 0 : item.Text.GetHashCode();
                int hashValue = item.Value == null ? 0 : item.Value.GetHashCode();
                return hashText ^ hashValue;
            }
        }
        public class UserRoleView
        {
            public int RunId { get; set; }
            public bool Active { get; set; }
            public string Username { get; set; }
            public string FullName { get; set; }
            public int RoleID { get; set; }
            public string RoleName { get; set; }
        }

    }
}