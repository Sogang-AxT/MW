using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour {
    [SerializeField] private GameControlDictionary.VideoPlayerDic videoPlayerDic;

    private string _videoURL;
    private List<VideoFromPy.Segment> _segments;
    
    private readonly Dictionary<int, VideoPlayer.EventHandler> _prepareHandlers = new();
    
    public void VideoSetup(VideoFromPy.VideoInfoDto videoInfoDto) {
        this._videoURL = videoInfoDto.videoPath;
        this._segments = videoInfoDto.segments;

        foreach (var segment in this._segments) {
            var id = segment.id;
            var startT = segment.start;

            var player = this.videoPlayerDic[id];

            if (this._prepareHandlers.TryGetValue(id, out var oldHandler) && oldHandler != null) {
                player.prepareCompleted -= oldHandler;
            }
            
            player.url = this._videoURL;

            VideoPlayer.EventHandler handler = source => {
                source.time = startT;
                source.Play();
            };
            
            this._prepareHandlers[id] = handler;
            player.prepareCompleted += handler;


            this.videoPlayerDic[id].Prepare();
        }
    }
}