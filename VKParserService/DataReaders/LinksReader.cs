using SharedLib;
using VkParser.SharedLib.Common;

namespace VKParserService.DataReaders
{
    internal class LinksReader : DataReader<Links>
    {
        public LinksReader(string fileName) : base(new ProcessLocker(fileName), fileName)
        { }
    }
}
