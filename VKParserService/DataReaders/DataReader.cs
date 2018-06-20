using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SharedLib;

namespace VKParserService.DataReaders
{
    internal abstract class DataReader<T>: IDisposable 
    {
        private readonly ProcessLocker _locker;
        private readonly string _filePath;

        protected DataReader(ProcessLocker locker, string filePath)
        {
            this._locker = locker;
            this._filePath = filePath;
        }

        public void Dispose()
        {
            this._locker.Dispose();
        }

        public List<T> Read()
        {
            if (File.Exists(this._filePath))
            {
                using (this._locker.Acquire(Consts.DefaultWaitTime))
                {
                    string data = File.ReadAllText(this._filePath);
                    return JsonConvert.DeserializeObject<List<T>>(data);
                }
            }
            return new List<T>();
        }
    }
}
