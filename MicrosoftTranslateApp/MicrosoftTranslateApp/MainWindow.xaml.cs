using System;
using System.Windows;
using System.Collections.Generic;
using MicrosoftTranslateApp.Traslators;

namespace MicrosoftTranslateApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string BING_SPELL_CHECK_API_ENDPOINT = "https://westus.api.cognitive.microsoft.com/bing/v7.0/spellcheck/";

        ITranslator translator;

        private SortedDictionary<string, string> languageCodesAndTitles;

        // Global exception handler to display error message and exit
        private static void HandleExceptions(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show("Caught " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            System.Windows.Application.Current.Shutdown();
        }
        // MainWindow constructor
        public MainWindow()
        {
            // Display a message if unexpected error is encountered
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleExceptions);

            /*if (COGNITIVE_SERVICES_KEY.Length != 32)
            {
                MessageBox.Show("One or more invalid API subscription keys.\n\n" +
                    "Put your keys in the *_API_SUBSCRIPTION_KEY variables in MainWindow.xaml.cs.",
                    "Invalid Subscription Key(s)", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Windows.Application.Current.Shutdown();
            }
            else*/
            {
                // Start GUI
                InitializeComponent();

                translator = new GoogleTranslator(MicrosoftTranslateApp.Resources.ResourcesKeys.GoogleJson);

                // Populate drop-downs with values from GetLanguagesForTranslate
                PopulateLanguageMenus();
            }
        }
        
        private void PopulateLanguageMenus()
        {
            // Add option to automatically detect the source language
            FromLanguageComboBox.Items.Add("Detect");

            languageCodesAndTitles = translator.Languages;

            foreach (string menuItem in languageCodesAndTitles.Keys)
            {
                FromLanguageComboBox.Items.Add(menuItem);
                ToLanguageComboBox.Items.Add(menuItem);
            }

            // Set default languages
            /*FromLanguageComboBox.SelectedItem = "Detect";
            ToLanguageComboBox.SelectedItem = "English";*/
            FromLanguageComboBox.SelectedIndex = 0;
            ToLanguageComboBox.SelectedIndex = 0;
        }

        // NOTE:
        // In the following sections, we'll add code below this.

        // ***** CORRECT SPELLING OF TEXT TO BE TRANSLATED
        /*private string CorrectSpelling(string text)
        {
            string uri = BING_SPELL_CHECK_API_ENDPOINT + "?mode=spell&mkt=en-US";

            // Create a request to Bing Spell Check API
            HttpWebRequest spellCheckWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            spellCheckWebRequest.Headers.Add("Ocp-Apim-Subscription-Key", COGNITIVE_SERVICES_KEY);
            spellCheckWebRequest.Method = "POST";
            spellCheckWebRequest.ContentType = "application/x-www-form-urlencoded"; // doesn't work without this

            // Create and send the request
            string body = "text=" + System.Web.HttpUtility.UrlEncode(text);
            byte[] data = Encoding.UTF8.GetBytes(body);
            spellCheckWebRequest.ContentLength = data.Length;
            using (var requestStream = spellCheckWebRequest.GetRequestStream())
                requestStream.Write(data, 0, data.Length);
            HttpWebResponse response = (HttpWebResponse)spellCheckWebRequest.GetResponse();

            // Read and parse the JSON response; get spelling corrections
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var responseStream = response.GetResponseStream();
            var jsonString = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")).ReadToEnd();
            dynamic jsonResponse = serializer.DeserializeObject(jsonString);
            var flaggedTokens = jsonResponse["flaggedTokens"];

            // Construct sorted dictionary of corrections in reverse order (right to left)
            // This ensures that changes don't impact later indexes
            var corrections = new SortedDictionary<int, string[]>(Comparer<int>.Create((a, b) => b.CompareTo(a)));
            for (int i = 0; i < flaggedTokens.Length; i++)
            {
                var correction = flaggedTokens[i];
                var suggestion = correction["suggestions"][0];  // Consider only first suggestion
                if (suggestion["score"] > (decimal)0.7)         // Take it only if highly confident
                    corrections[(int)correction["offset"]] = new string[]   // dict key   = offset
                        { correction["token"], suggestion["suggestion"] };  // dict value = {error, correction}
            }

            // Apply spelling corrections, in order, from right to left
            foreach (int i in corrections.Keys)
            {
                var oldtext = corrections[i][0];
                var newtext = corrections[i][1];

                // Apply capitalization from original text to correction - all caps or initial caps
                if (text.Substring(i, oldtext.Length).All(char.IsUpper)) newtext = newtext.ToUpper();
                else if (char.IsUpper(text[i])) newtext = newtext[0].ToString().ToUpper() + newtext.Substring(1);

                text = text.Substring(0, i) + newtext + text.Substring(i + oldtext.Length);
            }
            return text;
        }*/
        // NOTE:
        // In the following sections, we'll add code below this.

        // ***** PERFORM TRANSLATION ON BUTTON CLICK
        private void TranslateButton_Click(object sender, EventArgs e)
        {
            try
            {
                string textToTranslate = TextToTranslate.Text.Trim();
                string toLanguageCode = languageCodesAndTitles[ToLanguageComboBox.SelectedValue.ToString()];
                string fromLanguage = FromLanguageComboBox.SelectedValue.ToString();
                string fromLanguageCode;

                // auto-detect source language if requested
                if (fromLanguage == "Detect")
                {
                    TextTranslated.Text = translator.Translate(toLanguageCode, textToTranslate);
                }
                else
                {
                    TextTranslated.Text = translator.Translate(fromLanguage, toLanguageCode, textToTranslate);
                }

                

                // spell-check the source text if the source language is English
                /*if (fromLanguageCode == "en")
                {
                    if (textToTranslate.StartsWith("-"))    // don't spell check in this case
                        textToTranslate = textToTranslate.Substring(1);
                    else
                    {
                        textToTranslate = CorrectSpelling(textToTranslate);
                        TextToTranslate.Text = textToTranslate;     // put corrected text into input field
                    }
                }*/
                // handle null operations: no text or same source/target languages
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
