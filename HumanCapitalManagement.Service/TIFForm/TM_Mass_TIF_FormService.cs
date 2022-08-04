using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_Mass_TIF_FormService : ServiceBase<TM_Mass_TIF_Form>
    {
        public TM_Mass_TIF_FormService(IRepository<TM_Mass_TIF_Form> repo) : base(repo)
        {
            //
        }

        public TM_Mass_TIF_Form GetActiveTIFFormNew(int nId)
        {
            if (nId != 0)
            {
                return Query().Where(s => s.Id == nId).OrderByDescending(o => o.action_date).FirstOrDefault();
            }
            else
            {
                return Query().Where(s => s.active_status == "Y").OrderByDescending(o => o.action_date).FirstOrDefault();
            }

        }
        //public TM_Mass_TIF_Form FindByMappingID(int id)
        //{
        //    var Return = Query().FirstOrDefault(s => s.TM_MassTIF_Form_Question.Where(w => w.TM_Mass_TIF_Form.Id == id) ;
        //    return Return != null ? Return : null;

        //    //   return Query().SingleOrDefault(s => s.TM_PR_Candidate_Mapping.Id == id && s.active_status == "Y");
        //}
        public TM_Mass_TIF_Form Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public IEnumerable<TM_Mass_TIF_Form> GetDataForSelectAll()
        {
            return Query().OrderByDescending(o => o.action_date).ToList();
            //return sQuery.ToList();
        }
        public TM_Mass_TIF_Form GetActiveTIFForm()
        {
            
            return Query().Where(s => s.active_status == "Y").OrderByDescending(o => o.action_date).FirstOrDefault();
        }
        public TM_Mass_TIF_Form GetActiveMassTIFForm(int nID = 0)
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
        public IEnumerable<TM_Mass_TIF_Form> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
       
        public IEnumerable<TM_Mass_TIF_Form> GetTIF_FormForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<TM_Mass_TIF_Form> GetTIF_Form(string name, string status)//,bool isAdmin
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
        public int CreateNew(ref TM_Mass_TIF_Form s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Mass_TIF_Form s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {

                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateInactive(TM_Mass_TIF_Form s)
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
