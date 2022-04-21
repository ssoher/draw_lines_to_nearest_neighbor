/*
Find Nearest Neighbors
Sarper Soher - https://www.sarpersoher.com
Mar 04 2021 - 10:35
*/

using UnityEngine;

/* NOTE(sarper Mar 04 21):
    Requests it's closest neighbor from 'NearestNeighborFinder' every 'SearchInterval'
    Doesn't run every frame for optimization purposes, if more accuracy is required SearchInterval may be given a lower value
    Keeps a reference to the found neighbor to draw a gizmo line to it
*/
public sealed class NearestNeighborTracker : MonoBehaviour {
    public float SearchInterval;
    public Transform NearestNeighbor;

    private float _lastSearchTime;
    private bool _drawGizmo;

    private void Update() {
        if(Time.time > _lastSearchTime + SearchInterval) {
            _drawGizmo = NearestNeighborFinder.Instance.Find(transform, out NearestNeighbor);
            _lastSearchTime = Time.time;
        }
    }

    private void OnDrawGizmos() {
        if(!_drawGizmo) return;
        Gizmos.DrawLine(transform.position, NearestNeighbor.position);
    }
}