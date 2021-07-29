using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasterrLib.classes
{
    public class castUtility
    {
    }
    public enum MediaStatus
    {
        Opening,
        Stop,
        Pause,
        Buffering,
        Playing,
        Error,
        Corked,
    }
    public class Cast_RendererItem
    {
        public String id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool CanRenderVideo { get; set; }
        public bool CanRenderAudio { get; set; }
        public Cast_PlaybackValues playback { get; set; }
    }
    public class Cast_MicroRendererItem
    {
        public String id { get; set; }
        public Cast_PlaybackValues playback { get; set; }
    }
    public class Cast_Position
    {
        public String id { get; set; }
        public float position { get; set; }
    }
    public class Cast_PlaybackValues
    {
        public MediaStatus status { get; set; }
        public float volume { get; set; }
        public Boolean mute { get; set; }
        public string mediaTitle { get; set; }
        public int seekable { get; set; }
        public float position { get; set; }
        public long mediaLength { get; set; }
        public int audioTrack { get; set; }
        public int subTrack { get; set; }
        public List<Cast_AudioTrack> audioTracks { get; set; } = new List<Cast_AudioTrack>();
        public List<Cast_SubtitleTrack> subTracks { get; set; } = new List<Cast_SubtitleTrack>();
    }
    public class Cast_AudioTrack
    {
        public int index { get; set; }
        public uint channels { get; set; }
        public uint rate { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
    }
    public class Cast_SubtitleTrack
    {
        public int index { get; set; }
        public string Encoding { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
    }
}
