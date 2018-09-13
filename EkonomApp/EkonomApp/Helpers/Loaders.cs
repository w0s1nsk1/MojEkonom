using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EkonomApp.Helpers
{
    public class Loader
    {
        public string url { get; set; }
        public string xpath { get; set; }
        public HtmlNodeCollection GetNodes()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument html = web.Load(url);
                return html.DocumentNode.SelectNodes(xpath);
            }
            catch
            {
                return null;
            }
        }
        public HtmlNode GetNode()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument html = web.Load(url);
                return html.DocumentNode.SelectSingleNode(xpath);
            }
            catch
            {
                return null;
            }
        }
    }
}
