using System.Diagnostics;

namespace LexiMate
{
    public partial class MainPage : ContentPage
    {
        private bool CloseApp = true;
        private static string AudioFileText = null;
        private CancellationTokenSource _speakButtonCancellationTokenSource;
        private bool IsSpeaking = false;

        public MainPage()
        {
            InitializeComponent();
            App.CheckInternetConnectivity(this.lbl_NoInternet, this);
        }

        protected override bool OnBackButtonPressed()
        {
            if (IsSpeaking)
            {
                HandleSpeakCancel();
            }
            if (!CloseApp)
                return false;

            Dispatcher.Dispatch(async () =>
            {
                CloseApp = await DisplayAlert("LexiMate", "Do you want to exit the application?", "No", "Yes");
                if (!CloseApp)
                {
                    Process.GetCurrentProcess().CloseMainWindow();
                    Process.GetCurrentProcess().Close();
                }
                else
                {
                    base.OnBackButtonPressed();
                }
            });

            return CloseApp;
        }

        private async void pickPhoto_Clicked(object sender, EventArgs e)
        {

            await HandlePhotoSelection();
        }

        private async void takePhoto_Clicked(object sender, EventArgs e)
        {

            await HandlePhotoCapture();
        }

        private async void Play_Clicked(object sender, EventArgs e)
        {
            IsSpeaking = true;
            _speakButtonCancellationTokenSource = new CancellationTokenSource();
            await TextToSpeech.SpeakAsync(AudioFileText, _speakButtonCancellationTokenSource.Token);
        }
    }
}
