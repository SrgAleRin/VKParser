// <copyright file="DataSaver.cs" company="LANIT">
// 
//     Copyright (c) LAboratory of New Information Technologies. All rights reserved. 2018
// 
// </copyright>

using System;
using SharedLib;
using VKParserService.DataReaders;
using VKParserService.DataSavers;

namespace VKParserService
{
    public class ServiceLogic: IDisposable
    {
        private readonly string _mainDataFile;
        private readonly string _LinksFile;
        private readonly string _ImagesFile;
        private readonly FullDataReader _fullDataReader;
        private readonly DataSaver _saver = new DataSaver();

        public ServiceLogic(string sourcePath)
        {
            this._mainDataFile = Consts.MainDataFilePath;
            this._LinksFile = Consts.LinksFilePath;
            this._ImagesFile = Consts.ImagesFilePath;
            this._fullDataReader = new FullDataReader(this._mainDataFile, this._LinksFile, this._ImagesFile);
        }

        public void Dispose()
        {
            this._fullDataReader.Dispose();
        }

        public void Save()
        {
            var posts = this._fullDataReader.Read();
            this._saver.SaveToDB(posts);
        }
    }
}