﻿using System;
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
            items.Add("realwheelsdavao.com/");
            items.Add("realwheelsdavao.com/CarRental/Index");
            items.Add("realwheelsdavao.com/CarRental/About");
            items.Add("realwheelsdavao.com/CarRental/Contact");
            items.Add("realwheelsdavao.com/Articles/WhyBook");
            items.Add("realwheelsdavao.com/CarRental/FAQ");
            items.Add("realwheelsdavao.com/CarRental/PriceList");
            items.Add("realwheelsdavao.com/CarRental/Featured");
            items.Add("realwheelsdavao.com/CarRental/Reservation");
            items.Add("realwheelsdavao.com/CarRental/PriceQuote");
            items.Add("realwheelsdavao.com/CarRental/BookingGuide");
            items.Add("realwheelsdavao.com/CarRental/Car-Rental-Services");

            //articles
            items.Add("realwheelsdavao.com/Articles/NewSeatCapacity");
            items.Add("realwheelsdavao.com/Articles/VisitDavao");
            items.Add("realwheelsdavao.com/Articles/SUVRental");
            items.Add("realwheelsdavao.com/Articles/mindanao-qr-code-list-2021");
            items.Add("realwheelsdavao.com/Articles/Safe-Car-Rental-2021");

            //services
            items.Add("realwheelsdavao.com/cargo-delivery-services");
            items.Add("realwheelsdavao.com/shuttle-service");

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
                    items.Add("realwheelsdavao.com/CarRental/ToyotaTourer-for-rent");

            //ads

            //items.Add("realwheelsdavao.com/ads/toyota-avanza-1-3e-at/");
            //items.Add("realwheelsdavao.com/ads/toyota-hiace-gl-grandia/");
            //items.Add("realwheelsdavao.com/ads/4x4-rental-suv-for-rent-davao/");
            //items.Add("realwheelsdavao.com/ads/rent-a-car-davao-city-self-drive/");
            //items.Add("realwheelsdavao.com/ads/rent-a-car-suv-for-rent-davao-city/");
            //items.Add("realwheelsdavao.com/ad-tag/rent-a-car-davao-city/");
            //items.Add("realwheelsdavao.com/ad-tag/car-rental-davao/");
            //items.Add("realwheelsdavao.com/ads/honda-city-automatic/");
            //items.Add("realwheelsdavao.com/ad-tag/davao-rent-a-car/");
            //items.Add("realwheelsdavao.com/ad-tag/van-for-rent-davao-city/");
            //items.Add("realwheelsdavao.com/ads/");
            //items.Add("realwheelsdavao.com/ad-category/others/");
            //items.Add("realwheelsdavao.com/ad-category/vans/");
            //items.Add("realwheelsdavao.com/ad-category/sedans/");
            //items.Add("realwheelsdavao.com/ads/innovacar-for-rent-davao-city/");
            //items.Add("realwheelsdavao.com/ads/page/2/");
            //items.Add("realwheelsdavao.com/ads/4x4-rental-pickup-for-rent-davao/");
            //items.Add("realwheelsdavao.com/ads/page/1/");

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