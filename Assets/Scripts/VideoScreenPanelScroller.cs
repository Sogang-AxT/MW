using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VideoScreenPanelScroller : MonoBehaviour {
    public static bool IsScrollable;
    public static bool IsTimeOut;
    
    [SerializeField] private RectTransform playerScreen;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    
    private float scrollTime;
    private RectTransform _playerGroupTF;
    private Vector3 _playerGroupPos;
    private bool _isTimerRunning;
    
    
    private void Init() {
        this.scrollTime = 20f;
        this._isTimerRunning = false;

        this._playerGroupTF = this.GetComponent<RectTransform>();
        this._playerGroupPos = this._playerGroupTF.position;
    }

    private void Awake() {
        Init();
    }

    private void Update() {
        PanelScroll();
    }

    public void OnButtonClick() {
        if (IsScrollable) return;
        
        this.scrollTime = 20f;
        this._playerGroupTF.position = this._playerGroupPos;
        IsScrollable = true;
        IsTimeOut = false;

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

        this._isTimerRunning = false;
        IsTimeOut = true;
    }
    
    
    private void PanelScroll() {
        if (!IsScrollable) return;
        if (this.scrollSpeed == 0f) Debug.LogWarning("ScrollSpeed is zero");
        
        this._playerGroupTF.transform.Translate(Vector3.down * (this.scrollSpeed * Time.deltaTime));
    }
}