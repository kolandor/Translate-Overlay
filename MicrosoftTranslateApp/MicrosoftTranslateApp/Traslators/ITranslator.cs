﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicrosoftTranslateApp.Traslators
{
    public interface ITranslator
    {
        /// <summary>
        /// Languages list
        /// </summary>
        SortedDictionary<string, string> Languages { get; }

        /// <summary>
        /// Base Translate
        /// </summary>
        /// <param name="sourceLang"></param>
        /// <param name="targetLang"></param>
        /// <param name="textToTranslate"></param>
        /// <returns></returns>
        string Translate(string sourceLang, string targetLang, string textToTranslate);

        /// <summary>
        /// Auto detect source language
        /// </summary>
        /// <param name="targetLang"></param>
        /// <param name="textToTranslate"></param>
        /// <returns></returns>
        string Translate(string targetLang, string textToTranslate);
    }
}
