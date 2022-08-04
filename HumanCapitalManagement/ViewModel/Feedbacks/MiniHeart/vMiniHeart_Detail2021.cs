using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.Feedbacks.vMiniHeart_Detail2021
{
 
    public class vMiniHeart_Detail_Main2021 : CSearchMiniHeart_Detail
    {
        public List<vMiniHeart_Detail2021_obj> lstData { get; set; }
    }

    public class vMiniHeart_Detail2021_Return : CResutlWebMethod
    {
        public vMiniHeart_Detail2021_obj_save maindata { get; set; }
        public List<vMiniHeart_Detail2021_obj_save> lstData { get; set; }
    }
    public class vMiniHeart_Detail2021_obj
    {
        public int Id { get; set; }
        public string Rank { get; set; }
        public string Group { get; set; }
        public string Group_Text { get; set; }
        public string Emp_No { get; set; }
        public string Emp_Name { get; set; }
        public string Img_Emp { get; set; }
        public string Rank_Level { get; set; }
        public string Reason { get; set; }
        public string Active_Status { get; set; }
        public DateTime? Create_Date { get; set; }
        public string Create_User { get; set; }
        public DateTime? Update_Date { get; set; }
        public string Update_User { get; set; }
        public int? MiniHeart_Main_Id { get; set; }
        public string Show_Name { get; set; }
        public string Stamp_Name { get; set; }
    }


    public class vMiniHeart_Detail2021_obj_save : CResutlWebMethod
    {
        public string Id { get; set; }
        public string Rank { get; set; }
        public string Emp_No { get; set; }
        public string Reason { get; set; }
        public string Active_Status { get; set; }
        public string Create_Date { get; set; }
        public string Create_User { get; set; }
        public string Update_Date { get; set; }
        public string Update_User { get; set; }
        public string MiniHeart_Main_Id { get; set; }
        public string Show_Name { get; set; }
    }

    
}