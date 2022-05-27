// Decompiled with JetBrains decompiler
// Type: ArTrax41.Controllers.DataServiceController
// Assembly: ArTrax41, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CD51CC0E-3147-47B5-B370-049179D64418
// Assembly location: C:\Dev\raient\LoadTracWeb\LoadTracWeb\Git\LoadTrac3\DeployedNoCode\bin\ArTrax41.dll

using System;
using System.Linq;
using System.Web.Mvc;

namespace ArTrax472.Controllers
{
    [Authorize(Roles = "TICKETENTRY")]
    public class DataServiceController : Controller
    {

        ArTrax472.Models.ArTraxTraxDbEntities db = new Models.ArTraxTraxDbEntities();

        public ActionResult GetTrucks2()
        {
            return View(db.Trucks.ToList());
        }

    }
}
