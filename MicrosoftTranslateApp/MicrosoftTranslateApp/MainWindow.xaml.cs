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

        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(HandleExceptions);

            InitializeComponent();

            translator = new GoogleTranslator(MicrosoftTranslateApp.Resources.ResourcesKeys.GoogleJson);

            PopulateLanguageMenus();
        }
        
        private void PopulateLanguageMenus()
        {
            FromLanguageComboBox.Items.Add("Detect");

            languageCodesAndTitles = translator.Languages;

            foreach (string menuItem in languageCodesAndTitles.Keys)
            {
                FromLanguageComboBox.Items.Add(menuItem);
                ToLanguageComboBox.Items.Add(menuItem);
            }

            FromLanguageComboBox.SelectedIndex = 0;
            ToLanguageComboBox.SelectedIndex = 0;
        }

        // ***** PERFORM TRANSLATION ON BUTTON CLICK
        private void TranslateButton_Click(object sender, EventArgs e)
        {
            try
            {
                string textToTranslate = TextToTranslate.Text.Trim();
                string toLanguageCode = languageCodesAndTitles[ToLanguageComboBox.SelectedValue.ToString()];
                string fromLanguage = FromLanguageComboBox.SelectedValue.ToString();
                string fromLanguageCode;

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
