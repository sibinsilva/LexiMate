using Google.Cloud.Vision.V1;
using Newtonsoft.Json.Linq;

namespace LexiMate
{
    public partial class MainPage
    {
        private void ShowResult(dynamic? annotations)
        {
            var resultText = "";
            if (annotations == null)
            {
                lblResult.Text = "Unable to Identify";
                AudioFileText = "Unable to Identify Texts";
                return;
            }
            resultText = annotations[0].ToString();
            EnableLabels();
            lblResult.Text = resultText;
            GoogleClientFactory clientFactory = new GoogleClientFactory("translate");
            var translated_text = clientFactory.TranslateText(resultText,"en-US");
            TranslatedTextLabel.Text = translated_text;
            AudioFileText = translated_text;
        }

        private void EnableLabels()
        {
            activity.IsRunning = false;
            lblImageText.IsVisible = true;
            lblLang.IsVisible = true;
            lblTranslated.IsVisible = true;
            frmImgText.IsVisible = true;
            frmResult.IsVisible = true;
            frmTranslatedResult.IsVisible = true;
            Play.IsVisible = true;
            lblResult.Text = string.Empty;
            TranslatedTextLabel.Text = string.Empty;
            DetectedLanguageLabel.Text = string.Empty;
        }

        private void HandleSpeakCancel()
        {
            IsSpeaking = false;
            if (_speakButtonCancellationTokenSource?.IsCancellationRequested ?? true)
                return;

            _speakButtonCancellationTokenSource.Cancel();
        }
    }
}
