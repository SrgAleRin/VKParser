using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class PostContent : AbstractElementParser
    {
        private PostContent(IWebElement webElement) : base(webElement)
        {
            this.Header = PostHeader.Create(this.Element);
            this.Content = PostContentContent.Create(this.Element);
        }

        public PostHeader Header { get; }
        public PostContentContent Content { get; }

        public static PostContent Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new PostContent(elem);
        }

        public static string ClassName { get { return "_post_content"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
                return root.FindElement(By.ClassName(ClassName));
        }
    }
}
