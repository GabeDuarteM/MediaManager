// Developed by: Gabriel Duarte
// 
// Created at: 20/07/2015 21:10
// Last update: 19/04/2016 02:57

using System;
using Newtonsoft.Json;

namespace MediaManager.Model
{
    public class Usuario
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

        //public Images images { get; set; }

        //[JsonProperty("images")]
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
        public Usuario user { get; set; }

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
