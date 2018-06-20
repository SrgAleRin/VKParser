using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class HeaderInfo : AbstractElementParser
    {
        private HeaderInfo(IWebElement webElement) : base(webElement)
        {
            this.Author = PostAuthor.Create(webElement);
        }

        public PostAuthor Author { get; }

        public static HeaderInfo Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new HeaderInfo(elem);
        }

        public static string ClassName { get { return "post_header_info"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
                return root.FindElement(By.ClassName(ClassName));
        }
    }
}
