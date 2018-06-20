using SharedLib;
using VkParser.SharedLib.Common;

namespace VKParserService.DataReaders
{
    internal class ImageReader : DataReader<Images>
    {
        public ImageReader(string fileName) : base(new ProcessLocker(fileName), fileName)
        { }
    }
}
