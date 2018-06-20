using System;
using System.Threading;

namespace VKParserService
{
    public static class PeriodicTaskUtils
    {
        public static IPeriodicTimerTask StartPeriodicTask(TimeSpan delay, TimeSpan period, Action action)
        {
            return new PeriodicTask<int>(delay, period, action);
        }

        private sealed class PeriodicTask<T> : IPeriodicTimerTask
        {
            private readonly object _execLock = new object();
            private readonly object _configLock = new object();
            private Timer _timer;
            private readonly TimeSpan _timeout;
            private readonly Action _actionInfo;


            public PeriodicTask(TimeSpan delay, TimeSpan time, Action actionData)
            {
                this._timeout = time;
                this._actionInfo = actionData;
                this._timer = new Timer(Callback, this, -1, -1);
                this.StartInternal(delay);

            }

            #region Implementation of IDisposable

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                Timer tmr;
                this.Stop();
                lock (this._execLock)
                {
                    lock (this._configLock)
                    {
                        tmr = this._timer;
                        this._timer = null;
                    }
                }
                if (tmr != null)
                    tmr.Dispose();
            }

            #endregion

            #region Implementation of IPeriodicTimerTask

            public void Start()
            {
                lock (this._execLock)
                {
                    this.StartInternal(this._timeout);
                }
            }

            public void Stop()
            {
                lock (this._execLock)
                {
                    this.StopInternal();
                }
            }

            #endregion

            private void StartInternal(TimeSpan timeSpan)
            {
                lock (this._configLock)
                {
                    this._timer.Change(timeSpan, TimeSpan.FromMilliseconds(-1));
                }
            }

            private static void Callback(object state)
            {
                PeriodicTask<T> task = state as PeriodicTask<T>;
                task.Execute();
            }

            private void Execute()
            {
                try
                {
                    lock (this._execLock)
                    {
                        this._actionInfo();
                    }
                }
                catch (Exception)
                {
                    //loging here
                }
                finally
                {
                    this.StartInternal(this._timeout);
                }
            }

            private void StopInternal()
            {
                lock (this._configLock)
                {
                    if (this._timer == null)
                        return;
                    this._timer.Change(-1, -1);
                }
            }

        }
    }
}