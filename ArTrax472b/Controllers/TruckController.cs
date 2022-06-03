// Decompiled with JetBrains decompiler
// Type: ArTrax41.Controllers.TruckController
// Assembly: ArTrax41, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CD51CC0E-3147-47B5-B370-049179D64418
// Assembly location: C:\Dev\raient\LoadTracWeb\LoadTracWeb\Git\LoadTrac3\DeployedNoCode\bin\ArTrax41.dll

using ArTrax472b.Models;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace ArTrax472.Controllers
{
  [Authorize(Roles = "TICKETENTRY")]
  public class TruckController : Controller
  {
    private  ArTraxTraxDbEntities db = new  ArTraxTraxDbEntities();

    public ActionResult Index() => (ActionResult) this.View((object) this.db.Trucks.OrderBy<Truck, string>((Expression<Func<Truck, string>>) (t => t.Name)).ToList<Truck>());

    public ActionResult Details(int id = 0)
    {
      Truck model = this.db.Trucks.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    public ActionResult Create() => (ActionResult) this.View();

    [HttpPost]
    public ActionResult Create(Truck truck)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) truck);
      this.db.Trucks.Add(truck);
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    public ActionResult Edit(int id = 0)
    {
      Truck model = this.db.Trucks.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [HttpPost]
    public ActionResult Edit(Truck truck)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) truck);
     // this.db.Entry<Truck>(truck).State = EntityState.Modified;
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    public ActionResult Delete(int id = 0)
    {
      Truck model = this.db.Trucks.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [HttpPost]
    [ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      this.db.Trucks.Remove(this.db.Trucks.Find(new object[1]
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
