using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSCODE.Models;
using System.Globalization;
namespace CMSCODE.Controllers.Display
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
            var Menu = db.tblGroupProducts.Where(p => p.Priority == true && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < Menu.Count;i++ )
            { 
                chuoi+="<div class=\"Product\">";
               chuoi+="<div class=\"nVar\">";
                chuoi+="<div class=\"Left\">";
                chuoi+="<h2> "+Menu[i].Name+"</h2>";
                chuoi+="</div>";
                    chuoi+="<div class=\"Right\">";
                        chuoi+="<select class=\"Arrange\">";
                            chuoi+="<option value=\"0\" selected=\"selected\">Sắp xếp theo giá</option> ";
                            chuoi+="<option value=\"1\">Tăng dần</option>";
                            chuoi+="<option value=\"2\">Giảm dần</option>";
                        chuoi+="</select>";
                   chuoi+= "</div>";
                chuoi+="</div>";
                chuoi+="<div class=\"Content\">";
                string Level = Menu[i].Level;
                var listMenuParent = db.tblGroupProducts.Where(p => p.Level.Substring(0, Level.Length) == Level && p.Active == true).OrderBy(p => p.Ord).ToList();
                for(int x=0;x<listMenuParent.Count;x++)
                {
                    int idMenu = listMenuParent[x].Id;
                    var listProduct=db.tblProducts.Where(p=>p.idCate==idMenu && p.Active==true && p.ViewHomes==true).OrderBy(p=>p.Ord).ToList();
                    if(sx!="" || sx!=null)
                    {
                        if(sx=="1")
                        {

                            listProduct = db.tblProducts.Where(p => p.idCate == idMenu && p.Active == true && p.ViewHomes == true).OrderByDescending(p=>p.Price).ToList();

                        }
                        else
                        {

                            listProduct = db.tblProducts.Where(p => p.idCate == idMenu && p.Active == true && p.ViewHomes == true).OrderBy(p => p.Price).ToList();
                        }
                    }
                    for(int j=0;j<listProduct.Count;j++)
                    {
                          chuoi+= "<div class=\"Tear_1\">";
                        if(listProduct[j].New==true)
                        {
                            chuoi += "<div class=\"New\">";
                            chuoi += "</div>";
                        }
                              
                              chuoi+= "<div class=\"img\">";
                              chuoi += "<a href=\"/Product/"+listProduct[j].Tag+"-"+listProduct[j].idCate+"-"+listProduct[j].id+".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>";
                              chuoi+= "</div>";
                              chuoi += "<a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\">" + listProduct[j].Name + "</a>";
                              chuoi+= "<p class=\"Price\">Giá : <span>"+string.Format("{0:#,#}", listProduct[j].Price)+" đ</span></p>";
                              chuoi+= "<p class=\"PriceSale\"> Khuyến mại : <span>"+string.Format("{0:#,#}", listProduct[j].PriceSale)+" đ</span></p>";
                         chuoi+= " </div> ";

                    }
                }
                  chuoi+= "</div>";
              chuoi+= "</div>";
            
            }
            ViewBag.chuoi = chuoi;
                return View();
        }
        public PartialViewResult SlidePartial()
        {
            var listSlide = db.tblImages.Where(p => p.idMenu == 1 && p.Active == true).OrderBy(p => p.Ord).Take(1).ToList();
            return PartialView(listSlide);
        }
        public PartialViewResult TopPartial(int id=1)
        {
            tblConfig config = db.tblConfigs.Find(id);
            var Menu=db.tblGroupProducts.Where(p=>p.Level.Length==5 && p.Active==true).OrderBy(p=>p.Ord).ToList();
            string Chuoi="";
            if (Session["h1"] != null)
            {
                ViewBag.h1 = Session["h1"];
                Session["h1"] = "";
            }
            for (int i = 0; i < Menu.Count; i++)
            { 
                 Chuoi+="<li class=\"li1\">";
                 Chuoi += " <a href=\"/ListProduct/"+Menu[i].Tag+"-"+Menu[i].Id+".aspx\" title=\"" + Menu[i].Name + "\">" + Menu[i].Name + "</a>";                     
                        string Level=Menu[i].Level;
                        var Menu1=db.tblGroupProducts.Where(p=>p.Level.Length==10 && p.Level.Substring(0,5)==Level && p.Level!=Level && p.Active==true).OrderBy(p=>p.Ord).ToList();
                         if(Menu1.Count>0)
                         {  
                             
                             Chuoi+="<ul class=\"ul2\">";
                             for(int j=0;j<Menu1.Count;j++)
                             {

                                 Chuoi+="<li class=\"li2\">";
                                 Chuoi += "<a href=\"/ListProduct/"+Menu1[j].Tag+"-"+Menu1[j].Id+".aspx\" title=\"" + Menu1[j].Name + "\">" + Menu1[j].Name + "</a>";
                                 string Level1 = Menu1[j].Level;
                                 var Menu2=db.tblGroupProducts.Where(p=>p.Level.Length==15 && p.Level.Substring(0,10)==Level1 && p.Level!=Level1 && p.Active==true).OrderBy(p=>p.Ord).ToList();
                                
                                 if(Menu2.Count>0)
                                 { 
                                     Chuoi+="<ul class=\"ul3\">";
                                    for(int k=0;k<Menu2.Count;k++)
                                    {
                                      Chuoi+="<li class=\"li3\">";
                                      Chuoi += "<a href=\"/ListProduct/" + Menu2[k].Tag + "-" + Menu2[k].Id + ".aspx\" title=\"" + Menu2[k].Name + "\">› " + Menu2[k].Name + "</a>";
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
            var GroupsProducts = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var listpage = new List<SelectListItem>();
            listpage.Clear();
            listpage.AddRange(GroupsProducts.Select(t => new SelectListItem { Text = "" + StringClass.ShowNameLevel(t.Name, t.Level), Value = "/danh-muc-san-pham/" + t.Tag.ToString(CultureInfo.InvariantCulture) }));
            var menuModel = db.tblGroupProducts.OrderBy(m => m.Level).ToList();
            var lstMenu = new List<SelectListItem>();
            lstMenu.Clear();
            foreach (var menu in menuModel)
            {
                lstMenu.Add(new SelectListItem { Text = StringClass.ShowNameLevel(menu.Name, menu.Level), Value = menu.Id.ToString() });
            }
            ViewBag.one = lstMenu;

          return PartialView(config);

        }
        [HttpPost]
        public ActionResult Dropdownlist(int id)
        {
            var listProduct = db.tblGroupProducts.Find(id);

            return Redirect("/ListProduct/" + listProduct.Tag + "." + id + ".aspx");
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
            var listProduct = db.tblGroupProducts.Where(p => p.Level.Length == 5 && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listProduct.Count;i++ )
            {
                sanpham += "<a href=\"/ListProduct/" + listProduct[i].Tag + "-" + listProduct[i].Id + ".aspx\" title=\"" + listProduct[i].Name + "\">" + listProduct[i].Name + "</a>";

            }
            ViewBag.sanpham = sanpham;
            tblMap tblmap = db.tblMaps.First();
            ViewBag.maps = tblmap.Content;
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
    }
}
