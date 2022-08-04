using HumanCapitalManagement.ViewModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Common
{
    public class UploadFileController : Controller
    {
        // GET: UploadFile
        

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase fileContent)
        {
            try
            {
                if (fileContent.ContentLength > 0)
                {
                    //string _FileName = Path.GetFileName(file.FileName);

                    //string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    //file.SaveAs(_path);

                    var fileName = Path.GetFileName(fileContent.FileName);

                    #region savefiletopath
                    //var gen_namefile = itemdata.maindata.Staff_No + "-AP" + proform.Id + "_" + runnum.ToString("00#") + "." + fileName.Split('.')[1];

                    //var path = HttpContext.Server.MapPath("~/Attachment/Action_Plans/");
                    //fileContent.SaveAs(Path.Combine(path, gen_namefile));
                    #endregion savefiletopath

                    var filedata = new byte[] { };
                    string[] aType = new string[] { ".pdf", ".docx", ".doc", ".xlsx", ".xls", ".csv" };
                    // get a stream
                    var stream = fileContent.InputStream;
                    using (var package = new ExcelPackage(stream))
                    {
                        var ws = package.Workbook.Worksheets.First();
                        int rows = ws.Dimension.Rows; // 20
                        int columns = ws.Dimension.Columns; // 7

                        List<vStrengthFinder> strengthFinderHistory = new List<vStrengthFinder>();
                        // loop through the worksheet rows and columns
                        for (int i = 1; i <= rows; i++)
                        {
                            for (int j = 1; j <= columns; j++)
                            {
                                var value = ws.Cells[i, j].Value;
                                if (value != null)
                                {
                                    string content = value.ToString();
                                    strengthFinderHistory.Add(new vStrengthFinder());
                                }
                                
                                /* Do something ...*/
                            }
                        }
                    }
                   
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Msg = e.Message + "";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }
    }
}