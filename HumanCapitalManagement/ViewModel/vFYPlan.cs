using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.ViewModel.CommonVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel
{

    public class vFY_Plan : CSearchFYPlan
    {
        public List<vFY_Plan_obj> lstData { get; set; }
    }

    public class vFY_Plan_Return : CResutlWebMethod
    {
        public List<vFY_Plan_obj> lstData { get; set; }
    }


    public class vFY_Plan_Detail_obj
    {
        public string Id { get; set; }
        public string division { get; set; }
        public string group_code { get; set; }
        public string sgroup { get; set; }
        public decimal? para { get; set; }
        public decimal? aa { get; set; }
        public decimal? sr { get; set; }
        public decimal? am { get; set; }
        public decimal? mgr { get; set; }
        public decimal? ad { get; set; }
        public decimal? dir { get; set; }
        public decimal? ptr { get; set; }
        public decimal? total { get; set; }
        public string IdEncrypt { get; set; }
    }

    public class vFY_Plan_obj
    {
        public string Edit { get; set; }
        public string View { get; set; }
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string name_th { get; set; }
        public string name_en { get; set; }
        public string short_name_en { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }
        public string active_name { get; set; }
        public string create_date { get; set; }
        public string create_user { get; set; }
        public string update_date { get; set; }
        public string update_user { get; set; }
        public string replaced_user { get; set; }
        public string need_ceo { get; set; }
    }


    public class vFY_Plan_obj_save
    {
        public int Id { get; set; }
        public string IdEncrypt { get; set; }
        public string code { get; set; }

        public string active_status { get; set; }

        public string fy_year { get; set; }

        public string create_date { get; set; }

        public string create_user { get; set; }

        public string update_date { get; set; }

        public string update_user { get; set; }

        public List<vFY_Plan_detail> lstData { get; set; }

    }

    public class vFY_PlanApprover_obj_save
    {
        public string IdEncrypt { get; set; }
        public string emp_no { get; set; }
        public string description { get; set; }
        public string active_status { get; set; }

        public string fy_year { get; set; }

        public string create_date { get; set; }

        public string create_user { get; set; }

        public string update_date { get; set; }

        public string update_user { get; set; }

    }
    public class vFY_Plan_detail_obj_save {
        public string Id { get; set; }
        public string division { get; set; }
        public string group_code { get; set; }
        public string sgroup { get; set; }
        public decimal? para { get; set; }
        public decimal? aa { get; set; }
        public decimal? sr { get; set; }
        public decimal? am { get; set; }
        public decimal? mgr { get; set; }
        public decimal? ad { get; set; }
        public decimal? dir { get; set; }
        public decimal? ptr { get; set; }
        public decimal? total { get; set; }
        public string IdEncrypt { get; set; }
    }

    public class vFY_Plan_detail
    {
        public string Id { get; set; }
        public string division { get; set; }
        public string group_code { get; set; }
        public string sgroup { get; set; }
        public string para { get; set; }
        public string aa { get; set; }
        public string sr { get; set; }
        public string am { get; set; }
        public string mgr { get; set; }
        public string ad { get; set; }
        public string dir { get; set; }
        public string ptr { get; set; }
        public string total { get; set; }
    }
}