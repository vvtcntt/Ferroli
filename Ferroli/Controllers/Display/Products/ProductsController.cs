﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ferroli.Models;
using System.Text;
namespace Ferroli.Controllers.Display.Products
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
        string nUrl = "";
        public string UrlNews(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <li itemprop=\"itemListElement\" itemscope itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"/ListProduct/" + ListMenu[i].Tag + "-" + ListMenu[i].id + ".aspx\"> <span itemprop=\"name\">" + ListMenu[i].Name + "</span></a> <meta itemprop=\"position\" content=\"" + (ListMenu[i].Level + 2) + "\" /> </li> › " + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlNews(id);
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
        public ActionResult ProductDetail(string tag,string id)
        {
            int nId = int.Parse(id);

            tblProduct product = db.tblProducts.First(p => p.id==nId);
            int idp = product.id;
            string ntag = product.Tag;
            if (ntag != tag)
            {
                return Redirect("/Product/" + ntag + "-" + product.idCate + "-" + id + ".aspx");
            }
            ViewBag.Title = "<title>" + product.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + product.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + product.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + product.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Binhnonglanhferroli.vn/Product/" + StringClass.NameToTag(product.Tag) + "-" + product.idCate + "-" + product.id + ".aspx\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + product.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + product.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"" + product.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + product.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"" + product.ImageLinkThumb + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Binhnonglanhferroli.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + product.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
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
          
            var GroupProduct = db.tblGroupProducts.First(p => p.id == idMenu);
            int idCate = int.Parse(product.idCate.ToString());
            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Binhnonglanhferroli.vn\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlNews(idCate) + "</ol> <div itemscope itemtype=\"http://schema.org/Product\"><h1 itemprop=\"name\">" + product.Title + "</h1>  </div>";
            var ListGroupCri = db.tblGroupCriterias.Where(p => p.idCate == idCate).ToList();
            List<int> Mangs = new List<int>();
            for (int i = 0; i < ListGroupCri.Count; i++)
            {
                Mangs.Add(int.Parse(ListGroupCri[i].idCri.ToString()));
            }
            var ListCri = db.tblCriterias.Where(p => Mangs.Contains(p.id) && p.Active == true).ToList();
            string chuoi = "";
            #region[Lọc thuộc tính]


            for (int i = 0; i < ListCri.Count; i++)
            {
                int idCre = int.Parse(ListCri[i].id.ToString());
                var ListCr = (from a in db.tblConnectCriterias
                              join b in db.tblInfoCriterias on a.idCre equals b.id
                              where a.idpd == idp && b.idCri == idCre && b.Active == true
                              select new
                              {
                                  b.Name,
                                  b.Url,
                                  b.Ord
                              }).OrderBy(p => p.Ord).ToList();
                if (ListCr.Count > 0)
                {
                    chuoi += "<tr>";
                    chuoi += "<td>" + ListCri[i].Name + "</td>";
                    chuoi += "<td>";
                    int dem = 0;
                    string num = "";
                    if (ListCr.Count > 1)
                        num = "⊹ ";
                    foreach (var item in ListCr)
                        if (item.Url != null && item.Url != "")
                        {
                            chuoi += "<a href=\"" + item.Url + "\" title=\"" + item.Name + "\">";
                            if (dem == 1)
                                chuoi += num + item.Name;
                            else
                                chuoi += num + item.Name;
                            dem = 1;
                            chuoi += "</a>";
                        }
                        else
                        {
                            if (dem == 1)
                                chuoi += num + item.Name + "</br> ";
                            else
                                chuoi += num + item.Name + "</br> "; ;
                            dem = 1;
                        }
                    chuoi += "</td>";
                    chuoi += " </tr>";
                }
            }
            #endregion
            ViewBag.chuoi = chuoi;
            if (product.Capacity.ToString() != null & product.Capacity.ToString() != "")
            {
                int idcap = int.Parse(product.Capacity.ToString());
                var tblcapacity = db.tblCapacities.Find(idcap);
                ViewBag.capa = "<h3><a href=\"/" + tblcapacity.Tag + ".html\" title=\"" + tblcapacity.Name + "\">" + tblcapacity.Capacity + "</a><h3>";
                ViewBag.songuoisd = tblcapacity.Note;
                ViewBag.cap1 = tblcapacity.Capacity;
            }
            if (product.Capacity.ToString() != null && product.Capacity.ToString() != "")
            {
                int idcap = int.Parse(product.Capacity.ToString());
                var listproduct = db.tblProducts.Where(p => p.Active == true && p.Capacity == idcap && p.id != idp).OrderBy(p => p.PriceSale).ToList();
                string spk = "";
                foreach (var item in listproduct)
                {
                    spk += "<div class=\"Tear_1\">";
                    if (item.New == true)
                    {
                        spk += "<div class=\"New\">";
                        spk += "</div>";
                    }

                    spk += "<div class=\"img\">";
                    spk += "<a href=\"/Product/" + item.Tag + "-" + item.idCate + "-" + item.id + ".aspx\" title=\"" + item.Name + "\"><img src=\"" + item.ImageLinkThumb + "\" alt=\"" + item.Name + "\" /></a>";
                    spk += "</div>";
                    spk += "<h3><a class=\"Name\" href=\"/Product/" + item.Tag + "-" + item.idCate + "-" + item.id + ".aspx\" title=\"\">" + item.Name + "</a></h3>";
                    spk += "<p class=\"Price\">Giá : <span>" + string.Format("{0:#,#}", item.Price) + " đ</span></p>";
                    spk += "<p class=\"PriceSale\"> Khuyến mại : <span>" + string.Format("{0:#,#}", item.PriceSale) + " đ</span></p>";
                    spk += " </div> ";
                }
                ViewBag.spk = spk;
            }
            string resultImage = "";
            var listImages = db.tblImageProducts.Where(p => p.idProduct == idp).ToList();
            for (int i = 0; i < listImages.Count; i++)
            {
                resultImage += "<li><img src=\"" + listImages[i].Images + "\" alt=\"" + product.Name + "\"></li>";
            }
            ViewBag.resultImages = resultImage;
            string address = product.Address.ToString();

            string resultAddress = "";
            if (address != null && address != "")
            {
                int idaddress = int.Parse(address);
                if (db.tblAddresses.FirstOrDefault(p => p.id == idaddress) != null)
                    resultAddress = db.tblAddresses.FirstOrDefault(p => p.id == idaddress).Name;
            }
            ViewBag.address = resultAddress;
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
        public ActionResult ListProduct(string tag, string id)
        {
            string chuoi = "";
            int nId = int.Parse(id);
            tblGroupProduct groupPproduct = db.tblGroupProducts.First(p => p.id==nId);
            int idCate = groupPproduct.id;
            ViewBag.Content = groupPproduct.Content; 
            ViewBag.Title = "<title>" + groupPproduct.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + groupPproduct.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupPproduct.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupPproduct.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Binhnonglanhferroli.vn/ListProduct/" + StringClass.NameToTag(groupPproduct.Tag) + "-" + groupPproduct.id + ".aspx\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + groupPproduct.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + groupPproduct.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"" + groupPproduct.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + groupPproduct.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"" + groupPproduct.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Binhnonglanhferroli.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + groupPproduct.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
           
            var ListGroup = db.tblGroupProducts.Where(p => p.ParentID == idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
            if(ListGroup.Count==0)
            { 
                chuoi += "<div class=\"Product\">";
                chuoi += "<div class=\"nVar\">";
                chuoi += "<div class=\"Left\">";
                chuoi += "<span> " + groupPproduct.Name + "</span>";
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


                List<string> Mangpd = new List<string>();
                Mangpd = Arrayid(idCate);
                Mangpd.Add(idCate.ToString());
                
                 
                    var listProduct = db.tblProducts.Where(p => Mangpd.Contains(p.idCate.ToString()) && p.Active == true).OrderBy(p => p.Ord).ToList();
               
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
                
                chuoi += "</div>";
                chuoi += "</div>"; Mangphantu.Clear();
            }
            else
            {
                 for (int i = 0; i < ListGroup.Count;i++ )
                {
                    if (ListGroup[i].id != idCate)
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
                        List<string> Mangpd = new List<string>();
                        int idcates = ListGroup[i].id;
                        Mangpd = Arrayid(idcates);
                        Mangpd.Add(idcates.ToString());
                        var listProduct = db.tblProducts.Where(p => Mangpd.Contains(p.idCate.ToString()) && p.Active == true && p.idCate != idCate).OrderBy(p => p.Ord).ToList();

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
                Mangphantu.Clear();
                }
            }
            ViewBag.chuoi = chuoi;


            ViewBag.nUrl = "<ol itemscope itemtype=\"http://schema.org/BreadcrumbList\">   <li itemprop=\"itemListElement\" itemscope  itemtype=\"http://schema.org/ListItem\"> <a itemprop=\"item\" href=\"http://Binhnonglanhferroli.vn\">  <span itemprop=\"name\">Trang chủ</span></a> <meta itemprop=\"position\" content=\"1\" />  </li>   ›" + UrlNews(groupPproduct.id) + "</ol> <h1>" + groupPproduct.Title + "</h1>";
            return View();


        }
        public ActionResult ListCapacity(string tag)
        {
            string chuoi = "";
            var tblcapacity = db.tblCapacities.First(p => p.Tag == tag);
            int idcap = tblcapacity.id;
            ViewBag.Title = "<title>" + tblcapacity.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblcapacity.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblcapacity.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblcapacity.Keyword + "\" /> ";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Binhnonglanhferroli.vn/" + StringClass.NameToTag(tblcapacity.Tag) + ".html\" />";
            string meta = "";
            meta += "<meta itemprop=\"name\" content=\"" + tblcapacity.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblcapacity.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"" + tblcapacity.Images + "\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblcapacity.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"" + tblcapacity.Images + "\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://Binhnonglanhferroli.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblcapacity.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta;
            chuoi += "<div class=\"Product\">";
            chuoi += "<div class=\"nVar\">";
            chuoi += "<div class=\"Left\">";
            chuoi += "<span>" + tblcapacity.Name + "</span>";
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
            List<string> Mangpd = new List<string>();
            var listProduct = db.tblProducts.Where(p => p.Capacity == idcap && p.Active == true).OrderBy(p => p.Ord).ToList();
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
            chuoi += "</div>";
            chuoi += "</div>";
            ViewBag.chuoi = chuoi;
            ViewBag.nUrl = "<a href=\"http://Binhnonglanhferroli.vn\" title=\"Trang chu\" rel=\"nofollow\"><span class=\"iCon\"></span>Trang chủ</a> / <h1>"+tblcapacity.Title+"</h1>";
            ViewBag.Content = tblcapacity.Content;             
            return View();
        }
    }
}
