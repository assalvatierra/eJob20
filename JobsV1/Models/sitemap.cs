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
    public class SitemapNode
    {
        public SitemapFrequency? Frequency { get; set; }
        public DateTime? LastModified { get; set; }
        public double? Priority { get; set; }
        public string Url { get; set; }
    }

    public enum SitemapFrequency
    {
        Never,
        Yearly,
        Monthly,
        Weekly,
        Daily,
        Hourly,
        Always
    }

    public class SiteMap
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
            items.Add("realwheelsdavao.com/CarRental/Index");
            items.Add("realwheelsdavao.com/CarRental/About");
            items.Add("realwheelsdavao.com/CarRental/Contact");

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

        public List<string> GetCarList()
        {
            List<string> items = new List<string>();
                    items.Add("realwheelsdavao.com/CarRental/van-for-rent");
                    items.Add("realwheelsdavao.com/CarRental/NissanUrvanPremium-for-rent");
                    items.Add("realwheelsdavao.com/CarRental/suvpickup4x4-rental-rates");
                    items.Add("realwheelsdavao.com/CarRental/toyota-innova-for-rent");
                    items.Add("realwheelsdavao.com/CarRental/sedan-rental");
                    items.Add("realwheelsdavao.com/CarRental/pickup-rental");
            
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
            List<string> itemProduct = this.GetCarList();
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