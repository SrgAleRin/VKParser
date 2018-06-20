using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class PostContentContent : AbstractElementParser
    {
        private PostContentContent(IWebElement webElement) : base(webElement)
        {
            this.Info = PostInfo.Create(this.Element);
        }

        public PostInfo Info { get; }

        public static PostContentContent Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new PostContentContent(elem);
        }

        public static string ClassName { get { return "post_content"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
                return root.FindElement(By.ClassName(ClassName));
        }
    }
}
