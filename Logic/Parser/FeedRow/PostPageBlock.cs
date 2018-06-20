using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class PostPageBlock: AbstractElementParser
    {
        private PostPageBlock(IWebElement webElement): base(webElement)
        {   
            this.Content = PostContent.Create(this.Element);
            this.Id = this.Element.GetAttribute("id");
        }

        public string Id { get; private set; }

        public List<string> AllLinks
        {
            get
            {
                List<string> result = new List<string>();
                this.GetAllLinks(this.Element, result);
                return result;
            }
        }

        public PostContent Content { get; private set; }       

        public static PostPageBlock Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new PostPageBlock(elem);
        }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
            {
                var elements = root.FindElements(By.TagName("div")).Where(e => CheckAttr(e, "class", "ads_feed_placeholder")).ToList();
                if (elements.Any()) return null;

                elements = root.FindElements(By.TagName("div"))
                    .Where(e => CheckAttr(e, "class", "_post post page_block post_copy")).ToList();
                if (elements.Any()) return elements.First();

                elements = root.FindElements(By.TagName("div"))
                    .Where(e => CheckAttr(e, "class", "_post post page_block")).ToList();
                if (elements.Any()) return elements.First();

                elements = root.FindElements(By.TagName("div")).ToList();
                if (!elements.Any()) return null;

                foreach (var item in elements)
                {
                    if (item.FindElements(By.TagName("div")).Where(e => CheckAttr(e, "class", "_post_content")).Any())
                        return item;
                }

                var newRoot = elements.First();
                elements = root.FindElements(By.TagName("div")).ToList();
                if (!elements.Any()) return null;

                foreach (var item in elements)
                {
                    if (item.FindElements(By.TagName("div")).Where(e => CheckAttr(e, "class", "_post_content")).Any())
                        return item;
                }
            }


            //elements = root.FindElements(By.TagName("div")).ToList();
            //foreach (var element in elements)
            //{
            //    var item = FindCurrentElement(element);
            //    if (item != null) return item;
            //}
            return null;
        }

        private static bool CheckAttr(IWebElement element, string name, string expected)
        {
            var attr = element.GetAttribute(name);
            if (string.IsNullOrEmpty(attr)) return false;
            return attr.Equals(expected);
        }

        private void GetAllLinks(IWebElement root, List<string> result)
        {
            lock (lockObject)
            {
                var elements = root.FindElements(By.TagName("a")).ToList();
                foreach (var element in elements)
                {
                    var href = element.GetAttribute("href");
                    if (href != null)
                        result.Add(href);
                    else
                        GetAllLinks(element, result);
                }
            }
        }
    }
}
