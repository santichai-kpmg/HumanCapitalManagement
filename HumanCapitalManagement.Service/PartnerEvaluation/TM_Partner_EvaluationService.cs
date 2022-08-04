using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class TM_Partner_EvaluationService : ServiceBase<TM_Partner_Evaluation>
    {
        public TM_Partner_EvaluationService(IRepository<TM_Partner_Evaluation> repo) : base(repo)
        {
            //
        }
        public TM_Partner_Evaluation Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_Partner_Evaluation GetActiveEvaForm(int? nyear)
        {
            return Query().Where(s => s.active_status == "Y" && s.action_date.Value.Year == nyear).OrderByDescending(o => o.action_date).FirstOrDefault();
        }
        public IEnumerable<TM_Partner_Evaluation> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Partner_Evaluation> GetTIF_FormForSave(int[] aID)//,bool isAdmin
        {
            var sQuery = Query();
            if (aID != null)
            {
                sQuery = sQuery.Where(w => aID.Contains(w.Id));
            }
            else
            {
                sQuery = sQuery.Take(0);
            }

            return sQuery.ToList();
        }
        public IEnumerable<TM_Partner_Evaluation> GetTIF_Form(string name, string status)//,bool isAdmin
        {
            var sQuery = Query().Where(w => 1 == 1);

            if (!string.IsNullOrEmpty(status))
            {
                sQuery = sQuery.Where(w => w.active_status == status);
            }
            //if(!isAdmin)
            //{
            //    sQuery = sQuery.Where(w => w.ListTempTransaction.code == code);
            //}
            return sQuery.ToList();
        }

        #region Save Edit Delect 
        public int CreateNew(ref TM_Partner_Evaluation s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Partner_Evaluation s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {

                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateInactive(TM_Partner_Evaluation s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id != s.Id && w.active_status == "Y" && w.action_date.Value.Year == s.action_date.Value.Year).ToList();
            if (_getData.Any())
            {
                _getData.ForEach(ed =>
                {
                    ed.active_status = "N";
                    ed.update_date = DateTime.Now;
                    ed.update_user = s.update_user;
                });
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion
    }
}
