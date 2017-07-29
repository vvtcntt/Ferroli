using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSCODE.Models;
namespace CMSCODE.Controllers.Display.Left
{
    public class LeftController : Controller
    {
        //
        // GET: /Left/
        private FerroliContext db = new FerroliContext();

        public ActionResult Index()
        {
         
                return View();
        }
        public PartialViewResult SupportPartial()
        {
            tblConfig tblconfig = db.tblConfigs.Find(1);
            string chuoisuport = "";
            var listSupport = db.tblSupports.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            chuoisuport+=" <div id=\"Support\">";
            chuoisuport+="<div id=\"Top_Support\"> <span>Hỗ trợ trực tuyến</span></div>";
            chuoisuport+="<div id=\"Bottom_Support\">";
            for (int i = 0; i < listSupport.Count; i++)
            {
                chuoisuport += "<div class=\"Tear_Support\">";
                chuoisuport += "<div class=\"Left\">";
                chuoisuport += "<span class=\"sp1\">" + listSupport[i].Mission + " :</span>";
                chuoisuport += "<span class=\"sp2\">" + listSupport[i].Name + " :</span>";
                chuoisuport += "</div>";
                chuoisuport += "<div class=\"Right\">";
                chuoisuport += "<div class=\"Yahoo\">";
                chuoisuport += "<a href=\"ymsgr:sendim?" + listSupport[i].Yahoo + "\">";
                chuoisuport += "<img src=\"http://opi.yahoo.com/online?u=" + listSupport[i].Yahoo + "&m=g&t=1\" alt=\"Yahoo\" class=\"imgYahoo\" />";
                chuoisuport += " </a>";
                chuoisuport += "</div>";
                chuoisuport += " <div class=\"Skype\">";
                chuoisuport += "<a href=\"Skype:" + listSupport[i].Skyper + "?chat\">";
                chuoisuport += "<img class=\"imgSkyper\" src=\"/Content/Display/iCon/skype-icon.png\" title=\"Kangaroo\" alt=\"Skyper\">";
                chuoisuport += "</a>";
                chuoisuport += "</div>";
                chuoisuport += "</div>";
                chuoisuport += " </div>";

            }
            chuoisuport+="</div><div class=\"Clear\"></div> </div>";
            ViewBag.chuoi = chuoisuport;
            return PartialView(tblconfig);

        }
        public PartialViewResult LeftContent()
        {
            return PartialView();
        }

        public PartialViewResult PartialboxNews()
        {
            var listnew = db.tblNews.Where(p => p.Active == true).OrderByDescending(p => p.Ord).Take(8).ToList();
            return PartialView(listnew);
        }
         public PartialViewResult MenuProductPartial()
        {
            string chuoi = "";
            var Menu = db.tblGroupProducts.Where(p => p.Level.Length == 5 && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < Menu.Count;i++ )
            { 
                chuoi+="<li class=\"li_1\">";
                chuoi+="<div class=\"MenuName\">";
                chuoi += "<a href=\"/ListProduct/" + Menu[i].Tag + "-" + Menu[i].Id + ".aspx\" title=\"" + Menu[i].Name + "\">" + Menu[i].Name + "</a>";
                chuoi+="</div>"; 
                string level1=Menu[i].Level;
                var Menu1=db.tblGroupProducts.Where(p=>p.Active==true && p.Level.Length==10 && p.Level.Substring(0,5)==level1 && p.Level!=level1).OrderBy(p=>p.Ord).ToList();
                if(Menu1.Count>0)
                { 
                    
                 chuoi+="<ul class=\"ul_2\">";
                    for(int j=0;j<Menu1.Count;j++)
                    {
                 chuoi+="<li class=\"li_2\">";
                 chuoi += "<a href=\"/ListProduct/" + Menu1[j].Tag + "-" + Menu1[j].Id + ".aspx\" title=\"" + Menu1[j].Name + "\">› " + Menu1[j].Name + "</a>";
                        string level2=Menu1[j].Level;
                        var Menu2=db.tblGroupProducts.Where(p=>p.Active==true && p.Level.Length==15 && p.Level.Substring(0,10)==level2 && p.Level!=level2).OrderBy(p=>p.Ord).ToList();
                        if(Menu2.Count>0)
                        { 
                         chuoi+="<ul class=\"ul_3\">";
                            for(int k=0;k<Menu2.Count;k++)
                            {
                           chuoi+="<li class=\"li_3\">";
                           chuoi += "<a href=\"/ListProduct/" + Menu2[k].Tag + "-" + Menu2[k].Id + ".aspx\" title=\"" + Menu1[j].Name + "\">&bull; " + Menu2[k].Name + "</a>";
                           chuoi+="</li>";
                                }
                            
                       chuoi+="</ul>";
                      }  
                   chuoi+="</li>";
                  }
                   chuoi+="</ul>";
                   
                }
                chuoi += "</li>";
            
            }
            ViewBag.Menu = chuoi;
                return PartialView();
        }
        public PartialViewResult UrlPartial()
         {
            string chuoi="";
             var tblUrl = db.tblUrls.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
             for (int i = 0; i < tblUrl.Count;i++ )
             {
                 chuoi += "<a href=\""+tblUrl[i].Url+"\" title=\""+tblUrl[i].Name+"\" rel=\""+tblUrl[i].Rel+"\"> - "+tblUrl[i].Name+"</a>";


             }
             ViewBag.chuoi = chuoi;
                 return PartialView();
         }
        public PartialViewResult LeftNewsPartail()
         {
             string chuoi = "";
             var listMenunew = db.tblGroupNews.Where(p => p.Active == true && p.Level.Length==5).OrderBy(p => p.Ord).ToList();
             for (int i = 0; i < listMenunew.Count;i++ )
             {
                 	chuoi+="<li class=\"li_n\">";
                    	chuoi+="<a href=\"/ListNew/"+listMenunew[i].Tag+"-"+listMenunew[i].id+".aspx\" title=\""+listMenunew[i].Name+"\">"+listMenunew[i].Name+"</a>";
                        string level=listMenunew[i].Level;
                        var listMenunewschild=db.tblGroupNews.Where(p=>p.Level.Substring(0,level.Length)==level && p.Level!=level && p.Active==true).OrderBy(p=>p.Ord).ToList();
                 if(listMenunewschild.Count>0)
                 {
                   
                        chuoi+="<ul class=\"ul_n1\">";
                       for(int j=0;j<listMenunewschild.Count;j++)
                            { 
                        	 chuoi+="<li class=\"li_n1\">";
                             chuoi += "<a href=\"/ListNew/" + listMenunewschild[j].Tag + "-" + listMenunewschild[j].id + ".aspx\" title=\"" + listMenunewschild[j].Name + "\">› " + listMenunewschild[j].Name + "</a>";
                             chuoi+="</li>";
                       }
                        chuoi+="</ul>";
                 }
                    chuoi+="</li>";

             }
             ViewBag.chuoi = chuoi;
             string chuoiNPP = "";
             var GroupManufacturers = db.tblGroupManufacturers.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
             for (int j = 0; j < GroupManufacturers.Count; j++)
             {

                 chuoiNPP += "<ul class=\"ul_n1\">";

                 chuoiNPP += "<li class=\"li_n1\">";
                 chuoiNPP += "<a href=\"/9/" + GroupManufacturers[j].Tag + "-" + GroupManufacturers[j].id + ".aspx\" title=\"" + GroupManufacturers[j].Name + "\">› " + GroupManufacturers[j].Name + "</a>";
                 chuoiNPP += "</li>";

                 chuoiNPP += "</ul>";
                 

             }
             ViewBag.chuoiNPP = chuoiNPP;
                 return PartialView();
        }
    }
}
