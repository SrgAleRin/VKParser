using System.Linq;
using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class WallPostContent : AbstractElementParser
    {
        private WallPostContent(IWebElement webElement) : base(webElement)
        {
            this.WallText = WallPostText.Create(this.Element);
            this.WallThumbs = WallPostThumbs.Create(this.Element);

        }

        public WallPostText WallText { get; }
        public WallPostThumbs WallThumbs { get; }

        public static WallPostContent Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new WallPostContent(elem);
        }

        public static string ClassName { get { return "copy_quote"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
            {
                var elements = root.FindElements(By.ClassName(ClassName)).ToList();
                if (elements.Any()) return elements.First();

                elements = root.FindElements(By.TagName("div"))
                    .Where(e => e.GetAttribute("class") != null
                    && e.GetAttribute("class").Equals("wall_post_cont _wall_post_cont"))
                    .ToList();
                if (elements.Any()) return elements.First();
            }

            return null;
        }
    }
}
