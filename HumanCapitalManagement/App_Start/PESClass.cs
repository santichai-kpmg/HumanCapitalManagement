using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.App_Start
{
    public class PESClass
    {
        public const string Risk_Management = "00001627";

        public const string COO = "00001616";

        public const string Deputy_Ceo = "";

        public const string KhunSithakarn = "00001584";

        public const string CEO = "00001445";

        public const string KhunSukit = "00001658";

        public enum StepApprovePlan
        {
            Self = 1,
            gHead = 2,
            pHead = 3,
            Ceo = 4,

        }
        public enum StepApproveEvaluate
        {

            Self = 8,
            gHead = 5,
            pHead = 6,
            Ceo = 7,
            DCEO = 9,
        }
        public enum KPIsBase
        {
            Billing = 6,
            Chargeability = 5,
            Collections = 7,
            Contribution_margin = 2,
            Fee_Managed = 1,
            Lock_up = 4,
            Recovery_rate = 3,
        }
        public enum Eva_Status
        {
            Draft_Plan = 1,
            Waiting_for_Planning_Approval = 2,
            Waiting_for_Revised_Plan = 3,
            Planning_Completed = 4,
            Draft_Evaluate = 5,
            Waiting_for_Evaluation_Approval = 6,
            Waiting_for_Revised_Evaluate = 7,
            Evaluation_Completed = 8
        }
        public enum PRT_Menu
        {
            Evaluation_Form = 68,
            ApproveEvaluation_Form = 69,
            Evaluation_Report = 70,
            Evaluation = 95,
            Plan = 96,
            Plan_Form = 97,
            ApprovePlan_Form = 98,
            Plan_Report = 99,


        }

        public enum Incidents_Score
        {
            Excellent = 1,
            High = 2,
            Low = 3,
            NI = 4,
            Meet = 5,
        }
        public enum Annual_Rating
        {
            Excellently = 1,
            Expectations = 2,
            Expectations_high = 4,
            Expectations_low = 5,
            Improvement = 3,
        }

        public enum SignaturesStep
        {
            Self = 1,
            Sponsoring_Partner = 2,
            Group_Head = 3,
            HOP = 4,
            Nominating = 5,
            Risk_Management = 6,
            COO = 7,
            Deputy_Ceo = 8,
            Ceo = 9,
        }
        public enum SignaturesStepSeq
        {
            Self = 1,
            Sponsoring_Partner = 2,
            Group_Head = 3,
            HOP = 5,
            Nominating = 6,
            Risk_Management = 4,
            COO = 7,
            Deputy_Ceo = 8,
            Ceo = 9,
        }

        public enum Nomination_Status
        {
            Draft_Form = 1,
            Waiting_Approval = 2,
            Revised_Form = 3,
            Form_Completed = 4,
        }
        public enum Nomination_Type
        {
            Partner = 1,
            Director = 2,
            Shareholder = 3,
        }

        public enum Nomination_Questions
        {
            Client_Q1 = 1,
            Client_Q2 = 2,
            Client_Q3 = 3,
            Client_Q4 = 4,
            Business_Skills_Q1 = 5,
            Business_Skills_Q2 = 6,
            Management_Leadership_Q1 = 7,
            Management_Leadership_Q2 = 8,
            Management_Leadership_Q3 = 9,
            Social_Skills_Q1 = 10,
            Social_Skills_Q2 = 11,
            Thinking_Skills_Q1 = 12,
            Thinking_Skills_Q2 = 13,
            Thinking_Skills_Q3 = 14,
            Technical_Competence_Q1 = 15,
            RISK_MANAGEMENT_Q1 = 16,
            RISK_MANAGEMENT_Q2 = 17,
            Business_Skills_Q3 = 18,
            Technical_Competence_Q2 = 19,
            Technical_Competence_Q3 = 20,
        }
        public enum Evaluation_Questions
        {
            Development_Goal_stretch = 21,
            Development_Goal_Plan = 22,
            Development_Goal_Achievements = 26,
        }
    }
}