using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Casterr.Data.classes.media
{
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private ApplicationDbContext _ctx;
        private IConfiguration _conf;
        private IWebHostEnvironment _env;
        private IEmailSender _mail;

        public WebhookController(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment env)
        {
            _ctx = context;
            _conf = configuration;
        }
        [HttpPost]
        [Route("webhook/sonarr")]
        public async Task<IActionResult> PostSonarrAsync(WH_Sonarr sonarrPush)
        {
            //TODO: update
            return new JsonResult(true);
        }
        [HttpPost]
        [Route("webhook/radarr")]
        public async Task<IActionResult> PostRadarrAsync(WH_Radarr radarrPush)
        {
            //TODO: update
            return new JsonResult(true);
        }
    }
}
