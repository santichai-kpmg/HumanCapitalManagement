using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PreInternAssessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PreInternAssessment
{
    public class TM_PInternAssessment_ActivitiesService : ServiceBase<TM_PInternAssessment_Activities>
    {
        public TM_PInternAssessment_ActivitiesService(IRepository<TM_PInternAssessment_Activities> repo) : base(repo)
        {
            //
        }
        public TM_PInternAssessment_Activities FindById(int id)
        {
            return Query().FirstOrDefault(s => s.Id == id);
        }
        public TM_PInternAssessment_Activities FindById2(string id)
        {
            return Query().FirstOrDefault(s => s.Id +"" == id);
        }
        public TM_PInternAssessment_Activities FindByCode(string code)
        {
            return Query().SingleOrDefault(s => s.Id+"" == code);
        }
        public TM_PInternAssessment_Activities FindByEmpID(string id)
        {
            return Query().SingleOrDefault(s => s.Id + "" == id);
        }
        public IEnumerable<TM_PInternAssessment_Activities> GetDataAllActive()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        public TM_PInternAssessment_Activities FindAddUploadFile(string id)
        {
            if (id != "")
            {
                return Query().FirstOrDefault(s => s.Id+"" == id);
            }
            else
            {
                return null;
            }

        }
        public IEnumerable<TM_PInternAssessment_Activities> GetDataAll()
        {
            var sQuery = Query();
            return sQuery.ToList();
        }


        #region Save Edit Delect 
        public int CreateNew(TM_PInternAssessment_Activities s)
        {
            
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public bool CanSave(TM_PInternAssessment_Activities s)
        {
            bool sCan = false;

            if (s.Id == 0)
            {
                var sCheck = Query().FirstOrDefault(w => (w.Activities_name_en + "").Trim() == (s.Activities_name_en + "").Trim());
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            else
            {
                var sCheck = Query().FirstOrDefault(w => (w.Activities_name_en + "").Trim() == (s.Activities_name_en + "").Trim() && w.Id != s.Id);
                if (sCheck == null)
                {
                    sCan = true;
                }
            }
            return sCan;
        }
        public int CreateNewOrUpdate(TM_PInternAssessment_Activities s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            


            if (_getData == null)
            {
                Add(s);
                sResult = SaveChanges();
            }
            else
            {

                _getData.Activities_name_en = s.Activities_name_en;
                _getData.active_status = s.active_status;
                _getData.update_user = s.update_user;
                _getData.update_date = s.update_date;
 
                sResult = SaveChanges();


            }
            //sResult = SaveChanges();
            return sResult;
        }

        public int UpdateStatus(TM_PInternAssessment_Activities s)
        {
            var sResult = 0;
            
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
            

                sResult = SaveChanges();
            }

            return sResult;
        }

        public int AdminUpdate(TM_PInternAssessment_Activities s)
        {
            var sResult = 0;

            var _getDataAD = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getDataAD != null)
            {
             

                sResult = SaveChanges();
            }
            return sResult;

        }

        public int Update(TM_PInternAssessment_Activities s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                _getData.active_status = s.active_status;
                _getData.update_date = s.update_date;
                _getData.update_user = s.update_user;
                _getData.Activities_name_en = (s.Activities_name_en + "").Trim();
                _getData.Activities_name_th = (s.Activities_name_en + "").Trim();
                _getData.Activities_descriptions = s.Activities_descriptions;
                sResult = SaveChanges();
            }
            return sResult;
        }
        #endregion


    }
}
