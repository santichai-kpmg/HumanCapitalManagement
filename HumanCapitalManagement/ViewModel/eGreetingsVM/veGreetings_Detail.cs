using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.eGreetingsVM
{
 
    public class veGreetings_Detail_Main : CSearcheGreetings_Detail
    {
        public List<veGreetings_Detail_obj> lstData { get; set; }
    }

    public class veGreetings_Detail_Return : CResutlWebMethod
    {
        public veGreetings_Detail_obj_save maindata { get; set; }
        public List<veGreetings_Detail_obj_save> lstData { get; set; }
    }
    public class veGreetings_Detail_obj
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
        public int? eGreetings_Main_Id { get; set; }
        public string Show_Name { get; set; }
        public string Stamp_Name { get; set; }
    }


    public class veGreetings_Detail_obj_save : CResutlWebMethod
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
        public string eGreetings_Main_Id { get; set; }
        public string Show_Name { get; set; }
    }

    
}