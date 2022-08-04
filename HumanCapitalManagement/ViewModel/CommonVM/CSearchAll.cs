using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.ViewModel.CommonVM
{
    public class CSearchAll
    {
    }
    public class C_USERS_RETURN
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_last_name { get; set; }
        public string unit_name { get; set; }
        public string user_position { get; set; }
        public string user_rank { get; set; }
        public string user_no { get; set; }

    }
    public class CSearchTracking
    {
        [DefaultValue("")]
        public string id { get; set; }

        [DefaultValue("")]
        public string pr_status { get; set; }
        [DefaultValue("")]
        public string division { get; set; }
    }

    public class CSearchGroup_Permission
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchUser_Permission
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string group_permiss_id { get; set; }
        [DefaultValue("")]
        public string group_code { get; set; }
    }

    public class CSearchPRForm
    {
        [DefaultValue("")]
        public string id { get; set; }

        [DefaultValue("")]
        public string pr_status { get; set; }
        [DefaultValue("")]
        public string division { get; set; }
    }
    public class CSearchUnit_Group
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchPool
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchCompany
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchDocument
    {

        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchRequestType
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchMajor
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchUniversityMajor
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchFaculty
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }

    public class CSearchFYPlan
    {
        [DefaultValue("")]
        public string id { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        //[DefaultValue("")]
        //public string name { get; set; }
        [DefaultValue("")]
        public string fy_year { get; set; }
        [DefaultValue("")]
        public string status_id { get; set; }
        //[DefaultValue("")]
        //public string group_id { get; set; }
        [DefaultValue("")]
        public string session { get; set; }
    }


    public class CSearchUniversity
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchEmploymentType
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchCandidateStatus
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchRank
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchInterview
    {
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string tif_type { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
        
        [DefaultValue("")]
        public string ActivitiesTrainee_code { get; set; }
        [DefaultValue("")]
        public string PR_No { get; set; }
    }
    public class CSearchPoolRank
    {

        [DefaultValue("")]
        public string pool_id { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchSubGroup
    {
        [DefaultValue("")]
        public string group_id { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchPosition
    {

        [DefaultValue("")]
        public string group_id { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchPersonnelRequest
    {


        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string pr_status { get; set; }
        [DefaultValue("")]
        public string ref_no { get; set; }
        [DefaultValue("")]
        public string sub_group_id { get; set; }
        [DefaultValue("")]
        public string position_id { get; set; }
    }
    public class CSearchRCMTracking
    {
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string pr_status { get; set; }
        [DefaultValue("")]
        public string ref_no { get; set; }
        [DefaultValue("")]
        public string sub_group_id { get; set; }
        [DefaultValue("")]
        public string position_id { get; set; }
    }
    public class CSearchMailContent
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchCandidates
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string lastname { get; set; }
        [DefaultValue("")]
        public string idcard { get; set; }
        [DefaultValue("")]
        public string sex { get; set; }
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string pr_status { get; set; }
        [DefaultValue("")]
        public string ref_no { get; set; }
        [DefaultValue("")]
        public string sub_group_id { get; set; }
        [DefaultValue("")]
        public string position_id { get; set; }
    }
    public class CSearchPRCandidates
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string pr_status { get; set; }
        [DefaultValue("")]
        public string ref_no { get; set; }
        [DefaultValue("")]
        public string sub_group_id { get; set; }
        [DefaultValue("")]
        public string position_id { get; set; }
        [DefaultValue("")]
        public string rank_id { get; set; }
    }
    public class CSearchRecruitmentTeam
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchCandidateRank
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchTIFForm
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchCandidate_tif_approve
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string lastname { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchAdInfo
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchMassTIFForm
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchEvaluationForm
    {


        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchAcKnowledge
    {
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string ActivitiesTrainee_code { get; set; }
        [DefaultValue("")]
        public string PR_No { get; set; }
    }
    public class CSearchTraineeEva
    {
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string eva_name { get; set; }
        [DefaultValue("")]
        public string eva_ic_name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string eva_status { get; set; }
        [DefaultValue("")]
        public string session { get; set; }
        [DefaultValue("")]
        public string start_date { get; set; }
        [DefaultValue("")]
        public string start_to { get; set; }
        [DefaultValue("")]
        public string end_date { get; set; }
        [DefaultValue("")]
        public string end_to { get; set; }
    }
    public class CSearchTIFReport
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string ref_no { get; set; }
        [DefaultValue("")]
        public string sub_group_id { get; set; }
        [DefaultValue("")]
        public string position_id { get; set; }
        [DefaultValue("")]
        public string hr_owner { get; set; }
        [DefaultValue("")]
        public string tif_type { get; set; }
        [DefaultValue("")]
        public string target_start { get; set; }
        [DefaultValue("")]
        public string target_end { get; set; }
        [DefaultValue("")]
        public string session { get; set; }
        [DefaultValue("")]
        public string ActivitiesTrainee_code { get; set; }
        [DefaultValue("")]
        public string PR_No { get; set; }
    }
    public class CSearchPRAdmin
    {
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string pr_status { get; set; }
        [DefaultValue("")]
        public string ref_no { get; set; }
        [DefaultValue("")]
        public string sub_group_id { get; set; }
        [DefaultValue("")]
        public string position_id { get; set; }
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string has_edit { get; set; }
    }

    public class CSearchTimeSheet
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchFeedback
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }

    public class CSearchProbation
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchConsent
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchMiniHeart_Main
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }

    public class CSearchMiniHeart_Detail
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchMiniHeart
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearchPreTraineeAssessment
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
        [DefaultValue("")]
        public string group_code { get; set; }
        [DefaultValue("")]
        public string ActivitiesTrainee_code { get; set; }
        [DefaultValue("")]
        public string PR_No { get; set; }
    }
    public class CSearchEmployee
    {
        [DefaultValue("")]
        public string Customer_code { get; set; }
        [DefaultValue("")]
        public string Employee_name { get; set; }
        [DefaultValue("")]
        public string Employee_lastname { get; set; }
       
        [DefaultValue("")]
        public string Company_Id { get; set; }
        [DefaultValue("")]
        public string Prefix_Id { get; set; }
        [DefaultValue("")]
        public string Staff_Id { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearcheGreetings
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    public class CSearcheGreetings_Main
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }

    public class CSearcheGreetings_Detail
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
    
    public class CSearcheForAll
    {
        [DefaultValue("")]
        public string name { get; set; }
        [DefaultValue("")]
        public string active_status { get; set; }
    }
}