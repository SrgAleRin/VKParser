using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using VkParser.Logic.Messages;
using VkParser.SharedLib.Common;

namespace VkParser.Logic.Actors
{
    public class FullPostActor: Actor
    {
        private readonly string _tempFileName;

        public FullPostActor(string tempFileName)
        {
            this._tempFileName = tempFileName;
        }

        public void Handle(DataResultMessage<PostItem> message)
        {
            List<PostItem> result = message.Data;

            string serializedResult = JsonConvert.SerializeObject(result);
            File.WriteAllText(this._tempFileName, serializedResult, Encoding.UTF8);
        }
    }
}
