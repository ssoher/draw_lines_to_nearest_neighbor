/*
Find Nearest Neighbors
Sarper Soher - https://www.sarpersoher.com
Mar 04 2021 - 02:43
*/

using System.Collections.Generic;
using UnityEngine;

public sealed class Pool : MonoBehaviour {
    // NOTE(sarper Mar 04 21): These two events are here only to update the active item count label when they are invoked
    public static event System.Action ItemActivated;
    public static event System.Action ItemExpired;

    // NOTE(sarper Mar 04 21): The list of prefabs and their counts, to create on startup
    public List<DefaultPoolItem> DefaultPoolItems;

    // NOTE(sarper Mar 04 21): Whether create the startup items active or expired
    public bool DefaultPoolItemsStartActive;

    public List<InstancedPoolItem> ActiveList;
    public List<InstancedPoolItem> ExpiredList;

    private void Start() {
        // NOTE(sarper Mar 04 21): Create the default pool items
        for(int i = 0; i < DefaultPoolItems.Count; i++) {
            for(int j = 0; j < DefaultPoolItems[i].Count; j++) {
                CreateItem(DefaultPoolItems[i].Prefab, DefaultPoolItemsStartActive, MovementSceneBounds.Instance.RandomPosition());
            }
        }
    }

    public void ActivateItem(GameObject prefab, Vector3 position) {
        InstancedPoolItem instanceInExpiredList;

        // NOTE(sarper Mar 04 21): Try to find an expired instance of the given prefab first, if none present, then create a new one and add it to the pool
        if(FindExpiredInstanceOfPrefab(prefab, out instanceInExpiredList)) {
            instanceInExpiredList.gameObject.SetActive(true);

            instanceInExpiredList.transform.position = position;

            ExpiredList.Remove(instanceInExpiredList);
            ActiveList.Add(instanceInExpiredList);
        } else {
            CreateItem(prefab, true, position);
        }

        ItemActivated?.Invoke();
    }

    public void ExpireItem(InstancedPoolItem item) {
        if(ActiveList.Count == 0) {
            Debug.LogWarning("No active items in the pool to expire");
            return;
        }

        // NOTE(sarper Mar 04 21): Getting the index instead of the item itself, this saves us from checking against null in this particular case (see https://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/)
        int index = ActiveList.IndexOf(item);

        if(index == -1) {
            Debug.LogError("ActiveList does not contain the given item to expire");
            return;
        }

        item.gameObject.SetActive(false);

        ActiveList.Remove(item);
        ExpiredList.Add(item);

        ItemExpired?.Invoke();
    }

    // NOTE(sarper Mar 04 21): Overriden version for expiring the last item in the active items list
    public void ExpireItem() {
        ExpireItem(ActiveList[ActiveList.Count - 1]);
    }

    // NOTE(sarper Mar 04 21): Creates the item, makes it a child of the pool, initializes it's OriginatedPrefabInstanceId to the prefab's Instance id, so that we can compare this instance to a prefab later on
    public void CreateItem(GameObject prefab, bool startsActive, Vector3 position) {
        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        instance.transform.SetParent(transform);

        InstancedPoolItem ipi = instance.AddComponent<InstancedPoolItem>();
        ipi.OriginatedPrefabInstanceId = prefab.GetInstanceID();

        if(startsActive) {
            ActiveList.Add(ipi);
            ItemActivated?.Invoke();
        } else {
            instance.SetActive(false);
            ExpiredList.Add(ipi);
        }
    }

    /* NOTE(sarper Mar 04 21): Finds an expired version of the given prefab, the instance is found via comparing the prefab's instance id and the stored OriginatedPrefabInstanceId on the InstancePoolItem
    'result' is set as an 'out' parameter so that we can return a bool stating the success status. Otherwise we would have to compare the return value against null at the point this is called
    (see https://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/)
    */
    public bool FindExpiredInstanceOfPrefab(GameObject prefab, out InstancedPoolItem result) {
        for(int i = 0; i < ExpiredList.Count; i++) {
            if(ExpiredList[i].OriginatedPrefabInstanceId == prefab.GetInstanceID()) {
                result = ExpiredList[i];
                return true;
            }
        }

        result = null;
        return false;
    }
}