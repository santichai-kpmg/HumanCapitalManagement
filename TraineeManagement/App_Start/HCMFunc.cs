using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using TraineeManagement.ViewModels.CommonVM;

namespace TraineeManagement.App_Start
{
    public class HCMFunc
    {
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "HCM";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "HCM";
            try
            {
                byte[] cipherBytes = Convert.FromBase64String((cipherText.Replace(" ", "+")));
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch
            {
                return "";

            }

        }
        public static string GetUrl(string sType, string sID, string Url, string emp_no)
        {

            string sUrl = "";
            vObjectMail obj = new vObjectMail();
            if (!string.IsNullOrEmpty(sType) && !string.IsNullOrEmpty(sID))
            {

                obj.id = sType + Encrypt(sID);
                obj.type = sType;
                obj.emp_no = emp_no;
                try
                {

                    string qryStr = JsonConvert.SerializeObject(obj,
                                    Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore,
                                        MissingMemberHandling = MissingMemberHandling.Ignore,
                                        DefaultValueHandling = DefaultValueHandling.Ignore,
                                    });
                    string qryStrEncrypt = Encrypt(qryStr);

                    sUrl = Url + "?qryStr=" + qryStrEncrypt;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            return sUrl;
        }

        //public static vObjectLogin_return funcLogin(string emp_no)
        //{
        //    vObjectLogin_return bReturn = new vObjectLogin_return();
        //    string winloginname = HttpContext.Current.User.Identity.Name;
        //    string EmpUserID = winloginname.Substring(winloginname.IndexOf("\\") + 1);
        //    bReturn.sSucc = false;
        //    try
        //    {
        //        wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        //        if (!string.IsNullOrEmpty(emp_no))
        //        {

        //            DataTable staff = wsHRis.getActiveStaffByEmpNo(emp_no);
        //            if (staff != null)
        //            {
        //                if (staff.Rows.Count == 1)
        //                {
        //                    New_HRISEntities dbHr = new New_HRISEntities();
        //                    var userID = staff.Columns.Contains("UserID") ? staff.Rows[0].Field<string>("UserID") + "" : "";
        //                    var UserNo = staff.Columns.Contains("EmpNo") ? staff.Rows[0].Field<string>("EmpNo") + "" : "";
        //                    if (UserNo != "" && userID != "" && UserNo == emp_no && userID == EmpUserID)
        //                    {
        //                        User newUser = new User(userID);
        //                        //string[] UserAdmin = WebConfigurationManager.AppSettings["UserAdmin"].Split(';');
        //                        newUser.UserId = staff.Columns.Contains("UserID") ? staff.Rows[0].Field<string>("UserID") + "" : "";
        //                        newUser.EmployeeNo = staff.Columns.Contains("EmpNo") ? staff.Rows[0].Field<string>("EmpNo") + "" : "";
        //                        newUser.FirstName = staff.Columns.Contains("EmpFirstName") ? staff.Rows[0].Field<string>("EmpFirstName") + "" : "";
        //                        newUser.LastName = staff.Columns.Contains("EmpSurname") ? staff.Rows[0].Field<string>("EmpSurname") + "" : "";
        //                        newUser.FullName = staff.Columns.Contains("EmpName") ? staff.Rows[0].Field<string>("EmpName") + "" : "";
        //                        newUser.EMail = staff.Columns.Contains("Email") ? staff.Rows[0].Field<string>("Email") + "" : "";
        //                        newUser.OfficePhone = staff.Columns.Contains("OfficePhone") ? staff.Rows[0].Field<string>("OfficePhone") + "" : "";
        //                        newUser.Company = staff.Columns.Contains("CompanyCode") ? staff.Rows[0].Field<string>("CompanyCode") + "" : "";
        //                        newUser.PI = staff.Columns.Contains("PI") ? staff.Rows[0].Field<string>("PI") + "" : "";
        //                        newUser.Pool = staff.Columns.Contains("Pool") ? staff.Rows[0].Field<string>("Pool") + "" : "";
        //                        newUser.Division = staff.Columns.Contains("Division") ? staff.Rows[0].Field<string>("Division") + "" : "";
        //                        newUser.UnitGroup = staff.Columns.Contains("UnitGroup") ? staff.Rows[0].Field<string>("UnitGroup") + "" : "";
        //                        newUser.Rank = staff.Columns.Contains("RankCode") ? staff.Rows[0].Field<string>("RankCode") + "" : "";
        //                        newUser.lstDivision = new List<lstDivision>();
        //                        var CheckDivi = dbHr.vw_Unit.Where(w => w.UnitID + "" == newUser.Division + "").FirstOrDefault();
        //                        if (CheckDivi != null)
        //                        {
        //                            newUser.lstDivision.Add(new lstDivision
        //                            {
        //                                sID = CheckDivi.UnitGroupID + "",// dbHr.vw_Unit.Where(w => w.UnitID == newUser.Division).Select(s => s.UnitGroupID + "").FirstOrDefault(),
        //                                sName = newUser.UnitGroup,
        //                                from_role = "Y",

        //                            });
        //                        }
        //                        var CheckCEO = dbHr.tbMaster_CompanyHead.Where(w => w.EmployeeNo == newUser.EmployeeNo).ToList();
        //                        var CheckPool = dbHr.tbMaster_PoolHead.Where(w => w.EmployeeNo == newUser.EmployeeNo).ToList();
        //                        var CheckGroupH = dbHr.tbMaster_UnitGroupHead.Where(w => w.EmployeeNo == newUser.EmployeeNo && w.RankID == 0).ToList();
        //                        if (CheckCEO.Any())
        //                        {
        //                            newUser.lstDivision.AddRange(dbHr.vw_Unit.Where(w => (CheckCEO.Select(s => s.CompanyID).ToArray()).Contains(w.CompanyID)).Select(s => new lstDivision
        //                            {
        //                                sID = s.UnitGroupID + "",
        //                                sName = s.UnitName,
        //                                from_role = "Y",
        //                            }).ToList());
        //                        }
        //                        if (CheckPool.Any())
        //                        {
        //                            newUser.lstDivision.AddRange(dbHr.vw_Unit.Where(w => (CheckPool.Select(s => s.PoolID).ToArray()).Contains(w.PoolID)).Select(s => new lstDivision
        //                            {
        //                                sID = s.UnitGroupID + "",
        //                                sName = s.UnitName,
        //                                from_role = "Y",
        //                            }).ToList());
        //                        }
        //                        if (CheckGroupH.Any())
        //                        {
        //                            newUser.lstDivision.AddRange(dbHr.vw_Unit.Where(w => (CheckGroupH.Select(s => s.UnitGroupID).ToArray()).Contains(w.UnitGroupID)).Select(s => new lstDivision
        //                            {
        //                                sID = s.UnitGroupID + "",
        //                                sName = s.UnitName,
        //                                from_role = "Y",
        //                            }).ToList());
        //                        }
        //                        var GetPic = dbHr.vw_StaffPhoto_FileName.Where(w => w.Employeeno == newUser.EmployeeNo).FirstOrDefault();
        //                        try
        //                        {
        //                            //string sPicturePath = @"\\thbkkfsr05\data3$\PMPS\HR\Photo\Phoomchai_All\Phoomchai_Active\";
        //                            string sPicturePath = System.Web.Configuration.WebConfigurationManager.AppSettings["PathPic"];// "/thbkkfsr05/data3$/PMPS/HR/Photo/Phoomchai_All/Phoomchai_Active/";
        //                            var absolutePath = HttpContext.Current.Server.MapPath("~/Image/noimgaaaa.png");
        //                            if (GetPic != null)
        //                            {
        //                                sPicturePath = (sPicturePath + GetPic.PhotoName + ".jpg").Replace("/", "\\");
        //                                if (File.Exists(sPicturePath))
        //                                {
        //                                    // newUser.Picture = String.Format("data:image/jpg;base64,{0}", File.ReadAllBytes(sPicturePath));
        //                                    newUser.aPicture = File.ReadAllBytes(sPicturePath);
        //                                }
        //                                else if (File.Exists(absolutePath))
        //                                {
        //                                    //newUser.Picture = String.Format("data:image/png;base64,{0}", File.ReadAllBytes("~/Image/noimgaaaa.png"));
        //                                    newUser.aPicture = File.ReadAllBytes(absolutePath);
        //                                    //newUser.aPicture = File.ReadAllBytes("~/Image/noimgaaaa.png");
        //                                }
        //                                else
        //                                {
        //                                    newUser.aPicture = null;
        //                                }
        //                            }
        //                        }
        //                        catch (Exception)
        //                        {
        //                            newUser.aPicture = null;
        //                        }
        //                        HttpContext.Current.Session["UserInfo"] = newUser;

        //                    }
        //                    else
        //                    {
        //                        bReturn.sSucc = true;
        //                    }

        //                }
        //                else
        //                {
        //                    bReturn.sSucc = true;
        //                }
        //            }
        //            else
        //            {
        //                bReturn.sSucc = true;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        bReturn.sSucc = true;
        //        // throw;
        //        return bReturn;
        //    }
        //    if (!string.IsNullOrEmpty(HttpContext.Current.Session["UserInfo"] as string))
        //    {
        //        bReturn.sSucc = true;
        //    }
        //    return bReturn;
        //}
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        public static bool SendMail(vObjectMail_Send objMail, ref string msg)
        {
            wsEmail2.EMail_NewSoapClient wsMail = new wsEmail2.EMail_NewSoapClient();
            bool bSuss = true;
            try
            {
                string UseMail = System.Web.Configuration.WebConfigurationManager.AppSettings["UseMail"];
                string demoMailTo = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailTo"];
                string demoMailCc = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailCC"];
                bSuss = wsMail.SendMail(
                          objMail.mail_from
                          , objMail.title_mail_from
                          , UseMail != "1" ? demoMailTo + "" : objMail.mail_to
                          , UseMail != "1" ? demoMailCc + "" : objMail.mail_cc
                          , ""
                          , objMail.mail_subject
                          , objMail.mail_content);
            }
            catch (Exception e)
            {
                msg = e.Message;
                bSuss = false;

            }
            return bSuss;
        }
        public static bool SendMailPES(vObjectMail_Send objMail, ref string msg)
        {
            wsEmail2.EMail_NewSoapClient wsMail = new wsEmail2.EMail_NewSoapClient();
            bool bSuss = true;
            try
            {
                string UseMail = System.Web.Configuration.WebConfigurationManager.AppSettings["UseMail"];
                string demoMailTo = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailTo"];
                string demoMailCc = System.Web.Configuration.WebConfigurationManager.AppSettings["demoMailCC"];
                wsMail.SendMailAsync(
                          objMail.mail_from
                          , objMail.title_mail_from
                          , UseMail != "1" ? demoMailTo + "" : objMail.mail_to
                          , UseMail != "1" ? demoMailCc + "" : objMail.mail_cc
                          , UseMail != "1" ? "" : ""
                          , objMail.mail_subject
                          , objMail.mail_content);
            }
            catch (Exception e)
            {
                msg = e.Message;
                bSuss = false;

            }
            return bSuss;
        }
    }
    public static class SHA
    {

        private const int SALT_SIZE = 8;
        private const int NUM_ITERATIONS = 1000;
        private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        public static string GenerateSHA256String(string inputString)
        {
            byte[] buf = new byte[SALT_SIZE];
            rng.GetBytes(buf);
            string salt = Convert.ToBase64String(buf);

            Rfc2898DeriveBytes deriver2898 = new Rfc2898DeriveBytes(inputString.Trim(), buf, NUM_ITERATIONS);
            string hash = Convert.ToBase64String(deriver2898.GetBytes(16));
            return salt + ':' + hash;
        }

        public static bool IsPasswordValid(string password, string saltHash)
        {
            string[] parts = saltHash.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)

                return false;
            byte[] buf = Convert.FromBase64String(parts[0]);
            Rfc2898DeriveBytes deriver2898 = new Rfc2898DeriveBytes(password.Trim(), buf, NUM_ITERATIONS);
            string computedHash = Convert.ToBase64String(deriver2898.GetBytes(16));
            return parts[1].Equals(computedHash);
        }
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

    }
}