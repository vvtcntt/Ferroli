using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ferroli.Models;
using System.Globalization;
using System.Text;
namespace Ferroli.Controllers.Display
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/
        private FerroliContext db = new FerroliContext();
        public ActionResult Index(string sx)
        {
            tblConfig tblconfig  = db.tblConfigs.Find(1);
            ViewBag.Title = "<title>" + tblconfig.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblconfig.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblconfig.Keywords + "\" /> ";
            Session["h1"] = "<h1 class=\"h1\">" + tblconfig.Title + "</h1>";
            string chuoi = "";
            var ListCapacity = db.tblCapacities.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            foreach (var item in ListCapacity)
            {
                chuoi += "<div class=\"Product\">";
                chuoi += "<div class=\"nVar\">";
                chuoi += "<div class=\"Left\">";
                chuoi += "<h2> <a href=\"/" + item.Tag + ".html\" title=\"" + item.Name + "\"> " + item.Name + "</a></h2>";
                chuoi += "</div>";
                chuoi += "<div class=\"Right\">";
                chuoi += "<select class=\"Arrange\">";
                chuoi += "<option value=\"0\" selected=\"selected\">Sắp xếp theo giá</option> ";
                chuoi += "<option value=\"1\">Tăng dần</option>";
                chuoi += "<option value=\"2\">Giảm dần</option>";
                chuoi += "</select>";
                chuoi += "</div>";
                chuoi += "</div>";
                chuoi += "<div class=\"Content\">";
                int idCap = item.id;
                var listProduct = db.tblProducts.Where(p => p.Capacity == idCap && p.Active == true && p.ViewHomes == true).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listProduct.Count; j++)
                {
                    chuoi += "<div class=\"Tear_1\">";
                    if (listProduct[j].New == true)
                    {
                        chuoi += "<div class=\"New\">";
                        chuoi += "</div>";
                    }
                    chuoi += "<div class=\"img\">";
                    chuoi += "<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Title + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>";
                    chuoi += "</div>";
                    chuoi += "<h3><a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>";
                    chuoi += "<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", listProduct[j].Price) + " đ</span></p>";
                    chuoi += "<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " đ</span></p>";
                    chuoi += " </div> ";
                }
                chuoi += "</div>";
                chuoi += "</div>";
            }
            var GroupProduct = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            string chuoi1 = "";
            foreach (var item in GroupProduct)
            {
                chuoi1 += "<div class=\"Product\">";
                chuoi1 += "<div class=\"nVar\">";
                chuoi1 += "<div class=\"Left\">";
                chuoi1 += "<h2> <a href=\"/ListProduct/" + item.Tag + "-" + item.id + ".aspx\" title=\"" + item.Title + "\"> " + item.Name + "</a></h2>";
                chuoi1 += "</div>";
                chuoi1 += "<div class=\"Right\">";
                chuoi1 += "<select class=\"Arrange\">";
                chuoi1 += "<option value=\"0\" selected=\"selected\">Sắp xếp theo giá</option> ";
                chuoi1 += "<option value=\"1\">Tăng dần</option>";
                chuoi1 += "<option value=\"2\">Giảm dần</option>";
                chuoi1 += "</select>";

                chuoi1 += "</div>";
                int idCate = item.id;
                List<string> Mang = new List<string>();
                Mang = Arrayid(idCate);
                Mang.Add(idCate.ToString());
                var listProduct = db.tblProducts.Where(p => Mang.Contains(p.idCate.ToString()) && p.Active == true && p.ViewHomes == true).OrderBy(p => p.Ord).ToList();
                var listProduct1 = db.tblProducts.Where(p => Mang.Contains(p.idCate.ToString()) && p.Active == true ).OrderBy(p => p.Ord).ToList();
                chuoi1 += "</div>";
                chuoi1 += "<div class=\"adw\">";
                if (listProduct1.Count > 0)
                {
                    if (item.Images!=null && item.Images!="")
                        chuoi1 += " <a href=\"/product/" + listProduct1[0].Tag + "-" + item.id + "-" + listProduct1[0].id + ".aspx\" title=\"" + item.Title + "\"><img  src=\"" + item.Images + "\" alt=\"" + item.Name + "\" /></a>";

                }

                chuoi1 += "</div>";
                chuoi1 += "<div class=\"Content\">";
               
                for (int j = 0; j < listProduct.Count; j++)
                {
                    chuoi1 += "<div class=\"Tear_1\">";
                    if (listProduct[j].New == true)
                    {
                        chuoi1 += "<div class=\"New\">";
                        chuoi1 += "</div>";
                    }
                    chuoi1 += "<div class=\"img\">";
                    chuoi1 += "<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>";
                    chuoi1 += "</div>";
                    chuoi1 += "<h3><a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a></h3>";
                    chuoi1 += "<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", listProduct[j].Price) + " đ</span></p>";
                    chuoi1 += "<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " đ</span></p>";
                    chuoi1 += " </div> ";
                }
                chuoi1 += "</div>";
                chuoi1 += "</div>";
                Mangphantu.Clear();
            }
            ViewBag.chuoi = chuoi; ViewBag.chuoi1 = chuoi1;
                return View();
        }
        public PartialViewResult callPartial()
        {
            return PartialView(db.tblConfigs.First());
        }
        public PartialViewResult SlidePartial()
        {
            var listSlide = db.tblImages.Where(p => p.idCate == 1 && p.Active == true).OrderBy(p => p.Ord).Take(5).ToList();
            string chuoi = "";
            for (int i = 0; i < listSlide.Count; i++)
            {
                if (i == 0)
                    chuoi += "url(" + listSlide[i].Images + ") 0 0 no-repeat";
                else
                    chuoi += "," + "url(" + listSlide[i].Images + ") " + (i * 1200) + "px 0 no-repeat";
            }
            ViewBag.url = chuoi;
            return PartialView(listSlide);
        }
        string nUrl = "";
        public string UrlProduct(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <a href=\"/ListProduct/" + ListMenu[i].Tag + "-" + ListMenu[i].id + ".aspx\" title=\"" + ListMenu[i].Name + "\"> " + " " + ListMenu[i].Name + "</a> <i></i>" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlProduct(id);
                }
            }
            return nUrl;
        }
        List<string> Mangphantu = new List<string>();
        public List<string> Arrayid(int idParent)
        {

            var ListMenu = db.tblGroupProducts.Where(p => p.ParentID == idParent).ToList();

            for (int i = 0; i < ListMenu.Count; i++)
            {
                Mangphantu.Add(ListMenu[i].id.ToString());
                int id = int.Parse(ListMenu[i].id.ToString());
                Arrayid(id);

            }

            return Mangphantu;
        }
        List<SelectListItem> carlist = new List<SelectListItem>();
        public PartialViewResult TopPartial()
        {
            tblConfig config = db.tblConfigs.First();
            var Menu=db.tblGroupProducts.Where(p=>p.ParentID==null&& p.Active==true).OrderBy(p=>p.Ord).ToList();
            string Chuoi="";
            if (Session["h1"] != null)
            {
                ViewBag.h1 = Session["h1"];
                Session["h1"] = "";
            }
            for (int i = 0; i < Menu.Count; i++)
            { 
                 Chuoi+="<li class=\"li1\">";
                 Chuoi += " <a href=\"/ListProduct/"+Menu[i].Tag+"-"+Menu[i].id+".aspx\" title=\"" + Menu[i].Name + "\">" + Menu[i].Name + "</a>";
                 int idCate = Menu[i].id;
                        var Menu1=db.tblGroupProducts.Where(p=>p.ParentID==idCate && p.Active==true).OrderBy(p=>p.Ord).ToList();
                         if(Menu1.Count>0)
                         {  
                             
                             Chuoi+="<ul class=\"ul2\">";
                             for(int j=0;j<Menu1.Count;j++)
                             {

                                 Chuoi+="<li class=\"li2\">";
                                 Chuoi += "<h2><a href=\"/ListProduct/"+Menu1[j].Tag+"-"+Menu1[j].id+".aspx\" title=\"" + Menu1[j].Name + "\">" + Menu1[j].Name + "</a></h2>";
                                 int idCate1 = Menu1[j].id;
                                 var Menu2=db.tblGroupProducts.Where(p=>p.ParentID==idCate1 && p.Active==true).OrderBy(p=>p.Ord).ToList();
                                
                                 if(Menu2.Count>0)
                                 { 
                                     Chuoi+="<ul class=\"ul3\">";
                                    for(int k=0;k<Menu2.Count;k++)
                                    {
                                      Chuoi+="<li class=\"li3\">";
                                      Chuoi += "<h3><a href=\"/ListProduct/" + Menu2[k].Tag + "-" + Menu2[k].id + ".aspx\" title=\"" + Menu2[k].Name + "\">› " + Menu2[k].Name + "</a></h3>";
                                     Chuoi+=" </li>";
                                    } 
                                     Chuoi+="</ul>";
                                 }

                                 Chuoi+="</li>";
                         
                             }
                        
                            Chuoi+="</ul>";

                         }
                        
               Chuoi+=" </li>";
            }
            ViewBag.Menu = Chuoi;
            var menuModel = db.tblGroupProducts.Where(m => m.ParentID == null).OrderBy(m => m.id).ToList();
            carlist.Clear();
            string strReturn = "---";
            foreach (var item in menuModel)
            {
                carlist.Add(new SelectListItem { Text = item.Name, Value = item.id.ToString() });
                StringClass.DropDownListFor(item.id, carlist, strReturn);
                strReturn = "---";
            }

            ViewBag.one = carlist;

          return PartialView(config);

        }
        [HttpPost]
        public ActionResult Dropdownlist(int id)
        {
            var listProduct = db.tblGroupProducts.Find(id);

            return Redirect("/ListProduct/" + listProduct.Tag + "." + id + ".aspx");
        }
        public PartialViewResult partialNewsHomes()
        {
            StringBuilder chuoi = new StringBuilder();
            var tblnews = db.tblNews.Where(p => p.Active == true).OrderByDescending(p => p.Ord).Take(13).ToList();
            for (int i = 0; i < tblnews.Count; i++)
            {
                chuoi.Append("<h4><a href=\"/News/" + tblnews[i].Tag + "-" + tblnews[i].id + ".aspx\" title=\"" + tblnews[i].Name + "\">");
                if (tblnews[i].ViewHomes == true)
                {
                    chuoi.Append("<span class=\"rel\"></span>");
                }
                else
                {
                    chuoi.Append("<span class=\"blue\"></span>");
                }

                chuoi.Append(tblnews[i].Name);
                chuoi.Append("</a></h4>");
            }
            ViewBag.chuoi = chuoi;
            var tblvideo = db.tblVideos.FirstOrDefault();
            return PartialView(tblvideo);
        }

        public PartialViewResult FootterPartial()
        {
            tblConfig tblconfig = db.tblConfigs.Find(1);
            string chinhsach = "";
            var listchinhscach = db.tblNews.Where(p => p.idCate == 8 && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listchinhscach.Count;i++ )
            {
                chinhsach+="<a href=\"/News/"+listchinhscach[i].Tag+"-"+listchinhscach[i].id+".aspx\" title=\""+listchinhscach[i].Name+"\">"+listchinhscach[i].Name+"</a>";
 
            }
            ViewBag.chinhsach = chinhsach;
            string sanpham = "";
            var listProduct = db.tblGroupProducts.Where(p => p.ParentID==null && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listProduct.Count;i++ )
            {
                sanpham += "<h3><a href=\"/ListProduct/" + listProduct[i].Tag + "-" + listProduct[i].id + ".aspx\" title=\"" + listProduct[i].Title + "\">" + listProduct[i].Name + "</a></h3>";

            }
            ViewBag.sanpham = sanpham;
            tblMap tblmap = db.tblMaps.First();
            ViewBag.maps = tblmap.Content;
            var listCapacity = db.tblCapacities.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            string dungtich = "";
            foreach (var item in listCapacity)
            {
                dungtich += "<h3><a href=\"/" + item.Tag + ".html\" title=\"" + item.Title + "\">" + item.Name + "</a></h3>";
            }
            ViewBag.dungtich = dungtich;
            var Imagesadw = db.tblImages.Where(p => p.Active == true && p.idCate == 2).OrderByDescending(p => p.Ord).Take(1).ToList();
            if (Imagesadw.Count > 0)
                ViewBag.Chuoiimg = "<a href=\"" + Imagesadw[0].Url + "\" title=\"" + Imagesadw[0].Name + "\"><img src=\"" + Imagesadw[0].Images + "\" alt=\"" + Imagesadw[0].Name + "\" style=\"max-width:100%;\" /> </a>";
            var listUrl = db.tblUrls.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder resultUrl = new StringBuilder();
            for (int i = 0; i < listUrl.Count; i++)
            {
                resultUrl.Append("<h3 style = \"margin:0px; display:inline-block;   font-weight:normal\" ><a style = \"font-size:12px; margin:0px; color:#FFF\" href = \"" + listUrl[i].Url + "\" title = \"" + listUrl[i].Name + "\" > " + listUrl[i].Name + "</a ></h3 >");
            }
            ViewBag.resultUrl = resultUrl.ToString();
            return PartialView(tblconfig);
        }
        public ActionResult Search(FormCollection collection)
        {
            if (collection["btnSearch"] != null)
            {
                if (collection["txtSearch"]!="")
                {
                    Session["txtSearch"] = collection["txtSearch"];
                    return Redirect("/SearchProduct");
                }
            }
            return RedirectToAction("Index");
        }
        string chuoilevel = "";
        public string GetLevel(int idCate, int level)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.ParentID == idCate).ToList();

            for (int i = 0; i < ListMenu.Count; i++)
            {

                int id = ListMenu[i].id;
                tblGroupProduct tblgroupproduct = db.tblGroupProducts.Find(id);
                tblgroupproduct.Level = level + 1;
                db.SaveChanges();
                var kiemtra = db.tblGroupProducts.Where(p => p.ParentID == id).ToList();

                if (kiemtra.Count > 0)
                {
                    int levels = int.Parse(tblgroupproduct.Level.ToString());
                    GetLevel(id, levels);
                }
            }
            return chuoilevel;
        }
        public ActionResult Action()
        {
            var listgroup = db.tblGroupProducts.Where(p => p.ParentID == null).ToList();
            for (int i = 0; i < listgroup.Count; i++)
            {
                int id = listgroup[i].id;
                tblGroupProduct tblgroupproduct = db.tblGroupProducts.Find(id);
                tblgroupproduct.Level = 0;
                db.SaveChanges();
                var kiemtra = db.tblGroupProducts.Where(p => p.ParentID == id).ToList();
                if (kiemtra.Count > 0)
                {


                    GetLevel(id, 0);
                }
            }
            return View();
        }
    }
}
