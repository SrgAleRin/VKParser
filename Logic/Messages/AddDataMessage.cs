namespace VkParser.Logic.Messages
{
    /// <summary>
    /// Сообщение добавления данных
    /// </summary>
    /// <typeparam name="T">Тип данных</typeparam>
    public class AddDataMessage<T>: AbstractMessage
    {
        public AddDataMessage(T data)
        {
            this.Data = data;
        }

        public T Data { get; private set; }
    }
}