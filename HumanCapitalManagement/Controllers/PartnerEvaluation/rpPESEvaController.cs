using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Report.DataSet.PESDataset;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.PESVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.PartnerEvaluation
{
    public class rpPESEvaController : BaseController
    {
        // GET: rpPESEva
        #region export PDF

        public ActionResult ExpPESEvaluationlst(string qryStr)
        {
            if (CGlobal.IsUserExpired())
            {
                return RedirectToAction("Login", "Login");
            }
            DateTime dNow = DateTime.Now;

            if (!string.IsNullOrEmpty(qryStr))
            {
                rpvPTREvaluation_Session objSession = Session[qryStr] as rpvPTREvaluation_Session;
                if (objSession != null && objSession.lstData != null && objSession.lstData.Any())
                {
                    try
                    {
                        XrTemp xrTemp = new XrTemp();
                        var stream = new MemoryStream();
                        xrTemp.CreateDocument();
                        DataTable dtR = SystemFunction.LinqToDataTable(objSession.lstData);
                        dsPESEvaluation ds = new dsPESEvaluation();
                        ds.dsMain.Merge(dtR);
                        ReportEvaluation report = new ReportEvaluation();
                        XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                        xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                        report.DataSource = ds;
                        report.CreateDocument();
                        XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                        xlsxOptions.ShowGridLines = true;
                        xlsxOptions.SheetName = "sheet_";
                        xlsxOptions.TextExportMode = TextExportMode.Text;
                        xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                        report.ExportToXlsx(stream, xlsxOptions);
                        stream.Seek(0, SeekOrigin.Begin);
                        byte[] breport = stream.ToArray();
                        return File(breport, "application/excel", "PESEvaluation" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

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
                //  return View();
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }

        }
        public ActionResult ExpPESPlanlst(string qryStr)
        {
            if (CGlobal.IsUserExpired())
            {
                return RedirectToAction("Login", "Login");
            }
            DateTime dNow = DateTime.Now;

            if (!string.IsNullOrEmpty(qryStr))
            {
                rpvPTREvaluation_Session objSession = Session[qryStr] as rpvPTREvaluation_Session;
                if (objSession != null && objSession.lstData != null && objSession.lstData.Any())
                {
                    try
                    {
                        XrTemp xrTemp = new XrTemp();
                        var stream = new MemoryStream();
                        xrTemp.CreateDocument();
                        DataTable dtR = SystemFunction.LinqToDataTable(objSession.lstData);
                        dsPESEvaluation ds = new dsPESEvaluation();
                        ds.dsMain.Merge(dtR);
                        ReportPTRPlan report = new ReportPTRPlan();
                        XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                        xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                        report.DataSource = ds;
                        report.CreateDocument();
                        XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                        xlsxOptions.ShowGridLines = true;
                        xlsxOptions.SheetName = "sheet_";
                        xlsxOptions.TextExportMode = TextExportMode.Text;
                        xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                        report.ExportToXlsx(stream, xlsxOptions);
                        stream.Seek(0, SeekOrigin.Begin);
                        byte[] breport = stream.ToArray();
                        return File(breport, "application/excel", "PESPersonalPlan" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

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
                //  return View();
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }

        }
        #endregion
    }
}