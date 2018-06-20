using System.Linq;
using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class PostInfo : AbstractElementParser
    {
        private PostInfo(IWebElement webElement) : base(webElement)
        {
            this.Wall = WallText.Create(this.Element);
        }

        public WallText Wall { get; }

        public static PostInfo Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new PostInfo(elem);
        }

        public static string ClassName { get { return "post_info"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
            {
                var elements = root.FindElements(By.ClassName(ClassName));
                if (!elements.Any()) return null;
                return elements.First();
            }
        }
    }
}
