using UnityEngine;

public class VideoScreenPanelScroller : MonoBehaviour {
    public bool isScrollable;
    [SerializeField] private RectTransform playerScreen;
    [SerializeField] private float scrollSpeed;
    
    private RectTransform _playerGroupTF;
    private Vector3 _playerGroupPos;
    
    
    private void Init() {
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
        this._playerGroupTF.position = this._playerGroupPos;
        this.isScrollable = true;
    }
    
    private void PanelScroll() {
        if (!this.isScrollable) return;
        if (this.scrollSpeed == 0f) Debug.LogWarning("ScrollSpeed is zero");
        
        this._playerGroupTF.transform.Translate(Vector3.down * (this.scrollSpeed * Time.deltaTime));
    }
}