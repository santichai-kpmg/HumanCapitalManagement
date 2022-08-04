using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PES_Nomination_CompetenciesService : ServiceBase<PES_Nomination_Competencies>
    {
        public PES_Nomination_CompetenciesService(IRepository<PES_Nomination_Competencies> repo) : base(repo)
        {
            //
        }


        public IEnumerable<PES_Nomination_Competencies> GetScored(
      int nID,
      bool isAdmin = false)//,bool isAdmin
        {
            //var sQuery = Query().Where(w => w.active_status == "Y" && (w.TM_PR_Status.Id == 3 || w.TM_PR_Status.Id == 2));
            var sQuery = Query().Where(w => w.active_status == "Y" && w.PES_Nomination != null);
            if ((nID > 0))
            {
                sQuery = sQuery.Where(w => nID == w.PES_Nomination.Id);
            }
            else
            {
                sQuery = sQuery.Take(0);
            }
            return sQuery.ToList();
        }

        public int UpdateAnswer(List<PES_Nomination_Competencies> s, int nEmID, string UserUpdate, DateTime dNow)
        {
            var sResult = 0;
            var _getAdv = Query().Where(w => w.PES_Nomination.Id == nEmID).ToList();
            //set old to inactive
            _getAdv.Where(w => !s.Select(s2 => s2.TM_PES_NMN_Competencies.Id).ToArray().Contains(w.TM_PES_NMN_Competencies.Id) && w.active_status == "Y").ToList().ForEach(ed =>
            {
                ed.active_status = "N";
                ed.update_user = UserUpdate;
                ed.update_date = dNow;
            });
            //set old to active

            foreach (var item in s.Where(w => w.PES_Nomination != null))
            {
                var Addnew = _getAdv.Where(w => w.TM_PES_NMN_Competencies.Id == item.TM_PES_NMN_Competencies.Id).FirstOrDefault();
                if (Addnew == null)
                {
                    Add(item);
                }
                else
                {
                    _getAdv.Where(w => w.TM_PES_NMN_Competencies.Id == item.TM_PES_NMN_Competencies.Id).ToList().ForEach(ed =>
                    {

                        ed.update_user = UserUpdate;
                        ed.update_date = dNow;
                        ed.answer = item.answer + "";
                        ed.TM_PES_NMN_Competencies_Rating_Id = item.TM_PES_NMN_Competencies_Rating_Id;

                    });
                }
            }

            sResult = SaveChanges();
            return sResult;
        }

    }
}
