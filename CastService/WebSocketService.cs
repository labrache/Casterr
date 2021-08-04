using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CasterrLib.classes;
using CastService.Classes;
using LibVLCSharp.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CastService
{
    public class WebSocketService : BackgroundService
    {
        HubConnection connection;
        private readonly ILogger<WebSocketService> _logger;

        private readonly CastService castService;
        private String site;
        private appAuth auth;
        public WebSocketService(ILogger<WebSocketService> logger, IConfiguration config)
        {
            _logger = logger;
            site = config["settings:url"];
            auth = new appAuth(site, config["settings:username"], config["settings:password"]);
            connection = new HubConnectionBuilder().WithUrl(site + "cast", options =>
            {
                options.Cookies = auth.container;
            }).WithAutomaticReconnect().Build();
            connection.Closed += async (error) =>
            {
                _logger.LogWarning("Connection Closed: {0}", error.Message);
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            castService = new CastService(_logger, this);
        }
        private string getResUrl(String storage, String file)
        {
            return String.Format("{0}get/{1}/{2}", site, storage, file);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            AuthenticateRes res = await auth.auth();
            if (res.success)
            {
                connection.On<string>("Connected", (cid) => connection.InvokeAsync("updateRenderersToClient", castService.getCast(), cid));
                connection.On<string, string, string>("StartPlayingUrl", (player, storage, file) => castService.StartPlaying(player, getResUrl(storage, file)));
                connection.On<string, int>("SetVolume", (player, vol) => castService.ControlVolume(player, vol));
                connection.On<string, int>("SetPosition", (player, pos) => castService.ControlPosition(player, pos));
                connection.On<string, int>("ControlPlayback", (player, code) => castService.ControlPlayback(player, code));
                connection.On<string, Boolean, int>("setTrack", (player, isAudio, track) => castService.setTrack(player, isAudio, track));

                try
                {
                    await connection.StartAsync();
                    _logger.LogInformation("Connection started");
                    await castService.StartAsync(stoppingToken);
                    _logger.LogInformation("CastService started");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            } else
            {
                _logger.LogError(res.Message);
            }
        }

        internal async Task updateRenderers(List<Cast_RendererItem> rendererItems)
        {
            if (canSend())
                try
                {
                    await connection.InvokeAsync("updateRenderers", rendererItems);
                }
                catch (Microsoft.AspNetCore.SignalR.HubException e)
                {
                    _logger.LogError(e.Message);
                }
        }
        internal async Task updateRendererInfo(Cast_RendererItem itemInfo)
        {
            if (canSend())
                try
                {
                    await connection.InvokeAsync("updateRendererInfo", new { id = itemInfo.id, playback = itemInfo.playback });
                }
                catch (Microsoft.AspNetCore.SignalR.HubException e)
                {
                    _logger.LogError(e.Message);
                }
        }
        internal async Task updateRendererPosition(String itemid, float position)
        {
            if (canSend())
                try
                {
                    await connection.InvokeAsync("updateRendererPos", new { id = itemid, position = position });
                }
                catch (Microsoft.AspNetCore.SignalR.HubException e)
                {
                    _logger.LogError(e.Message);
                }
        }
        internal Boolean canSend()
        {
            Boolean canSend = connection.State == HubConnectionState.Connected;
            if(!canSend)
                _logger.LogWarning(String.Format("Send fail, State {0}", connection.State.ToString()));
            return canSend;
        }


    }
}
