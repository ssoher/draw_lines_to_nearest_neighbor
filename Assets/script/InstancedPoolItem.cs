/*
Ironbelly Programming Test
Sarper Soher - https://www.accidentalee.com
Mar 04 2021 - 04:25
*/

using UnityEngine;

public sealed class InstancedPoolItem : MonoBehaviour {
    /* NOTE(sarper Mar 04 21): We keep the prefab's instance id on instanced pool items to later on check for a specific type of item with a prefab reference
    (e.g. find an expired item in the pool to re-active instead of creating a new instance of that prefab in the pool)*/
    public int OriginatedPrefabInstanceId;
}