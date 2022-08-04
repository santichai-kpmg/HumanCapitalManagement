using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.PartnerEvaluation
{
    public class PTR_Evaluation_AuthorizedService : ServiceBase<PTR_Evaluation_Authorized>
    {
        public PTR_Evaluation_AuthorizedService(IRepository<PTR_Evaluation_Authorized> repo) : base(repo)
        {
            //
        }
        public PTR_Evaluation_Authorized Find(int id)
        {
            return Query().SingleOrDefault(s => s.Id == id);
        }
        public PTR_Evaluation_Authorized FindForDelete(int id, int nFileid)
        {
            return Query().SingleOrDefault(s => s.Id == nFileid && s.PTR_Evaluation.Id == id);
        }
        public bool CanSave(PTR_Evaluation_Authorized PTR_Evaluation_Authorized)
        {
            bool sCan = false;
            var sCheck = Query().FirstOrDefault(w => w.PTR_Evaluation.Id == PTR_Evaluation_Authorized.PTR_Evaluation.Id
                && w.authorized_user == PTR_Evaluation_Authorized.authorized_user
                && w.active_status == "Y");
            if (sCheck == null)
            {
                sCan = true;
            }

            return sCan;
        }
        public int CreateNew(PTR_Evaluation_Authorized s)
        {
            Add(s);
            var sResult = SaveChanges();
            return sResult;
        }
        public int Update(PTR_Evaluation_Authorized s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int DeleteFile(PTR_Evaluation_Authorized s)
        {
            var sResult = 0;
            var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
            if (_getData != null)
            {
                sResult = SaveChanges();
            }
            return sResult;
        }
        public int CreateNewOrUpdate(PTR_Evaluation_Authorized s)
        {
            var sResult = 0;
            var objSave = s;
            var _getDataTIF = Query().Where(w => w.PTR_Evaluation.Id == objSave.PTR_Evaluation.Id && w.authorized_user == objSave.authorized_user).FirstOrDefault();
            if (_getDataTIF == null)
            {
                Add(s);
            }
            else
            {
                _getDataTIF.update_user = s.update_user;
                _getDataTIF.update_date = s.update_date;
                _getDataTIF.active_status = "Y";
                s = _getDataTIF;
            }
            sResult = SaveChanges();
            return sResult;
        }
    }
}
