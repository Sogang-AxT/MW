using System;
using UnityEngine;
using UnityEngine.Serialization;

public class VideoScreenPanelScroller : MonoBehaviour {
    private RectTransform _playerGroupTF;

    public bool isScrollable;
    [SerializeField] private RectTransform playerScreen;
    [SerializeField] private float scrollSpeed;
    
    
    private void Init() {
        this.isScrollable = true;
        this._playerGroupTF = this.GetComponent<RectTransform>();
    }

    private void Awake() {
        Init();
    }

    private void Update() {
        PanelScroll();
    }

    private void PanelScroll() {
        if (!this.isScrollable) return;
        if (this.scrollSpeed == 0f) Debug.LogWarning("ScrollSpeed is zero");
        
        this._playerGroupTF.transform.Translate(Vector3.down * (this.scrollSpeed * Time.deltaTime));
    }

    private void ScreenSizeCalculate() {
        var screenHeight = this.playerScreen.rect.height;
        
        
        
        
        
    }
}