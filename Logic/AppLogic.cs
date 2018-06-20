using System;
using SharedLib;
using VkParser.Logic.Actors;
using VkParser.Logic.Messages;
using VkParser.Logic.Parser;
using VkParser.Logic.Parser.Events;
using VkParser.SharedLib.Common;

namespace VkParser.Logic
{
    /// <summary>
    /// Основная логира приложения
    /// </summary>
    public class AppLogic: IDisposable
    {
        private readonly PostDataActor<MainData> _mainDataActor = new PostDataActor<MainData>(Consts.MainDataFilePath);
        private readonly PostDataActor<Links> _linksActor = new PostDataActor<Links>(Consts.LinksFilePath);
        private readonly PostDataActor<Images> _imagesActor = new PostDataActor<Images>(Consts.ImagesFilePath);
        private readonly FullPostActor _fullPostActor = new FullPostActor(Consts.FullPostFilePath);
        private readonly MergeActor _mergeActor;
        private SiteParser _parser;

        public AppLogic()
        {
            this._mergeActor = new MergeActor(this._fullPostActor);            
        }

        public event EventHandler<int> MoveNext;

        /// <summary>
        /// Основной метод запуска работы
        /// </summary>
        public void Start()
        {
            if (this._parser == null) return;
            this._parser.Start();
        }

        public void Login(string login, string password)
        {
            CreateParser();
            this._parser.Login(login, password);
        }

        public void Stop()
        {
            if (this._parser == null) return;
            var prsr = this._parser;
            this._parser = null;
            prsr.Dispose();
        }

        private void GetPosts()
        {
            this._mainDataActor.Send(new GetDataMessage(this._mergeActor, 0, -1));
            this._linksActor.Send(new GetDataMessage(this._mergeActor, 0, -1));
            this._imagesActor.Send(new GetDataMessage(this._mergeActor, 0, -1));
        }

        private void OnMainDataParsed(object sender, MainDataEventArgs data)
        {
            if (data == null || data.Data == null) { this.OnMoveNext(1); return; }
            this._mainDataActor.Send(new AddDataMessage<MainData>(data.Data));
            this.OnMoveNext(1);
        }

        private void OnLinksParsed(object sender, LinksEventArgs data)
        {
            if (data == null || data.Data == null) { this.OnMoveNext(2); return; }
            this._linksActor.Send(new AddDataMessage<Links>(data.Data));
            this.OnMoveNext(2);
        }

        private void OnImagesParsed(object sender, ImagesEventArgs data)
        {
            if (data == null || data.Data == null) { this.OnMoveNext(3); return; }
            this._imagesActor.Send(new AddDataMessage<Images>(data.Data));
            this.OnMoveNext(3);
        }

        public void Dispose()
        {
            this._mainDataActor.Dispose();
            this._linksActor.Dispose();
            this._imagesActor.Dispose();
        }

        private void CreateParser()
        {
            if (this._parser != null) return;
            this._parser = new SiteParser("https://vk.com/feed");
            this._parser.MainDataParsed += this.OnMainDataParsed;
            this._parser.LinksParsed += this.OnLinksParsed;
            this._parser.ImagesParsed += this.OnImagesParsed;
            this._parser.Done += this.OnFinished;
        }

        private void OnFinished(object sender, EventArgs e)
        {
            OnMoveNext(4);
        }

        private void OnMoveNext(int i)
        {
            var handler = MoveNext;
            if (handler == null) return;
            handler(this, i);
            this.GetPosts();
        }
    }
}