using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class WallPostThumbs : AbstractElementParser
    {
        private WallPostThumbs(IWebElement webElement) : base(webElement)
        {
            this.ImageUrl = this.GetImageUrl();
        }

        public List<string> ImageUrl { get; }

        public static WallPostThumbs Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new WallPostThumbs(elem);
        }

        private List<string> GetImageUrl()
        {
            if (this.Element == null) return new List<string>();
            ReadOnlyCollection<IWebElement> hrefs = this.Element.FindElements(By.TagName("a"));
            return hrefs.Select(h => GetUrl(h)).ToList();
        }

        private string GetUrl(IWebElement href)
        {
            var style = href.GetAttribute("style");
            int start = style.LastIndexOf("https://", StringComparison.OrdinalIgnoreCase);
            if (start == -1) return "";
            int stop = style.LastIndexOf(");");
            int count = stop - start;
            return style.Substring(start, count);
        }

        public static string ClassName { get { return ".page_post_sized_thumbs.clear_fix"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
            {
                var elements = root.FindElements(By.TagName("div"))
                    .Where(e => e.GetAttribute("class") != null
                    && e.GetAttribute("class").Equals("page_post_sized_thumbs  clear_fix"))
                    .ToList();
                if (elements.Any()) return elements.First();
            }

            return null;
        }
    }
}
