
using ArTrax472b.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ArTrax472.Controllers
{
  [Authorize(Roles = "INVOICING, TICKETENTRY")]
  public class CustomerController : Controller
  {
    private  ArTraxTraxDbEntities db = new  ArTraxTraxDbEntities();

    public ActionResult Get(int pId) => (ActionResult) this.Json((object) this.db.Customers.Find(new object[1]
    {
      (object) pId
    }), JsonRequestBehavior.AllowGet);

    public ActionResult IndexJ() => (ActionResult) this.Json((object) this.db.Customers.ToList<Customer>().OrderBy<Customer, string>((Func<Customer, string>) (c => c.CustomerName)), JsonRequestBehavior.AllowGet);

    public ActionResult List(string searchString) => (ActionResult) this.View((object) this.db.Customers.Where<Customer>((Expression<Func<Customer, bool>>) (c => c.CustomerName.Contains("TEST"))).OrderBy<Customer, string>((Expression<Func<Customer, string>>) (c => c.CustomerName)).ToList<Customer>());

    public ViewResult IndexM() => this.View((object) this.db.Customers.Where<Customer>((Expression<Func<Customer, bool>>) (c => c.Active == true)).OrderBy<Customer, string>((Expression<Func<Customer, string>>) (c => c.CustomerName)).ToList<Customer>());

    public ViewResult Index(string sortOrder, string searchString)
    {
      
        IQueryable<Customer> source1 = this.db.Customers.Select<Customer, Customer>((Expression<Func<Customer, Customer>>) (s => s));
      if (!string.IsNullOrEmpty(searchString))
        source1 = source1.Where<Customer>((Expression<Func<Customer, bool>>) (s => s.CustomerName.ToUpper().StartsWith(searchString.ToUpper())));
      IQueryable<Customer> source2;
      switch (sortOrder)
      {
        case "CustomerName":
          source2 = (IQueryable<Customer>) source1.OrderByDescending<Customer, string>((Expression<Func<Customer, string>>) (s => s.CustomerName));
          break;
        case "City":
          source2 = (IQueryable<Customer>) source1.OrderBy<Customer, string>((Expression<Func<Customer, string>>) (s => s.City));
          break;
        default:
          source2 = (IQueryable<Customer>) source1.OrderBy<Customer, string>((Expression<Func<Customer, string>>) (s => s.CustomerName));
          break;
      }
      return this.View((object) source2.ToList<Customer>());
    }

    public ActionResult Details(int id = 0)
    {
      Customer model = this.db.Customers.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    public ActionResult Create() => (ActionResult) this.View();

    [HttpPost]
    public ActionResult Create(Customer customer)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) customer);
      customer.CreatedOn = DateTime.Now;
      customer.CreatedBy = "t";
      customer.LastUpdatedOn = DateTime.Now;
      customer.LastUpdatedBy = Environment.UserName;
      this.db.Customers.Add(customer);
      this.db.SaveChanges();
     // AuditLogHandler.RecordAuditLogEntry(customer.Id, "Customer", nameof (Create), (Controller) this);
      return (ActionResult) this.RedirectToAction("Index");
    }

    public ActionResult Edit(int id = 0)
    {
      Customer model = this.db.Customers.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [HttpPost]
    public ActionResult Edit(Customer customer)
    {
      if (this.ModelState.IsValid)
      {
        //this.db.Entry<Customer>(customer).State = EntityState.Modified;
        customer.LastUpdatedOn = DateTime.Now;
        customer.LastUpdatedBy = Environment.UserName;
        this.db.SaveChanges();
        //AuditLogHandler.RecordAuditLogEntry(customer.Id, "Customer", nameof (Edit), (Controller) this);
      }
      return (ActionResult) this.View((object) customer);
    }

    public ActionResult Delete(int id = 0)
    {
      Customer model = this.db.Customers.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [HttpPost]
    [ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Customer entity = this.db.Customers.Find(new object[1]
      {
        (object) id
      });
      this.db.Customers.Remove(entity);
      this.db.SaveChanges();
     // AuditLogHandler.RecordAuditLogEntry(entity.Id, "Customer", "Delete", (Controller) this);
      return (ActionResult) this.RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      this.db.Dispose();
      base.Dispose(disposing);
    }
  }
}
