using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.VisExpiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanCapitalManagement.Service.VisaExpiry
{

    public class TM_Company_VisaService : ServiceBase<TM_Company_Visa>
    {
        public TM_Company_VisaService(IRepository<TM_Company_Visa> repo) : base(repo)
        {
            //
        }
        public IEnumerable<TM_Company_Visa> GetDataForSelect()
        {
            var sQuery = Query().Where(w => w.active_status == "Y");
            return sQuery.ToList();
        }
        //public TM_Company_Visa Find(int id)
        //{
        //    return Query().SingleOrDefault(s => s.Id == id);
        //}
        //public IEnumerable<TM_Company_Visa> GetDataByEmp(int IdEmp)
        //{
        //    var sQuery = Query().Where(w => w.TM_EmployeeForeign_Visa.Id == IdEmp && w.active_status == "Y");
        //    return sQuery.ToList();
        //}

        //public TM_Company_Visa FindDocumentNumber(string doc_num)
        //{
        //    var Return = Query().FirstOrDefault(s => s.doc_number == doc_num);
        //    return Return != null ? Return : null;
        //}
        //public IEnumerable<TM_Company_Visa> GetDataforSearchactive(string active_status)
        //{
        //    //Search all
        //    var sQuery = Query().Where(w => w.active_status != "");
        //    //Check active status
        //    if (!string.IsNullOrEmpty(active_status) && active_status == "Y")
        //    {
        //        sQuery = sQuery.Where(w => w.active_status + "" == "Y");
        //    }
        //    if (!string.IsNullOrEmpty(active_status) && active_status == "N")
        //    {
        //        sQuery = sQuery.Where(w => w.active_status + "" == "N");
        //    }
        //    //if (!string.IsNullOrEmpty(active_status) && active_status == "A")
        //    //{
        //    //    sQuery = sQuery.Where(w => w.active_status + "" == "Y" && w.active_status == "N");
        //    //}
        //    return sQuery.ToList();
        //}
        //public IEnumerable<TM_Company_Visa> GetDataAllActive()
        //{
        //    var sQuery = Query().Where(w => w.active_status == "Y");
        //    return sQuery.ToList();
        //}
        //public TM_Company_Visa FindDocumentNumber_Type(string doc_num, string type)
        //{
        //    //var Return = Query().FirstOrDefault(s => s.doc_number == doc_num && s.TM_Type_Document_Id+"" == type);
        //    var Return = Query().Where(w => w.doc_number == doc_num && w.TM_Type_Document_Id + "" == type).FirstOrDefault();
        //    return Return != null ? Return : null;
        //}
        //#region Save Edit Delect 
        //public int CreateNew(ref TM_Company_Visa s)
        //{
        //    //s.Id = SelectMax();
        //    Add(s);
        //    var sResult = SaveChanges();
        //    return sResult;
        //}
        //public int Update(TM_Company_Visa s)
        //{
        //    var sResult = 0;
        //    var _getData = Query().Where(w => w.Id == s.Id).FirstOrDefault();
        //    if (_getData != null)
        //    {
        //        sResult = SaveChanges();
        //    }
        //    return sResult;
        //}
        //public int CreateNewOrUpdate(ref TM_Company_Visa s)
        //{
        //    var sResult = 0;
        //    var objSave = s;
        //    var _getDataObj = Query().Where(w => w.Id == objSave.Id).FirstOrDefault();
        //    if (_getDataObj == null)
        //    {
        //        Add(s);
        //    }
        //    else
        //    {
        //        _getDataObj.update_user = s.update_user;
        //        _getDataObj.update_date = s.update_date;
        //        _getDataObj.doc_number = s.doc_number;
        //        _getDataObj.active_status = s.active_status;
        //        _getDataObj.date_of_issued = s.date_of_issued;
        //        _getDataObj.valid_date = s.valid_date;
        //        _getDataObj.TM_Country_Id = s.TM_Country_Id;
        //        _getDataObj.TM_Type_Document_Id = s.TM_Type_Document_Id;
        //        s = _getDataObj;
        //    }
        //    sResult = SaveChanges();
        //    return sResult;
        //}

        //#endregion
    }
}
