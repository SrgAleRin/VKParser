using SharedLib;
using VkParser.SharedLib.Common;

namespace VKParserService.DataReaders
{
    internal class MainDataReader: DataReader<MainData>
    {
        public MainDataReader(string fileName): base(new ProcessLocker(fileName), fileName)
        { }
    }
}
