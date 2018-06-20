using System;

namespace VKParserService
{
    public interface IPeriodicTimerTask: IDisposable
    {
        void Start();

        void Stop();
    }
}