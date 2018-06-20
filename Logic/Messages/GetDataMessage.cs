using VkParser.Logic.Actors;

namespace VkParser.Logic.Messages
{
    /// <summary>
    /// Сообщение для получения данных поста
    /// </summary>
    public class GetDataMessage: AbstractMessage
    {
        public Actor Receiver { get; private set; }
        public int StartsFrom { get; private set; }
        public int Count { get; private set; }

        public GetDataMessage(Actor receiver, int startsFrom, int count)
        {
            this.Receiver = receiver;
            this.StartsFrom = startsFrom;
            this.Count = count;
        }
    }
}