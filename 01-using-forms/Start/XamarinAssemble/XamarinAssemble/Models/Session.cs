using Newtonsoft.Json;
using System;

namespace XamarinAssemble.Models
{
    public class Session
    {
        [JsonProperty("end")]
        public DateTime End { get; set; }

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("abstract")]
        public string Abstract { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("presenter")]
        public string Presenter { get; set; }

        [JsonProperty("biography")]
        public string Biography { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("room")]
        public string Room { get; set; }
    }
}
