using System;
using VkParser.SharedLib.Common;

namespace VkParser.Logic.Parser.Events
{
    public class LinksEventArgs: EventArgs
    {
        public LinksEventArgs(Links data)
        {
            this.Data = data;
        }

        public Links Data { get; }
    }
}
