using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSCODE.Models;
namespace CMSCODE.Controllers.Display.Products
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/
        private FerroliContext db = new FerroliContext();

        public ActionResult Index()
        {
            return View();
        }
        
        public PartialViewResult ProductDetail(string tag)
        {
            int idp;
            string Chuoi = tag;
            string[] Mang = Chuoi.Split('-');
            int one = int.Parse(Mang.Length.ToString());
            string chuoi1 = Mang[one - 1].ToString();
            string[] Mang1 = chuoi1.Split('.');
            idp = int.Parse(Mang1[0].ToString());
            tblProduct product = db.tblProducts.Find(idp);
            ViewBag.Title = "<title>" + product.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + product.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + product.Keyword + "\" /> ";
            int idMenu=int.Parse(product.idCate.ToString());
            string splq = "";
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idMenu && p.id != idp).OrderBy(p => p.Ord).ToList(); 
            for (int i = 0; i < listProduct.Count;i++ )
            {

               splq+=" <div class=\"Tear_5\">";
                            splq+="  <div class=\"Top_Tear5\">";
                                 splq+=" <a href=\"/Product/"+listProduct[i].Tag+"-"+listProduct[i].idCate+"-"+listProduct[i].id+".aspx\" title=\"\">";
                                     splq+=" <img src=\""+listProduct[i].ImageLinkThumb+"\" alt=\""+listProduct[i].Name+"\" />";
                                 splq+=" </a>";
                             splq+=" </div>";
                             splq += " <a href=\"/Product/" + listProduct[i].Tag + "-" + listProduct[i].idCate + "-" + listProduct[i].id + ".aspx\" title=\""+listProduct[i].Name+"\">"+listProduct[i].Name+"</a>";
                         splq+=" </div>";
            }
            ViewBag.splq = splq;
            string nUrl = "";
            var GroupProduct = db.tblGroupProducts.First(p => p.Id == idMenu);
            int dodai = GroupProduct.Level.Length / 5;
            for (int i = 0; i < dodai; i++)
            {
                int  leht = GroupProduct.Level.Substring(0, (i + 1) * 5).Length;
                var NameGroups = db.tblGroupProducts.First(p => p.Level.Substring(0, (i + 1) * 5) == GroupProduct.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
                nUrl = nUrl + " <a href=\"/ListProduct/" + NameGroups.Tag + "-"+NameGroups.Id+".aspx\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + "</a> / ";
            }
            ViewBag.nUrl = "<a href=\"http://binhnonglanhferroli.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> &raquo;" + nUrl + " " + product.Name; 
            return PartialView(product);
        }
        public ActionResult Command(FormCollection collection,string tag)
        {
            if (collection["btnOrder"] != null)
            {

                Session["idProduct"] = collection["idPro"];
              Session["idMenu"] = collection["idCate"];
              Session["OrdProduct"] = collection["txtOrd"];
              Session["Url"] = Request.Url.ToString();
              return RedirectToAction("OrderIndex", "Order");            


            }
            return View();
        }
        public ActionResult SearchProduct()
        {
            if(Session["txtSearch"]!=""&& Session["txtSearch"]!=null)
            {
                string Name = Session["txtSearch"].ToString();
                string chuoi="";
                var groupPproduct = db.tblProducts.Where(p => p.Name.Contains(Name)).ToList();
                ViewBag.Title = "<title>" + Name + "</title>";
                ViewBag.Description = "<meta name=\"description\" content=\"" + Name + "\"/>";
                ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + Name + "\" /> ";
                Session["txtSearch"] = "";
                Session["txtSearch"] = "";
                ViewBag.chuoi = Name;
            return View(groupPproduct);
             
            }
            return View();

        }
        public ActionResult ListProduct(string tag)
        {
            string chuoi = "";
            int idCate;
            string Chuoi = tag;
            string[] Mang = Chuoi.Split('-');
            int one = int.Parse(Mang.Length.ToString());
            string chuoi1 = Mang[one - 1].ToString();
            string[] Mang1 = chuoi1.Split('.');
            idCate = int.Parse(Mang1[0].ToString());
            tblGroupProduct groupPproduct = db.tblGroupProducts.Find(idCate);
            string levels = groupPproduct.Level;
            int leghts = levels.Length;
            ViewBag.Title = "<title>" + groupPproduct.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupPproduct.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupPproduct.Keyword + "\" /> ";
            var ListGroup = db.tblGroupProducts.Where(p => p.Level.Substring(0, leghts) == levels && p.Active == true).OrderBy(p => p.Level).ToList();
            if(ListGroup.Count<2)
            { 
                chuoi += "<div class=\"Product\">";
                chuoi += "<div class=\"nVar\">";
                chuoi += "<div class=\"Left\">";
                chuoi += "<h1> " + ListGroup[0].Name + "</h1>";
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


                string Level = ListGroup[0].Level;
                var listMenuParent = db.tblGroupProducts.Where(p => p.Level.Substring(0, Level.Length) == Level && p.Active == true).OrderBy(p => p.Ord).ToList();
                for (int x = 0; x < listMenuParent.Count; x++)
                {
                    int idMenu = listMenuParent[x].Id;
                    var listProduct = db.tblProducts.Where(p => p.idCate == idMenu && p.Active == true).OrderBy(p => p.Ord).ToList();
               
                    for (int j = 0; j < listProduct.Count; j++)
                    {
                        chuoi += "<div class=\"Tear_1\">";
                        if (listProduct[j].New == true)
                        {
                            chuoi += "<div class=\"New\">";
                            chuoi += "</div>";
                        }

                        chuoi += "<div class=\"img\">";
                        chuoi += "<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>";
                        chuoi += "</div>";
                        chuoi += "<a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"\">" + listProduct[j].Name + "</a>";
                        chuoi += "<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", listProduct[j].Price) + " đ</span></p>";
                        chuoi += "<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " đ</span></p>";
                        chuoi += " </div> ";

                    }
                }
                chuoi += "</div>";
                chuoi += "</div>";
            }
            else
            {
                 for (int i = 0; i < ListGroup.Count;i++ )
                {
                    if (ListGroup[i].Id != idCate)
                    {
                        chuoi += "<div class=\"Product\">";
                        chuoi += "<div class=\"nVar\">";
                        chuoi += "<div class=\"Left\">";
                        chuoi += "<h2> " + ListGroup[i].Name + "</h2>";
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
                        string Level = ListGroup[i].Level;

                        var listMenuParent = db.tblGroupProducts.Where(p => p.Level.Substring(0, Level.Length) == Level && p.Active == true && p.Level != levels).OrderBy(p => p.Ord).ToList();
                        for (int x = 0; x < listMenuParent.Count; x++)
                        {
                            int idMenu = listMenuParent[x].Id;
                            var listProduct = db.tblProducts.Where(p => p.idCate == idMenu && p.Active == true && p.idCate != idCate).OrderBy(p => p.Ord).ToList();

                            for (int j = 0; j < listProduct.Count; j++)
                            {
                                chuoi += "<div class=\"Tear_1\">";
                                if (listProduct[j].New == true)
                                {
                                    chuoi += "<div class=\"New\">";
                                    chuoi += "</div>";
                                }

                                chuoi += "<div class=\"img\">";
                                chuoi += "<a href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"" + listProduct[j].Name + "\"><img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" /></a>";
                                chuoi += "</div>";
                                chuoi += "<a class=\"Name\" href=\"/Product/" + listProduct[j].Tag + "-" + listProduct[j].idCate + "-" + listProduct[j].id + ".aspx\" title=\"\">" + listProduct[j].Name + "</a>";
                                chuoi += "<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", listProduct[j].Price) + " đ</span></p>";
                                chuoi += "<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", listProduct[j].PriceSale) + " đ</span></p>";
                                chuoi += " </div> ";

                            }
                        }
                    }
                    else
                    { 
                chuoi += "<div class=\"Product\">";
                chuoi += "<div class=\"nVar\">";
                chuoi += "<div class=\"Left\">";
                chuoi += "<h1> " + ListGroup[i].Name + "</h1>";
                chuoi += "</div>";
                
                chuoi += "</div>";
                chuoi += "<div class=\"Content\">";
                    }
                
               
                chuoi += "</div>";
                chuoi += "</div>";

                }
            }
            ViewBag.chuoi = chuoi;

            //URL
            string nUrl = "";
            int dodai = groupPproduct.Level.Length / 5;
            for (int i = 0; i < dodai; i++)
            {
                var NameGroups = db.tblGroupProducts.First(p => p.Level.Substring(0, (i + 1) * 5) == groupPproduct.Level.Substring(0, (i + 1) * 5) && p.Level.Length==(i+1)*5);
                string id = NameGroups.Id.ToString();
                string levals = groupPproduct.Level.Substring(0, (i + 1) * 5);
                nUrl = nUrl + " <a href=\"/ListProduct/" + NameGroups.Tag + "-"+NameGroups.Id+".aspx\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + "</a> /";
            }
            ViewBag.nUrl = "<a href=\"http://Binhnonglanhferroli.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> /" + nUrl;
            return View();


        }
    }
}
