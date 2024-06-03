using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LexiMate
{
    public interface ISecretService
    {
        string GetSecretValue(string key);
    }

    public class SecretService : ISecretService
    {
        private readonly JObject _config;

        public SecretService()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("LexiMate.appSettings.json"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                _config = JObject.Parse(json);
            }
        }

        public string GetSecretValue(string key)
        {
            return _config["AppSettings"][key].ToString();
        }
    }

}
