using System.Linq;
using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class WallPostText : AbstractElementParser
    {
        private WallPostText(IWebElement webElement) : base(webElement)
        {
            this.Text = this.Element.Text;
        }

        public string Text { get; }

        public static WallPostText Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new WallPostText(elem);
        }

        public static string ClassName { get { return "wall_post_text"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
            {
                var elements = root.FindElements(By.ClassName(ClassName)).ToList();
                if (elements.Any()) return elements.First();
            }

            return null;
        }
    }
}
