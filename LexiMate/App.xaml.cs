using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Microsoft.Maui.Networking;

namespace LexiMate
{
    public partial class App : Application
    {
        private static Label labelscreen;
        private static Timer timer;
        private static bool IsInternetAvailable;
        private static Page CurrentPage;
        public static bool NoInternetShowAlert;

        private readonly iNetworkChecker _networkChecker;

        public App(iNetworkChecker networkChecker)
        {
            InitializeComponent();
            _networkChecker = networkChecker;
            MainPage = new MainPage();
        }

        public static void CheckInternetConnectivity(Label label, Page page)
        {
            if (label == null || page == null)
            {
                throw new ArgumentNullException(nameof(label), "Label and Page cannot be null.");
            }

            labelscreen = label;
            label.Text = "No Internet Connection";
            label.IsVisible = false;
            IsInternetAvailable = true;
            CurrentPage = page;

            if (timer == null)
            {
                timer = new Timer((e) =>
                {
                    CheckInternetOverTime();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        private static void CheckInternetOverTime()
        {
            try
            {
                var networkChecker = IPlatformApplication.Current.Services.GetService<iNetworkChecker>();
                if (networkChecker == null)
                {
                    throw new InvalidOperationException("NetworkChecker service is not available.");
                }

                var isConnected = networkChecker.IsConnected;
                if (!isConnected)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        if (IsInternetAvailable)
                        {
                            if (!NoInternetShowAlert)
                            {
                                IsInternetAvailable = false;
                                if (labelscreen != null)
                                {
                                    labelscreen.IsVisible = true;
                                }
                                await ShowDisplayAlert();
                            }
                        }
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        IsInternetAvailable = true;
                        if (labelscreen != null)
                        {
                            labelscreen.IsVisible = false;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                // Log exception (you might use a logging framework or simply debug output)
                Console.WriteLine($"Exception in CheckInternetOverTime: {ex.Message}");
            }
        }

        private static async Task ShowDisplayAlert()
        {
            if (CurrentPage != null)
            {
                NoInternetShowAlert = false;
                await CurrentPage.DisplayAlert("Internet", "Internet Connection Lost. Please reconnect and try again", "Ok");
                NoInternetShowAlert = false;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
