using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using VkParser.Logic.Messages;
using VkParser.SharedLib.Common;

namespace VkParser.Logic.Actors
{
    public class MergeActor: Actor
    {
        private readonly ConcurrentDictionary<string, PostItem> _items = new ConcurrentDictionary<string, PostItem>();
        private readonly Actor receiver;

        public MergeActor(Actor receiver)
        {
            this.receiver = receiver;
        }

        public void Handle<T>(DataResultMessage<T> message)
        {
            IEnumerable<MainData> data = message.Data.OfType<MainData>();
            AddToItems(data);
            IEnumerable<Links> links = message.Data.OfType<Links>();
            AddToItems(links);
            IEnumerable<Images> images = message.Data.OfType<Images>();
            AddToItems(images);

            this.receiver.Send(
                new DataResultMessage<PostItem>
                {
                    Data = this._items.Values.Where(i => i.Data != null && i.Images != null && i.Links != null).ToList()
                });
        }

        private void AddToItems(IEnumerable<MainData> list)
        {
            foreach (var part in list)
            {
                PostItem item = new PostItem();
                item.Id = part.Id;
                item.Data = part;
                this._items.AddOrUpdate(part.Id, item, (key, existing) => { existing.Data = part; return existing; });
            }
        }

        private void AddToItems(IEnumerable<Links> list)
        {
            foreach (var part in list)
            {
                PostItem item = new PostItem();
                item.Id = part.Id;
                item.Links = part;
                this._items.AddOrUpdate(part.Id, item, (key, existing) => { existing.Links = part; return existing; });
            }
        }

        private void AddToItems(IEnumerable<Images> list)
        {
            foreach (var part in list)
            {
                PostItem item = new PostItem();
                item.Id = part.Id;
                item.Images = part;
                this._items.AddOrUpdate(part.Id, item, (key, existing) => { existing.Images = part; return existing; });
            }
        }
    }
}
