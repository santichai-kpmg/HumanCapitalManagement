using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Models.MainModels
{
    public class TM_PR_Candidate_Mapping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ntime { get; set; }
        public int? seq { get; set; }
        [StringLength(500)]
        public string description { get; set; }
        [StringLength(10)]
        public string active_status { get; set; }
        public DateTime? create_date { get; set; }
        [StringLength(50)]
        public string create_user { get; set; }
        public DateTime? update_date { get; set; }
        [StringLength(50)]
        public string update_user { get; set; }
        public virtual PersonnelRequest PersonnelRequest { get; set; }
        public virtual TM_PInternAssessment_Activities  TM_PInternAssessment_Activities { get; set; }
        public virtual TM_Candidates TM_Candidates { get; set; }
        public virtual ICollection<TM_Candidate_Status_Cycle> TM_Candidate_Status_Cycle { get; set; }
        public virtual TM_Recruitment_Team TM_Recruitment_Team { get; set; }
        public virtual TM_Candidate_Rank TM_Candidate_Rank { get; set; }
        public DateTime? trainee_start { get; set; }
        public DateTime? trainee_end { get; set; }

        public decimal? daily_wage { get; set; }

        public virtual TM_Divisions TM_Divisions { get; set; }



        public string Current_Status()
        {
            string sReturn = "";
            if (TM_Candidate_Status_Cycle.Any(a => a.active_status == "Y"))
            {
                sReturn = TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status != null ? s.TM_Candidate_Status.candidate_status_name_en + "" : "").FirstOrDefault();
            }
            return sReturn;
        }
        public DateTime? Current_Status_date()
        {
            DateTime? sReturn = null;
            if (TM_Candidate_Status_Cycle.Any(a => a.active_status == "Y"))
            {
                sReturn = TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.action_date).FirstOrDefault();
            }
            return sReturn;
        }

        public string Pool_name()
        {
            string sReturn = "";
            if (PersonnelRequest != null && PersonnelRequest.TM_Divisions != null && PersonnelRequest.TM_Divisions.TM_Pool != null)
            {
                sReturn = PersonnelRequest.TM_Divisions.TM_Pool.Pool_short_name_en + "";
            }
            return sReturn;
        }
        public string Group_name()
        {
            string sReturn = "";
            if (PersonnelRequest != null && PersonnelRequest.TM_Divisions != null)
            {
                sReturn = PersonnelRequest.TM_Divisions.division_name_en + "";
            }
            return sReturn;
        }
        public string RequestType_name()
        {
            string sReturn = "";
            if (PersonnelRequest != null && PersonnelRequest.TM_Employment_Request != null && PersonnelRequest.TM_Employment_Request.TM_Request_Type != null)
            {
                sReturn = PersonnelRequest.TM_Employment_Request.TM_Request_Type.request_type_name_en + "";
            }
            return sReturn;
        }
        public string Position_name()
        {
            string sReturn = "";
            if (PersonnelRequest != null && PersonnelRequest.TM_Position != null)
            {
                sReturn = PersonnelRequest.TM_Position.position_name_en + "";
            }
            return sReturn;
        }
        public string Candidate_name()
        {
            string sReturn = "";
            if (TM_Candidates != null)
            {
                sReturn = TM_Candidates.candidate_full_name() + "";
            }
            return sReturn;
        }
    }


}
