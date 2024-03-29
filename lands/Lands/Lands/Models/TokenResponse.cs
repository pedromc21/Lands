﻿namespace Lands.Models
{
    using System;
    using Newtonsoft.Json;

    public class TokenResponse
    {
        #region Properties
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = ".issued")]
        public string Issued { get; set; }

        [JsonProperty(PropertyName = ".expires")]
        public string Expires { get; set; }

        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }
        #endregion
    }
}
