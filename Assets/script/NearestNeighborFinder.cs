/*
Find Nearest Neighbors
Sarper Soher - https://www.sarpersoher.com
Mar 04 2021 - 10:15
*/

using UnityEngine;

/* NOTE(sarper Mar 04 21):
Finds nearest neighbors to the given position

Here is how it works:
    For a maximum of 'MaxSearchSteps' times
        Cast an Overlap Sphere from the given position with a LayerMask for optimization
            Any objects found other than the one requested this operation?
                Iterate over all the found ones, find the closest one by comparing the squared distance (we don't care about the actual distance, don't need a sqr root op.) with the shortest distance thus far
            No objects found?
                Increase the Overlap Sphere size for the next try by 'SearchRadiusIncreasePerStep' and cast again
    If no objects found after 'MaxSearchStep' times, return false
    Otherwise set the nearestNeighbor to the nearest and return true
*/
public sealed class NearestNeighborFinder : MonoBehaviour {
    private static NearestNeighborFinder _instance;

    public static NearestNeighborFinder Instance {
        get {
            if(_instance == null) _instance = FindObjectOfType(typeof(NearestNeighborFinder)) as NearestNeighborFinder;
            if(_instance == null) Debug.LogError("No instance of type NearestNeighborFinder is available in the scene!");
            return _instance;
        }
    }

    private const int MaxSearchSteps = 5;
    private const int MaxResults = 5;
    private const float SearchRadiusIncreasePerStep = 5;

    public LayerMask SearchLayerMask;

    private bool _initialized;
    private float _currentSearchRadius;
    private Collider[] _searchResults;

    /* NOTE(sarper Mar 04 21): nearestNeighbor is set as an 'out' parameter so that we can return bool,
    if we return the found object the result may be null and we would have to check against null
    (see https://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/)
    */
    public bool Find(Transform requester, out Transform nearestNeighbor) {
        // NOTE(sarper Mar 04 21): Lazy initialization, doing everything in Awake add up and increase entering Play Mode duration down the line
        if(!_initialized) {
            _searchResults = new Collider[MaxResults];
            _initialized = true;
        }

        int searchResultCount = 0;
        int tries = 0;
        _currentSearchRadius = 1;

        // NOTE(sarper Mar 04 21): Try finding neighbor 'MaxSearchStep' times until one is found that is not the requester, increase OverlapSphere size each try
        do {
            searchResultCount = Physics.OverlapSphereNonAlloc(requester.position, _currentSearchRadius, _searchResults, SearchLayerMask);
            _currentSearchRadius += _currentSearchRadius * SearchRadiusIncreasePerStep;
            tries++;
        } while((searchResultCount == 0 ||
                (searchResultCount == 1 && _searchResults[0].transform == requester)) &&
                tries < MaxSearchSteps);

        // NOTE(sarper Mar 04 21): Found no neighbors or found 1 but that is the requester, so fail with a warning
        if(searchResultCount == 0 || (searchResultCount == 1 && _searchResults[0].transform == requester)) {
            Debug.LogWarning($"Couldn't find a neighbor in {MaxSearchSteps} tries, try increasing MaxSearchSteps and/or SearchRadiusIncreasePerStep");
            nearestNeighbor = null;
            return false;
        }

        // NOTE(sarper Mar 04 21): Found multiple neighbors, find the closest one between them
        float shortestDistance = float.MaxValue;
        Transform shortestDistanceNeighbor = null;

        for(int i = 0; i < searchResultCount; i++) {
            // NOTE(sarper Mar 04 21): Skip over the requester
            if(_searchResults[i].transform == requester) continue;

            // NOTE(sarper Mar 04 21): Square magnitude for optimization, we don't need the exact distance, just the ratio to the current shortest distance
            float sqrDistance = (requester.position - _searchResults[i].transform.position).sqrMagnitude;

            if(sqrDistance < shortestDistance) {
                shortestDistance = sqrDistance;
                shortestDistanceNeighbor = _searchResults[i].transform;
            }
        }

        nearestNeighbor = shortestDistanceNeighbor;
        return true;
    }
}