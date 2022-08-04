using DevExpress.XtraPrinting;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Report.DataSet.ReportHeadcount;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.ReportVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.MainReport
{
    public class rpStaffMovementController : BaseController
    {
        private DivisionService _DivisionService;
        private TM_FY_PlanService _TM_FY_PlanService;
        private TM_FY_DetailService _TM_FY_DetailService;
        private PersonnelRequestService _PersonnelRequestService;
        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();
        public rpStaffMovementController(DivisionService DivisionService, TM_FY_PlanService TM_FY_PlanService, TM_FY_DetailService TM_FY_DetailService, PersonnelRequestService PersonnelRequestService)
        {
            _DivisionService = DivisionService;
            _TM_FY_PlanService = TM_FY_PlanService;
            _TM_FY_DetailService = TM_FY_DetailService;
            _PersonnelRequestService = PersonnelRequestService;
        }

        // GET: rpStaffMovement
        public ActionResult ExpStaffMove(string qryStr, string sMode)
        {
            var sCheck = acCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            vStaffMovement result = new vStaffMovement();

            if (qryStr != "")
            {
                if (Session[qryStr] != null)
                {
                    result = Session[qryStr] as vStaffMovement;

                    try
                    {
                        var stream = new MemoryStream();
                        XrTemp xrTemp = new XrTemp();
                        xrTemp.CreateDocument();

                        if (result.lstData.Any())
                        {
                            DataTable dtR = SystemFunction.LinqToDataTable(result.lstData);
                            dsStaffMovement ds = new dsStaffMovement();
                            ds.dsMain.Merge(dtR);
                            ReportStaffMovement report = new ReportStaffMovement();
                            //XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                            //xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                            report.DataSource = ds;
                            report.CreateDocument();
                            //Add page
                            xrTemp.Pages.AddRange(report.Pages);
                        }

                        if (result.lstPlan.Any())
                        {
                            DataTable dtR = SystemFunction.LinqToDataTable(result.lstPlan);
                            dsStaffMovement ds = new dsStaffMovement();
                            ds.dsPlan.Merge(dtR);
                            ReportStaffPlan report = new ReportStaffPlan();
                            //XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                            //xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                            report.DataSource = ds;
                            report.CreateDocument();
                            //Add page
                            xrTemp.Pages.AddRange(report.Pages);
                        }

                        XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                        xlsxOptions.ShowGridLines = true;
                        xlsxOptions.SheetName = "sheet_";
                        xlsxOptions.TextExportMode = TextExportMode.Value;
                        xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                        xrTemp.ExportToXlsx(stream, xlsxOptions);
                        stream.Seek(0, SeekOrigin.Begin);
                        byte[] breport = stream.ToArray();
                        return File(breport, "application/excel", "rpStaffMovement_" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");


                        return View();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                        throw;
                    }

                }
                else
                {
                    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                }
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }
        }
    }
}