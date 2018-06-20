using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using SharedLib;
using VkParser.Logic.Messages;
using VkParser.SharedLib.Common;

namespace VkParser.Logic.Actors
{
    /// <summary>
    /// Актор пишет и читает данные поста
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PostDataActor<T>: Actor, IDisposable where T: AbstractPost
    {
        private readonly string _tempFileName;
        private readonly ProcessLocker _locker;

        public PostDataActor(string tempFileName)
        {
            this._tempFileName = tempFileName;
            this._locker = new ProcessLocker(this._tempFileName);
        }

        public void Dispose()
        {
            this._locker.Dispose();
        }

        public void Handle(AddDataMessage<T> message)
        {
            List<T> result = new List<T>();

            if (File.Exists(this._tempFileName))
            {
                string data = "";
                using (this._locker.Acquire(Consts.DefaultWaitTime))
                {
                    data = File.ReadAllText(this._tempFileName, Encoding.UTF8);
                }
                result = JsonConvert.DeserializeObject<List<T>>(data);
            }

            if (!result.Any(r => r.Id.Equals(message.Data.Id)))
                result.Add(message.Data);

            string serializedResult = JsonConvert.SerializeObject(result);
            using (this._locker.Acquire(Consts.DefaultWaitTime))
            {
                File.WriteAllText(this._tempFileName, serializedResult, Encoding.UTF8);
            }
        }

        public void Handle(GetDataMessage message)
        {
            List<T> result = new List<T>();

            if (File.Exists(this._tempFileName))
            {
                string data = "";
                using (this._locker.Acquire(Consts.DefaultWaitTime))
                {
                    data = File.ReadAllText(this._tempFileName, Encoding.UTF8);
                }
                result = JsonConvert.DeserializeObject<List<T>>(data);
            }

            message.Receiver.Send(
                new DataResultMessage<T>
                {
                    Data = message.Count > -1 ? result.Skip(message.StartsFrom).Take(message.Count).ToList() : result.Skip(message.StartsFrom).ToList()
                });
        }
    }
}