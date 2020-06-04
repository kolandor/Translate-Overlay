using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace MicrosoftTranslateApp.Traslators
{
    public class MicrosoftTranslator : ITranslator
    {
        private readonly string API_KEY;
        private const string TEXT_TRANSLATION_API_ENDPOINT = "https://api.cognitive.microsofttranslator.com/";
        private const string API_VERSION = "?api-version=3.0";

        MicrosoftTranslator(string apiKey)
        {
            API_KEY = apiKey;
        }

        public SortedDictionary<string, string> Languages
        {
            get
            {
                SortedDictionary<string, string> languageCodesAndTitles = new SortedDictionary<string, string>();

                string uri = ApiUriCreator("languages", new Dictionary<string, string> { { "scope", "translation" } });
                WebRequest WebRequest = WebRequest.Create(uri);
                WebRequest.Headers.Add("Accept-Language", "en");
                WebResponse response = null;
                // Read and parse the JSON response
                response = WebRequest.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), UnicodeEncoding.UTF8))
                {
                    var result = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(reader.ReadToEnd());
                    var languages = result["translation"];

                    foreach (var kv in languages)
                    {
                        languageCodesAndTitles.Add(kv.Value["name"], kv.Key);
                    }
                }

                return languageCodesAndTitles;
            }
        }

        public string Translate(string codeFrom, string codeTo, string textToTranslate)
        {
            throw new System.NotImplementedException();
        }

        private string ApiUriCreator(string method)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(TEXT_TRANSLATION_API_ENDPOINT);
            sb.Append(method);
            sb.Append(API_VERSION);

            return sb.ToString();
        }

        private string ApiUriCreator(string method, Dictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ApiUriCreator(method));

            foreach (var param in parameters)
            {
                sb.Append($"&{param.Key}={param.Value}");
            }

            return sb.ToString();
        }
    }
}
