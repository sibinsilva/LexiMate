using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;


namespace LexiMate
{
    public class GoogleClientFactory
    {
        internal RestClient client;
        internal string functionUrl;
        private readonly ISecretService _secretService;

        internal GoogleClientFactory(string function)
        {
            client = new RestClient();
            _secretService = new SecretService();
            functionUrl = buildUrl(function);
        }

        private string? buildUrl(string function)
        {
            string baseUrl = _secretService.GetSecretValue("Base_URL");

            switch (function)
            {
                case "vision":
                    return $"{baseUrl}/googlevisionclient";
                case "translate":
                    return $"{baseUrl}/googletranslateclient";
                case "texttospeech":
                    return $"{baseUrl}/texttospeech";
                default:
                    throw new ArgumentException("Invalid function name");
            }
        }

        public string TranslateText(string text, string targetLanguage)
        {
            var request = new RestRequest(this.functionUrl, Method.Post);
            request.AddJsonBody(new { text = text, target_language = targetLanguage });
            request.AddHeader("Content-Type", "application/json");

            try
            {
                var response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"HTTP request failed with status code {response.StatusCode}");
                }

                dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
                if (jsonResponse == null || jsonResponse.translated_text == null)
                {
                    throw new Exception("Translated text not found in the response");
                }
                string translatedText = jsonResponse.translated_text;

                return translatedText;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the request.", ex);
            }
        }

        public object CallVisionApi(byte[] imageBytes, string feature)
        {
            var request = new RestRequest(this.functionUrl, Method.Post);
            request.AddJsonBody(new { image_bytes = imageBytes, feature = feature });
            request.AddHeader("Content-Type", "application/json");

            try
            {
                var response = client.Execute(request);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"HTTP request failed with status code {response.StatusCode}");
                }
                var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);

                // Handle detected texts
                if (responseData.ContainsKey("detected_texts"))
                {
                    return responseData["detected_texts"];
                }
                // Handle label annotations
                else if (responseData.ContainsKey("label_annotations"))
                {
                    return responseData["label_annotations"];
                }
                // Handle face annotations
                else if (responseData.ContainsKey("face_annotations"))
                {
                    return responseData["face_annotations"];
                }
                // Handle landmark annotations
                else if (responseData.ContainsKey("landmark_annotations"))
                {
                    return responseData["landmark_annotations"];
                }
                // Handle logo annotations
                else if (responseData.ContainsKey("logo_annotations"))
                {
                    return responseData["logo_annotations"];
                }
                // Handle localized object annotations
                else if (responseData.ContainsKey("localized_object_annotations"))
                {
                    return responseData["localized_object_annotations"];
                }
                // Handle image properties annotation
                else if (responseData.ContainsKey("image_properties_annotation"))
                {
                    return responseData["image_properties_annotation"];
                }
                // Handle safe search annotation
                else if (responseData.ContainsKey("safe_search_annotation"))
                {
                    return responseData["safe_search_annotation"];
                }
                // Handle web detection
                else if (responseData.ContainsKey("web_detection"))
                {
                    return responseData["web_detection"];
                }
                else
                {
                    return "Unsupported feature or no data returned.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the request.", ex);
            }
        }
    }
}

