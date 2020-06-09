using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using System.Collections.Generic;

namespace MicrosoftTranslateApp.Traslators
{
    class GoogleTranslator : ITranslator
    {
        private readonly TranslationClient translator;

        public GoogleTranslator()
        {
            GoogleCredential credential = GoogleCredential.FromFile(@"C:\Repos\Translate-Overlay\MicrosoftTranslateApp\MicrosoftTranslateApp\Resources\My Transtate Project-6825cc43e735.json");
            translator = TranslationClient.Create(credential);
            /*var response = translator.TranslateText(
            text: "Hello World.",
            targetLanguage: "ru",  // Russian
            sourceLanguage: "en");  // English
            string s = response.TranslatedText;*/
        }

        public SortedDictionary<string, string> Languages
        {
            get
            {
                SortedDictionary<string, string> languageCodesAndTitles = new SortedDictionary<string, string>();

                foreach (var kv in translator.ListLanguages())
                {
                    languageCodesAndTitles.Add(kv.Code, kv.Code);
                }

                return languageCodesAndTitles;
            }
        }

        public string Translate(string sourceLang, string targetLang, string textToTranslate)
        {
            throw new System.NotImplementedException();
        }

        public string Translate(string targetLang, string textToTranslate)
        {
            throw new System.NotImplementedException();
        }
    }
}
