using CasterrLib.classes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casterr.Hubs
{
    public class CastHub : Hub
    {
        private readonly IConfiguration _config;
        private readonly IHubContext<WebHub> _webHub;
        public CastHub(IConfiguration configuration, IHubContext<WebHub> webhub)
        {
            _config = configuration;
            _webHub = webhub;
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public async Task updateRenderers(List<Cast_RendererItem> rendererers)
        {
            await _webHub.Clients.All.SendAsync("ShowRenderer", rendererers);
        }
        public async Task updateRenderersToClient(List<Cast_RendererItem> rendererers, String cid)
        {
            await _webHub.Clients.Client(cid).SendAsync("ShowRenderer", rendererers);
        }
        public async Task updateRendererInfo(Cast_MicroRendererItem rendererer)
        {
            await _webHub.Clients.All.SendAsync("UpdateRenderer", rendererer);
        }
        public async Task updateRendererPos(Cast_Position rendererPos)
        {
            await _webHub.Clients.All.SendAsync("updateRendererPos", rendererPos);
        }
    }
    public class WebHub : Hub
    {
        private readonly IConfiguration _config;
        private readonly IHubContext<CastHub> _castHub;
        public WebHub(IConfiguration configuration, IHubContext<CastHub> casthub)
        {
            _config = configuration;
            _castHub = casthub;
        }
        public async Task StartPlaying(String id, string storage, string fileId)
        {
            await _castHub.Clients.All.SendAsync("StartPlayingUrl", id, storage, fileId);
        }
        public async Task SetPosition(String id, int pos)
        {
            await _castHub.Clients.All.SendAsync("SetPosition", id, pos);
        }
        public async Task SetVolume(String id, int vol)
        {
            await _castHub.Clients.All.SendAsync("SetVolume", id, vol);
        }
        public async Task ControlPlayback(String id, int code)
        {
            await _castHub.Clients.All.SendAsync("ControlPlayback",id, code);
        }
        public async Task setTrack(String id, Boolean isAudio, int track)
        {
            await _castHub.Clients.All.SendAsync("setTrack", id, isAudio, track);
        }
        public async Task Connected()
        {
            await _castHub.Clients.All.SendAsync("Connected", Context.ConnectionId);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
