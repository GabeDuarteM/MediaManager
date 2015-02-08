using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MediaManager.Code.Modelos
{
    [DataContract]
    public class Avatar
    {
        [JsonProperty("full")]
        public string full { get; set; }
    }

    public class Image
    {
        [JsonProperty("avatar")]
        public Avatar avatar { get; set; }
    }

    public class User
    {
        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("private")]
        public bool privateProfile { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("vip")]
        public bool vip { get; set; }

        [JsonProperty("joined_at")]
        public DateTime joined_at { get; set; }

        [JsonProperty("location")]
        public string location { get; set; }

        [JsonProperty("about")]
        public string about { get; set; }

        [JsonProperty("gender")]
        public string gender { get; set; }

        [JsonProperty("age")]
        public int age { get; set; }

        [JsonProperty("images")]
        public Image images { get; set; }
    }

    public class Account
    {
        [JsonProperty("timezone")]
        public string timezone { get; set; }

        [JsonProperty("cover_image")]
        public object cover_image { get; set; }

        [JsonProperty("token")]
        public object token { get; set; }
    }

    public class Connections
    {
        [JsonProperty("facebook")]
        public bool facebook { get; set; }

        [JsonProperty("twitter")]
        public bool twitter { get; set; }

        [JsonProperty("google")]
        public bool google { get; set; }

        [JsonProperty("tumblr")]
        public bool tumblr { get; set; }
    }

    public class SharingText
    {
        [JsonProperty("watching")]
        public string watching { get; set; }

        [JsonProperty("watched")]
        public string watched { get; set; }
    }

    public class UserInfo
    {
        [JsonProperty("user")]
        public User user { get; set; }

        [JsonProperty("account")]
        public Account account { get; set; }

        [JsonProperty("connections")]
        public Connections connections { get; set; }

        [JsonProperty("sharing_text")]
        public SharingText sharing_text { get; set; }
    }

    public class UserAuth
    {
        [JsonProperty("access_token")]
        public string access_token { get; set; }

        [JsonProperty("token_type")]
        public string token_type { get; set; }

        [JsonProperty("expires_in")]
        public string expires_in { get; set; }

        [JsonProperty("refresh_token")]
        public string refresh_token { get; set; }

        [JsonProperty("scope")]
        public string scope { get; set; }
    }
}