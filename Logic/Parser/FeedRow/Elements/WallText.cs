using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class WallText : AbstractElementParser
    {
        private WallText(IWebElement webElement) : base(webElement)
        {
            this.Content = WallPostContent.Create(this.Element);
        }

        public WallPostContent Content { get; }

        public static WallText Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new WallText(elem);
        }

        public static string ClassName { get { return "wall_text"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
                return root.FindElement(By.ClassName(ClassName));
        }
    }
}
