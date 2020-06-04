using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System;

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

        /// <summary>
        /// Languages list
        /// </summary>
        public SortedDictionary<string, string> Languages
        {
            get
            {
                SortedDictionary<string, string> languageCodesAndTitles = new SortedDictionary<string, string>();

                string uri = ApiUriCreator("languages", new Dictionary<string, string> { { "scope", "translation" } });
                WebRequest WebRequest = WebRequest.Create(uri);
                WebRequest.Headers.Add("Accept-Language", "en");
                WebResponse response = WebRequest.GetResponse();
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

        /// <summary>
        /// Base Translate
        /// </summary>
        /// <param name="sourceLang"></param>
        /// <param name="targetLang"></param>
        /// <param name="textToTranslate"></param>
        /// <returns></returns>
        public string Translate(string sourceLang, string targetLang, string textToTranslate)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Auto detect source language
        /// </summary>
        /// <param name="targetLang"></param>
        /// <param name="textToTranslate"></param>
        /// <returns></returns>
        public string Translate(string targetLang, string textToTranslate)
        {
            string detectUri = ApiUriCreator("detect");

            // Create request to Detect languages with Translator Text
            HttpWebRequest detectLanguageWebRequest = (HttpWebRequest)WebRequest.Create(detectUri);
            detectLanguageWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", API_KEY);
            detectLanguageWebRequest.Headers.Add("Ocp-Apim-Subscription-Region", "westeurope");
            detectLanguageWebRequest.ContentType = "application/json; charset=utf-8";
            detectLanguageWebRequest.Method = "POST";

            // Send request
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonText = serializer.Serialize(textToTranslate);

            string body = "[{ \"Text\": " + jsonText + " }]";
            byte[] data = Encoding.UTF8.GetBytes(body);

            detectLanguageWebRequest.ContentLength = data.Length;

            using (var requestStream = detectLanguageWebRequest.GetRequestStream())
                requestStream.Write(data, 0, data.Length);

            HttpWebResponse response = (HttpWebResponse)detectLanguageWebRequest.GetResponse();

            // Read and parse JSON response
            var responseStream = response.GetResponseStream();
            var jsonString = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")).ReadToEnd();
            dynamic jsonResponse = serializer.DeserializeObject(jsonString);

            // Fish out the detected language code
            var languageInfo = jsonResponse[0];
            if (languageInfo["score"] > (decimal)0.5)
            {
                return Translate(languageInfo["language"], targetLang, textToTranslate);
            }

            throw new Exception("Unable to confidently detect input language.");
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
