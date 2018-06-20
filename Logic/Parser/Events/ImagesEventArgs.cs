using System;
using VkParser.SharedLib.Common;

namespace VkParser.Logic.Parser.Events
{
    public class ImagesEventArgs: EventArgs
    {
        public ImagesEventArgs(Images data)
        {
            this.Data = data;
        }

        public Images Data { get; }
    }
}
