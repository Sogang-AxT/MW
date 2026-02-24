using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VideoScreenPanelScroller : MonoBehaviour {
    public static bool IsScrollable;
    public static bool IsTimeOut;

    [SerializeField] private RectTransform playerScreen;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float scrollTime;
    
    private List<Vector3> _playersPos;
    private List<RectTransform> _playersTF;
    private RectTransform _playerGroupTF;
    private Vector3 _playerGroupPos;
    private bool _isTimerRunning;
    private float _scrollTime;
    
    
    private void Init() {
        this._playerGroupTF = this.GetComponent<RectTransform>();
        this._playersTF = this.GetComponentsInChildren<RectTransform>().ToList();
        this._playersPos = new();
        
        this._scrollTime = this.scrollTime;
        this._isTimerRunning = false;
        this._playerGroupPos = this._playerGroupTF.position;
        
        foreach (var playerTF in this._playersTF) {
            this._playersPos.Add(playerTF.position);
        }
    }

    private void Awake() {
        Init();
    }

    private void Update() {
        PanelScroll();
    }

    public void OnButtonClick() {
        if (IsScrollable) return;
        
        IsScrollable = true;
        IsTimeOut = false;
        
        this.scrollTime = this._scrollTime;
        this._playerGroupTF.position = this._playerGroupPos;
        
        var i = 0;
        foreach (var playerTF in this._playersTF) {
            playerTF.position = this._playersPos[i];
            i++;
        }
        
        if (!this._isTimerRunning) {
            this._isTimerRunning = true;
            StartCoroutine(PlayTimer());
        }
    }

    private IEnumerator PlayTimer() {
        while (this.scrollTime > 0) {
            yield return new WaitForSeconds(1);
            this.scrollTime -= 1;
        }
        
        IsTimeOut = true;
        this._isTimerRunning = false;
        this.scrollTime = this._scrollTime;
    }
    
    private void PanelScroll() {
        if (!IsScrollable) return;
        if (this.scrollSpeed == 0f) Debug.LogWarning("ScrollSpeed is zero");
        
        this._playerGroupTF.anchoredPosition += Vector2.down * (this.scrollSpeed * Time.deltaTime);
    }
}