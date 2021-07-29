﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Casterr.Data.classes
{
    public class CasterrConfig
    {
        private string _configFile;
        public CasterrConfig_Struct config { get; }
        public CasterrConfig(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configFile = Path.Combine(env.ContentRootPath, "config", configuration["configFile"]);
            if (File.Exists(_configFile))
                config = JsonSerializer.Deserialize<CasterrConfig_Struct>(File.ReadAllText(_configFile));
            else
            {
                config = new CasterrConfig_Struct();
                save();
            }
        }

        public EmailSenderOptions emailOptions()
        {
            return config.mailOptions;
        }
        internal void setTmdb(CasterrConfig_tmdbApi tmdbConf)
        {
            config.tmdb = tmdbConf;
            save();
        }
        internal void setSonarr(CasterrConfig_ApiKey apiSonarr)
        {
            config.sonarr = apiSonarr;
            save();
        }
        internal void setRadarr(CasterrConfig_ApiKey apiRadarr)
        {
            config.radarr = apiRadarr;
            save();
        }
        internal void setEmail(EmailSenderOptions emailOptions)
        {
            config.mailOptions = emailOptions;
            save();
        }
        internal void save()
        {
            File.WriteAllText(_configFile, JsonSerializer.Serialize(config));
        }
    }
    public class CasterrConfig_Struct
    {
        public CasterrConfig_ApiKey sonarr { get; set; }
        public CasterrConfig_ApiKey radarr { get; set; }
        public CasterrConfig_tmdbApi tmdb { get; set; }
        public EmailSenderOptions mailOptions { get; set; }
    }
    public class CasterrConfig_ApiKey
    {
        public String url { get; set; }
        public String key { get; set; }
    }
    public class CasterrConfig_tmdbApi
    {
        public String key { get; set; }
        public String lang { get; set; }
    }
    public class IdentityConfOptions
    {
        public bool Password_RequireDigit { get; set; }
        public bool Password_RequireLowercase { get; set; }
        public bool Password_RequireNonAlphanumeric { get; set; }
        public bool Password_RequireUppercase { get; set; }
        public int Password_RequiredLength { get; set; }
        public int Password_RequiredUniqueChars { get; set; }
        public int Lockout_DefaultLockoutMinutes { get; set; }
        public int Lockout_MaxFailedAccessAttempts { get; set; }
        public bool Lockout_AllowedForNewUsers { get; set; }
        public string User_AllowedUserNameCharacters { get; set; }
        public bool User_RequireUniqueEmail { get; set; }
    }
}
