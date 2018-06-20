using System;
using VkParser.SharedLib.Common;

namespace VkParser.Logic.Parser.Events
{
    public class MainDataEventArgs: EventArgs
    {
        public MainDataEventArgs(MainData data)
        {
            this.Data = data;
        }

        public MainData Data { get; }
    }
}
