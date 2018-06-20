using System.Threading.Tasks.Dataflow;
using VkParser.Logic.Messages;

namespace VkParser.Logic.Actors
{
    public abstract class Actor
    {
        private readonly ActionBlock<AbstractMessage> _action;

        protected Actor()
        {
            this._action = new ActionBlock<AbstractMessage>(m =>
                                               {
                                                   dynamic actor = this;
                                                   dynamic message = m;
                                                   actor.Handle(message);
                                               });
        }

        public void Send(AbstractMessage message)
        {
            this._action.Post(message);
        }

    }
}