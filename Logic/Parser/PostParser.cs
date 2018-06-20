using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using VkParser.Logic.Parser.FeedRow;
using VkParser.SharedLib.Common;

namespace VkParser.Logic.Parser
{
    public class PostParser
    {
        private readonly IWebElement _element;

        private PostParser(IWebElement webElement)
        {
            this._element = webElement ?? throw new ArgumentNullException("webElement");
        }

        public MainData MainData { get; private set; }
        public Images Images { get; private set; }
        public Links Links { get; private set; }
        public string Id { get; private set; }

        public static MainData GetMainData(IWebElement webElement)
        {
            var parser = new PostParser(webElement);
            return parser.ParsePostMainData();
        }

        public static Links GetLinks(IWebElement webElement)
        {
            var parser = new PostParser(webElement);
            return parser.ParsePostLinks();
        }

        public static Images GetImages(IWebElement webElement)
        {
            var parser = new PostParser(webElement);
            return parser.ParsePostImages();
        }

        private MainData ParsePostMainData()
        {            
            var postBlock = PostPageBlock.Create(this._element);
            if (postBlock == null)
                return null;
            MainData result = new MainData();
            result.Id = postBlock.Id;
            result.Text = postBlock.Content.Content.Info == null || postBlock.Content.Content.Info.Wall.Content.WallText == null ? "" : postBlock.Content.Content.Info.Wall.Content.WallText.Text;
            result.Author = postBlock.Content.Header.Info.Author.Name;
            return result;
        }

        private Links ParsePostLinks()
        {
            var postBlock = PostPageBlock.Create(this._element);
            if (postBlock == null) return null;
            Links result = new Links();
            result.Id = postBlock.Id;
            result.Items = postBlock.AllLinks;
            return result;
        }

        private Images ParsePostImages()
        {
            var postBlock = PostPageBlock.Create(this._element);
            if (postBlock == null) return null;
            Images result = new Images();
            result.Id = postBlock.Id;
            result.ImageFiles = postBlock.Content.Content.Info == null || postBlock.Content.Content.Info.Wall.Content.WallThumbs == null 
                ? new List<string>() 
                : postBlock.Content.Content.Info.Wall.Content.WallThumbs.ImageUrl;
            return result;
        }
    }
}
