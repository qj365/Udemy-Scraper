using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Udemy_Scraper.Models;
using Udemy_Scraper.ViewModel;

namespace Udemy_Scraper.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(GrabLink());
        }

        public List<UdemyLink> GrabLink()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("https://www.discudemy.com/category");
            var threadItems = document.DocumentNode.QuerySelectorAll(".ui.text.container > a").ToList();
            var firstLinks = new List<FirstLink>();
            foreach (var item in threadItems)
            {
                var firstLink = new FirstLink();
                firstLink.link = item.Attributes["href"].Value;
                firstLinks.Add(firstLink);
            }

            //second Link
            var secondLinks = new List<FirstLink>();
            foreach (var item in firstLinks)
            {
                HtmlDocument doc = web.Load(item.link);
                var secondNode = doc.DocumentNode.QuerySelector(".ui.big.inverted.green.button.discBtn");
                var secondLink = new FirstLink();
                secondLink.link = secondNode.Attributes["href"].Value;

                var imageNode = doc.DocumentNode.QuerySelector("amp-img");
                secondLink.image = imageNode.Attributes["src"].Value;
                secondLinks.Add(secondLink);
            }

            var udemyList = new List<UdemyLink>();
            //udemy link
            foreach (var item in secondLinks)
            {
                HtmlDocument udemy = web.Load(item.link);
                var udemyLinkNode = udemy.DocumentNode.QuerySelector(".ui.segment > a");
                var udemyNameNode = udemy.DocumentNode.QuerySelector("h1");
                var udemyLink = new UdemyLink();

                udemyLink.name = udemyNameNode.InnerText;
                udemyLink.image = item.image;
                udemyLink.link = udemyLinkNode.Attributes["href"].Value;

                udemyList.Add(udemyLink);

            }
            return udemyList;
        }


    }
}