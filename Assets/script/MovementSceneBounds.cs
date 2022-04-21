/*
Find Nearest Neighbors
Sarper Soher - https://www.sarpersoher.com
Mar 04 2021 - 05:00
*/

using UnityEngine;

public sealed class MovementSceneBounds : MonoBehaviour {
    /* NOTE(sarper Mar 04 21): I would rather not use a simple singleton such as this one for they are promoting tight coupling.
    A central "System Finder" as the only singleton with references to classes such as this one would work better in a production grade project.
    The only reason for having this is to let dynamically created class instances can access it. to get `RandomPosition` within bounds
    */
    private static MovementSceneBounds _instance;

    public static MovementSceneBounds Instance {
        get {
            if(_instance == null) _instance = FindObjectOfType(typeof(MovementSceneBounds)) as MovementSceneBounds;
            if(_instance == null) Debug.LogError("No instance of type MovementSceneBounds is available in the scene!");
            return _instance;
        }
    }

    public Vector3 Size;

    // NOTE(sarper Mar 04 21): A vector to re-use each time someone asks for a random position. "new Vector3" with hundreds of objects would add up, possibly causing GC hiccups down the line
    private Vector3 _reusedPositionVector;

    public Vector3 RandomPosition() {
        float halfX = Size.x * 0.5f;
        float halfY = Size.y * 0.5f;
        float halfZ = Size.z * 0.5f;

        _reusedPositionVector.Set(Random.Range(-halfX, halfX), Random.Range(-halfY, halfY), Random.Range(-halfZ, halfZ));
        return transform.position + _reusedPositionVector;
    }

    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, Size);
    }
}