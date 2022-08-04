using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanCapitalManagement.App_Start
{
    public class HCMClass
    {
        public enum StatusCandidate
        {
            Interview = 3,
            Accepted = 12,
            AddNew = 20,
            OfferAccepted = 12,
            Turndown = 15,
            NoShow = 16,
            OnBoard = 14,
            Reject_Before_Sending_Hiring = 25,
            Withdraw_the_Offer_KPMG = 24,
            Reject_Before_Offer_Date = 22,
            Candidate_Withdrawn_Recruit = 19,
            Blacklist = 17,
            Offer_Rejected = 13,
            Transfer = 26,
            sendtoMGRHR = 10,
            BuPass = 5,
        }

        public enum StatusPR
        {
            Complete = 4,

        }
        public enum StatusTraineeEvaluation
        {
            Save_Draft = 1,
            Bu_Approve = 2,
            HR_Approve = 3,
            Complect = 4,
            Revise = 5,
            BuRevise = 6,
        }
        public enum MailContentTIFForm
        {
            Submit_TIFForm = 13,
            Approve = 14,
            Approve_HR_Change = 15,
            Evaluator_Rollback = 16,
            HR_Rollback = 17,
            Send_to_HR = 18,
            HR_Ack = 23,
        }
        public enum MailContentPreInternForm
        {
            Submit_PreInternForm = 48,
            Approve = 49,
            Approve_HR_Change = 50,
            Evaluator_Rollback = 51,
            HR_Rollback = 52,
            Send_to_HR = 53,
            HR_Ack = 54,
        }
        public enum Technical_Test
        {
            Oxford = 1,
        }

        public enum MailContentPTR
        {
            Eva_Submit = 6,
            Eva_Approve = 7,
            Eva_Revise = 8,
            Eva_Suscess = 12,
            Plan_Submit = 19,
            Plan_Approve = 20,
            Plan_Revise = 21,
            Plan_Suscess = 22,
            Nomination_Submit = 24,
            Nomination_Approve = 25,
            Nomination_Revise = 26,
            Nomination_Suscess = 27,

        }
        public static List<int> lstNotSelect()
        {
            List<int> lstNotSelect = new List<int>();
            lstNotSelect.Add((int)StatusCandidate.Turndown);
            lstNotSelect.Add((int)StatusCandidate.NoShow);
            lstNotSelect.Add((int)StatusCandidate.Reject_Before_Sending_Hiring);
            lstNotSelect.Add((int)StatusCandidate.Withdraw_the_Offer_KPMG);
            lstNotSelect.Add((int)StatusCandidate.Reject_Before_Offer_Date);
            lstNotSelect.Add((int)StatusCandidate.Candidate_Withdrawn_Recruit);
            lstNotSelect.Add((int)StatusCandidate.Blacklist);
            lstNotSelect.Add((int)StatusCandidate.Offer_Rejected);
            // lstNotSelect.Add((int)StatusCandidate.Transfer);
            return lstNotSelect.ToList();
        }
        public enum UnitGroup
        {
            HR = 18,

        }

    }
}