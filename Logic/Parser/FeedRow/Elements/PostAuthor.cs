using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal class PostAuthor : AbstractElementParser
    {
        private PostAuthor(IWebElement webElement) : base(webElement)
        {
            this.Name = this.GetAuthorName();
        }

        private string GetAuthorName()
        {
            IWebElement authorInfo = this.Element.FindElement(By.ClassName("author"));
            return authorInfo.Text;
        }

        public string Name { get; }

        public static PostAuthor Create(IWebElement root)
        {
            var elem = FindCurrentElement(root);
            if (elem == null) return null;
            return new PostAuthor(elem);
        }

        public static string ClassName { get { return "post_author"; } }

        private static IWebElement FindCurrentElement(IWebElement root)
        {
            lock (lockObject)
                return root.FindElement(By.ClassName(ClassName));
        }
    }
}
