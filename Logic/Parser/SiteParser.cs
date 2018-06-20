using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using VkParser.Logic.Parser.Events;
using VkParser.SharedLib.Common;

namespace VkParser.Logic.Parser
{
    /// <summary>
    /// Парсер сайта
    /// </summary>
    public class SiteParser: IDisposable
    {
        private readonly string _address;
        private readonly IWebDriver _chrome;
        private readonly Random _rnd = new Random();
        private CancellationTokenSource cts = new CancellationTokenSource();
        private readonly object _lock = new object();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="address">Адрес сайта</param>
        /// <param name="action">Асинхронная задача для обработки результатов</param>
        public SiteParser(string address)
        {
            this._address = address;
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("user-data-dir=C:/Users/srgal/AppData/Local/Google/Chrome/User Data/Default");
            //options.AddArgument("--start-maximized");
            this._chrome = new ChromeDriver(Path.GetDirectoryName(typeof(SiteParser).Assembly.Location), options);
        }

        public event EventHandler<MainDataEventArgs> MainDataParsed;
        public event EventHandler<LinksEventArgs> LinksParsed;
        public event EventHandler<ImagesEventArgs> ImagesParsed;
        public event EventHandler<EventArgs> Done;

        public void Dispose()
        {
            this.cts.Cancel();
            Task.Delay(5000).GetAwaiter().GetResult();
            this.DisposeChrome(this._chrome);
        }

        public Task Login(string login, string password)
        {
            var name = login;
            var pass = password;
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                name = AuthInfoHelper.Login;
            }

            if (string.IsNullOrEmpty(pass) || string.IsNullOrWhiteSpace(pass))
            {
                pass = AuthInfoHelper.Password;
            }
            return this.LoginInternal(this._chrome, name, pass);
        }

        /// <summary>
        /// Старт парсинга
        /// </summary>
        /// <returns></returns>
        public Task Start()
        {
            return this.StartInternal(this._chrome);
        }

        public void Stop()
        {
            var old = this.cts;
            old.Cancel();
            old.Dispose();
            this.cts = new CancellationTokenSource();
        }

        private Task LoginInternal(IWebDriver driver, string login, string password)
        {
            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            if (string.IsNullOrEmpty(login) || string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            return Task.Run(async () =>
            {
                driver.Navigate().GoToUrl(this._address);

                var loginForm = driver.FindElement(By.Id("login_form"));
                var email = loginForm.FindElement(By.Id("email"));
                var pass = loginForm.FindElement(By.Id("pass"));
                var loginButton = loginForm.FindElement(By.Id("login_button"));

                await this.SendKeysAsync(email, login);
                await this.SendKeysAsync(pass, password);
                await Task.Delay(500);
                pass.Submit();

            });
        }

        private async Task StartInternal (IWebDriver driver)
        {
            driver.Navigate().GoToUrl(this._address);
            driver.Manage().Window.Maximize();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.scrollBy(0,100000)");
            await Task.Delay(500);
            js.ExecuteScript("window.scrollBy(0,100000)");
            await Task.Delay(500);
            js.ExecuteScript("window.scrollBy(0,100000)");
            //js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

            IWebElement rowsElement = driver.FindElement(By.Id("feed_rows"));
            ReadOnlyCollection<IWebElement> posts = rowsElement.FindElements(By.ClassName("feed_row"));
            List<Task> tasks = new List<Task>();
            foreach (var post in posts)
            {
                var item = post;
                tasks.Add(Task.Run(() => this.ParsePostMainDataAsync(item, this.cts.Token)));
                tasks.Add(Task.Run(() => this.ParsePostLinksAsync(item, this.cts.Token)));
                tasks.Add(Task.Run(() => this.ParsePostImagesAsync(item, this.cts.Token)));
            }
            await Task.WhenAll(tasks);
            OnFinished();
        }


        private void ParsePostMainDataAsync(IWebElement webElement, CancellationToken token)
        {   
            MainData data = null;
            try
            {
                token.ThrowIfCancellationRequested();
                lock (this._lock)
                {
                    data = PostParser.GetMainData(webElement);   
                }
            }
            catch (OperationCanceledException)
            {

            }
            OnMainDataParsed(data);
        }

        private void ParsePostLinksAsync(IWebElement webElement, CancellationToken token)
        {
            Links data = null;
            try
            {
                token.ThrowIfCancellationRequested();
                lock (this._lock)
                {
                    data = PostParser.GetLinks(webElement);
                }
            }
            catch(OperationCanceledException)
            {

            }
            OnLinksParsed(data);
        }

        private void ParsePostImagesAsync(IWebElement webElement, CancellationToken token)
        {
            Images data = null;
            try
            {
                token.ThrowIfCancellationRequested();
                lock (this._lock)
                {
                    data = PostParser.GetImages(webElement);
                }
            }
            catch (OperationCanceledException)
            {

            }
            OnImagesParsed(data);
        }

        private async Task SendKeysAsync(IWebElement webElement, string text)
        {
            char[] charArray = text.ToArray();
            foreach (var item in charArray)
            {
                webElement.SendKeys(item.ToString());
                await Task.Delay(this._rnd.Next(100, 500));
            }
        }

        private void OnMainDataParsed(MainData data)
        {
            var handler = MainDataParsed;
            if (handler == null) return;
            handler(this, new MainDataEventArgs(data));
        }

        private void OnLinksParsed(Links data)
        {
            var handler = LinksParsed;
            if (handler == null) return;
            handler(this, new LinksEventArgs(data));
        }

        private void OnImagesParsed(Images data)
        {
            var handler = ImagesParsed;
            if (handler == null) return;
            handler(this, new ImagesEventArgs(data));
        }

        private void OnFinished()
        {
            var handler = Done;
            if (handler == null) return;
            handler(this, EventArgs.Empty);
        }

        private void DisposeChrome(IWebDriver driver)
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}