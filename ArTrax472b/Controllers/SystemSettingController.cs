
using ArTrax472b.Models;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ArTrax41.Controllers
{
  [Authorize(Roles = "INVOICING")]
  public class SystemSettingController : Controller
  {
    private  ArTraxTraxDbEntities db = new  ArTraxTraxDbEntities();

    public ActionResult Index() => (ActionResult) this.View((object) this.db.SystemSettings.ToList<SystemSetting>());

    public ActionResult Details(int id = 0)
    {
      SystemSetting model = this.db.SystemSettings.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    public ActionResult Create() => (ActionResult) this.View();

    [HttpPost]
    public ActionResult Create(SystemSetting systemsetting)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) systemsetting);
      this.db.SystemSettings.Add(systemsetting);
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    public ActionResult Edit(int id = 0)
    {
      SystemSetting model = this.db.SystemSettings.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [HttpPost]
    public ActionResult Edit(SystemSetting systemsetting)
    {
      if (this.ModelState.IsValid)
      {
       // this.db.Entry<SystemSetting>(systemsetting).State = EntityState.Modified;
        this.db.SaveChanges();
      }
      return (ActionResult) this.View((object) systemsetting);
    }

    public ActionResult Delete(int id = 0)
    {
      SystemSetting model = this.db.SystemSettings.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [ActionName("Delete")]
    [HttpPost]
    public ActionResult DeleteConfirmed(int id)
    {
      this.db.SystemSettings.Remove(this.db.SystemSettings.Find(new object[1]
      {
        (object) id
      }));
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      this.db.Dispose();
      base.Dispose(disposing);
    }
  }
}
