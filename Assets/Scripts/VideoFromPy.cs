using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable] 
public class VideoFromPy : MonoBehaviour {
    [Serializable]
    public class VideoInfoDto {
        [JsonProperty("video_path")] public string videoPath;
        [JsonProperty("segments")] public List<Segment> segments;
    }

    [Serializable]
    public class Segment {
        public int id;
        public int start;
        public int end;
    }
    
    //--//

    [SerializeField] private VideoScreenPanelScroller _videoScreenPanelScroller;
    [SerializeField] private VideoController _videoController;
    
    private string _videoDir;
    private string _exePath;
    private string _videoInfo;
    
    private SerialPort _serialPort;
    private bool _isProcessing;
    
    
    private void Init() {
        this._videoDir = Path.Combine(Application.streamingAssetsPath, "RandomPlayD3/video");
        this._exePath = Path.Combine(Application.streamingAssetsPath, "RandomPlayD3/dist/random_play_d3.exe");
        this._videoInfo = string.Empty;
        this._isProcessing = false;
        
        try {
            this._serialPort = new SerialPort("COM1", 9600);
            this._serialPort.NewLine = "\n";
            this._serialPort.ReadTimeout = 50;
            this._serialPort.Open();
            
            Debug.Log("Serial Connected to COM8");
        }
        catch (Exception e) {
            Debug.LogError("Serial Error: " + e.Message);
        }
    }

    private void Awake() {
        Init();
    }
    
    private void Update()
    {
        if (_serialPort == null || !_serialPort.IsOpen) return;

        try {
            var data = _serialPort.ReadLine();
            Debug.Log("Serial received: " + data);

            if (data.Trim() == "PLAY") {
                OnButtonClick();
                this._videoScreenPanelScroller.OnButtonClick();
            }
        }
        catch (TimeoutException) {
            // Normal — no data this frame
        }
        catch (Exception e) {
            Debug.LogError("Serial Read Error: " + e.Message);
        }
    }
    
    
    
    public async void OnButtonClick() {
        if (VideoScreenPanelScroller.IsScrollable) return;
        
        this._videoInfo = await GetVideoInfoAsync();
        
        var json = this._videoInfo.Trim();
        Debug.Log(json);

        try {
            var dto = JsonConvert.DeserializeObject<VideoInfoDto>(json);
            this._videoController.VideoSetup(dto);
        }
        catch (Exception e) {
            Debug.LogError("Deserializing Error");
        }
    }

    private Task<string> GetVideoInfoAsync() {
        return Task.Run(() => {
                try {
                    var processStartInfo = new ProcessStartInfo {
                        FileName = this._exePath,
                        Arguments = $"\"{this._videoDir}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using var process = Process.Start(processStartInfo);
                    using var outputReader = process.StandardOutput;
                    using var errorReader = process.StandardError;

                    var result = outputReader.ReadToEnd();
                    var error = errorReader.ReadToEnd();

                    if (!string.IsNullOrEmpty(error)) {
                        Debug.LogError($"py Error: {error}");
                    }
                    
                    return result;
                }
                catch (Exception e) {
                    Debug.LogError($"Process Start Error: {e.Message}");
                    return null;
                }
            }
        );
    }
}