using System;
using System.Windows;
using System.Collections.Generic;
using MicrosoftTranslateApp.Traslators;
using MicrosoftTranslateApp.SpeechToText;
using Microsoft.Win32;
using System.Threading;

namespace MicrosoftTranslateApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread recognizeThread = null;

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

        private void TranslateButton_Click(object sender, EventArgs e)
        {
            try
            {
                string textToTranslate = TextToTranslate.Text.Trim();

                TextTranslated.Text = Translate(textToTranslate);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TranslateFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();

                open.ShowDialog();

                string filePath = open.FileName;

                if (string.IsNullOrEmpty(open.FileName))
                {
                    return;
                }

                if(recognizeThread != null)
                {
                    recognizeThread.Abort();
                    recognizeThread = null;
                }

                ISpeechToText speechToText = new WindowsSpeechToText();

                recognizeThread = new Thread(() =>
                {
                    string recognizedText = speechToText.RecognizeFile(filePath);

                    Dispatcher.Invoke(() => TextTranslated.Text = Translate(recognizedText));

                    recognizeThread = null;
                });

                recognizeThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string Translate(string textToTranslate)
        {
            string toLanguageCode = languageCodesAndTitles[ToLanguageComboBox.SelectedValue.ToString()];
            string fromLanguage = FromLanguageComboBox.SelectedValue.ToString();

            if (fromLanguage == "Detect")
            {
                return translator.Translate(toLanguageCode, textToTranslate);
            }

            return translator.Translate(fromLanguage, toLanguageCode, textToTranslate);
        }
    }
}
