using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class PostHeader : AbstractElementParser
    {
        private PostHeader(IWebElement webElement) : base(webElement)
        {
            this.Info = HeaderInfo.Create(this.Element);
        }

        public HeaderInfo Info { get; }

        public static PostHeader Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new PostHeader(elem);
        }

        public static string ClassName { get { return "post_header"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
                return root.FindElement(By.ClassName(ClassName));
        }
    }
}
