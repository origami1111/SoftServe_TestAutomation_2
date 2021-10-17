﻿using Newtonsoft.Json;
using System;

namespace WHAT_Utilities
{
    public class WhatAccount
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }

        [JsonProperty("isActive")]
        public Activity Activity { get; set; }

        public override bool Equals(object obj)
        {
            WhatAccount other = (WhatAccount)obj;

            return (this.Email == other.Email
                && this.FirstName == other.FirstName
                && this.LastName == other.LastName
                && this.Role == other.Role
                && this.Activity == other.Activity);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
