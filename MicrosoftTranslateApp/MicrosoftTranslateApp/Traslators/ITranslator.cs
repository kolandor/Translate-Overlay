using System.Collections.Generic;

namespace MicrosoftTranslateApp.Traslators
{
    public interface ITranslator
    {
        SortedDictionary<string, string> Languages { get; }

        string Translate(string codeFrom, string codeTo, string textToTranslate);
    }
}
