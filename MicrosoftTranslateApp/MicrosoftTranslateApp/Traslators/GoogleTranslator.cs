using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using System.Collections.Generic;

namespace MicrosoftTranslateApp.Traslators
{
    class GoogleTranslator : ITranslator
    {
        private readonly TranslationClient translator;

        public GoogleTranslator(string googleJsonApi)
        {
            GoogleCredential credential = GoogleCredential.FromJson(googleJsonApi);
            translator = TranslationClient.Create(credential);
        }

        public SortedDictionary<string, string> Languages
        {
            get
            {
                SortedDictionary<string, string> languageCodesAndTitles = new SortedDictionary<string, string>(Comparer<string>.Create((a, b) => string.Compare(a, b, true)));

                foreach (var kv in translator.ListLanguages())
                {
                    languageCodesAndTitles.Add(kv.Code, kv.Code);
                }

                return languageCodesAndTitles;
            }
        }

        public string Translate(string sourceLang, string targetLang, string textToTranslate)
        {
            var response = translator.TranslateText(
            text: textToTranslate,
            targetLanguage: targetLang);
            return response.TranslatedText;
        }

        public string Translate(string targetLang, string textToTranslate)
        {
            return Translate(null, targetLang, textToTranslate);
        }
    }
}
