using System;
using OpenQA.Selenium;

namespace VkParser.Logic.Parser.FeedRow
{
    internal abstract class AbstractElementParser
    {
        protected static readonly object lockObject = new object();

        protected AbstractElementParser(IWebElement webElement)
        {
            this.Element = webElement ?? throw new ArgumentNullException("webElement");
        }

        protected IWebElement Element { get; set; }
    }
}
