using System;
using System.Collections.Generic;
using System.Text;
using CasterrLib.classes;
using LibVLCSharp.Shared;
using Microsoft.Extensions.Logging;

namespace CastService
{
    public class mediaPlayingUpdateEventArgs : EventArgs
    {
        public Cast_RendererItem itemInfo { get; set; }
    }
    public class mediaPlayingPosEventArgs : EventArgs
    {
        public String id { get; set; }
        public float position { get; set; }
    }
    class RendererMediaPlayer
    {
        private readonly ILogger<WebSocketService> _logger;
        private MediaPlayer _mediaPlayer;
        private LibVLC _libVLC;
        private Media _mediaFile;
        public Cast_RendererItem itemInfo;
        public event EventHandler<mediaPlayingUpdateEventArgs> mediaPlayingUpdate;
        public event EventHandler<mediaPlayingPosEventArgs> mediaPlayingPositionUpdate;
        public RendererMediaPlayer(LibVLC libVlc, RendererItem renderer, ILogger<WebSocketService> logger)
        {
            _logger = logger;
            _libVLC = libVlc;
            _mediaPlayer = new MediaPlayer(libVlc);
            _mediaPlayer.Volume = 1;
            _mediaPlayer.SetRenderer(renderer);
            itemInfo = new Cast_RendererItem()
            {
                CanRenderAudio = renderer.CanRenderAudio,
                CanRenderVideo = renderer.CanRenderVideo,
                Name = renderer.Name,
                Type = renderer.Type,
                id = Guid.NewGuid().ToString(),
                playback = new Cast_PlaybackValues()
                {
                    volume = _mediaPlayer.Volume,
                    mute = _mediaPlayer.Mute,
                    status = MediaStatus.Stop
                }
            };
            _mediaPlayer.MediaChanged += MediaPlayer_MediaChanged;

            _mediaPlayer.SeekableChanged += MediaPlayer_SeekingChanged;

            _mediaPlayer.PositionChanged += MediaPlayerPosChanged;
            _mediaPlayer.LengthChanged += MediaPlayer_LengthChanged;

            _mediaPlayer.VolumeChanged += MediaPlayer_VolumeChanged;

            _mediaPlayer.Muted += (sender,e) => MediaPlayer_Mute(sender, e,true);
            _mediaPlayer.Unmuted += (sender, e) => MediaPlayer_Mute(sender, e, false);

            _mediaPlayer.Opening += (sender, e) => MediaPlayer_Status(sender, e, MediaStatus.Opening);
            _mediaPlayer.Buffering += (sender, e) => MediaPlayer_Status(sender, e, MediaStatus.Buffering);
            _mediaPlayer.Playing += (sender, e) => MediaPlayer_Status(sender, e, MediaStatus.Playing);
            _mediaPlayer.Stopped += (sender, e) => MediaPlayer_Status(sender, e, MediaStatus.Stop);
            _mediaPlayer.EndReached += (sender, e) => MediaPlayer_Status(sender, e, MediaStatus.Stop);
            _mediaPlayer.Paused += (sender, e) => MediaPlayer_Status(sender, e, MediaStatus.Pause);
            

            _mediaPlayer.EncounteredError += (sender, e) => MediaPlayer_Status(sender, e, MediaStatus.Error);
            _mediaPlayer.Corked += (sender, e) => MediaPlayer_Status(sender, e, MediaStatus.Corked);
            _mediaPlayer.Uncorked += (sender, e) => MediaPlayer_Status(sender, e, MediaStatus.Playing);

            _mediaPlayer.ScrambledChanged += MediaPlayer_Scrambled;

            _mediaPlayer.ESSelected += MediaPlayer_ESSelected;
        }

        private void MediaPlayer_ESSelected(object sender, MediaPlayerESSelectedEventArgs e)
        {
            switch (e.Type)
            {
                case TrackType.Audio:
                    _logger.LogInformation("ESSelected Audio: {0}",e.Id);
                    itemInfo.playback.audioTrack = e.Id;
                    break;
                case TrackType.Text:
                    itemInfo.playback.subTrack = e.Id;
                    _logger.LogInformation("ESSelected Text: {0}", e.Id);
                    break;
            }
            onMediaUpdate();
        }

        private void MediaPlayer_Scrambled(object sender, EventArgs e)
        {
            _logger.LogInformation("MediaPlayer_Scrambled");
        }

        //playback controls
        public void StartPlayingUrl(String mrl)
        {
            _mediaFile = new Media(_libVLC, new Uri(mrl));
            _mediaFile.ParsedChanged += MediaFile_ParseChanged;
            _mediaPlayer.Play(_mediaFile);
        }
        private void MediaFile_ParseChanged(object sender, MediaParsedChangedEventArgs e)
        {
            if(e.ParsedStatus == MediaParsedStatus.Done)
            {
                itemInfo.playback.audioTracks.Clear();
                itemInfo.playback.subTracks.Clear();
                foreach (MediaTrack aTrack in _mediaFile.Tracks)
                {
                    switch (aTrack.TrackType)
                    {
                        case TrackType.Audio:
                            itemInfo.playback.audioTracks.Add(new Cast_AudioTrack()
                            {
                                index = aTrack.Id,
                                channels = aTrack.Data.Audio.Channels,
                                rate = aTrack.Data.Audio.Rate,
                                Language = aTrack.Language,
                                Description = aTrack.Description
                            });
                            break;
                        case TrackType.Text:
                            itemInfo.playback.subTracks.Add(new Cast_SubtitleTrack()
                            {
                                index = aTrack.Id,
                                Description = aTrack.Description,
                                Language = aTrack.Language,
                                Encoding = aTrack.Data.Subtitle.Encoding
                            });
                            break;
                    }
                }
                onMediaUpdate();
                _logger.LogInformation("ParsingDone");
            }
        }

        public void playback(int code)
        {
            switch (code)
            {
                case 1:
                    _mediaPlayer.Pause();
                    break;
                case 2:
                    _mediaPlayer.Play();
                    break;
                case 3:
                    _mediaPlayer.Stop();
                    break;
                case 4:
                    _logger.LogInformation("MediaPlayer_Back");
                    break;
                case 5:
                    _logger.LogInformation("MediaPlayer_Forw");
                    break;
                case 6:
                    _mediaPlayer.Mute = true;
                    break;
                case 7:
                    _mediaPlayer.Mute = false;
                    break;
            }
        }
        public void volume(int vol)
        {
            _mediaPlayer.Volume = vol;
        }
        public void seek(float pos)
        {
            if (_mediaPlayer.IsSeekable)
                _mediaPlayer.Position = pos;
        }
        public void changeAudioTrack(int trackIndex)
        {
            if(!_mediaPlayer.SetAudioTrack(trackIndex))
                _logger.LogError("Cannot change audio track");
        }
        public void changeSubTrack(int trackIndex)
        {
            if(!_mediaPlayer.SetSpu(trackIndex))
                _logger.LogError("Cannot change subtitle track");
        }
        // Others
        public void release()
        {

        }

        // Events from mediaplayer
        private void MediaPlayer_Status(object sender, EventArgs e, MediaStatus status)
        {           
            itemInfo.playback.status = status;
            onMediaUpdate();
        }
        private void MediaPlayer_Mute(object sender, EventArgs e, Boolean muted)
        {
            _logger.LogInformation("MediaPlayer_Mute");
            itemInfo.playback.mute = muted;
        }

        private void MediaPlayer_VolumeChanged(object sender, MediaPlayerVolumeChangedEventArgs e)
        {
            _logger.LogInformation("MediaPlayer_VolumeChanged");
            itemInfo.playback.volume = e.Volume;
            onMediaUpdate();
        }
        protected virtual void onMediaUpdate()
        {
            EventHandler<mediaPlayingUpdateEventArgs> handler = mediaPlayingUpdate;
            handler?.Invoke(this, new mediaPlayingUpdateEventArgs(){itemInfo = itemInfo});
        }
        protected virtual void onMediaPositionUpdate()
        {
            EventHandler<mediaPlayingPosEventArgs> handler = mediaPlayingPositionUpdate;
            handler?.Invoke(this, new mediaPlayingPosEventArgs(){id = itemInfo.id, position = itemInfo.playback.position});
        }
        private void MediaPlayer_LengthChanged(object sender, MediaPlayerLengthChangedEventArgs e)
        {
            itemInfo.playback.mediaLength = e.Length;
            onMediaUpdate();
        }

        private void MediaPlayerPosChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            itemInfo.playback.position = e.Position;
            onMediaPositionUpdate();
        }

        private void MediaPlayer_SeekingChanged(object sender, MediaPlayerSeekableChangedEventArgs e)
        {
            _logger.LogInformation("MediaPlayer_SeekingChanged");
            itemInfo.playback.seekable = e.Seekable;
            onMediaUpdate();
        }

        private void MediaPlayer_MediaChanged(object sender, MediaPlayerMediaChangedEventArgs e)
        {
            _logger.LogInformation("MediaPlayer_MediaChanged");
            onMediaUpdate();
        }
    }
}
