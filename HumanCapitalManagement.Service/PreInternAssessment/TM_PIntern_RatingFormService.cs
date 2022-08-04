using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PreInternAssessment
{
    public class TM_PIntern_RatingFormService : ServiceBase<TM_PIntern_RatingForm>
    {
        public TM_PIntern_RatingFormService(IRepository<TM_PIntern_RatingForm> repo) : base(repo)
        {
            //
        }
        public TM_PIntern_RatingForm Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public TM_PIntern_RatingForm GetActivePInternForm(int nID = 0)
        {
            if (nID != 0)
            {
                return Query().Where(s => s.Id == nID).OrderByDescending(o => o.action_date).FirstOrDefault();
            }
            else
            {
                return Query().Where(s => s.active_status == "Y").OrderByDescending(o => o.action_date).FirstOrDefault();
            }
           
        }
        public IEnumerable<TM_PIntern_RatingForm> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }

        public IEnumerable<TM_PIntern_RatingForm> GetPIntern_FormForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<TM_PIntern_RatingForm> GetPIntern_Form(string name, string status)//,bool isAdmin
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
        public int CreateNew(ref TM_PIntern_RatingForm s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_PIntern_RatingForm s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {

                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateInactive(TM_PIntern_RatingForm s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id != s.Id && w.active_status == "Y").ToList();
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
