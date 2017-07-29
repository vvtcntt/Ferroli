using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSCODE.Models;
using PagedList;
using PagedList.Mvc;
namespace CMSCODE.Controllers.Display.News
{
    public class NewsDisplayController : Controller
    {
        //
        // GET: /NewsDisplay/
        private FerroliContext db = new FerroliContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListNews(string tag, int? page)
        {
            int idMenu;
            string Chuoi = tag;
            string[] Mang = Chuoi.Split('-');
            int one = int.Parse(Mang.Length.ToString());
            string chuoi1 = Mang[one - 1].ToString();
            string[] Mang1 = chuoi1.Split('.');
            idMenu = int.Parse(Mang1[0].ToString());
            tblGroupNew groupnew = db.tblGroupNews.Find(idMenu);
            string levels = groupnew.Level;
            var listNews = db.tblNews.Where(p => p.idCate == idMenu && p.Active == true).OrderByDescending(p => p.Ord).ToList();

            var Countlistnew = db.tblGroupNews.Where(p => p.Level.Substring(0, levels.Length) == levels && p.Active == true ).OrderBy(p => p.Ord).ToList();
            if(Countlistnew.Count>1)
            {

                for(int i=0;i<Countlistnew.Count;i++)
                {
                    int idMenus = int.Parse(Countlistnew[i].id.ToString());
                    listNews = db.tblNews.Where(p => p.idCate == idMenus && p.Active == true).OrderByDescending(p => p.Ord).ToList();
                }

            }

            
            ViewBag.Name = groupnew.Name;
            ViewBag.Title = "<title>" + groupnew.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupnew.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupnew.Keyword + "\" /> ";
            int dodai = groupnew.Level.Length / 5;
            string nUrl = "";
            for (int i = 0; i < dodai; i++)
            {
                var NameGroups = db.tblGroupNews.First(p => p.Level.Substring(0, (i + 1) * 5) == groupnew.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
                nUrl = nUrl + " <a href=\"/ListNew/" + NameGroups.Tag + "-"+NameGroups.id+".aspx\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + "</a> /";
            }
            ViewBag.nUrl = "<a href=\"http://binhnonglanhferroli.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> /" + nUrl;
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

            return View(listNews.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult NewsDetail(string tag)
        {

            int idNew;
            string Chuoi = tag;
            string[] Mang = Chuoi.Split('-');
            int one = int.Parse(Mang.Length.ToString());
            string chuoi1 = Mang[one - 1].ToString();
            string[] Mang1 = chuoi1.Split('.');
            idNew = int.Parse(Mang1[0].ToString());
            tblNew tblnew = db.tblNews.Find(idNew);
            string chuoinew = "";
            var listnew = db.tblNews.Where(p => p.idCate == tblnew.idCate && p.id != tblnew.id && p.Active == true).OrderByDescending(p => p.Ord).Take(3).ToList();
            for (int i = 0; i < listnew.Count;i++ )
            {
                chuoinew+="<a href=\"/News/"+listnew[i].Tag+"-"+listnew[i].id+".aspx\" title=\""+listnew[i].Name+"\"> - "+listnew[i].Name+"</a>";


            }
            ViewBag.chuoinew = chuoinew;
            ViewBag.Title = "<title>" + tblnew.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblnew.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblnew.Keyword + "\" /> ";
            var Groupnews = db.tblGroupNews.First(p => p.id == tblnew.idCate);
            int dodai = Groupnews.Level.Length / 5;
            string nUrl = "";
            for (int i = 0; i < dodai; i++)
            {
                var NameGroups = db.tblGroupNews.First(p => p.Level.Substring(0, (i + 1) * 5) == Groupnews.Level.Substring(0, (i + 1) * 5));
                nUrl = nUrl + " <a href=\"/ListNew/" + NameGroups.Tag + " -"+NameGroups.id+".aspx\" title=\"\"> " + " " + NameGroups.Name + "</a> /";
            }
            ViewBag.nUrl = "<a href=\"http://binhnonglanhferroli.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> / " + nUrl + " " + tblnew.Name;
                return View(tblnew);


        }
    }
}
