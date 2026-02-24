using System;
using UnityEngine;

public class ScreenPanelPositionTracker : MonoBehaviour {
    private int _playerCount;
    
    
    private void OnTriggerEnter(Collider other) {
        if (!VideoScreenPanelScroller.IsScrollable) return;
        
        var tf = other.GetComponent<RectTransform>();
        var height = other.bounds.size.y;
        var targetPos = tf.position + Vector3.up * (height * 5f);

        if (VideoScreenPanelScroller.IsTimeOut && other.CompareTag("Finish")) {
            this._playerCount += 1;
            
            if (this._playerCount >= 5) {
                this._playerCount = 0;
                VideoScreenPanelScroller.IsScrollable = false;
            }
            
            return;
        }
        
        tf.position = targetPos;
    }
}