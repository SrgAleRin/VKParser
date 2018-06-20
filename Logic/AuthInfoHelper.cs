using System;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace VkParser.Logic
{
    internal static class AuthInfoHelper
    {
        private const string SettingsPath = "f:\\settings.json";
        private static readonly Lazy<AuthData> authInfo = new Lazy<AuthData>(() => InitAuth(), LazyThreadSafetyMode.PublicationOnly);

        public static string Login
        {
            get
            {
                return authInfo.Value.Login;
            }
        }

        public static string Password
        {
            get
            {
                return authInfo.Value.Password;
            }
        }

        private static AuthData InitAuth()
        {
            if (!File.Exists(SettingsPath)) return new AuthData();
            var data = File.ReadAllText(SettingsPath, Encoding.UTF8);
            return JsonConvert.DeserializeObject<AuthData>(data);
        }

        private class AuthData
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
    }
}
