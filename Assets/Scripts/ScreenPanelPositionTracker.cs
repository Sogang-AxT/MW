using System;
using UnityEngine;

public class ScreenPanelPositionTracker : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
        other.GetComponentInParent<VideoScreenPanelScroller>().isScrollable = false;
    }
}