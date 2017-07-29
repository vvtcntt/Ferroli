using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ferroli.Models;
namespace Ferroli.Controllers.Display.Maps
{
    public class MapsDisplayController : Controller
    {
        //
        // GET: /MapsDisplay/
        private FerroliContext db = new FerroliContext();

        public ActionResult Index()
        {
            var Map = db.tblMaps.First();
            ViewBag.Title = "<title>" + Map.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + Map.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + Map.Name + "\" /> ";
            return View(Map);
        }

    }
}
