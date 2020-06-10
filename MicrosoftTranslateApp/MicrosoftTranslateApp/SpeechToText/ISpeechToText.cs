using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftTranslateApp.SpeechToText
{
    /// <summary>
    /// Audio recognize service
    /// </summary>
    public interface ISpeechToText
    {
        /// <summary>
        /// Recognize audio file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string RecognizeFile(string path);

        /// <summary>
        /// Async recognize audio file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<string> RecognizeFileAsync(string path);

        /// <summary>
        /// Recognize audio stream
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string RecognizeAudioStream(Stream source);

        /// <summary>
        /// Async recognize audio stream
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<string> RecognizeAudioStreamAsync(Stream source);
    }
}
