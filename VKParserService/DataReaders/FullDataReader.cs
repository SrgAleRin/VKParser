using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkParser.SharedLib.Common;

namespace VKParserService.DataReaders
{
    internal class FullDataReader : IDisposable
    {
        private readonly MainDataReader _mainReader;
        private readonly LinksReader _linksReader;
        private readonly ImageReader _imagesReader;

        public FullDataReader(string mainDataFile, string linksFile, string imagesFile)
        {
            this._mainReader = new MainDataReader(mainDataFile);
            this._linksReader = new LinksReader(linksFile);
            this._imagesReader = new ImageReader(imagesFile);
        }

        public List<PostItem> Read()
        {
            var tuple = ReadData();
            return MergeData(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        public void Dispose()
        {
            this._mainReader.Dispose();
            this._linksReader.Dispose();
            this._imagesReader.Dispose();
        }

        private Tuple<List<MainData>, List<Links>, List<Images>> ReadData()
        {
            var mainDataTask = Task.Run(() => this._mainReader.Read());
            var linksTask = Task.Run(() => this._linksReader.Read());
            var imagesTask = Task.Run(() => this._imagesReader.Read());
            Task.WaitAll(mainDataTask, linksTask, imagesTask);
            return Tuple.Create(mainDataTask.Result, linksTask.Result, imagesTask.Result);
        }

        private List<PostItem> MergeData(List<MainData> main, List<Links> links, List<Images> images)
        {
            return main.Join(links, m => m.Id, l => l.Id, (m, l) => new PostItem { Id = m.Id, Data = m, Links = l })
                .Join(images, p => p.Id, i => i.Id, (p, i) => { p.Images = i; return p; })
                .ToList();              
        }
    }
}
