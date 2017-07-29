using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSCODE.Models;
using PagedList;
using PagedList.Mvc;
namespace CMSCODE.Controllers.Display.Manufacturers
{
    public class MenufacturersDisplayController : Controller
    {
        //
        // GET: /MenufacturersDisplay/
        private FerroliContext db = new FerroliContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenufacturerDetail(string tag, tblManufacturer tblmanufacturer)
        {
           
            int idNew;
            string Chuoi = tag;
            string[] Mang = Chuoi.Split('-');
            int one = int.Parse(Mang.Length.ToString());
            string chuoi1 = Mang[one - 1].ToString();
            string[] Mang1 = chuoi1.Split('.');
            idNew = int.Parse(Mang1[0].ToString());
              tblmanufacturer = db.tblManufacturers.Find(idNew);
             string chuoinew = "";
             var listManufacturers = db.tblManufacturers.Where(p => p.idMenu == tblmanufacturer.idMenu && p.id != tblmanufacturer.id && p.Active == true).OrderByDescending(p => p.Ord).Take(3).ToList();
             for (int i = 0; i < listManufacturers.Count; i++)
            {
                chuoinew += "<a href=\"/3/" + listManufacturers[i].Tag + "-" + listManufacturers[i].id + ".aspx\" title=\"" + listManufacturers[i].Name + "\"> - " + listManufacturers[i].Name + "</a>";


            }
            ViewBag.chuoinew = chuoinew;
            ViewBag.Title = "<title>" + tblmanufacturer.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblmanufacturer.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblmanufacturer.Name + "\" /> ";
            var GroupManufacturers = db.tblGroupManufacturers.First(p => p.id == tblmanufacturer.idMenu);
            int dodai = GroupManufacturers.Level.Length / 5;
            string nUrl = "";
            for (int i = 0; i < dodai; i++)
            {
                var NameGroups = db.tblGroupManufacturers.First(p => p.Level.Substring(0, (i + 1) * 5) == GroupManufacturers.Level.Substring(0, (i + 1) * 5));
                nUrl = nUrl + " <a href=\"/9/" + NameGroups.Tag + " -" + NameGroups.id + ".aspx\" title=\"\"> " + " " + NameGroups.Name + "</a> /";
            }
            ViewBag.nUrl = "<a href=\"http://binhnonglanhferroli.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> / " + nUrl + " " + GroupManufacturers.Name;
           
            return View(tblmanufacturer);
         

        }
        public ActionResult MenufacturerList(string tag, int? page)
        {
             
                int idMenu;
                string Chuoi = tag;
                string[] Mang = Chuoi.Split('-');
                int one = int.Parse(Mang.Length.ToString());
                string chuoi1 = Mang[one - 1].ToString();
                string[] Mang1 = chuoi1.Split('.');
                idMenu = int.Parse(Mang1[0].ToString());
                tblGroupManufacturer groupmanufacturers = db.tblGroupManufacturers.Find(idMenu);
                string levels = groupmanufacturers.Level;
                var listManufacturers = db.tblManufacturers.Where(p => p.idMenu == idMenu && p.Active == true).OrderByDescending(p => p.Ord).ToList();
                ViewBag.Name = groupmanufacturers.Name;
                ViewBag.Title = "<title>" + groupmanufacturers.Name + "</title>";
                ViewBag.Description = "<meta name=\"description\" content=\"" + groupmanufacturers.Description + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupmanufacturers.Name + "\" /> ";
                //int dodai = groupmanufacturers.Level.Length / 5;
                //string nUrl = "";
                //for (int i = 0; i < dodai; i++)
                //{
                //    var NameGroups = db.tblGroupNews.First(p => p.Level.Substring(0, (i + 1) * 5) == groupmanufacturers.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
                //    nUrl = nUrl + " <a href=\"/9/" + NameGroups.Tag + "-" + NameGroups.id + ".aspx\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + "</a> /";
                //}
                ViewBag.nUrl = "<a href=\"http://binhnonglanhferroli.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> /" + groupmanufacturers.Name;
                const int pageSize = 12;
                var pageNumber = (page ?? 1);
                // Thiết lập phân trang
                var ship = new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                    DisplayLinkToLastPage = PagedListDisplayMode.Always,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = true,
                    DisplayPageCountAndCurrentLocation = false,
                    MaximumPageNumbersToDisplay = 5,
                    DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                    EllipsesFormat = "&#8230;",
                    LinkToFirstPageFormat = "Trang đầu",
                    LinkToPreviousPageFormat = "«",
                    LinkToIndividualPageFormat = "{0}",
                    LinkToNextPageFormat = "»",
                    LinkToLastPageFormat = "Trang cuối",
                    PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                    ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                    FunctionToDisplayEachPageNumber = null,
                    ClassToApplyToFirstListItemInPager = null,
                    ClassToApplyToLastListItemInPager = null,
                    ContainerDivClasses = new[] { "pagination-container" },
                    UlElementClasses = new[] { "pagination" },
                    LiElementClasses = Enumerable.Empty<string>()
                };
                ViewBag.ship = ship;

                return View(listManufacturers.ToPagedList(pageNumber, pageSize));
             

        }
    }
}
