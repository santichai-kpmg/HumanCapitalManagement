using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Reflection;

/// <summary>
/// Summary description for SystemFunction
/// </summary>
public class SystemFunction
{
    public static string GetDefaultPage = "";//ConfigurationManager.AppSettings["DefaultPage"].ToString();
    public const string ASC = "asc";
    public const string DESC = "desc";

    public SystemFunction()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string Ob2Json(object ob)
    {
        try
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = 2147483644 };
            string res = serializer.Serialize(ob);//new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ob);

            return res;
        }
        catch
        {
            return "";
        }
    }

    #region //All Msg Dialog
    public static string Msg_HeadInfo = "Process result.";
    public static string Msg_HeadError = "An error occur, please contact administrator.";
    public static string Msg_HeadConfirm = "Process confirmation.";
    public static string Msg_HeadWarning = "Process result.";

    public static string Msg_ConfirmDel = "Do you want to delete?";
    public static string Msg_ConfirmSave = "Do you want to submit?";
    public static string Msg_AlertDel = "Please select item you want to delete.";
    public static string Msg_SaveComplete = "Save completed.";
    public static string Msg_DelComplete = "Delete completed.";
    public static string Msg_OverSize = "ขนาดไฟล์เกินขนาดที่กำหนด";
    public static string Msg_InvalidFileType = "ประเภทไฟล์ไม่ถูกต้อง";
    public static string Msg_Failed = "An error occur, please contact administrator.ในกระบวนการทำงานของระบบ";
    public static string Msg_Login = "กรุณา Login เข้าใช้งานระบบ";
    public static string Msg_Duplicate = " ซ้ำกับที่มีในระบบ";

    public static string process_SessionExpired = "SSEXP";
    public static string process_Success = "Success";
    public static string process_Failed = "Failed";
    public static string process_FileOversize = "OverSize";
    public static string process_FileInvalidType = "InvalidType";
    public static string process_Duplicate = "DUP";
    #endregion

    #region Dialog
    public static string PopupLogin()
    {
        return SystemFunction.DialogAlertLogin(SystemFunction.Msg_HeadInfo, SystemFunction.Msg_Login, SystemFunction.GetDefaultPage);
    }

    public static string DialogInfo(string head, string msg)
    {
        return "DialogInfo('" + head + "','" + msg + "')";
    }

    public static string DialogInfoRedirect(string head, string msg, string redirto)
    {
        return "DialogInfoRedirect('" + head + "','" + msg + "','" + redirto + "')";
    }

    public static string DialogError(string head, string msg)
    {
        return "DialogError('" + head + "','" + msg + "')";
    }

    public static string DialogErrorRedirect(string head, string msg, string redirto)
    {
        return "DialogErrorRedirect('" + head + "','" + msg + "','" + redirto + "')";
    }

    public static string DialogWarning(string head, string msg)
    {
        return "DialogWarning('" + head + "','" + msg + "')";
    }

    public static string DialogWarningRedirect(string head, string msg)
    {
        return "DialogWarningRedirect('" + head + "','" + msg + "')";
    }

    public static string DialogConfirm(string head, string msg, string funcYes, string funcNo)
    {
        return "DialogConfirm('" + head + "','" + msg + "'," + funcYes + "," + funcNo + ")";
    }

    public static string DialogSuccess(string head, string msg)
    {
        return "DialogSuccess('" + head + "','" + msg + "')";
    }

    public static string DialogSuccessRedirect(string head, string msg, string redirto)
    {
        return "DialogSuccessRedirect('" + head + "','" + msg + "','" + redirto + "')";
    }

    public static string DialogAlertLogin(string head, string msg, string redirto)
    {
        return "DialogAlertLogin('" + head + "','" + msg + "','" + redirto + "')";
    }
    #endregion

    public static string ConvertExponentialToString(string sVal)
    {
        string sRsult = "";
        try
        {
            decimal nTemp = 0;
            bool check = Decimal.TryParse(sVal, System.Globalization.NumberStyles.Float, null, out nTemp);
            if (check)
            {
                decimal d = Decimal.Parse(sVal, System.Globalization.NumberStyles.Float);
                sRsult = d + "";
            }
            else
            {
                sRsult = sVal;
            }
        }
        catch
        {
            sRsult = sVal;
        }

        return sRsult != null ? (sRsult.Length < 50 ? sRsult : sRsult.Remove(50)) : ""; //เพื่อไม่ให้ตอน Save Error หากค่าที่เกิดจากผลการคำนวนเกิน Type ใน DB (varchar(50))
    }

    public static decimal? GetDecimalNull(string sVal)
    {
        decimal? nTemp = null;
        decimal nCheck = 0;
        if (!string.IsNullOrEmpty(sVal))
        {
            sVal = ConvertExponentialToString(sVal);
            bool cCheck = decimal.TryParse(sVal, out nCheck);
            if (cCheck)
            {
                nTemp = decimal.Parse(sVal);
            }
        }

        return nTemp;
    }

    public static int? GetIntNull(string sVal)
    {
        int? nTemp = null;
        int nCheck = 0;
        if (!string.IsNullOrEmpty(sVal))
        {
            sVal = ConvertExponentialToString(sVal);
            bool cCheck = int.TryParse(sVal, out nCheck);
            if (cCheck)
            {
                nTemp = int.Parse(sVal);
            }
        }

        return nTemp;
    }

    public static int GetIntNullToZero(string sVal)
    {
        int nTemp = 0;
        int nCheck = 0;
        if (!string.IsNullOrEmpty(sVal))
        {
            sVal = ConvertExponentialToString(sVal);
            bool cCheck = int.TryParse(sVal, out nCheck);
            if (cCheck)
            {
                nTemp = int.Parse(sVal);
            }
        }

        return nTemp;
    }

    public static decimal GetNumberNullToZero(string sVal)
    {
        decimal nTemp = 0;
        sVal = ConvertExponentialToString(sVal);
        nTemp = decimal.TryParse(sVal, out nTemp) ? nTemp : 0;
        return nTemp;
    }

    public static bool IsNumberic(string sVal)
    {
        decimal nTemp = 0;
        sVal = ConvertExponentialToString(sVal);
        return decimal.TryParse(sVal, out nTemp);
    }

    public static DateTime ConvertStringToDateTime(string strDate, string strTime, string sFormat = "dd-MMM-yyyy")
    {
        DateTime dTemp;
        bool checkDate = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), sFormat + " HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp);
        if (!checkDate)
        {
            if (strTime.Length < 5)
            {
                dTemp = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? "0" + strTime : "00.00"), sFormat + " HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : DateTime.Now;
            }
        }
        else
        {
            dTemp = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), sFormat + " HH.mm", new CultureInfo("en-US"), DateTimeStyles.None, out dTemp) ? dTemp : DateTime.Now;
        }

        return dTemp;
    }

    /// <summary>
    /// this fucntion will convert from Thai Culture to en-US
    /// </summary>
    /// <param name="strDate"></param>
    /// <param name="strTime"></param>
    /// <returns>time in en-US</returns>
    public static string ConvertDateStringFromRVP(string strDate, string strTime)
    {
        DateTime dt = new DateTime();
        string format = "dd/MM/yyyy HH:mm:ss";
        try
        {
            string[] datearr = strDate.Split('/');
            string[] timearr = strTime.Split(':');

            if (string.IsNullOrEmpty(strTime))
                format = "dd/MM/yyyy";

            if (datearr.Length > 1)
            {
                if (timearr.Length > 1)
                {
                    dt = new DateTime(int.Parse(datearr[2]), int.Parse(datearr[1]), int.Parse(datearr[0]),
                                       int.Parse(timearr[0]), int.Parse(timearr[1]), int.Parse(timearr[2]));
                }
                else
                {
                    dt = new DateTime(int.Parse(datearr[2]), int.Parse(datearr[1]), int.Parse(datearr[0]));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Format("{0} {1}", strDate, strTime);
        }

        return dt.ToString(format);
    }
    public static DateTime ConvertStringToDateTimeThai(string strDate, string strTime)
    {
        DateTime dTemp;
        bool checkDate = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("th-TH"), DateTimeStyles.None, out dTemp);
        if (!checkDate)
        {
            if (strTime.Length < 5)
            {
                dTemp = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? "0" + strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("th-TH"), DateTimeStyles.None, out dTemp) ? dTemp : DateTime.Now;
            }
        }
        else
        {
            dTemp = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("th-TH"), DateTimeStyles.None, out dTemp) ? dTemp : DateTime.Now;
        }

        return dTemp;
    }

    public static string DatetimeForWhereOracle(string strDate, string strTime)
    {
        string sDate = "";
        DateTime dTemp;
        bool checkDate = DateTime.TryParseExact(strDate + " " + ((strTime) != "" ? strTime : "00.00"), "dd/MM/yyyy HH.mm", new CultureInfo("th-TH"), DateTimeStyles.None, out dTemp);
        if (checkDate)
        {
            DateTime dDate = ConvertStringToDateTimeThai(strDate, strTime);
            sDate = dDate.ToString("yyyy-MM-dd", new CultureInfo("en-US"));
        }

        return sDate;
    }
    public static string DatetimeForSaveOracle(DateTime? dDate)
    {
        string sDate = "";

        sDate = dDate.HasValue ? dDate.Value.ToString("yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US")) : "";

        return sDate;
    }
    public static List<T> ConvertObject<T>(object objData)
    {
        List<T> Temp = new List<T>();
        if (objData != null)
        {
            Temp = (List<T>)objData;
        }
        return Temp;
    }

    public static DataTable ConvertObject(object objData)
    {
        DataTable dt = new DataTable();
        if (objData != null)
        {
            dt = (DataTable)objData;
        }
        return dt;
    }

    public static bool ConvertObjectBool(object objData)
    {
        bool cIs = false;
        if (objData != null)
        {
            try
            {
                cIs = (bool)objData;
            }
            catch
            { }
        }
        return cIs;
    }

    public static string RandomNumber()
    {
        Random random = new Random(); return random.Next(11111, 99999).ToString();
    }

    public static string RandomString(int length)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static DataTable LinqToDataTable<T>(IEnumerable<T> Data)
    {
        DataTable dtReturn = new DataTable();
        if (Data.ToList().Count == 0) return null;
        // Could add a check to verify that there is an element 0

        T TopRec = Data.ElementAt(0);

        // Use reflection to get property names, to create table

        // column names

        PropertyInfo[] oProps = ((Type)TopRec.GetType()).GetProperties();

        foreach (PropertyInfo pi in oProps)
        {

            Type colType = pi.PropertyType; if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {

                colType = colType.GetGenericArguments()[0];

            }

            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
        }

        foreach (T rec in Data)
        {

            DataRow dr = dtReturn.NewRow(); foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
            }
            dtReturn.Rows.Add(dr);

        }
        return dtReturn;
    }


}


public static class myextension
{

    public static string encval(this object xxx)
    {

        return xxx.ToString();
        //Encryption.Hash();
    }


    public static string decval(this object xxx)
    {

        return xxx.ToString();
        //Encryption.Hash();
    }

    public static string AsDateTimeWithTimebyCulture(this object item, string format = "dd/MM/yyyy HH:mm:ss")
    {
        if (item == null || string.IsNullOrEmpty(item.ToString()))
        { return null; }

        DateTime result;
        if (!DateTime.TryParse(item.ToString().Replace("-", "/"), out result))
        { return ""; }

        return result.ToString(format, new CultureInfo("en-US"));
    }
    public static string DateTimeWithTimebyCulture(this DateTime item, string format = "dd-MMM-yyyy HH:mm:ss")
    {
        if (item == null || string.IsNullOrEmpty(item.ToString()))
        { return null; }

        return item.ToString(format, new CultureInfo("en-US"));
    }
    public static string DateTimebyCulture(this DateTime item, string format = "dd-MMM-yyyy")
    {
        if (item == null || string.IsNullOrEmpty(item.ToString()))
        { return null; }

        return item.ToString(format, new CultureInfo("en-US"));
    }
    public static string StringRemark(this string item, int nLength = 80)
    {
        if (item == null || string.IsNullOrEmpty(item.ToString()))
        { return null; }
        string sReturn = "";
        if (item.Length <= nLength)
        {
            sReturn = item;
        }
        else
        {
            sReturn = item.Substring(0, (nLength - 3)) + "...";
        }
        return sReturn;
    }
    public static string MoneyFormat(this decimal? item, string format = "en-US")
    {
        if (item == null || !item.HasValue)
        { return ""; }
        string sReturn = "";
        if (item.HasValue)
        {
            sReturn = item.Value.ToString("N", new CultureInfo(format));
        }
        return sReturn;
    }
    public static string NodecimalFormat(this decimal? item, string format = "en-US")
    {
        if (item == null || !item.HasValue)
        { return ""; }
        string sReturn = "";
        if (item.HasValue)
        {
            sReturn = item.Value.ToString("N0", new CultureInfo(format));
        }
        return sReturn;
    }
    public static string NodecimalFormatHasvalue(this decimal item, string format = "en-US")
    {
        return item.ToString("N0", new CultureInfo(format));
    }
}