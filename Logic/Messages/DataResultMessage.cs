using System.Collections.Generic;

namespace VkParser.Logic.Messages
{
    public class DataResultMessage<T>: AbstractMessage
    {
        public List<T> Data { get; set; }
    }
}