// <copyright file="Service1.cs" company="LANIT">
// 
//     Copyright (c) LAboratory of New Information Technologies. All rights reserved. 2018
// 
// </copyright>

using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;

namespace VKParserService
{
    public partial class Service1 : ServiceBase
    {
        private IPeriodicTimerTask _periodicTimerTask;
        private readonly Lazy<string> _filesPath;
        private ServiceLogic _logic;

        public Service1()
        {
            this.InitializeComponent();
            this._filesPath = new Lazy<string>(this.GetFilesPath, LazyThreadSafetyMode.PublicationOnly);
            this._logic = new ServiceLogic(this.FilesPath);
        }

        public static void Main()
        {
            Service1 service = new Service1();
            if (Environment.UserInteractive)
            {
                var args = Environment.GetCommandLineArgs();
                service.OnStart(args);

                Console.WriteLine(string.Concat("Сервер ", "Parser.Service",
                    " запущен. Нажмите Ctrl+C для выхода."));

                AutoResetEvent cancel = new AutoResetEvent(false);
                Console.CancelKeyPress += (sender, e) =>
                {
                    cancel.Set();
                };
                cancel.WaitOne();

                service.OnStop();
                service.Dispose();
            }
            else
            {
                Run(service);
            }
        }

        private TimeSpan Delay
        {
            get
            {
                TimeSpan result;
                var setting = ConfigurationManager.AppSettings.GetValues("Delay");
                var delayString = setting != null ? setting.FirstOrDefault() : "00:00:02";
                if (!TimeSpan.TryParse(delayString, out result))
                    return TimeSpan.FromSeconds(2);
                return result;
            }
        }

        private TimeSpan Period
        {
            get
            {
                TimeSpan result;
                var setting = ConfigurationManager.AppSettings.GetValues("Period");
                var delayString = setting != null ? setting.FirstOrDefault() : "00:00:01";
                if (!TimeSpan.TryParse(delayString, out result))
                    return TimeSpan.FromMilliseconds(300);
                return result;
            }
        }

        private string FilesPath
        {
            get
            {
                return this._filesPath.Value;
            }
        }

        protected override void OnStart(string[] args)
        {
            this._periodicTimerTask = PeriodicTaskUtils.StartPeriodicTask(this.Delay, this.Period, this.Action);
        }

        protected override void OnStop()
        {
            if (this._periodicTimerTask != null) this._periodicTimerTask.Dispose();
        }

        private void Action()
        {
            this._logic.Save();
        }


        private string GetFilesPath()
        {
            var setting = ConfigurationManager.AppSettings.GetValues("FilesPath");
            return setting != null ? setting.FirstOrDefault() : "";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
                this._logic.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
