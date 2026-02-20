using System;
using UnityEngine;

public class ScreenPanelPositionTracker : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        other.GetComponentInParent<VideoScreenPanelScroller>().isScrollable = false;
    }
}