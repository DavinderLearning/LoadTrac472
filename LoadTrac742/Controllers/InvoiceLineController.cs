// Decompiled with JetBrains decompiler
// Type: ArTrax41.Controllers.InvoiceLineController
// Assembly: ArTrax41, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CD51CC0E-3147-47B5-B370-049179D64418
// Assembly location: C:\Users\DavinderRai\Downloads\loadtrac\ArTrax41.dll

using LoadTrac742.Models;
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

namespace LoadTrac742.Controllers
{
    [Authorize(Roles = "TICKETENTRY")]
    public class InvoiceLineController : Controller
    {
        private ArTraxDbEntities db = new ArTraxDbEntities ();
        public ActionResult New()
        {
            InvoiceLine invoiceLine = new InvoiceLine { InvoiceId = 0, ProductId = 1, TicketNumber = "", TicketDate = DateTime.Now.Date };
            List<InvoiceLine> invoiceLines = new List<InvoiceLine>();
            invoiceLines.Add(invoiceLine);
            return Json(invoiceLines.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Get(int pId)
        {

            List<InvoiceLine> invoiceLines = new List<InvoiceLine>();
            invoiceLines.Add(db.InvoiceLines.Find(pId));
            return Json(invoiceLines, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Get(int pId) => (ActionResult)this.Json((object)this.db.InvoiceLines.Find(new object[1]
        //  {
        //      (object) pId
        //  }), (JsonRequestBehavior)0);


        [HttpPost]
        public int CreateJ(InvoiceLine invoiceline)
        {
            this.db.InvoiceLines.Add(invoiceline);
            ((DbContext)this.db).SaveChanges();
            //AuditLogHandler.RecordAuditLogEntry(invoiceline.Id, "InvoiceLine", "Create", (Controller) this);
            return invoiceline.Id;
        }

        [HttpPost]
        public string ValidateTicketNumber(InvoiceLine pLine)
        {
            int items = db.InvoiceLines.Where(p => p.CustomerId == pLine.CustomerId && p.TicketNumber.ToUpper() == pLine.TicketNumber.ToUpper()).Count();
            if (items > 0)
            {
                return $"TicketNumber {pLine.TicketNumber}aleady exits for this customer";
            }
            else
                return "";
        }

      
        [HttpPost]
        public void EditJ(InvoiceLine invoiceline)
        {
            ((DbContext)this.db).Entry<InvoiceLine>(invoiceline).State = EntityState.Modified;
            ((DbContext)this.db).SaveChanges();
        }

        public ActionResult GetOpenTickets(int CustomerId)
        {
            return Json(this.db.InvoiceLines.Where(x => x.CustomerId == CustomerId && x.InvoiceId == 0).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void AddLineToInvoice(InvoiceLine pLine)
        {
            this.db.InvoiceLines.Find(new object[1]
            {
        (object) pLine.Id
            }).InvoiceId = pLine.InvoiceId;
            ((DbContext)this.db).SaveChanges();
        }

        [HttpPost]
        public void RemoveLineFromInvoice(InvoiceLine pLine)
        {
            this.db.InvoiceLines.Find(new object[1]
            {
        (object) pLine.Id
            }).InvoiceId = 0;
            ((DbContext)this.db).SaveChanges();
        }
        public ActionResult Details(int id = 0)
        {
            InvoiceLine invoiceLine = this.db.InvoiceLines.Find(new object[1]
            {
        (object) id
            });
            return invoiceLine == null ? (ActionResult)this.HttpNotFound() : (ActionResult)this.View((object)invoiceLine);
        }

        public ActionResult CreateM2(int pCustomerId, int pTruckId)
        {
            return (ActionResult)this.View((object)new InvoiceLine()
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
                ((DbContext)this.db).SaveChanges();
                //AuditLogHandler.RecordAuditLogEntry(invoiceline.Id, "InvoiceLine", "Create", (Controller) this);
                return (ActionResult)this.RedirectToAction("Create", "InvoiceLine", (object)new
                {
                    pCustomerId = invoiceline.CustomerId
                });
            }
            // ISSUE: reference to a compiler-generated field
            return (ActionResult)this.View((object)invoiceline);
        }





        protected virtual void Dispose(bool disposing)
        {
            ((DbContext)this.db).Dispose();
            base.Dispose(disposing);
        }
    }
}
