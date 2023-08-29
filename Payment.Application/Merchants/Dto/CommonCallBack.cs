using Newtonsoft.Json;

namespace Payment.Application.Notifications.Dto
{
    public class CommonCallBack
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}
