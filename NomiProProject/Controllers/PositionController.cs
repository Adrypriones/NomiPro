using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NomiProProject.Models;
using System.Data.Entity;

namespace NomiProProject.Controllers
{
    public class PositionController : Controller
    {
        private NomiProEntities db = new NomiProEntities();

        // GET: Position
        public ActionResult Position()
        {
            var a = db.Cargoes.ToList();
            return View(a);
        }





       

       
    }
}
