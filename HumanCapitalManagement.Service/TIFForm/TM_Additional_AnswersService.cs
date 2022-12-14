using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.TIFForm
{
    public class TM_Additional_AnswersService : ServiceBase<TM_Additional_Answers>
    {
        public TM_Additional_AnswersService(IRepository<TM_Additional_Answers> repo) : base(repo)
        {
            //
        }
        public TM_Additional_Answers Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        
        public IEnumerable<TM_Additional_Answers> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public IEnumerable<TM_Additional_Answers> GetTIF_FormForSave(int[] aID)//,bool isAdmin
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
        public IEnumerable<TM_Additional_Answers> GetTIF_Form(string name, string status)//,bool isAdmin
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
        public int CreateNew(ref TM_Additional_Answers s)
        {
            //s.Id = SelectMax();
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(TM_Additional_Answers s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {

                sResult = SaveChanges();
            }
            return sResult;
        }
        public int UpdateInactive(TM_Additional_Answers s)
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
