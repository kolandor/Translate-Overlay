using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftTranslateApp.SpeechToText
{
    /// <summary>
    /// Microsoft audio recognize service
    /// </summary>
    public class WindowsSpeechToText : ISpeechToText
    {
        public string RecognizeAudioStream(Stream source)
        {
            throw new NotImplementedException();
        }

        public Task<string> RecognizeAudioStreamAsync(Stream source)
        {
            throw new NotImplementedException();
        }

        public string RecognizeFile(string path)
        {
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
            Grammar gr = new DictationGrammar();
            sre.LoadGrammar(gr);
            sre.SetInputToWaveFile(path);
            sre.BabbleTimeout = new TimeSpan(Int32.MaxValue);
            sre.InitialSilenceTimeout = new TimeSpan(Int32.MaxValue);
            sre.EndSilenceTimeout = new TimeSpan(100000000);
            sre.EndSilenceTimeoutAmbiguous = new TimeSpan(100000000);

            StringBuilder sb = new StringBuilder();

            //while (true)
            for (int i = 0; i < 200; i++)
            {
                try
                {
                    RecognitionResult recText = sre.Recognize();
                    if (recText == null)
                    {
                        break;
                    }

                    sb.Append(recText.Text);
                }
                catch (Exception ex)
                {
                    if (sb.Length <= 0)
                    {
                        throw ex;
                    }
                }
            }
            return sb.ToString();
        }

        public Task<string> RecognizeFileAsync(string path)
        {
            return Task.Run(()=> { return RecognizeFile(path); });
        }
    }
}
