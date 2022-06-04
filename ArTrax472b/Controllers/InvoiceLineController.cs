// Decompiled with JetBrains decompiler
// Type: ArTrax41.Controllers.InvoiceLineController
// Assembly: ArTrax41, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CD51CC0E-3147-47B5-B370-049179D64418
// Assembly location: C:\Users\DavinderRai\Downloads\loadtrac\ArTrax41.dll

using ArTrax472b.Models;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace ArTrax41.Controllers
{
  [Authorize(Roles = "TICKETENTRY")]
  public class InvoiceLineController : Controller
  {
        private ArTraxTraxDbEntities db = new ArTraxTraxDbEntities();

        public ActionResult New() => (ActionResult) this.Json((object) new InvoiceLine()
    {
      ProductId = 1,
      TicketNumber = ""
    }, (JsonRequestBehavior) 0);

    [HttpPost]
    public int CreateJ(InvoiceLine invoiceline)
    {
      this.db.InvoiceLines.Add(invoiceline);
      ((DbContext) this.db).SaveChanges();
      //AuditLogHandler.RecordAuditLogEntry(invoiceline.Id, "InvoiceLine", "Create", (Controller) this);
      return invoiceline.Id;
    }

    //[HttpPost]
    //public string ValidateTicketNumber(InvoiceLine pLine)
    //{
    //  // ISSUE: object of a compiler-generated type is created
    //  // ISSUE: variable of a compiler-generated type
    //  InvoiceLineController.\u003C\u003Ec__DisplayClass0 cDisplayClass0 = new InvoiceLineController.\u003C\u003Ec__DisplayClass0();
    //  // ISSUE: reference to a compiler-generated field
    //  cDisplayClass0.pLine = pLine;
    //  ParameterExpression parameterExpression;
    //  // ISSUE: method reference
    //  // ISSUE: method reference
    //  // ISSUE: field reference
    //  // ISSUE: method reference
    //  // ISSUE: method reference
    //  // ISSUE: field reference
    //  // ISSUE: method reference
    //  return ((IQueryable<InvoiceLine>) this.db.InvoiceLines).Where<InvoiceLine>(Expression.Lambda<Func<InvoiceLine, bool>>((Expression) Expression.AndAlso((Expression) Expression.Call((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (InvoiceLine.get_TicketNumber))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.Equals)), (Expression) Expression.Property((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass0), FieldInfo.GetFieldFromHandle(__fieldref (InvoiceLineController.\u003C\u003Ec__DisplayClass0.pLine))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (InvoiceLine.get_TicketNumber)))), (Expression) Expression.Equal((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (InvoiceLine.get_CustomerId))), (Expression) Expression.Property((Expression) Expression.Field((Expression) Expression.Constant((object) cDisplayClass0), FieldInfo.GetFieldFromHandle(__fieldref (InvoiceLineController.\u003C\u003Ec__DisplayClass0.pLine))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (InvoiceLine.get_CustomerId))))), parameterExpression)).Count<InvoiceLine>() > 0 ? "This ticket number has already been used." : "";
    //}

    public ActionResult Get(int pId) => (ActionResult) this.Json((object) this.db.InvoiceLines.Find(new object[1]
    {
      (object) pId
    }), (JsonRequestBehavior) 0);

    [HttpPost]
    public void EditJ(InvoiceLine invoiceline)
    {
      ((DbContext) this.db).Entry<InvoiceLine>(invoiceline).State = EntityState.Modified;
      ((DbContext) this.db).SaveChanges();
    }

        public ActionResult GetOpenTickets(int CustomerId)
        {
            return Json(this.db.InvoiceLines.Take(5).ToList(), JsonRequestBehavior.AllowGet);
        } 

    [HttpPost]
    public void AddLineToInvoice(InvoiceLine pLine)
    {
      this.db.InvoiceLines.Find(new object[1]
      {
        (object) pLine.Id
      }).InvoiceId = pLine.InvoiceId;
      ((DbContext) this.db).SaveChanges();
    }

    [HttpPost]
    public void RemoveLineFromInvoice(InvoiceLine pLine)
    {
      this.db.InvoiceLines.Find(new object[1]
      {
        (object) pLine.Id
      }).InvoiceId = 0;
      ((DbContext) this.db).SaveChanges();
    }
    public ActionResult Details(int id = 0)
    {
      InvoiceLine invoiceLine = this.db.InvoiceLines.Find(new object[1]
      {
        (object) id
      });
      return invoiceLine == null ? (ActionResult) this.HttpNotFound() : (ActionResult) this.View((object) invoiceLine);
    }

    public ActionResult CreateM2(int pCustomerId, int pTruckId)
    { 
            return (ActionResult) this.View((object) new InvoiceLine()
      {
        CustomerId = pCustomerId,
        IsLineTaxable = true,
        TicketDate = DateTime.Now.Date,
        Price = 0M,
        TruckId = pTruckId,
        ProductId = 1
      });
    }

    [HttpPost]
    public ActionResult CreateM2(InvoiceLine invoiceline)
    {
      invoiceline.ProductId = 1;
      if (this.ModelState.IsValid)
      {
        this.db.InvoiceLines.Add(invoiceline);
        ((DbContext) this.db).SaveChanges();
        //AuditLogHandler.RecordAuditLogEntry(invoiceline.Id, "InvoiceLine", "Create", (Controller) this);
        return (ActionResult) this.RedirectToAction("Create", "InvoiceLine", (object) new
        {
          pCustomerId = invoiceline.CustomerId
        });
      }
      // ISSUE: reference to a compiler-generated field
      return (ActionResult) this.View((object) invoiceline);
    }

     

     
 
    protected virtual void Dispose(bool disposing)
    {
      ((DbContext) this.db).Dispose();
      base.Dispose(disposing);
    }
  }
}
