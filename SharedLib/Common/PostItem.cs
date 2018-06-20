namespace VkParser.SharedLib.Common
{
    /// <summary>
    /// Полная информация о посте
    /// </summary>
    public class PostItem: AbstractPost
    {
        public Images Images { get; set; }

        public Links Links { get; set; }

        public MainData Data { get; set; }
    }
}