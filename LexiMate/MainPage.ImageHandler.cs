using Google.Cloud.Vision.V1;
using ImageSource = Microsoft.Maui.Controls.ImageSource;

namespace LexiMate
{
    public partial class MainPage
    {

        private async Task HandlePhotoSelection()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Pick a photo"
                });

                if (result == null)
                    return;

                activity.IsRunning = true;

                var stream = await result.OpenReadAsync();
                var imageBytes = await ConvertStreamToByteArrayAsync(stream);
                imgSelected.Source = ImageSource.FromStream(() => stream);
                GoogleClientFactory clientFactory = new GoogleClientFactory("vision");
                var annotations = await Task.Run(() => clientFactory.CallVisionApi(imageBytes, "text_detection"));

                ShowResult(annotations);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task HandlePhotoCapture()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take a photo"
                });

                if (result == null)
                    return;

                activity.IsRunning = true;

                var stream = await result.OpenReadAsync();
                var imageBytes = await ConvertStreamToByteArrayAsync(stream);
                imgSelected.Source = ImageSource.FromStream(() => stream);

                GoogleClientFactory clientFactory = new GoogleClientFactory("vision");
                var annotations = await Task.Run(() => clientFactory.CallVisionApi(imageBytes, "text_detection"));

                ShowResult(annotations);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task<byte[]> ConvertStreamToByteArrayAsync(Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
