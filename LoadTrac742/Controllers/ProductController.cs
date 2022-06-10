
using LoadTrac742.Models;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace LoadTrac742.Controllers
{
  [Authorize(Roles = "TICKETENTRY")]
  public class ProductController : Controller
  {
    private  ArTraxDbEntities db = new ArTraxDbEntities();

    public ActionResult Index() => (ActionResult) this.View((object) this.db.Products.ToList<Product>());

    public ActionResult Details(int id = 0)
    {
      Product model = this.db.Products.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    public ActionResult Create() => (ActionResult) this.View();

    [HttpPost]
    public ActionResult Create(Product product)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) product);
      this.db.Products.Add(product);
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    public ActionResult Edit(int id = 0)
    {
      Product model = this.db.Products.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [HttpPost]
    public ActionResult Edit(Product product)
    {
      if (!this.ModelState.IsValid)
        return (ActionResult) this.View((object) product);
     // this.db.Entry<Product>(product).State = EntityState.Modified;
      this.db.SaveChanges();
      return (ActionResult) this.RedirectToAction("Index");
    }

    public ActionResult Delete(int id = 0)
    {
      Product model = this.db.Products.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    [HttpPost]
    [ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      this.db.Products.Remove(this.db.Products.Find(new object[1]
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
