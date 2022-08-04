using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.Probation;

namespace HumanCapitalManagement.Controllers
{
    public class Probation_FormController : Controller
    {
        private StoreDb db = new StoreDb();

        // GET: Probation_Form
        public async Task<ActionResult> Index()
        {
            return View(await db.Probation_Form.ToListAsync());
        }

        // GET: Probation_Form/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Probation_Form probation_Form = await db.Probation_Form.FindAsync(id);
            if (probation_Form == null)
            {
                return HttpNotFound();
            }
            return View(probation_Form);
        }

        // GET: Probation_Form/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Probation_Form/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Assessment,Remark,Status,Probation_Active,Provident_Fund,Mail_Send,Start_Pro,End_Pro,Count_Date_Pro,HR_No,HR_Submit_Date,Staff_No,Staff_Acknowledge_Date,PM_No,PM_Submit_Date,GroupHead_No,GroupHead_Submit_Date,HOP_No,HOP_Submit_Date,Active_Status,Create_Date,Create_User,Update_Date,Update_User,Extend_Form,Extend_Status,Extend_Period,Remark_Revise,Send_Mail_Date,Staff_Action,PM_Action,GroupHead_Action,HOP_Action")] Probation_Form probation_Form)
        {
            if (ModelState.IsValid)
            {
                db.Probation_Form.Add(probation_Form);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(probation_Form);
        }

        // GET: Probation_Form/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Probation_Form probation_Form = await db.Probation_Form.FindAsync(id);
            if (probation_Form == null)
            {
                return HttpNotFound();
            }
            return View(probation_Form);
        }

        // POST: Probation_Form/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Assessment,Remark,Status,Probation_Active,Provident_Fund,Mail_Send,Start_Pro,End_Pro,Count_Date_Pro,HR_No,HR_Submit_Date,Staff_No,Staff_Acknowledge_Date,PM_No,PM_Submit_Date,GroupHead_No,GroupHead_Submit_Date,HOP_No,HOP_Submit_Date,Active_Status,Create_Date,Create_User,Update_Date,Update_User,Extend_Form,Extend_Status,Extend_Period,Remark_Revise,Send_Mail_Date,Staff_Action,PM_Action,GroupHead_Action,HOP_Action")] Probation_Form probation_Form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(probation_Form).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(probation_Form);
        }

        // GET: Probation_Form/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Probation_Form probation_Form = await db.Probation_Form.FindAsync(id);
            if (probation_Form == null)
            {
                return HttpNotFound();
            }
            return View(probation_Form);
        }

        // POST: Probation_Form/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Probation_Form probation_Form = await db.Probation_Form.FindAsync(id);
            db.Probation_Form.Remove(probation_Form);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
