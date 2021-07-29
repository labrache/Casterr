using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CasterrLib.classes;
using LibVLCSharp.Shared;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CastService
{
    public class CastService : BackgroundService
    {
        private readonly ILogger<WebSocketService> _logger;
        private LibVLC _libVLC;
        
        private RendererDiscoverer _rendererDiscoverer;
        private Dictionary<IntPtr, RendererMediaPlayer> _rendererItems = new Dictionary<IntPtr, RendererMediaPlayer>();

        private readonly WebSocketService _webSocket;

        public CastService(ILogger<WebSocketService> logger, WebSocketService webSocket)
        {
            _logger = logger;
            _webSocket = webSocket;
            LibVLCSharp.Shared.Core.Initialize();
            _libVLC = new LibVLC();
        }
        public List<Cast_RendererItem> getCast()
        {
            return _rendererItems.Values.Select(s => s.itemInfo).ToList();
        }
        private Boolean GetMediaPlayer(String id, out RendererMediaPlayer player)
        {
            player = null;
            if (_rendererItems.Where(s => s.Value.itemInfo.id == id).Count() == 1)
            {
                player = _rendererItems.Where(s => s.Value.itemInfo.id == id).Select(s => s.Value).SingleOrDefault();
                return true;
            }
            else
            {
                _logger.LogError("Cannot find renderer");
                return false;
            }
        }
        public void StartPlaying(String id, String url)
        {
            if(GetMediaPlayer(id, out RendererMediaPlayer player))
                player.StartPlayingUrl(url);
        }
        public void ControlPlayback(String id,int code)
        {
            if (GetMediaPlayer(id, out RendererMediaPlayer player))
                player.playback(code);
        }
        public void ControlPosition(String id, int pos)
        {
            if (pos >= 0 && pos <= 100)
            {
                float newpos = (float)pos / (float)100;
                if (GetMediaPlayer(id, out RendererMediaPlayer player))
                    player.seek(newpos);
            }

        }
        public void ControlVolume(String id, int vol)
        {
            if (vol >= 0 && vol <= 100)
            {
                if (GetMediaPlayer(id, out RendererMediaPlayer player))
                    player.volume(vol);
            }
        }
        public void setTrack(String id, Boolean isAudio, int track)
        {
            if (GetMediaPlayer(id, out RendererMediaPlayer player))
                if (isAudio)
                    player.changeAudioTrack(track);
                else
                    player.changeSubTrack(track);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting CastService...");
            
            _rendererDiscoverer = new RendererDiscoverer(_libVLC);
            _rendererDiscoverer.ItemAdded += RendererDiscoverer_ItemAdded;
            _rendererDiscoverer.ItemDeleted += RendererDiscoverer_ItemRemoved;
            if (!_rendererDiscoverer.Start())
                _logger.LogError("Cannot start rendererDiscoverer");
            _logger.LogInformation("Started CastService");

            while (!stoppingToken.IsCancellationRequested)
            {
                //await _webSocket.updateRenderers(_rendererItems.Values.Select(s => s.itemInfo).ToList());
                await Task.Delay(1000);
            }
        }
        private void RendererDiscoverer_ItemRemoved(object sender, RendererDiscovererItemDeletedEventArgs e)
        {           
            _logger.LogInformation("Deleted {0}: {1}", e.RendererItem.Type, e.RendererItem.Name);
            _rendererItems[e.RendererItem.NativeReference].release();
            _rendererItems.Remove(e.RendererItem.NativeReference);
            _webSocket.updateRenderers(_rendererItems.Values.Select(s => s.itemInfo).ToList()).Wait();
        }

        private void RendererDiscoverer_ItemAdded(object sender, RendererDiscovererItemAddedEventArgs e)
        {
            _logger.LogInformation("Discover {0}: {1}, (Support: video: {2}, audio: {3})",e.RendererItem.Type, e.RendererItem.Name, e.RendererItem.CanRenderVideo, e.RendererItem.CanRenderAudio);
            RendererMediaPlayer rmp = new RendererMediaPlayer(_libVLC, e.RendererItem,_logger);
            rmp.mediaPlayingUpdate += onMediaUpdate;
            rmp.mediaPlayingPositionUpdate += onMediaPositionUpdate;
            _rendererItems.Add(e.RendererItem.NativeReference, rmp);
            _webSocket.updateRenderers(_rendererItems.Values.Select(s => s.itemInfo).ToList()).Wait();
        }
        private void onMediaPositionUpdate(object sender, mediaPlayingPosEventArgs e)
        {
            _webSocket.updateRendererPosition(e.id, e.position).Wait();
        }
        private void onMediaUpdate(object sender, mediaPlayingUpdateEventArgs e)
        {
            _webSocket.updateRendererInfo(e.itemInfo).Wait();
        }
    }
}
