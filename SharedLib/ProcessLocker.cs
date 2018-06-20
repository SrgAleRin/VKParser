using System;
using System.IO;
using System.Threading;

namespace SharedLib
{
    public class ProcessLocker : IDisposable
    {
        private readonly Mutex _mutex;

        public ProcessLocker(string mutexName)
        {
            this._mutex = new Mutex(false, Path.GetFileName(mutexName));
        }

        public IDisposable Acquire(TimeSpan time)
        {
            try
            {
                if (this._mutex.WaitOne((int)time.TotalMilliseconds))
                    return new AcquiredMutex(this._mutex);
                throw new Exception("TimeOut");
            }
            catch(AbandonedMutexException)
            {
                return new AcquiredMutex(this._mutex);
            }
        }

        public void Dispose()
        {
            this._mutex.Dispose();
        }

        private class AcquiredMutex: IDisposable
        {
            public AcquiredMutex(Mutex mutex)
            {
                this.Mutex = mutex;
            }

            public Mutex Mutex { get; private set; }

            public void Dispose()
            {
                this.Mutex.ReleaseMutex();
            }
        }
    }
}
