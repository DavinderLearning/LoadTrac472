
using LoadTrac742.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LoadTrac742.Controllers
{
  [Authorize(Roles = "INVOICING")]
  public class InvoiceController : Controller
  {
    private ArTraxDbEntities db = new ArTraxDbEntities();

    public ActionResult Create(int pCustomerId) => (ActionResult) this.View((object) new Invoice()
    {
      CustomerId = pCustomerId,
      InvoiceNumber = this.getNextInvoiceNumber(),
      InvoiceDate = this.db.SystemSettings.FirstOrDefault<SystemSetting>().CurrentInvoiceDate,
      InvoiceDueDate = this.db.SystemSettings.FirstOrDefault<SystemSetting>().CurrentInvoiceDueDate
    });

    [HttpPost]
    public ActionResult Create(Invoice invoice)
    {
      invoice.InvoiceNumber = this.getNextInvoiceNumber();
      invoice.LastUpdatedOn = DateTime.Now.Date;
      invoice.CreatedOn = DateTime.Now.Date;
      invoice.CreatedBy = Environment.UserName;
      invoice.LastUpdatedBy = Environment.UserName;
      TaxRate activeTaxRate = this.getActiveTaxRate(invoice.InvoiceDate);
      if (activeTaxRate != null)
      {
        invoice.TaxName = activeTaxRate.Name;
        invoice.TaxRate = activeTaxRate.Rate;
      }
      this.db.Invoices.Add(invoice);
      this.db.SaveChanges();
      //AuditLogHandler.RecordAuditLogEntry(invoice.Id, "Invoice", nameof (Create), (Controller) this);
      return (ActionResult) this.RedirectToAction("Edit", (object) new
      {
        id = invoice.Id
      });
    }

    private string getNextInvoiceNumber()
    {
      if (this.db.SystemSettings.FirstOrDefault<SystemSetting>() == null)
        return "0";
      string nextInvoiceNumber = this.db.SystemSettings.FirstOrDefault<SystemSetting>().NextInvoiceNumber.ToString();
      int num = Convert.ToInt32(nextInvoiceNumber.ToString()) + 1;
      this.db.SystemSettings.First<SystemSetting>().NextInvoiceNumber = num.ToString();
      this.db.SaveChanges();
      return nextInvoiceNumber;
    }

    public TaxRate getActiveTaxRate(DateTime pSelectedDate) => this.db.TaxRates.Where<TaxRate>((Expression<Func<TaxRate, bool>>) (d => d.StartDate <= pSelectedDate && d.EndDate >= pSelectedDate)).FirstOrDefault<TaxRate>();

    public ActionResult Recalc(int pInvoiceId)
    {
      Invoice data = this.db.Invoices.Find(new object[1]
      {
        (object) pInvoiceId
      });
      List<InvoiceLine> list = this.db.InvoiceLines.Where<InvoiceLine>((Expression<Func<InvoiceLine, bool>>) (i => i.InvoiceId == pInvoiceId)).ToList<InvoiceLine>();
      Decimal d = 0M;
      Decimal num = 0M;
      //foreach (InvoiceLine invoiceLine in list)
      //  d += invoiceLine.ExtendedAmount;


      DbSet<InvoicePayment> invoicePayments = this.db.InvoicePayments;
      Expression<Func<InvoicePayment, bool>> predicate = (Expression<Func<InvoicePayment, bool>>) (i => i.InvoiceId == pInvoiceId);
      foreach (InvoicePayment invoicePayment in invoicePayments.Where<InvoicePayment>(predicate).ToList<InvoicePayment>())
        num += invoicePayment.Amount;
      data.Subtotal = Math.Round(d, 2);
      data.TaxAmount = Math.Round(d * (data.TaxRate / 100M), 2);
      data.Total = Math.Round(d + data.TaxAmount, 2);
      data.Balance = Math.Round(data.Total - num, 2);
      this.db.SaveChanges();
      return (ActionResult) this.Json((object) data, JsonRequestBehavior.AllowGet);
    }

    public ActionResult IndexJ(int pCustomerId) => (ActionResult) this.Json((object) this.db.Invoices.Where<Invoice>((Expression<Func<Invoice, bool>>) (o => o.CustomerId == pCustomerId)).OrderByDescending<Invoice, DateTime>((Expression<Func<Invoice, DateTime>>) (o => o.InvoiceDate)).ToList<Invoice>(), JsonRequestBehavior.AllowGet);

    public ActionResult IndexOpenInvoices(int pCustomerId) => (ActionResult) this.Json((object) this.db.Invoices.Where<Invoice>((Expression<Func<Invoice, bool>>) (o => o.CustomerId == pCustomerId && o.Balance > 0M)).OrderByDescending<Invoice, DateTime>((Expression<Func<Invoice, DateTime>>) (o => o.InvoiceDate)).ToList<Invoice>(), JsonRequestBehavior.AllowGet);

    public ActionResult Index() => (ActionResult) this.View((object) this.db.InvoiceLines.ToList<InvoiceLine>());

    public ActionResult Edit(int id = 0)
    {
      Invoice model = this.db.Invoices.Find(new object[1]
      {
        (object) id
      });
      return model == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) model);
    }

    protected override void Dispose(bool disposing)
    {
      this.db.Dispose();
      base.Dispose(disposing);
    }
  }
}
