using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class VideoFromPy : MonoBehaviour {
    private string videoDir = Path.Combine(Application.streamingAssetsPath, "videos");
    private string exePath = Path.Combine(Application.streamingAssetsPath, "dist/random_pay_d3.exe");
    
    private string _videoInfo;
    private List<Dictionary<string, int>> _segment;

    
    private void Init() {
        this._videoInfo = string.Empty;
        this._segment = new();
    }

    private void Awake() {
        Init();
    }
    
    public async void OnButtonClick() {
        this._videoInfo = await GetVideoPathAsync();
        UnityEngine.Debug.Log(this._videoInfo);
    }

    private Task<string> GetVideoPathAsync() {
        return Task.Run(() => {
                try {
                    var processStartInfo = new ProcessStartInfo {
                        FileName = this.exePath,
                        Arguments = $"\"{this.videoDir}\"",
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
                        UnityEngine.Debug.LogError($"py Error: {error}");
                    }
                    
                    return result;
                }
                catch (System.Exception e) {
                    UnityEngine.Debug.LogError($"Process Start Error: {e.Message}");
                    return null;
                }
            }
        );
    }
}