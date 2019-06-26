using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Routing;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

using System.Data;
using System.Data.Entity;
using JobsV1.Models;

namespace JobsV1.Models
{
    
    public class SiteMap_carhub
    {
        
        private JobDBContainer db = new JobDBContainer();

        public string GetSitemapDocument(IEnumerable<SitemapNode> sitemapNodes)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");

            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                    sitemapNode.LastModified == null ? null : new XElement(
                        xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    sitemapNode.Frequency == null ? null : new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.Priority == null ? null : new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }

        public List<string> GetItemRoot()
        {
            List<string> items = new List<string>();
            items.Add("Home/Index");
            items.Add("Home/About");
            items.Add("Home/Contact");

            return items;
        }
        /*
        public List<string> GetDestinations()
        {
            var data = db.CarUnitMetas.OrderBy(s => s.Sort).ToList();
            List<string> items = new List<string>();
            foreach (var tmp in data)
            {
                items.Add("TravelPackages/" + tmp.Id + "/" + tmp.Name);
            }
            return items;
        }

        public List<string> GetProducts()
        {
            var data = db.tpProducts.OrderBy(s => s.Sort).ToList();
            List<string> items = new List<string>();
            foreach (var tmp in data)
            {
                items.Add("TourPackages/" + tmp.Id + "/" + tmp.Name);
            }
            return items;

        }

    */

        public List<string> GetItemList()
        {
            List<string> items = new List<string>();
            items.Add("ads/honda-city-automatic/");
            items.Add("ads/rent-a-car-suv-for-rent-davao-city/");
            items.Add("ads/toyota-hiace-gl-grandia/");
            items.Add("ads/toyota-innova-d-4d");
            items.Add("ads/car-for-rent-davao-city/");
            items.Add("ads/rent-a-car-davao-city-self-drive-2/");


            items.Add("ads/minivan-and-sedan-rentals/");
            items.Add("ads/sedan-davao-city-car-rental/");
            items.Add("ads/ford-fiesta-1-6l-sedan-2012/");
            items.Add("ads/innovacar-for-rent-davao-city");
            items.Add("ads/rent-a-car-davao-city-self-drive/");
            items.Add("ads/car-rental-honda-city-for-rent-self-drive/");

            items.Add("ad-category/sedans/");
            items.Add("ads/");
            items.Add("ads/page/1/");
            items.Add("ads/page/2/");
            items.Add("ad-category/others/");
            items.Add("ad-category/vans/");

            items.Add("ad-category/mpv/");
            items.Add("ad-category/pickup");
            items.Add("ad-tag/car-rental-davao/");
            items.Add("ad-tag/davao-rent-a-car/");
            items.Add("ad-tag/rent-a-car-davao-city/");
            items.Add("ad-tag/van-for-rent-davao-city/");

            return items;

        }

        public IReadOnlyCollection<SitemapNode> GetSitemapNodes(string _website)
        {
            List<SitemapNode> nodes = new List<SitemapNode>();

            //root items
            List<string> itemroot = this.GetItemRoot();
            foreach (var item in itemroot)
            {
                nodes.Add(
                    new SitemapNode()
                    {
                        Url = string.Format(_website + "{0}", item),
                        LastModified = System.DateTime.Now,
                        Frequency = SitemapFrequency.Weekly,
                        Priority = 1
                    });
            }

            /*
            //package Areas
            List<string> itemAreas = this.GetDestinations();
            foreach (var item in itemAreas)
            {
                nodes.Add(
                    new SitemapNode()
                    {
                        Url = string.Format(_website + "{0}", item),
                        LastModified = System.DateTime.Now,
                        Frequency = SitemapFrequency.Weekly,
                        Priority = 1
                    });
            }

            //Products
            List<string> itemProduct = this.GetProducts();
            foreach (var item in itemProduct)
            {
                nodes.Add(
                    new SitemapNode()
                    {
                        Url = string.Format(_website + "{0}", item),
                        LastModified = System.DateTime.Now,
                        Frequency = SitemapFrequency.Weekly,
                        Priority = 1
                    });
            }
            */

            //Carlist
            List<string> itemProduct = this.GetItemList();
            foreach (var item in itemProduct)
            {
                nodes.Add(
                    new SitemapNode()
                    {
                        Url = string.Format(_website + "{0}", item),
                        LastModified = System.DateTime.Now,
                        Frequency = SitemapFrequency.Weekly,
                        Priority = 1
                    });
            }
            return nodes;
        }
        
    }

}